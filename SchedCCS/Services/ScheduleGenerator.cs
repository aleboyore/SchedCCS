using System;
using System.Collections.Generic;
using System.Linq;

namespace SchedCCS
{
    /// <summary>
    /// Core engine responsible for generating automated schedules.
    /// Utilizes a heuristic greedy approach with institutional constraint validation.
    /// </summary>
    public class ScheduleGenerator
    {
        #region Fields & Properties

        // Data repositories for system resources
        private readonly List<Room> _rooms;
        private readonly List<Teacher> _teachers;
        private readonly List<Section> _sections;

        // Public output collections
        public List<ScheduleItem> GeneratedSchedule { get; private set; } = new List<ScheduleItem>();
        public List<FailedEntry> FailedAssignments { get; private set; } = new List<FailedEntry>();

        #endregion

        #region Constructor

        public ScheduleGenerator(List<Room> r, List<Teacher> t, List<Section> s)
        {
            _rooms = r;
            _teachers = t;
            _sections = s;
        }

        #endregion

        #region 1. Main Execution Flow

        /// <summary>
        /// Initiates the schedule generation process.
        /// Sorts resources by workload weight to optimize allocation success.
        /// </summary>
        public void Generate()
        {
            // Clear previous results and reset resource availability
            GeneratedSchedule.Clear();
            FailedAssignments.Clear();
            ResetResourceStates();

            // Sort sections by subject volume to handle complex requirements early
            var sortedSections = _sections.OrderByDescending(s => s.SubjectsToTake.Count).ToList();

            foreach (var section in sortedSections)
            {
                // Prioritize Laboratory sessions and subjects with higher unit values
                var sortedSubjects = section.SubjectsToTake
                    .OrderByDescending(s => s.IsLab)
                    .ThenByDescending(s => s.Units)
                    .ToList();

                foreach (var subject in sortedSubjects)
                {
                    // Execute assignment based on subject type
                    bool success = subject.IsLab ? AssignLabSlot(section, subject) : AssignLectureSlot(section, subject);

                    // Record failures for manual optimization in the dashboard
                    if (!success)
                    {
                        FailedAssignments.Add(new FailedEntry
                        {
                            Section = section,
                            Subject = subject,
                            Reason = "Optimization Conflict: No suitable slot found."
                        });
                    }
                }
            }
        }

        #endregion

        #region 2. Assignment Strategies

