using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedCCS
{
    /// <summary>
    /// Core logic service for the application. 
    /// Manages automated generation, data synchronization, and manual scheduling overrides.
    /// </summary>
    public class ScheduleService
    {
        #region 1. Schedule Generation

        /// <summary>
        /// Executes the automated schedule generation algorithm.
        /// Performs multiple attempts to find the configuration with the fewest conflicts.
        /// </summary>
        /// <returns>A string indicating success or the number of remaining conflicts.</returns>
        public string GenerateSchedule()
        {
            int lowestConflictCount = int.MaxValue;
            List<ScheduleItem> bestSchedule = new List<ScheduleItem>();
            List<FailedEntry> bestFailures = new List<FailedEntry>();

            // Establish current data as the baseline if it exists
            if (DataManager.MasterSchedule.Count > 0)
            {
                lowestConflictCount = DataManager.FailedAssignments.Count;
                bestSchedule = new List<ScheduleItem>(DataManager.MasterSchedule);
                bestFailures = new List<FailedEntry>(DataManager.FailedAssignments);
            }

            // Execute 50 iterations to optimize the schedule configuration
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

                // Terminate search if a perfect schedule is found
                if (score == 0) break;
            }

            // Commit optimized results to the global data manager
            DataManager.MasterSchedule = bestSchedule;
            DataManager.FailedAssignments = bestFailures;

            RebuildBusyArrays();

            return lowestConflictCount == 0 ? "Success" : $"Generated with {lowestConflictCount} conflicts.";
        }

        /// <summary>
        /// Asynchronous wrapper for schedule generation to maintain UI responsiveness.
        /// </summary>
        public async Task<string> GenerateScheduleAsync()
        {
            return await Task.Run(() => GenerateSchedule());
        }

        #endregion

        #region 2. Data Synchronization

        /// <summary>
        /// Synchronizes the "IsBusy" status across all Rooms, Teachers, and Sections 
        /// based on the current state of the MasterSchedule.
        /// </summary>
        public void RebuildBusyArrays()
        {
            // Reset all availability matrices
            foreach (var r in DataManager.Rooms) r.IsBusy = new bool[7, 13];
            foreach (var t in DataManager.Teachers) t.IsBusy = new bool[7, 13];
            foreach (var s in DataManager.Sections) s.IsBusy = new bool[7, 13];

            // Map schedule items back to resource availability
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
                    slot.RoomObj = room;
                }
            }
        }

        #endregion

        #region 3. Manual Scheduling Operations

        /// <summary>
        /// Removes a specific subject block from the schedule and moves it to the pending list.
        /// </summary>
        public void UnassignSubject(ScheduleItem item)
        {
            bool isLabTarget = item.Subject.Contains("(Lab)");

            // Identify all related time blocks for the subject
            var relatedItems = DataManager.MasterSchedule
                .Where(s => s.Section == item.Section &&
                            s.Subject.Contains("(Lab)") == isLabTarget &&
                            CleanSubjectName(s.Subject) == CleanSubjectName(item.Subject))
                .ToList();

            if (relatedItems.Count == 0) return;

            DataManager.MasterSchedule.RemoveAll(x => relatedItems.Contains(x));

            var sec = DataManager.Sections.First(x => x.Name == item.Section);
            var sub = sec.SubjectsToTake.FirstOrDefault(x => CleanSubjectName(x.Code) == CleanSubjectName(item.Subject) && x.IsLab == isLabTarget);

            // Temporary subject reconstruction if missing from repository
            if (sub == null)
            {
                sub = new Subject { Code = item.Subject, IsLab = isLabTarget, Units = relatedItems.Count };
            }

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

        /// <summary>
        /// Attempts to manually place a subject into a specific room and time slot.
        /// Enforces institutional rules regarding sports facilities and academic subjects.
        /// </summary>
        public bool PlaceBlockManual(FailedEntry fail, int day, int startInfo, string roomName)
        {
            int duration = fail.Subject.Units;
            var teacher = DataManager.Teachers.FirstOrDefault(t => t.QualifiedSubjects.Contains(CleanSubjectName(fail.Subject.Code)));
            var room = DataManager.Rooms.First(r => r.Name == roomName);

            if (teacher == null) return false;

            // Restrict non-sport subjects from being scheduled in outdoor or gym facilities
            if (IsOutdoorRoom(roomName) && !IsSportSubject(fail.Subject.Code))
            {
                return false;
            }

            // Conflict detection for the requested time block
            List<ScheduleItem> obstacles = new List<ScheduleItem>();
            for (int i = 0; i < duration; i++)
            {
                int t = startInfo + i;
                if (t > 12) return false;

                if (teacher.IsBusy[day, t])
                {
                    bool busyWithOthers = DataManager.MasterSchedule.Any(s =>
                        s.Teacher == teacher.Name && s.DayIndex == day && s.TimeIndex == t &&
                        s.Section != fail.Section.Name);
                    if (busyWithOthers) return false;
                }

                var existing = DataManager.MasterSchedule.FirstOrDefault(s =>
                    s.DayIndex == day && s.TimeIndex == t &&
                    (s.Room == roomName || s.Section == fail.Section.Name));

                if (existing != null) obstacles.Add(existing);
            }

            // Resolve conflicts by unassigning existing obstacles
            foreach (var obstacle in obstacles)
            {
                if (DataManager.MasterSchedule.Contains(obstacle)) UnassignSubject(obstacle);
            }

            // Finalize new schedule entries
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

        /// <summary>
        /// Swaps an existing class with a pending one at the same time and location.
        /// </summary>
        public bool PerformSwap(ScheduleItem oldClass, FailedEntry newClass)
        {
            int d = oldClass.DayIndex;
            int t = oldClass.TimeIndex;
            string r = oldClass.Room;

            UnassignSubject(oldClass);
            return PlaceBlockManual(newClass, d, t, r);
        }

        #endregion

        #region 4. Internal Helpers

        private string CleanSubjectName(string s) => s.Replace(" (Lec)", "").Replace(" (Lab)", "").Trim();

        private string GetDayName(int d) => (d >= 1 && d <= 7)
            ? new[] { "", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" }[d]
            : "Err";

        private string GetTimeLabel(int t) => $"{7 + t}:00 - {8 + t}:00";

        private bool IsSportSubject(string subjectCode)
        {
            string code = subjectCode.ToUpper();
            return code.Contains("PE") || code.Contains("PATHFIT") ||
                   code.Contains("NSTP") || code.Contains("GYM");
        }

        private bool IsOutdoorRoom(string roomName)
        {
            string name = roomName.ToUpper();
            return name.Contains("FIELD") || name.Contains("GYM") || name.Contains("COURT");
        }

        #endregion
    }
}