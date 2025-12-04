using System;
using System.Collections.Generic;
using System.Linq;

namespace SchedCCS
{
    // The core scheduling engine responsible for assigning time slots.
    // [OOP Concept: Encapsulation of Logic]
    public class ScheduleGenerator
    {
        // Private fields encapsulated to prevent external modification.
        private List<Room> rooms;
        private List<Teacher> teachers;
        private List<Section> sections;

        // Public properties expose the results safely.
        public List<ScheduleItem> GeneratedSchedule { get; private set; } = new List<ScheduleItem>();
        public List<string> FailedAssignments { get; private set; } = new List<string>();

        // Constructor for dependency injection of data.
        public ScheduleGenerator(List<Room> r, List<Teacher> t, List<Section> s)
        {
            rooms = r;
            teachers = t;
            sections = s;
        }

        // =============================================================
        // MAIN ALGORITHM
        // =============================================================

        // Executes the scheduling process using a Greedy Algorithm with Constraints.
        public void Generate()
        {
            GeneratedSchedule.Clear();
            FailedAssignments.Clear();

            foreach (var section in sections)
            {
                foreach (var subject in section.SubjectsToTake)
                {
                    bool success = false;

                    if (subject.IsLab)
                    {
                        // Strategy A: Schedule Laboratory (Block)
                        success = AssignLabSlot(section, subject);
                        if (!success)
                            FailedAssignments.Add($"{section.Name}: {subject.Code} (Lab Block Conflict)");
                    }
                    else
                    {
                        // Strategy B: Schedule Lecture (Distributed Hours)
                        int hoursNeeded = subject.Units;
                        for (int i = 0; i < hoursNeeded; i++)
                        {
                            success = AssignLectureSlot(section, subject);
                            if (!success)
                                FailedAssignments.Add($"{section.Name}: {subject.Code} (Lec Hour Conflict)");
                        }
                    }
                }
            }
        }

        // =============================================================
        // ASSIGNMENT STRATEGIES
        // =============================================================

        // Logic for assigning contiguous laboratory blocks (e.g., 3 hours straight).
        private bool AssignLabSlot(Section section, Subject subject)
        {
            // Randomize days (Mon-Sat) for distribution variation.
            var preferredDays = new List<int> { 0, 1, 2, 3, 4, 5 }
                                .OrderBy(x => Guid.NewGuid()).ToList();

            var randomRoomOrder = rooms.OrderBy(x => Guid.NewGuid()).ToList();

            foreach (int dayIndex in preferredDays)
            {
                int duration = subject.Units;

                // Constraint: Must finish by 6:00 PM (Index 11).
                for (int startHour = 0; startHour <= 11 - duration; startHour++)
                {
                    if (IsBlockAvailable(section, dayIndex, startHour, duration, randomRoomOrder, subject.Code, out Room foundRoom, out Teacher foundTeacher))
                    {
                        BookSlot(section, subject, dayIndex, startHour, duration, foundRoom, foundTeacher);
                        return true;
                    }
                }
            }
            return false;
        }

        // Logic for assigning single lecture hours with fatigue and distribution checks.
        private bool AssignLectureSlot(Section section, Subject subject)
        {
            // Lectures prefer Mon-Fri, Saturday is fallback.
            var weekdays = new List<int> { 0, 1, 2, 3, 4 }.OrderBy(x => Guid.NewGuid()).ToList();
            weekdays.Add(5); // Saturday

            var randomRoomOrder = rooms.OrderBy(x => Guid.NewGuid()).ToList();

            foreach (int dayIndex in weekdays)
            {
                // Constraint: Max 1 hour of this subject per day.
                int hoursToday = GeneratedSchedule.Count(x => x.Section == section.Name && x.Subject == subject.Code && x.Day == ((Day)dayIndex).ToString());
                if (hoursToday >= 1) continue;

                // Variation: Random start time to avoid always filling 7am first.
                Random rnd = new Random();
                int startOffset = (rnd.Next(1, 10) > 6) ? 2 : 0;

                for (int hourIndex = startOffset; hourIndex < 11; hourIndex++)
                {
                    // Constraints: No Sat afternoon, No Lunch (12pm), No Fatigue.
                    if (dayIndex == 5 && hourIndex > 6) continue;
                    if (hourIndex == 5) continue;
                    if (section.IsBusy[dayIndex, hourIndex]) continue;
                    if (IsSectionFatigued(section, dayIndex, hourIndex)) continue;

                    // Resource Finding
                    var availableRoom = randomRoomOrder.FirstOrDefault(r =>
                        r.Type == RoomType.Lecture && !r.IsBusy[dayIndex, hourIndex]);

                    var availableTeacher = teachers.FirstOrDefault(t =>
                        (t.QualifiedSubjects.Contains(subject.Code) || t.QualifiedSubjects.Contains(CleanSubjectName(subject.Code))) &&
                        !t.IsBusy[dayIndex, hourIndex]);

                    // Constraint: Teacher Vacant Gap (prevent back-to-back burnout).
                    if (availableTeacher != null && hourIndex > 0)
                    {
                        if (availableTeacher.IsBusy[dayIndex, hourIndex - 1]) continue;
                    }

                    BookSlot(section, subject, dayIndex, hourIndex, 1, availableRoom, availableTeacher);
                    return true;
                }
            }
            return false;
        }