        /// <summary>
        /// Assigns lab subjects with a strategy favoring Saturdays and randomized weekdays.
        /// </summary>
        private bool AssignLabSlot(Section section, Subject subject)
        {
            // Gather teachers qualified for this specific subject code
            var qualifiedTeachers = _teachers
                .Where(t => t.QualifiedSubjects.Contains(CleanSubjectName(subject.Code)))
                .OrderBy(x => Guid.NewGuid())
                .ToList();

            // Set search priority: Saturday first, then randomized weekdays
            List<int> daysToCheck = new List<int> { 6 };
            var weekdays = new List<int> { 1, 2, 3, 4, 5 };
            var rng = new Random();
            daysToCheck.AddRange(weekdays.OrderBy(x => rng.Next()));

            foreach (var teacher in qualifiedTeachers)
            {
                foreach (int d in daysToCheck)
                {
                    // Calculate start hours that accommodate the subject's total unit duration
                    List<int> possibleStartHours = Enumerable.Range(0, 12 - subject.Units)
                        .OrderBy(x => rng.Next())
                        .ToList();

                    foreach (int startH in possibleStartHours)
                    {
                        // Validate availability for the required block duration
                        if (IsBlockAvailable(section, teacher, d, startH, subject.Units, subject.IsLab, subject.Code, out Room foundRoom))
                        {
                            // Avoid back-to-back scheduling for sections to prevent fatigue
                            if ((startH + subject.Units < 11 && section.IsBusy[d, startH + subject.Units]) ||
                                (startH > 0 && section.IsBusy[d, startH - 1]))
                            {
                                continue;
                            }

                            // Persistent booking in schedule collection and resource matrices
                            BookSlot(section, subject, d, startH, subject.Units, foundRoom, teacher);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Assigns lecture subjects using an incremental randomized slot selection strategy.
        /// </summary>
        private bool AssignLectureSlot(Section section, Subject subject)
        {
            int unitsNeeded = subject.Units;
            var qualifiedTeachers = _teachers
                .Where(t => t.QualifiedSubjects.Contains(CleanSubjectName(subject.Code)))
                .OrderBy(x => Guid.NewGuid())
                .ToList();

            foreach (var teacher in qualifiedTeachers)
            {
                // Track proposed slots to ensure the teacher can cover the full unit load
                List<(int day, int hour, Room room)> proposedSlots = new List<(int day, int hour, Room room)>();
                var randomSlots = GetRandomizedSlots();

                foreach (var slot in randomSlots)
                {
                    if (proposedSlots.Count >= unitsNeeded) break;

                    // Verify single-hour availability per slot
                    if (IsSingleSlotAvailable(section, teacher, slot.day, slot.hour, subject.IsLab, subject.Code, out Room foundRoom))
                    {
                        proposedSlots.Add((slot.day, slot.hour, foundRoom));
                    }
                }

                // Finalize assignment only if all required units are fulfilled
                if (proposedSlots.Count == unitsNeeded)
                {
                    foreach (var s in proposedSlots)
                        BookSlot(section, subject, s.day, s.hour, 1, s.room, teacher);
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region 3. Constraint Checkers

        /// <summary>
        /// Evaluates multi-hour availability considering teacher load and facility rules.
        /// </summary>
        private bool IsBlockAvailable(Section section, Teacher teacher, int day, int startHour, int duration, bool isLab, string subjectCode, out Room foundRoom)
        {
            foundRoom = null;

            // Check resource matrix conflicts
            for (int i = 0; i < duration; i++)
            {
                int h = startHour + i;
                if (section.IsBusy[day, h] || teacher.IsBusy[day, h]) return false;
            }

            // Enforce teacher workload cap (9 hours per day)
            if (GetTeacherDailyLoad(teacher, day) + duration > 9) return false;

            // Filter rooms by pedagogical type and balance load based on usage
            var correctRoomType = isLab ? RoomType.Laboratory : RoomType.Lecture;
            var candidateRooms = _rooms
                .Where(r => r.Type == correctRoomType)
                .OrderBy(r => CountRoomUsage(r))
                .ThenBy(x => Guid.NewGuid())
                .ToList();

            foreach (var room in candidateRooms)
            {
                // Facility Rule: Restrict academic subjects from sports fields/gyms
                if (IsOutdoorRoom(room.Name) && !IsSportSubject(subjectCode)) continue;
                if (IsSportSubject(subjectCode) && !IsOutdoorRoom(room.Name)) continue;

                // Validate room availability across the entire duration
                bool roomIsFree = true;
                for (int i = 0; i < duration; i++)
                {
                    if (room.IsBusy[day, startHour + i])
                    {
                        roomIsFree = false;
                        break;
                    }
                }

                if (roomIsFree)
                {
                    foundRoom = room;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Evaluates single-hour availability including fatigue and facility constraints.
        /// </summary>
        private bool IsSingleSlotAvailable(Section section, Teacher teacher, int day, int hour, bool isLab, string subjectCode, out Room foundRoom)
        {
            foundRoom = null;

            // Basic busy-state check
            if (section.IsBusy[day, hour] || teacher.IsBusy[day, hour]) return false;

            // Basic fatigue checks: max daily load and max consecutive activity
            if (GetTeacherDailyLoad(teacher, day) >= 9) return false;
            if (IsTeacherFatigued(teacher, day, hour) || IsSectionFatigued(section, day, hour)) return false;

            var correctRoomType = isLab ? RoomType.Laboratory : RoomType.Lecture;
            var candidateRooms = _rooms
                .Where(r => r.Type == correctRoomType)
                .OrderBy(r => CountRoomUsage(r))
                .ThenBy(x => Guid.NewGuid())
                .ToList();

            foreach (var room in candidateRooms)
            {
                // Apply outdoor facility restrictions
                if (IsOutdoorRoom(room.Name) && !IsSportSubject(subjectCode)) continue;
                if (IsSportSubject(subjectCode) && !IsOutdoorRoom(room.Name)) continue;

                if (!room.IsBusy[day, hour])
                {
                    foundRoom = room;
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region 4. Internal Logic & Utilities

        /// <summary>
        /// Updates the global schedule and resource matrices to mark slots as occupied.
        /// </summary>
        private void BookSlot(Section section, Subject subject, int day, int startHour, int duration, Room room, Teacher teacher)
        {
            for (int i = 0; i < duration; i++)
            {
                int h = startHour + i;
                section.IsBusy[day, h] = true;
                teacher.IsBusy[day, h] = true;
                room.IsBusy[day, h] = true;

                GeneratedSchedule.Add(new ScheduleItem
                {
                    Section = section.Name,
                    Subject = subject.Code,
                    Teacher = teacher.Name,
                    Room = room.Name,
                    Day = GetDayName(day),
                    Time = GetTimeLabel(h),
                    DayIndex = day,
                    TimeIndex = h,
                    RoomObj = room
                });
            }
        }

        // Returns total hours occupied for a specific room
        private int CountRoomUsage(Room r)
        {
            int count = 0;
            for (int d = 0; d < 7; d++)
                for (int h = 0; h < 13; h++)
                    if (r.IsBusy[d, h]) count++;
            return count;
        }

        // Generates a randomized list of standard academic time slots
        private List<(int day, int hour)> GetRandomizedSlots()
        {
            var slots = new List<(int, int)>();
            for (int d = 1; d <= 5; d++)
                for (int h = 0; h < 11; h++)
                    slots.Add((d, h));

            var rng = new Random();
            return slots.OrderBy(x => rng.Next()).ToList();
        }

        // Returns total assigned hours for a teacher on a specific day
        private int GetTeacherDailyLoad(Teacher t, int day)
        {
            int hours = 0;
            for (int h = 0; h < 11; h++) if (t.IsBusy[day, h]) hours++;
            return hours;
        }

        private bool IsTeacherFatigued(Teacher t, int day, int hour) => CheckConsecutiveHours(t.IsBusy, day, hour);

        private bool IsSectionFatigued(Section s, int day, int hour) => CheckConsecutiveHours(s.IsBusy, day, hour);

        // Validates if an entity has exceeded 4 consecutive hours of activity
        private bool CheckConsecutiveHours(bool[,] busyMatrix, int day, int hour)
        {
            int consecutive = 0;
            for (int i = 1; i <= 4; i++)
            {
                if (hour - i >= 0 && busyMatrix[day, hour - i]) consecutive++;
                else break;
            }
            return consecutive >= 4;
        }

        // Resets availability matrices for a fresh generation attempt
        private void ResetResourceStates()
        {
            foreach (var r in _rooms) r.IsBusy = new bool[7, 13];
            foreach (var t in _teachers) t.IsBusy = new bool[7, 13];
            foreach (var s in _sections) s.IsBusy = new bool[7, 13];
        }

        private string CleanSubjectName(string raw) => raw.Replace(" (Lec)", "").Replace(" (Lab)", "").Trim();

        private string GetDayName(int d) => d >= 0 && d < 8
            ? new[] { "", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" }[d]
            : "Err";

        private string GetTimeLabel(int t) => $"{7 + t}:00 - {8 + t}:00";

        // Logic to identify sports-related subjects
        private bool IsSportSubject(string subjectCode)
        {
            string code = subjectCode.ToUpper();
            return code.Contains("PE") || code.Contains("PATHFIT") ||
                   code.Contains("NSTP") || code.Contains("GYM");
        }

        // Logic to identify outdoor facility resources
        private bool IsOutdoorRoom(string roomName)
        {
            string name = roomName.ToUpper();
            return name.Contains("FIELD") || name.Contains("GYM") || name.Contains("COURT");
        }

        #endregion
    }
}