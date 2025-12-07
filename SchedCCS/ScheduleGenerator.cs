using System;
using System.Collections.Generic;
using System.Linq;

namespace SchedCCS
{
    public class ScheduleGenerator
    {
        // 1. Data Fields
        private List<Room> rooms;
        private List<Teacher> teachers;
        private List<Section> sections;

        // 2. Output
        public List<ScheduleItem> GeneratedSchedule { get; private set; } = new List<ScheduleItem>();
        public List<FailedEntry> FailedAssignments { get; private set; } = new List<FailedEntry>();

        // 3. Constructor
        public ScheduleGenerator(List<Room> r, List<Teacher> t, List<Section> s)
        {
            rooms = r;
            teachers = t;
            sections = s;
        }

        // =============================================================
        // MAIN ALGORITHM
        // =============================================================

        public void Generate()
        {
            // Reset State
            GeneratedSchedule.Clear();
            FailedAssignments.Clear();
            ResetResourceStates();

            // Loop Sections
            foreach (var section in sections)
            {
                // Prioritize Labs first (harder to fit), then Lectures
                var sortedSubjects = section.SubjectsToTake
                    .OrderByDescending(s => s.IsLab)
                    .ThenByDescending(s => s.Units)
                    .ToList();

                foreach (var subject in sortedSubjects)
                {
                    bool success = false;
                    if (subject.IsLab)
                    {
                        success = AssignLabSlot(section, subject);
                    }
                    else
                    {
                        success = AssignLectureSlot(section, subject);
                    }

                    if (!success)
                    {
                        FailedAssignments.Add(new FailedEntry
                        {
                            Section = section,
                            Subject = subject,
                            Reason = "Conflict: No Teacher/Room available for all units."
                        });
                    }
                }
            }
        }

        // =============================================================
        // ASSIGNMENT STRATEGIES (ATOMIC & CONSTRAINT-AWARE)
        // =============================================================

        // Strategy A: Assign Contiguous Blocks (e.g. 3 hours straight for Lab)
        // REPLACE YOUR 'AssignLabSlot' METHOD WITH THIS:

