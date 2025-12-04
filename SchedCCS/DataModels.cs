using System;
using System.Collections.Generic;

namespace SchedCCS
{
    #region 1. Interfaces & Enums (Abstraction & Organization)

    // Interface enforcing specific properties and behavior on implementing classes.
    // [OOP Concept: Interface]
    public interface IIdentifiable
    {
        int Id { get; set; }
        string Name { get; set; }
        string GetDetails(); // Polymorphic method definition
    }

    // Enumerations for defining fixed sets of constants.
    public enum Day { Mon, Tue, Wed, Thu, Fri, Sat, Sun }
    public enum RoomType { Lecture, Laboratory }

    #endregion

    #region 2. Base Classes (Abstraction)

    // Abstract base class representing a generic person.
    // Cannot be instantiated directly.
    // [OOP Concept: Abstraction]
    public abstract class Person : IIdentifiable
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Abstract method forcing implementation in derived classes.
        // [OOP Concept: Abstract Method]
        public abstract string GetDetails();
    }

    #endregion

    #region 3. Derived Classes (Inheritance & Polymorphism)

    // Represents a faculty member. Inherits from Person.
    // [OOP Concept: Inheritance]
    public class Teacher : Person
    {
        // Encapsulated list of subjects the teacher is qualified to teach.
        // [OOP Concept: Encapsulation]
        public List<string> QualifiedSubjects { get; set; }

        // 2D Array representing the teacher's weekly schedule availability.
        public bool[,] IsBusy { get; set; }

        // Constructor initializes lists and arrays to prevent null errors.
        // [OOP Concept: Constructors]
        public Teacher()
        {
            QualifiedSubjects = new List<string>();
            IsBusy = new bool[6, 13];
        }

        // Overrides the base method to provide specific teacher details.
        // [OOP Concept: Polymorphism / Overriding]
        public override string GetDetails()
        {
            return $"Faculty: {Name} (Qualified for {QualifiedSubjects.Count} subjects)";
        }
    }

    #endregion

    #region 4. Resource Classes (Encapsulation & Composition)

    // Represents a physical room resource. Implements IIdentifiable.
    public class Room : IIdentifiable
    {
        // Private backing field for encapsulation.
        private string _name = string.Empty;

        public int Id { get; set; }

        // Property with validation logic in the setter.
        // [OOP Concept: Encapsulation with Logic]
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
        public bool[,] IsBusy { get; set; }

        public Room()
        {
            IsBusy = new bool[6, 13];
        }

        public string GetDetails()
        {
            return $"Room: {Name} ({Type})";
        }
    }

    // Represents a class section.
    public class Section : IIdentifiable
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // --- NEW PROPERTIES ---
        public int YearLevel { get; set; } // e.g., 1, 2, 3, 4
        public string Program { get; set; } // e.g., "BSCS", "BSIT"

        public List<Subject> SubjectsToTake { get; set; } = new List<Subject>();
        public bool[,] IsBusy { get; set; } = new bool[6, 13];

        public string GetDetails()
        {
            return $"[{Program}-{YearLevel}] {Name} - {SubjectsToTake.Count} Subjects";
        }
    }

    // Represents a specific course/subject.
    public class Subject
    {
        public string Code { get; set; } = string.Empty;
        public int Units { get; set; }
        public bool IsLab { get; set; }
    }

    #endregion

    #region 5. Data Transfer Objects (DTOs)

    // Helper class used for displaying data in Grids and exporting to CSV/PDF.
    public class ScheduleItem
    {
        // Nullable strings allow for flexible data handling without crashes.
        public string? Section { get; set; }
        public string? Subject { get; set; }
        public string? Teacher { get; set; }
        public string? Room { get; set; }
        public string? Day { get; set; }
        public string? Time { get; set; }
    }

    #endregion
}