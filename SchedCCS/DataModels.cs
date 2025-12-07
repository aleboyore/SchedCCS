using System;
using System.Collections.Generic;

namespace SchedCCS
{
    #region 1. Interfaces & Enums

    // Defines a contract for objects that have an ID and Name.
    public interface IIdentifiable
    {
        int Id { get; set; }
        string Name { get; set; }
        string GetDetails();
    }

    public enum Day { Mon, Tue, Wed, Thu, Fri, Sat, Sun }

    public enum RoomType { Lecture, Laboratory }

    #endregion

    #region 2. Base Classes

    // Base class for any person entity in the system.
    public abstract class Person : IIdentifiable
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public abstract string GetDetails();
    }

    #endregion

    #region 3. Derived Entities

    // Represents a faculty member with specific subject qualifications.
    public class Teacher : Person
    {
        public List<string> QualifiedSubjects { get; set; }

        // Availability Matrix: [7 Days, 13 Hours] (Mon-Sun, 7am-7pm)
        public bool[,] IsBusy { get; set; }

        public Teacher()
        {
            QualifiedSubjects = new List<string>();
            IsBusy = new bool[7, 13];
        }

        public override string GetDetails()
        {
            return $"Faculty: {Name} (Qualified for {QualifiedSubjects.Count} subjects)";
        }
    }

    #endregion

    #region 4. Resource & Structure Classes

    // Represents a physical room within the facility.
    public class Room : IIdentifiable
    {
        private string _name = string.Empty;

        public int Id { get; set; }

        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Room name cannot be empty");
                _name = value;
            }
        }

        public RoomType Type { get; set; }

        // Availability Matrix: [7 Days, 13 Hours]
        public bool[,] IsBusy { get; set; }

        public Room()
        {
            IsBusy = new bool[7, 13];
        }

        public string GetDetails()
        {
            return $"Room: {Name} ({Type})";
        }
    }

    // Represents a student section (class group).
    public class Section : IIdentifiable
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int YearLevel { get; set; }
        public string Program { get; set; } = string.Empty;

        public List<Subject> SubjectsToTake { get; set; }

        // Availability Matrix: [7 Days, 13 Hours]
        public bool[,] IsBusy { get; set; }

        public Section()
        {
            SubjectsToTake = new List<Subject>();
            IsBusy = new bool[7, 13];
        }

        public string GetDetails()
        {
            return $"[{Program}-{YearLevel}] {Name} - {SubjectsToTake.Count} Subjects assigned";
        }
    }

    // Represents a specific subject or course.
    public class Subject
    {
        public string Code { get; set; } = string.Empty;
        public int Units { get; set; }
        public bool IsLab { get; set; }
    }

    #endregion

    #region 5. Data Transfer Objects (DTOs)

    // Flat structure for grid display and CSV export.
    public class ScheduleItem
    {
        public string? Section { get; set; }
        public string? Subject { get; set; }
        public string? Teacher { get; set; }
        public string? Room { get; set; }
        public string? Day { get; set; }
        public string? Time { get; set; }
    }

    // Tracks assignment failures for the "Pending" list.
    public class FailedEntry
    {
        public Section Section { get; set; }
        public Subject Subject { get; set; }
        public string Reason { get; set; }
    }

    #endregion
}