        // =============================================================
        // HELPERS & VALIDATION
        // =============================================================

        private void BookSlot(Section section, Subject subject, int day, int startHour, int duration, Room room, Teacher teacher)
        {
            string tName = teacher?.Name ?? "Professor XYZ"; // Fallback for nulls
            string rName = room?.Name ?? "Room TBA";

            for (int i = 0; i < duration; i++)
            {
                int currentHour = startHour + i;

                // Update State (Encapsulation of State)
                section.IsBusy[day, currentHour] = true;
                if (room != null) room.IsBusy[day, currentHour] = true;
                if (teacher != null) teacher.IsBusy[day, currentHour] = true;

                GeneratedSchedule.Add(new ScheduleItem
                {
                    Section = section.Name,
                    Subject = subject.Code,
                    Teacher = tName,
                    Room = rName,
                    Day = ((Day)day).ToString(),
                    Time = $"{7 + currentHour}:00 - {8 + currentHour}:00"
                });
            }
        }

        private bool IsBlockAvailable(Section section, int day, int startHour, int duration, List<Room> shuffledRooms, string subjectCode, out Room foundRoom, out Teacher foundTeacher)
        {
            foundRoom = null;
            foundTeacher = null;

            // Check Section Availability
            for (int i = 0; i < duration; i++)
            {
                if (startHour + i >= 13) return false;
                if (section.IsBusy[day, startHour + i]) return false;
            }

            // Find Room
            foreach (var room in shuffledRooms.Where(r => r.Type == RoomType.Laboratory))
            {
                bool roomIsGood = true;
                for (int i = 0; i < duration; i++)
                {
                    if (room.IsBusy[day, startHour + i]) { roomIsGood = false; break; }
                }
                if (roomIsGood) { foundRoom = room; break; }
            }

            // Find Teacher
            foreach (var teacher in teachers)
            {
                if (teacher.QualifiedSubjects.Contains(subjectCode) || teacher.QualifiedSubjects.Contains(CleanSubjectName(subjectCode)))
                {
                    bool teacherIsGood = true;
                    for (int i = 0; i < duration; i++)
                    {
                        if (teacher.IsBusy[day, startHour + i]) { teacherIsGood = false; break; }
                    }

                    // Check Teacher Gap (Prevent continuous load)
                    if (teacherIsGood && startHour > 0)
                    {
                        if (teacher.IsBusy[day, startHour - 1]) teacherIsGood = false;
                    }

                    if (teacherIsGood) { foundTeacher = teacher; break; }
                }
            }
            return true;
        }

        private bool IsSectionFatigued(Section section, int day, int currentHour)
        {
            // Rule: Students need a break after 3 consecutive hours.
            if (currentHour < 3) return false;

            if (section.IsBusy[day, currentHour - 1] &&
                section.IsBusy[day, currentHour - 2] &&
                section.IsBusy[day, currentHour - 3])
            {
                return true;
            }
            return false;
        }

        private string CleanSubjectName(string rawName)
        {
            return rawName.Replace(" (Lec)", "").Replace(" (Lab)", "").Trim();
        }
    }
}