using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SchedCCS
{
    #region 1. Foundation Interfaces & Enums

    /// <summary>
    /// Defines a standardized contract for identifiable entities within the scheduling system.
    /// </summary>
    public interface IIdentifiable
    {
        int Id { get; set; }
        string Name { get; set; }
        string GetDetails();
    }

    public enum Day { Mon, Tue, Wed, Thu, Fri, Sat, Sun }

    public enum RoomType { Lecture, Laboratory }

    #endregion

    #region 2. Base & Person Entities

    /// <summary>
    /// Abstract base class providing common identity properties for all people in the system.
    /// </summary>
    public abstract class Person : IIdentifiable
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public abstract string GetDetails();
    }

    /// <summary>
    /// Represents a faculty member including their subject qualifications and schedule availability.
    /// </summary>
    public class Teacher : Person
    {
        public List<string> QualifiedSubjects { get; set; }

        /// <summary>
        /// Availability Matrix: [7 Days, 13 Hours] (7:00 AM to 7:00 PM).
        /// Ignored during JSON backup to save space.
        /// </summary>
        [JsonIgnore]
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

    #region 3. Infrastructure & Academic Entities

    /// <summary>
    /// Represents a physical classroom or laboratory within the campus.
    /// </summary>
    public class Room : IIdentifiable
    {
        private string _name = string.Empty;

        public int Id { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Room name cannot be empty");
                _name = value;
            }
        }

        public RoomType Type { get; set; }

        /// <summary>
        /// Availability Matrix: [7 Days, 13 Hours].
        /// </summary>
        [JsonIgnore]
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

    /// <summary>
    /// Represents a specific class group or student section.
    /// </summary>
    public class Section : IIdentifiable
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int YearLevel { get; set; }
        public string Program { get; set; } = string.Empty;

        public List<Subject> SubjectsToTake { get; set; }

        /// <summary>
        /// Availability Matrix: [7 Days, 13 Hours].
        /// </summary>
        [JsonIgnore]
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

    /// <summary>
    /// Represents a course or subject within a curriculum.
    /// </summary>
    public class Subject
    {
        public string Code { get; set; } = string.Empty;
        public int Units { get; set; }
        public bool IsLab { get; set; }
    }

    #endregion

    #region 4. Data Transfer Objects (DTOs)

    /// <summary>
    /// Optimized flat structure for UI grid displays, database persistence, and exports.
    /// </summary>
    public class ScheduleItem
    {
        public string Section { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Teacher { get; set; } = string.Empty;
        public string Room { get; set; } = string.Empty;
        public string Day { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;

        // Logical indices for schedule positioning
        public int DayIndex { get; set; }
        public int TimeIndex { get; set; }

        /// <summary>
        /// Reference to the actual Room object for conflict checking.
        /// </summary>
        public Room? RoomObj { get; set; }
    }

    /// <summary>
    /// Encapsulates details regarding subjects that could not be assigned a valid schedule slot.
    /// </summary>
    public class FailedEntry
    {
        public Section Section { get; set; } = new Section();
        public Subject Subject { get; set; } = new Subject();
        public string Reason { get; set; } = string.Empty;
    }

    #endregion
}