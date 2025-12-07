using System;
using System.Collections.Generic;
using System.Linq;

namespace SchedCCS
{
    public class ScheduleGenerator
    {
        #region 1. Fields & Properties

        private List<Room> rooms;
        private List<Teacher> teachers;
        private List<Section> sections;
        private Dictionary<string, Teacher> assignedTeachers = new Dictionary<string, Teacher>();

        // Public properties to expose results to the dashboard
        public List<ScheduleItem> GeneratedSchedule { get; private set; } = new List<ScheduleItem>();
        public List<FailedEntry> FailedAssignments { get; private set; } = new List<FailedEntry>();

        private class ScheduleTask
        {
            public Section Section;
            public Subject Subject;
        }

        #endregion

        #region 2. Constructor

        public ScheduleGenerator(List<Room> r, List<Teacher> t, List<Section> s)
        {
            rooms = r;
            teachers = t;
            sections = s;
        }

        #endregion

        #region 3. Main Algorithm

        public void Generate()
        {
            // Reset internal state
            GeneratedSchedule.Clear();
            FailedAssignments.Clear();
            assignedTeachers.Clear();

            // 1. Flatten all tasks
            var allTasks = new List<ScheduleTask>();
            foreach (var section in sections)
            {
                foreach (var subject in section.SubjectsToTake)
                {
                    allTasks.Add(new ScheduleTask { Section = section, Subject = subject });
                }
            }

            // 2. Prioritize Tasks
            // Order: Labs (Hardest) -> PE/Block -> High Year Levels -> Random Shuffle
            var sortedTasks = allTasks
                .OrderByDescending(t => t.Subject.IsLab)
                .ThenByDescending(t => IsPE(t.Subject))
                .ThenByDescending(t => t.Section.YearLevel)
                .ThenBy(t => Guid.NewGuid()) // Randomness ensures different results per run
                .ToList();

            // 3. Allocation Loop
            foreach (var task in sortedTasks)
            {
                bool success = false;

                if (task.Subject.IsLab || IsPE(task.Subject))
                {
                    // Block Scheduling: Assign as a single continuous block
                    success = AssignSlot(task.Section, task.Subject);

                    if (!success)
                        RecordFailure(task.Section, task.Subject, "Block/PE Conflict");
                }
                else
                {
                    // Lecture Scheduling: Split into 1-hour chunks
                    int hoursNeeded = task.Subject.Units;
                    for (int i = 0; i < hoursNeeded; i++)
                    {
                        success = AssignSlot(task.Section, task.Subject);
                        if (!success)
                            RecordFailure(task.Section, task.Subject, "Lec Conflict");
                    }
                }
            }
        }

        #endregion

        #region 4. Slot Allocation Logic

        private bool AssignSlot(Section section, Subject subject)
        {
            string code = subject.Code.ToUpper();
            bool isPE = IsPE(subject);

            // Define preferred days based on subject type
            List<int> preferredDays;
            if (isPE)
            {
                preferredDays = new List<int> { 0, 1, 2, 3, 4, 5, 6 }.OrderBy(x => Guid.NewGuid()).ToList();
            }
            else if (subject.IsLab)
            {
                var days = new List<int> { 5 }; // Prefer Saturday
                days.AddRange(new List<int> { 0, 1, 2, 3, 4 }.OrderBy(x => Guid.NewGuid()));
                preferredDays = days;
            }
            else
            {
                var weekdays = new List<int> { 0, 1, 2, 3, 4 }.OrderBy(x => Guid.NewGuid()).ToList();
                weekdays.Add(5);
                preferredDays = weekdays;
            }

            var randomRoomOrder = rooms.OrderBy(x => Guid.NewGuid()).ToList();
            Teacher requiredTeacher = GetAssignedTeacher(section, subject.Code);

            foreach (int dayIndex in preferredDays)
            {
                // Validate array bounds to prevent index errors (e.g., Sunday index 6 vs Array size 6)
                if (dayIndex >= section.IsBusy.GetLength(0)) continue;

                // Constraint: Only one Lab per day per section
                if (subject.IsLab && HasLabToday(section, dayIndex)) continue;

                // Constraint: Daily hour cap (7 hours max)
                int duration = (subject.IsLab || isPE) ? subject.Units : 1;
                if (GetSectionDailyHours(section, dayIndex) + duration > 7) continue;

                // Search for time slots
                for (int startHour = 0; startHour <= 11 - duration; startHour++)
                {
                    if (!IsSectionFree(section, dayIndex, startHour, duration)) continue;

                    // Fatigue Check: Avoid creating > 4 hour continuous blocks (except for Labs/PE)
                    if (!subject.IsLab && !isPE && IsSectionFatigued(section, dayIndex, startHour, duration)) continue;

                    // Find Room
                    Room availableRoom = null;
                    if (!isPE)
                    {
                        availableRoom = FindAvailableRoom(randomRoomOrder, subject.IsLab, dayIndex, startHour, duration);
                        if (availableRoom == null) continue;
                    }

                    // Find Teacher
                    Teacher finalTeacher = null;
                    if (requiredTeacher != null)
                    {
                        if (IsTeacherAvailable(requiredTeacher, dayIndex, startHour, duration))
                            finalTeacher = requiredTeacher;
                    }
                    else
                    {
                        finalTeacher = FindNewAvailableTeacher(subject.Code, dayIndex, startHour, duration);
                    }

                    // Book the Slot if all resources are found
                    if (finalTeacher != null)
                    {
                        SetAssignedTeacher(section, subject.Code, finalTeacher);
                        BookSlot(section, subject, dayIndex, startHour, duration, availableRoom, finalTeacher, isPE);
                        return true;
                    }
                }
            }

            return AssignFallbackSlot(section, subject, preferredDays, isPE);
        }