        private bool AssignLabSlot(Section section, Subject subject)
        {
            // 1. Find valid teachers & Shuffle them
            var qualifiedTeachers = teachers
                .Where(t => t.QualifiedSubjects.Contains(CleanSubjectName(subject.Code)))
                .OrderBy(x => Guid.NewGuid())
                .ToList();

            // 2. DEFINE THE SEARCH ORDER
            // PRIORITIZE SATURDAY (6), then check Weekdays (1-5)
            int[] daysToCheck = { 6, 1, 2, 3, 4, 5 };

            foreach (var teacher in qualifiedTeachers)
            {
                // Iterate through days based on our priority list
                foreach (int d in daysToCheck)
                {
                    // For Saturday (6), we often allow 7am-6pm. 
                    // For Weekdays, the logic remains the same.

                    // Check hours 0 (7am) to (11 - units)
                    for (int startH = 0; startH <= 11 - subject.Units; startH++)
                    {
                        // Check Constraints for the WHOLE block
                        if (IsBlockAvailable(section, teacher, d, startH, subject.Units, subject.IsLab, out Room foundRoom))
                        {
                            // SUCCESS: Book the block
                            BookSlot(section, subject, d, startH, subject.Units, foundRoom, teacher);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        // Strategy B: Assign Distributed Hours (e.g. 1 hr Mon, 1 hr Wed, 1 hr Fri)
        private bool AssignLectureSlot(Section section, Subject subject)
        {
            int unitsNeeded = subject.Units;

            // 1. Find valid teachers
            var qualifiedTeachers = teachers
                .Where(t => t.QualifiedSubjects.Contains(CleanSubjectName(subject.Code)))
                .OrderBy(x => Guid.NewGuid()) // Shuffle
                .ToList();

            foreach (var teacher in qualifiedTeachers)
            {
                List<(int day, int hour, Room room)> proposedSlots = new List<(int day, int hour, Room room)>();

                // 2. Generate ALL possible random slots
                var randomSlots = GetRandomizedSlots();

                foreach (var slot in randomSlots)
                {
                    if (proposedSlots.Count >= unitsNeeded) break;

                    // CHECK CONSTRAINTS (Human & Physical)
                    if (IsSingleSlotAvailable(section, teacher, slot.day, slot.hour, subject.IsLab, out Room foundRoom))
                    {
                        proposedSlots.Add((slot.day, slot.hour, foundRoom));
                    }
                }

                // 3. ATOMIC CHECK: Did this teacher fit ALL units?
                if (proposedSlots.Count == unitsNeeded)
                {
                    // Yes -> Book them all
                    foreach (var s in proposedSlots)
                    {
                        BookSlot(section, subject, s.day, s.hour, 1, s.room, teacher);
                    }
                    return true;
                }
            }
            return false;
        }

        // =============================================================
        // HELPERS & VALIDATION
        // =============================================================

        // Checks if a contiguous block (duration > 1) is free
        private bool IsBlockAvailable(Section section, Teacher teacher, int day, int startHour, int duration, bool isLab, out Room foundRoom)
        {
            foundRoom = null;

            // 1. Scan the whole block for basic conflicts
            for (int i = 0; i < duration; i++)
            {
                int h = startHour + i;

                // Lunch Break Rule (No classes 12:00-1:00)
                if (h == 5) return false;

                // Basic Busy Checks
                if (section.IsBusy[day, h]) return false;
                if (teacher.IsBusy[day, h]) return false;
            }

            // 2. Teacher Fatigue Check (Daily Limit)
            int currentLoad = GetTeacherDailyLoad(teacher, day);
            if (currentLoad + duration > 9) return false;

            // 3. Find a Room that is free for the ENTIRE block
            var candidateRooms = rooms
                .Where(r => r.Type == (isLab ? RoomType.Laboratory : RoomType.Lecture))
                .OrderBy(x => Guid.NewGuid())
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

        // Checks if a single 1-hour slot is free
        private bool IsSingleSlotAvailable(Section section, Teacher teacher, int day, int hour, bool isLab, out Room foundRoom)
        {
            foundRoom = null;

            // 1. Lunch Break Constraint
            if (hour == 5) return false;

            // 2. Busy Checks
            if (section.IsBusy[day, hour]) return false;
            if (teacher.IsBusy[day, hour]) return false;

            // 3. Teacher Fatigue (Daily)
            if (GetTeacherDailyLoad(teacher, day) >= 9) return false;

            // 4. Teacher Fatigue (Continuous 4-hour limit)
            if (IsTeacherFatigued(teacher, day, hour)) return false;

            // 5. Student Fatigue (Spread out majors)
            if (IsSectionFatigued(section, day, hour)) return false;

            // 6. Room Check
            foundRoom = rooms.FirstOrDefault(r =>
                r.Type == (isLab ? RoomType.Laboratory : RoomType.Lecture) &&
                !r.IsBusy[day, hour]);

            return foundRoom != null;
        }

        private void BookSlot(Section section, Subject subject, int day, int startHour, int duration, Room room, Teacher teacher)
        {
            for (int i = 0; i < duration; i++)
            {
                int h = startHour + i;

                // Update Arrays
                section.IsBusy[day, h] = true;
                teacher.IsBusy[day, h] = true;
                room.IsBusy[day, h] = true;

                // Add to List
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

        // =============================================================
        // UTILITIES
        // =============================================================

        private List<(int day, int hour)> GetRandomizedSlots()
        {
            var slots = new List<(int, int)>();
            for (int d = 1; d <= 5; d++)
                for (int h = 0; h < 11; h++)
                    slots.Add((d, h));

            // Shuffle to prevent "Packing"
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
            // Check previous 4 hours for continuous work
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
            // Avoid more than 4 hours straight for students too
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

        private string CleanSubjectName(string raw)
        {
            return raw.Replace(" (Lec)", "").Replace(" (Lab)", "").Trim();
        }

        private string GetDayName(int d) => d >= 0 && d < 8 ? new[] { "", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" }[d] : "Err";

        private string GetTimeLabel(int t) => $"{7 + t}:00 - {8 + t}:00";
    }
}