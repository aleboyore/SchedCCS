using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SchedCCS
{
    #region 1. Foundation Interfaces & Enums

    /// <summary>
    /// Contract for entities requiring unique identification and basic detail reporting.
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
    /// Abstract representation of a person within the system.
    /// </summary>
    public abstract class Person : IIdentifiable
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public abstract string GetDetails();
    }

    /// <summary>
    /// Represents faculty members, including subject specializations and availability.
    /// </summary>
    public class Teacher : Person
    {
        public List<string> QualifiedSubjects { get; set; }

        /// <summary>
        /// Availability Matrix: [7 Days, 13 Hours] representing 7:00 AM to 7:00 PM.
        /// Excluded from JSON serialization to optimize storage.
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
    /// Represents a physical classroom or laboratory resource.
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
    /// Represents a student section, including its assigned curriculum and occupied slots.
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
    /// Defines a course subject, its credit units, and facility requirements.
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
    /// Flat structure optimized for UI display, database persistence, and exports.
    /// </summary>
    public class ScheduleItem
    {
        public string Section { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Teacher { get; set; } = string.Empty;
        public string Room { get; set; } = string.Empty;
        public string Day { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;

        public int DayIndex { get; set; }
        public int TimeIndex { get; set; }

        /// <summary>
        /// Associated Room object for conflict validation.
        /// </summary>
        public Room? RoomObj { get; set; }
    }

    /// <summary>
    /// Represents a subject assignment that failed validation during schedule generation.
    /// </summary>
    public class FailedEntry
    {
        public Section Section { get; set; } = new Section();
        public Subject Subject { get; set; } = new Subject();
        public string Reason { get; set; } = string.Empty;
    }

    #endregion
}