        private bool AssignFallbackSlot(Section section, Subject subject, List<int> days, bool isPE)
        {
            var randomRoomOrder = rooms.OrderBy(x => Guid.NewGuid()).ToList();
            int duration = (isPE || subject.IsLab) ? subject.Units : 1;

            foreach (int day in days)
            {
                if (day >= 6 && section.IsBusy.GetLength(0) < 7) continue;
                if (subject.IsLab && HasLabToday(section, day)) continue;

                for (int startHour = 0; startHour <= 11 - duration; startHour++)
                {
                    if (IsSectionFree(section, day, startHour, duration))
                    {
                        // Fallback: Book without a teacher (Professor XYZ) if room is found
                        Room r = isPE ? null : FindAvailableRoom(randomRoomOrder, subject.IsLab, day, startHour, duration);

                        if (r != null || isPE)
                        {
                            BookSlot(section, subject, day, startHour, duration, r, null, isPE);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void BookSlot(Section section, Subject subject, int day, int startHour, int duration, Room room, Teacher teacher, bool isPE)
        {
            string tName = teacher?.Name ?? "Professor XYZ";
            string rName = isPE ? "GYM / FIELD" : (room?.Name ?? "Room TBA");

            if (teacher == null)
                RecordFailure(section, subject, "Teacher Unavailable (Assigned to XYZ)");

            for (int i = 0; i < duration; i++)
            {
                int h = startHour + i;
                section.IsBusy[day, h] = true;
                if (room != null) room.IsBusy[day, h] = true;
                if (teacher != null) teacher.IsBusy[day, h] = true;

                GeneratedSchedule.Add(new ScheduleItem
                {
                    Section = section.Name,
                    Subject = subject.Code,
                    Teacher = tName,
                    Room = rName,
                    Day = ((Day)day).ToString(),
                    Time = $"{7 + h}:00 - {8 + h}:00"
                });
            }
        }

        #endregion

        #region 5. Helper Methods (Validation & Lookups)

        private bool IsPE(Subject s)
        {
            string c = s.Code.ToUpper();
            return c.Contains("PE") || c.Contains("PATHFIT") || c.Contains("NSTP");
        }

        private bool IsTeacherAvailable(Teacher teacher, int day, int startHour, int duration)
        {
            for (int i = 0; i < duration; i++)
                if (teacher.IsBusy[day, startHour + i]) return false;

            if (GetTeacherDailyHours(teacher, day) + duration > 9) return false;

            return true;
        }

        private Teacher FindNewAvailableTeacher(string subjectCode, int day, int startHour, int duration)
        {
            string baseCode = CleanSubjectName(subjectCode);
            foreach (var teacher in teachers)
            {
                // Check qualifications (exact match or base name match)
                if (!teacher.QualifiedSubjects.Contains(subjectCode) && !teacher.QualifiedSubjects.Contains(baseCode)) continue;

                if (IsTeacherAvailable(teacher, day, startHour, duration)) return teacher;
            }
            return null;
        }

        private Room FindAvailableRoom(List<Room> shuffledRooms, bool isLab, int day, int startHour, int duration)
        {
            RoomType type = isLab ? RoomType.Laboratory : RoomType.Lecture;
            return shuffledRooms.FirstOrDefault(r => r.Type == type && IsRoomFree(r, day, startHour, duration));
        }

        private bool IsRoomFree(Room r, int d, int s, int dur)
        {
            for (int i = 0; i < dur; i++)
                if (r.IsBusy[d, s + i]) return false;
            return true;
        }

        private bool IsSectionFree(Section s, int d, int h, int dur)
        {
            for (int i = 0; i < dur; i++)
                if (h + i >= 12 || s.IsBusy[d, h + i]) return false;
            return true;
        }

        private bool HasLabToday(Section section, int day)
        {
            return GeneratedSchedule.Any(s => s.Section == section.Name &&
                                              s.Day == ((Day)day).ToString() &&
                                              (s.Subject.Contains("(Lab)") || s.Room.Contains("LAB")));
        }

        private bool IsSectionFatigued(Section section, int day, int startHour, int duration)
        {
            int before = 0;
            for (int h = startHour - 1; h >= 0; h--) { if (section.IsBusy[day, h]) before++; else break; }

            int after = 0;
            for (int h = startHour + duration; h < 12; h++) { if (section.IsBusy[day, h]) after++; else break; }

            return (before + duration + after) > 4;
        }

        private int GetSectionDailyHours(Section section, int day)
        {
            int count = 0;
            for (int h = 0; h < 12; h++) if (section.IsBusy[day, h]) count++;
            return count;
        }

        private int GetTeacherDailyHours(Teacher t, int d)
        {
            int count = 0;
            for (int h = 0; h < 12; h++) if (t.IsBusy[d, h]) count++;
            return count;
        }

        private Teacher GetAssignedTeacher(Section s, string sub)
        {
            string key = $"{s.Name}_{CleanSubjectName(sub)}";
            return assignedTeachers.ContainsKey(key) ? assignedTeachers[key] : null;
        }

        private void SetAssignedTeacher(Section s, string sub, Teacher t)
        {
            string key = $"{s.Name}_{CleanSubjectName(sub)}";
            if (!assignedTeachers.ContainsKey(key)) assignedTeachers[key] = t;
        }

        private void RecordFailure(Section s, Subject sub, string r)
        {
            FailedAssignments.Add(new FailedEntry { Section = s, Subject = sub, Reason = r });
        }

        private string CleanSubjectName(string raw) => raw.Replace(" (Lec)", "").Replace(" (Lab)", "").Trim();

        #endregion
    }
}