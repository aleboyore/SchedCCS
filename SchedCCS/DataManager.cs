using System;
using System.Collections.Generic;
using System.Linq;

namespace SchedCCS
{
    public class User
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = "Student";
        public string? StudentSection { get; set; }
    }

    public static class DataManager
    {
        #region 1. Data Repositories

        public static List<Room> Rooms { get; set; } = new List<Room>();
        public static List<Teacher> Teachers { get; set; } = new List<Teacher>();
        public static List<Section> Sections { get; set; } = new List<Section>();
        public static List<User> Users { get; set; } = new List<User>();
        public static List<ScheduleItem> MasterSchedule { get; set; } = new List<ScheduleItem>();
        public static List<FailedEntry> FailedAssignments { get; set; } = new List<FailedEntry>();

        public static readonly List<string> Programs = new List<string>
        {
            "BSCS", "BSINFO", "GAV", "IS", "SMP", "WMAD", "NA"
        };

        #endregion

        #region 2. Initialization

        public static void Initialize()
        {
            Rooms.Clear();
            Teachers.Clear();
            Sections.Clear();
            Users.Clear();

            LoadUsers();
            LoadRooms();
            LoadTeachers();
            LoadSections();
        }

        #endregion

        #region 3. Data Loaders

        private static void LoadUsers()
        {
            // Default Admin
            Users.Add(new User
            {
                Username = "admin",
                Password = "123",
                Role = "Admin",
                FullName = "System Admin"
            });

            // Student Accounts
            Users.Add(new User
            {
                Username = "0324-2227",
                Password = "123",
                Role = "Student",
                StudentSection = "CS 2A",
                FullName = "Alezer Boyore"
            });

            Users.Add(new User
            {
                Username = "student",
                Password = "123",
                Role = "Student",
                StudentSection = "CS 2A",
                FullName = "Student User"
            });
        }

        private static void LoadRooms()
        {
            // Building A (Lecture)
            for (int i = 1; i <= 15; i++)
                Rooms.Add(new Room { Id = i, Name = $"LEC {i}", Type = RoomType.Lecture });

            // Building B (Laboratory)
            for (int i = 1; i <= 10; i++)
                Rooms.Add(new Room { Id = 20 + i, Name = $"LAB {i}", Type = RoomType.Laboratory });
        }

        private static void LoadTeachers()
        {
            AddTeacher(1, "Mr. Desamero", "ITEC 102");
            AddTeacher(2, "Mr. Funtila", "GEC 103");
            AddTeacher(3, "Ms. De Torres", "ITEC 101", "ITEL 415", "ITEP 415");
            AddTeacher(4, "Ms. Cabreza", "GEC 102");
            AddTeacher(5, "Ms. Parica", "PE 1");
            AddTeacher(6, "Mr. Calosa", "GEC 101");
            AddTeacher(7, "Ms. Balinsayo", "KOMFIL");
            AddTeacher(9, "Mr. Dorado", "ITEC 102", "ITEL 201", "CMSC 203", "ITST 301", "GEC 104");
            AddTeacher(10, "Ms. Arida", "ITEC 101", "ITEC 104", "CMSC 307", "CSST 101");
            AddTeacher(11, "Mr. Sael", "PE 1");
            AddTeacher(12, "Mr. Agravante", "PI 100");
            AddTeacher(13, "Ms. Encanto", "ITEC 204", "ITST 306");
            AddTeacher(14, "Ms. Escote", "ITEL 201", "ITEL 203", "ITEP 203");
            AddTeacher(15, "Ms. Basaca", "GEC 107");
            AddTeacher(16, "Ms. Del Valle", "SOSLIT");
            AddTeacher(17, "Mr. Dungo", "ITEL 202");
            AddTeacher(18, "Mr. Moreno", "PE 3");
            AddTeacher(19, "Ms. Cabuyao", "CMSC 202", "CMSC 306", "ITST 306", "CSST 106");
            AddTeacher(20, "Mr. Bombio", "MATH 24");
            AddTeacher(21, "Mr. Lara", "ITEC 205", "CSST 102", "CSST 106");
            AddTeacher(22, "Mr. Buna", "GEC 106");
            AddTeacher(23, "Mr. Manaloto", "ITEP 308");
            AddTeacher(24, "Mr. Manzanero", "ITEP 310", "ITST 302", "ITST 306");
            AddTeacher(25, "Mr. Del Rosario", "ITEL 304");
            AddTeacher(26, "Ms. Viojan", "ITST 302", "ITEP 309");
            AddTeacher(27, "Ms. Espinueva", "ITEP 310", "ITST 301");
            AddTeacher(28, "Mr. Andres", "ITEL 304", "ITEL 202");
            AddTeacher(29, "Mr. Ual", "ITEL 308", "ITEP 415", "ITEL 414", "ITEP 308", "ITEP 414");
            AddTeacher(30, "Engr. De Luna", "ITST 302", "ITST 301");
            AddTeacher(31, "Mr. Ilagan", "CMSC 310");
            AddTeacher(32, "Ms. Castillo", "CMSC 309");
            AddTeacher(33, "Mr. Villareal", "CSST 102", "CMSC 502");
            AddTeacher(34, "Ms. Ompangco", "CMSC 305", "CMSC 308");
            AddTeacher(35, "Mr. Amora", "CSST 101", "ITST 306", "ITEC 205");
            AddTeacher(36, "Engr. Santos", "ITEL 413", "ITEP 413");
            AddTeacher(37, "Mr. Dela Cruz", "ITEP 415", "ITST 306");
        }

        private static void LoadSections()
        {
            // 1st Year BSINFO
            var info1Subjs = new[] {
                ("ITEC 102", "Mr. Desamero"), ("GEC 103", "Mr. Funtila"), ("ITEC 101", "Ms. De Torres"),
                ("GEC 102", "Ms. Cabreza"), ("PE 1", "Ms. Parica"), ("GEC 101", "Mr. Calosa"),
                ("KOMFIL", "Ms. Balinsayo"), ("GEC 104", "Mr. Dorado")
            };
            CreateSection("INFO 1A", "BSINFO", 1, info1Subjs);
            CreateSection("INFO 1B", "BSINFO", 1, info1Subjs);
            CreateSection("INFO 1C", "BSINFO", 1, info1Subjs);
            CreateSection("INFO 1D", "BSINFO", 1, info1Subjs);

            // 1st Year BSCS
            var cs1Subjs = new[] {
                ("ITEC 102", "Mr. Dorado"), ("GEC 103", "Mr. Funtila"), ("ITEC 101", "Ms. Arida"),
                ("GEC 102", "Ms. Cabreza"), ("PE 1", "Mr. Sael"), ("GEC 101", "Mr. Calosa"),
                ("KOMFIL", "Ms. Balinsayo"), ("PI 100", "Mr. Agravante")
            };
            CreateSection("CS 1A", "BSCS", 1, cs1Subjs);
            CreateSection("CS 1B", "BSCS", 1, cs1Subjs);

            // 2nd Year BSINFO
            var info2Subjs = new[] {
                ("ITEC 204", "Ms. Encanto"), ("ITEL 201", "Mr. Dorado"), ("ITEP 203", "Ms. Escote"),
                ("ITEC 205", "Mr. Amora"), ("GEC 107", "Ms. Basaca"), ("SOSLIT", "Ms. Del Valle"),
                ("ITEL 202", "Mr. Andres"), ("PE 3", "Mr. Moreno")
            };
            CreateSection("INFO 2A", "BSINFO", 2, info2Subjs);
            CreateSection("INFO 2B", "BSINFO", 2, info2Subjs);

            // 2nd Year BSINFO Variations
            CreateSection("INFO 2C", "BSINFO", 2, new[] {
                ("ITEC 204", "Ms. Encanto"), ("ITEL 201", "Ms. Escote"), ("ITEP 203", "Ms. Escote"),
                ("ITEC 205", "Mr. Amora"), ("GEC 107", "Ms. Basaca"), ("SOSLIT", "Ms. Del Valle"),
                ("ITEL 202", "Mr. Dungo"), ("PE 3", "Mr. Moreno")
            });
            CreateSection("INFO 2D", "BSINFO", 2, new[] {
                ("ITEC 204", "Ms. Encanto"), ("ITEL 201", "Ms. Escote"), ("ITEP 203", "Ms. Escote"),
                ("ITEC 205", "Mr. Amora"), ("GEC 107", "Ms. Basaca"), ("SOSLIT", "Ms. Del Valle"),
                ("ITEL 202", "Mr. Dungo"), ("PE 3", "Mr. Moreno")
            });

            // 2nd Year BSCS
            var cs2Subjs = new[] {
                ("CMSC 202", "Ms. Cabuyao"), ("MATH 24", "Mr. Bombio"), ("ITEC 205", "Mr. Lara"),
                ("PE 3", "Mr. Moreno"), ("ITEC 104", "Ms. Arida"), ("SOSLIT", "Ms. Del Valle"),
                ("GEC 106", "Mr. Buna"), ("CMSC 203", "Mr. Dorado")
            };
            CreateSection("CS 2A", "BSCS", 2, cs2Subjs);
            CreateSection("CS 2B", "BSCS", 2, cs2Subjs);

            // 3rd Year Majors
            var wmad3 = new[] { ("ITEP 308", "Mr. Manaloto"), ("ITEP 310", "Mr. Manzanero"), ("ITEL 304", "Mr. Del Rosario"), ("ITST 301", "Mr. Dorado"), ("ITST 302", "Ms. Viojan"), ("ITEP 309", "Ms. Viojan") };
            CreateSection("3WMAD1", "WMAD", 3, wmad3);
            CreateSection("3WMAD2", "WMAD", 3, wmad3);
            CreateSection("3WMAD3", "WMAD", 3, new[] { ("ITEP 308", "Mr. Manaloto"), ("ITEP 310", "Ms. Espinueva"), ("ITEL 304", "Mr. Del Rosario"), ("ITST 301", "Mr. Dorado"), ("ITST 302", "Ms. Viojan"), ("ITEP 309", "Ms. Viojan") });

            CreateSection("3SMP1", "SMP", 3, new[] { ("ITST 302", "Mr. Manzanero"), ("ITEL 304", "Mr. Andres"), ("ITEP 308", "Mr. Manaloto"), ("ITEP 310", "Mr. Manzanero"), ("ITST 301", "Ms. Espinueva"), ("ITEP 309", "Mr. Ual") });

            var na3 = new[] { ("ITEL 304", "Mr. Andres"), ("ITEP 308", "Mr. Ual"), ("ITEP 309", "Ms. Viojan"), ("ITST 302", "Engr. De Luna"), ("ITEP 310", "Mr. Manzanero"), ("ITST 301", "Engr. De Luna") };
            CreateSection("3NA1", "NA", 3, na3);
            CreateSection("3NA2", "NA", 3, na3);

            var is3 = new[] { ("CMSC 310", "Mr. Ilagan"), ("CMSC 309", "Ms. Castillo"), ("CSST 102", "Mr. Villareal"), ("CMSC 305", "Ms. Ompangco"), ("CMSC 306", "Ms. Cabuyao"), ("CSST 101", "Mr. Amora"), ("CMSC 308", "Ms. Ompangco"), ("CMSC 307", "Ms. Arida") };
            CreateSection("3IS1", "IS", 3, is3);
            CreateSection("3IS2", "IS", 3, is3);

            CreateSection("3GAV1", "GAV", 3, new[] { ("CMSC 305", "Ms. Ompangco"), ("CMSC 309", "Ms. Castillo"), ("CMSC 310", "Mr. Ilagan"), ("CSST 102", "Mr. Lara"), ("CSST 101", "Ms. Arida"), ("CMSC 306", "Ms. Cabuyao"), ("CMSC 307", "Ms. Arida"), ("CMSC 308", "Ms. Ompangco") });

            // 4th Year Majors
            CreateSection("4WMAD1", "WMAD", 4, new[] { ("ITEP 413", "Engr. Santos"), ("ITST 306", "Ms. Encanto"), ("ITEP 414", "Mr. Ual"), ("ITEP 415", "Mr. Dela Cruz") });
            CreateSection("4WMAD2", "WMAD", 4, new[] { ("ITEP 413", "Engr. Santos"), ("ITST 306", "Ms. Cabuyao"), ("ITEP 414", "Mr. Ual"), ("ITEP 415", "Mr. Dela Cruz") });
            CreateSection("4WMAD3", "WMAD", 4, new[] { ("ITEP 413", "Engr. Santos"), ("ITST 306", "Mr. Amora"), ("ITEP 414", "Mr. Ual"), ("ITEP 415", "Ms. De Torres") });

            CreateSection("4SMP1", "SMP", 4, new[] { ("ITEP 413", "Engr. Santos"), ("ITEP 414", "Mr. Ual"), ("ITEP 415", "Ms. De Torres"), ("ITST 306", "Mr. Manzanero") });
            CreateSection("4SMP2", "SMP", 4, new[] { ("ITEP 413", "Engr. Santos"), ("ITEP 414", "Mr. Ual"), ("ITEP 415", "Ms. De Torres"), ("ITST 306", "Mr. Dela Cruz") });

            CreateSection("4IS1", "IS", 4, new[] { ("CMSC 502", "Mr. Villareal"), ("CSST 106", "Mr. Lara") });
            CreateSection("4GAV1", "GAV", 4, new[] { ("CMSC 502", "Mr. Villareal"), ("CSST 106", "Ms. Cabuyao") });
        }

        #endregion

        #region 4. Helper Methods

        private static void AddTeacher(int id, string name, params string[] subjects)
        {
            Teacher t = new Teacher { Id = id, Name = name };
            foreach (var s in subjects) t.QualifiedSubjects.Add(s);
            Teachers.Add(t);
        }

        private static void CreateSection(string name, string program, int year, (string, string)[] subjects)
        {
            Section s = new Section { Id = Sections.Count + 1, Name = name, Program = program, YearLevel = year };

            foreach (var subj in subjects)
            {
                string code = subj.Item1;
                string teacherName = subj.Item2;

                if (code.Contains("NSTP")) continue;

                // Identify Subject Types
                bool isLabSubject = code.StartsWith("ITEC") || code.StartsWith("ITEL") ||
                                    code.StartsWith("ITEP") || code.StartsWith("ITST") ||
                                    code.StartsWith("CMSC") || code.StartsWith("CSST") ||
                                    code.StartsWith("MATH");

                bool isPE = code.StartsWith("PE") || code.StartsWith("PATHFIT");

                // Smart Teacher Qualification Assignment
                var teacher = Teachers.FirstOrDefault(t => t.Name.ToUpper().Contains(teacherName.ToUpper().Split(' ').Last()));
                if (teacher != null)
                {
                    if (!teacher.QualifiedSubjects.Contains(code)) teacher.QualifiedSubjects.Add(code);
                    if (isLabSubject)
                    {
                        if (!teacher.QualifiedSubjects.Contains(code + " (Lec)")) teacher.QualifiedSubjects.Add(code + " (Lec)");
                        if (!teacher.QualifiedSubjects.Contains(code + " (Lab)")) teacher.QualifiedSubjects.Add(code + " (Lab)");
                    }
                }

                // Add Subject to Section
                if (isPE)
                {
                    s.SubjectsToTake.Add(new Subject { Code = code, Units = 2, IsLab = false });
                }
                else if (isLabSubject)
                {
                    // Split: Lecture (2 units) + Lab (3 units)
                    s.SubjectsToTake.Add(new Subject { Code = code + " (Lec)", Units = 2, IsLab = false });
                    s.SubjectsToTake.Add(new Subject { Code = code + " (Lab)", Units = 3, IsLab = true });
                }
                else
                {
                    // Standard Lecture (3 units)
                    s.SubjectsToTake.Add(new Subject { Code = code, Units = 3, IsLab = false });
                }
            }
            Sections.Add(s);
        }

        #endregion
    }
}