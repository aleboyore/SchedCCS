using System;
using System.Collections.Generic;
using System.Linq;

namespace SchedCCS
{
    public class ScheduleGenerator
    {
        private List<Room> rooms;
        private List<Teacher> teachers;
        private List<Section> sections;

        public List<ScheduleItem> GeneratedSchedule { get; private set; } = new List<ScheduleItem>();
        public List<FailedEntry> FailedAssignments { get; private set; } = new List<FailedEntry>();

        public ScheduleGenerator(List<Room> r, List<Teacher> t, List<Section> s)
        {
            rooms = r;
            teachers = t;
            sections = s;
        }

        #region 1. Main Execution Flow

        public void Generate()
        {
            GeneratedSchedule.Clear();
            FailedAssignments.Clear();
            ResetResourceStates();

            // Process sections with heaviest loads first
            var sortedSections = sections.OrderByDescending(s => s.SubjectsToTake.Count).ToList();

            foreach (var section in sortedSections)
            {
                // Prioritize Labs, then heavy subjects
                var sortedSubjects = section.SubjectsToTake
                    .OrderByDescending(s => s.IsLab)
                    .ThenByDescending(s => s.Units)
                    .ToList();

                foreach (var subject in sortedSubjects)
                {
                    bool success = subject.IsLab ? AssignLabSlot(section, subject) : AssignLectureSlot(section, subject);

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

        // STRATEGY A: LAB ASSIGNMENT (Priority: Saturday -> Weekdays)
        private bool AssignLabSlot(Section section, Subject subject)
        {
            var qualifiedTeachers = teachers
                .Where(t => t.QualifiedSubjects.Contains(CleanSubjectName(subject.Code)))
                .OrderBy(x => Guid.NewGuid())
                .ToList();

            List<int> daysToCheck = new List<int> { 6 }; // Saturday first
            var weekdays = new List<int> { 1, 2, 3, 4, 5 };
            var rng = new Random();
            daysToCheck.AddRange(weekdays.OrderBy(x => rng.Next()));

            foreach (var teacher in qualifiedTeachers)
            {
                foreach (int d in daysToCheck)
                {
                    List<int> possibleStartHours = Enumerable.Range(0, 12 - subject.Units)
                        .OrderBy(x => rng.Next())
                        .ToList();

                    foreach (int startH in possibleStartHours)
                    {
                        if (IsBlockAvailable(section, teacher, d, startH, subject.Units, subject.IsLab, out Room foundRoom))
                        {
                            // Enforce Gaps
                            if ((startH + subject.Units < 11 && section.IsBusy[d, startH + subject.Units]) ||
                                (startH > 0 && section.IsBusy[d, startH - 1]))
                            {
                                continue;
                            }

                            BookSlot(section, subject, d, startH, subject.Units, foundRoom, teacher);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        // STRATEGY B: LECTURE ASSIGNMENT (Fully Randomized)
        private bool AssignLectureSlot(Section section, Subject subject)
        {
            int unitsNeeded = subject.Units;

            var qualifiedTeachers = teachers
                .Where(t => t.QualifiedSubjects.Contains(CleanSubjectName(subject.Code)))
                .OrderBy(x => Guid.NewGuid())
                .ToList();

            foreach (var teacher in qualifiedTeachers)
            {
                List<(int day, int hour, Room room)> proposedSlots = new List<(int day, int hour, Room room)>();
                var randomSlots = GetRandomizedSlots();

                foreach (var slot in randomSlots)
                {
                    if (proposedSlots.Count >= unitsNeeded) break;

                    if (IsSingleSlotAvailable(section, teacher, slot.day, slot.hour, subject.IsLab, out Room foundRoom))
                    {
                        proposedSlots.Add((slot.day, slot.hour, foundRoom));
                    }
                }

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

        private bool IsBlockAvailable(Section section, Teacher teacher, int day, int startHour, int duration, bool isLab, out Room foundRoom)
        {
            foundRoom = null;

            // Check Teacher & Section availability
            for (int i = 0; i < duration; i++)
            {
                int h = startHour + i;
                if (section.IsBusy[day, h] || teacher.IsBusy[day, h]) return false;
            }

            if (GetTeacherDailyLoad(teacher, day) + duration > 9) return false;

            // Find best room (Load Balancing)
            var correctRoomType = isLab ? RoomType.Laboratory : RoomType.Lecture;
            var candidateRooms = rooms
                .Where(r => r.Type == correctRoomType)
                .OrderBy(r => CountRoomUsage(r))
                .ThenBy(x => Guid.NewGuid())
                .ToList();

            foreach (var room in candidateRooms)
            {
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

        private bool IsSingleSlotAvailable(Section section, Teacher teacher, int day, int hour, bool isLab, out Room foundRoom)
        {
            foundRoom = null;

            if (section.IsBusy[day, hour]) return false;
            if (teacher.IsBusy[day, hour]) return false;
            if (GetTeacherDailyLoad(teacher, day) >= 9) return false;
            if (IsTeacherFatigued(teacher, day, hour)) return false;
            if (IsSectionFatigued(section, day, hour)) return false;

            // Room Load Balancing
            var correctRoomType = isLab ? RoomType.Laboratory : RoomType.Lecture;
            var candidateRooms = rooms
                .Where(r => r.Type == correctRoomType)
                .OrderBy(r => CountRoomUsage(r))
                .ThenBy(x => Guid.NewGuid())
                .ToList();

            foreach (var room in candidateRooms)
            {
                if (!room.IsBusy[day, hour])
                {
                    foundRoom = room;
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region 4. Utilities & Helpers

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

        private int CountRoomUsage(Room r)
        {
            int count = 0;
            for (int d = 0; d < 7; d++)
                for (int h = 0; h < 13; h++)
                    if (r.IsBusy[d, h]) count++;
            return count;
        }

        private List<(int day, int hour)> GetRandomizedSlots()
        {
            var slots = new List<(int, int)>();
            for (int d = 1; d <= 5; d++)
                for (int h = 0; h < 11; h++)
                    slots.Add((d, h));

            var rng = new Random();
            return slots.OrderBy(x => rng.Next()).ToList();
        }

        private int GetTeacherDailyLoad(Teacher t, int day)
        {
            int hours = 0;
            for (int h = 0; h < 11; h++) if (t.IsBusy[day, h]) hours++;
            return hours;
        }

        private bool IsTeacherFatigued(Teacher t, int day, int hour)
        {
            int consecutive = 0;
            for (int i = 1; i <= 4; i++)
            {
                if (hour - i >= 0 && t.IsBusy[day, hour - i]) consecutive++;
                else break;
            }
            return consecutive >= 4;
        }

        private bool IsSectionFatigued(Section s, int day, int hour)
        {
            int consecutive = 0;
            for (int i = 1; i <= 4; i++)
            {
                if (hour - i >= 0 && s.IsBusy[day, hour - i]) consecutive++;
                else break;
            }
            return consecutive >= 4;
        }

        private void ResetResourceStates()
        {
            foreach (var r in rooms) r.IsBusy = new bool[7, 13];
            foreach (var t in teachers) t.IsBusy = new bool[7, 13];
            foreach (var s in sections) s.IsBusy = new bool[7, 13];
        }

        private string CleanSubjectName(string raw) => raw.Replace(" (Lec)", "").Replace(" (Lab)", "").Trim();
        private string GetDayName(int d) => d >= 0 && d < 8 ? new[] { "", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" }[d] : "Err";
        private string GetTimeLabel(int t) => $"{7 + t}:00 - {8 + t}:00";

        #endregion
    }
}