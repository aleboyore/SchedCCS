using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedCCS
{
    // The "Brain" of the application. Handles all logic, no UI code.
    public class ScheduleService
    {
        #region 1. Schedule Generation

        // AUTOMATED GENERATION (Synchronous Core)
        public string GenerateSchedule()
        {
            // Setup tracking
            int lowestConflictCount = int.MaxValue;
            List<ScheduleItem> bestSchedule = new List<ScheduleItem>();
            List<FailedEntry> bestFailures = new List<FailedEntry>();

            // If current data exists, keep it as the baseline to beat
            if (DataManager.MasterSchedule.Count > 0)
            {
                lowestConflictCount = DataManager.FailedAssignments.Count;
                bestSchedule = new List<ScheduleItem>(DataManager.MasterSchedule);
                bestFailures = new List<FailedEntry>(DataManager.FailedAssignments);
            }

            // Run 50 attempts to find the best configuration
            int attempts = 50;
            ScheduleGenerator generator = new ScheduleGenerator(DataManager.Rooms, DataManager.Teachers, DataManager.Sections);

            for (int i = 0; i < attempts; i++)
            {
                generator.Generate();
                int score = generator.FailedAssignments.Count;

                if (score < lowestConflictCount)
                {
                    lowestConflictCount = score;
                    bestSchedule = new List<ScheduleItem>(generator.GeneratedSchedule);
                    bestFailures = new List<FailedEntry>(generator.FailedAssignments);
                }

                if (score == 0) break; // Perfection found
            }

            // Commit the best result
            DataManager.MasterSchedule = bestSchedule;
            DataManager.FailedAssignments = bestFailures;

            RebuildBusyArrays(); // Sync the "IsBusy" flags

            if (lowestConflictCount == 0) return "Success";
            return $"Generated with {lowestConflictCount} conflicts.";
        }

        // ASYNC GENERATION WRAPPER
        // Moves the heavy work to a background thread to keep UI responsive.
        public async Task<string> GenerateScheduleAsync()
        {
            return await Task.Run(() =>
            {
                return GenerateSchedule();
            });
        }

        #endregion

        #region 2. Data Synchronization

        public void RebuildBusyArrays()
        {
            // Wipe clean
            foreach (var r in DataManager.Rooms) r.IsBusy = new bool[7, 13];
            foreach (var t in DataManager.Teachers) t.IsBusy = new bool[7, 13];
            foreach (var s in DataManager.Sections) s.IsBusy = new bool[7, 13];

            // Re-mark busy slots
            foreach (var slot in DataManager.MasterSchedule)
            {
                var room = DataManager.Rooms.FirstOrDefault(r => r.Name == slot.Room);
                var teacher = DataManager.Teachers.FirstOrDefault(t => t.Name == slot.Teacher);
                var section = DataManager.Sections.FirstOrDefault(s => s.Name == slot.Section);

                if (room != null && teacher != null && section != null)
                {
                    room.IsBusy[slot.DayIndex, slot.TimeIndex] = true;
                    teacher.IsBusy[slot.DayIndex, slot.TimeIndex] = true;
                    section.IsBusy[slot.DayIndex, slot.TimeIndex] = true;
                    slot.RoomObj = room; // Re-link object
                }
            }
        }

        #endregion

        #region 3. Manual Scheduling Operations

        public void UnassignSubject(ScheduleItem item)
        {
            bool isLabTarget = item.Subject.Contains("(Lab)");

            // Find all related chunks (e.g. 3 hour block)
            var relatedItems = DataManager.MasterSchedule
                .Where(s => s.Section == item.Section &&
                            s.Subject.Contains("(Lab)") == isLabTarget &&
                            CleanSubjectName(s.Subject) == CleanSubjectName(item.Subject))
                .ToList();

            if (relatedItems.Count == 0) return;

            // Remove from schedule
            DataManager.MasterSchedule.RemoveAll(x => relatedItems.Contains(x));

            // Create Pending Entry
            var sec = DataManager.Sections.First(x => x.Name == item.Section);
            var sub = sec.SubjectsToTake.First(x => CleanSubjectName(x.Code) == CleanSubjectName(item.Subject) && x.IsLab == isLabTarget);

            if (!DataManager.FailedAssignments.Any(f => f.Section == sec && f.Subject == sub))
            {
                DataManager.FailedAssignments.Add(new FailedEntry
                {
                    Section = sec,
                    Subject = sub,
                    Reason = "Manually Unassigned"
                });
            }

            RebuildBusyArrays();
        }

        public bool PlaceBlockManual(FailedEntry fail, int day, int startInfo, string roomName)
        {
            int duration = fail.Subject.Units;
            var teacher = DataManager.Teachers.FirstOrDefault(t => t.QualifiedSubjects.Contains(CleanSubjectName(fail.Subject.Code)));
            var room = DataManager.Rooms.First(r => r.Name == roomName);

            if (teacher == null) return false;

            // Check conflicts
            List<ScheduleItem> obstacles = new List<ScheduleItem>();
            for (int i = 0; i < duration; i++)
            {
                int t = startInfo + i;
                if (t > 12) return false;

                // Check Teacher Availability
                if (teacher.IsBusy[day, t])
                {
                    bool busyWithOthers = DataManager.MasterSchedule.Any(s =>
                        s.Teacher == teacher.Name && s.DayIndex == day && s.TimeIndex == t &&
                        s.Section != fail.Section.Name);
                    if (busyWithOthers) return false;
                }

                // Check Room/Section Obstacles
                var existing = DataManager.MasterSchedule.FirstOrDefault(s =>
                    s.DayIndex == day && s.TimeIndex == t &&
                    (s.Room == roomName || s.Section == fail.Section.Name));

                if (existing != null) obstacles.Add(existing);
            }

            // Clear obstacles
            foreach (var obstacle in obstacles)
            {
                if (DataManager.MasterSchedule.Contains(obstacle)) UnassignSubject(obstacle);
            }

            // Commit new block
            for (int i = 0; i < duration; i++)
            {
                int t = startInfo + i;
                ScheduleItem newItem = new ScheduleItem
                {
                    Section = fail.Section.Name,
                    Subject = fail.Subject.Code,
                    Teacher = teacher.Name,
                    Room = room.Name,
                    Day = GetDayName(day),
                    Time = GetTimeLabel(t),
                    DayIndex = day,
                    TimeIndex = t,
                    RoomObj = room
                };
                DataManager.MasterSchedule.Add(newItem);
            }

            if (DataManager.FailedAssignments.Contains(fail))
                DataManager.FailedAssignments.Remove(fail);

            RebuildBusyArrays();
            return true;
        }

        public bool PerformSwap(ScheduleItem oldClass, FailedEntry newClass)
        {
            // Capture old location before deleting
            int d = oldClass.DayIndex;
            int t = oldClass.TimeIndex;
            string r = oldClass.Room;

            UnassignSubject(oldClass);

            // Try to place new one in same spot
            bool success = PlaceBlockManual(newClass, d, t, r);
            return success;
        }

        #endregion

        #region 4. Helpers

        private string CleanSubjectName(string s) => s.Replace(" (Lec)", "").Replace(" (Lab)", "").Trim();
        private string GetDayName(int d) => (d >= 1 && d <= 7) ? new[] { "", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" }[d] : "Err";
        private string GetTimeLabel(int t) => $"{7 + t}:00 - {8 + t}:00";

        #endregion
    }
}