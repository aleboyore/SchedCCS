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
            // Admin (Password: "123")
            Users.Add(new User
            {
                Username = "admin",
                Password = "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3",
                Role = "Admin",
                FullName = "System Admin"
            });

            // Student 1 (Password: "123")
            Users.Add(new User
            {
                Username = "0324-2227",
                Password = "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3",
                Role = "Student",
                StudentSection = "CS 2A",
                FullName = "Alezer Boyore"
            });

            // Student 2 (Password: "123")
            Users.Add(new User
            {
                Username = "student",
                Password = "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3",
                Role = "Student",
                StudentSection = "CS 2A",
                FullName = "Student User"
            });
        }

        private static void LoadRooms()
        {
            // Building A (Lecture Rooms)
            Rooms.Add(new Room { Id = 1, Name = "LEC 2", Type = RoomType.Lecture });
            Rooms.Add(new Room { Id = 2, Name = "LEC 3", Type = RoomType.Lecture });
            Rooms.Add(new Room { Id = 3, Name = "LEC 4", Type = RoomType.Lecture });
            Rooms.Add(new Room { Id = 4, Name = "LEC 5", Type = RoomType.Lecture });
            Rooms.Add(new Room { Id = 5, Name = "LEC 6", Type = RoomType.Lecture });
            Rooms.Add(new Room { Id = 6, Name = "LEC 7", Type = RoomType.Lecture });
            Rooms.Add(new Room { Id = 7, Name = "LEC 8", Type = RoomType.Lecture });
            Rooms.Add(new Room { Id = 8, Name = "LEC 9", Type = RoomType.Lecture });
            Rooms.Add(new Room { Id = 9, Name = "LEC 10", Type = RoomType.Lecture });
            Rooms.Add(new Room { Id = 10, Name = "LEC 11", Type = RoomType.Lecture });
            Rooms.Add(new Room { Id = 11, Name = "LEC 12", Type = RoomType.Lecture });

            // Building B (Labs + Mixed)
            Rooms.Add(new Room { Id = 20, Name = "LEC 1", Type = RoomType.Lecture });
            Rooms.Add(new Room { Id = 21, Name = "LAB 1", Type = RoomType.Laboratory });
            Rooms.Add(new Room { Id = 22, Name = "LAB 2", Type = RoomType.Laboratory });
            Rooms.Add(new Room { Id = 23, Name = "LAB 3", Type = RoomType.Laboratory });
            Rooms.Add(new Room { Id = 24, Name = "LAB 4", Type = RoomType.Laboratory });
            Rooms.Add(new Room { Id = 25, Name = "LAB 5", Type = RoomType.Laboratory });
            Rooms.Add(new Room { Id = 26, Name = "LAB 6", Type = RoomType.Laboratory });
        }

        private static void LoadTeachers()
        {
            // General Education & Common
            AddTeacher(1, "Mr. Desamero", "ITEC 102");
            AddTeacher(2, "Mr. Funtila", "GEC 103");
            AddTeacher(3, "Ms. De Torres", "ITEC 101", "ITEL 415", "ITEP 415");
            AddTeacher(4, "Ms. Cabreza", "GEC 102");
            AddTeacher(5, "Ms. Parica", "PE 1");
            AddTeacher(6, "Mr. Calosa", "GEC 101");
            AddTeacher(7, "Ms. Balinsayo", "KOMFIL");

            // Major Subjects
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
            // 1st Year
            var info1 = new[] {
                ("ITEC 102", "Mr. Desamero"), ("GEC 103", "Mr. Funtila"), ("ITEC 101", "Ms. De Torres"),
                ("GEC 102", "Ms. Cabreza"), ("PE 1", "Ms. Parica"), ("GEC 101", "Mr. Calosa"),
                ("KOMFIL", "Ms. Balinsayo"), ("GEC 104", "Mr. Dorado")
            };
            CreateSection("INFO 1A", "BSINFO", 1, info1);
            CreateSection("INFO 1B", "BSINFO", 1, info1);

            var cs1 = new[] {
                ("ITEC 102", "Mr. Dorado"), ("GEC 103", "Mr. Funtila"), ("ITEC 101", "Ms. Arida"),
                ("GEC 102", "Ms. Cabreza"), ("PE 1", "Mr. Sael"), ("GEC 101", "Mr. Calosa"),
                ("KOMFIL", "Ms. Balinsayo"), ("PI 100", "Mr. Agravante")
            };
            CreateSection("CS 1A", "BSCS", 1, cs1);
            CreateSection("CS 1B", "BSCS", 1, cs1);

            // 2nd Year
            var cs2 = new[] {
                ("CMSC 202", "Ms. Cabuyao"), ("MATH 24", "Mr. Bombio"), ("ITEC 205", "Mr. Lara"),
                ("PE 3", "Mr. Moreno"), ("ITEC 104", "Ms. Arida"), ("SOSLIT", "Ms. Del Valle"),
                ("GEC 106", "Mr. Buna"), ("CMSC 203", "Mr. Dorado")
            };
            CreateSection("CS 2A", "BSCS", 2, cs2);
            CreateSection("CS 2B", "BSCS", 2, cs2);

            var info2 = new[] {
                ("ITEC 204", "Ms. Encanto"), ("ITEL 201", "Mr. Dorado"), ("ITEP 203", "Ms. Escote"),
                ("ITEC 205", "Mr. Amora"), ("GEC 107", "Ms. Basaca"), ("SOSLIT", "Ms. Del Valle"),
                ("ITEL 202", "Mr. Andres"), ("PE 3", "Mr. Moreno")
            };
            CreateSection("INFO 2A", "BSINFO", 2, info2);
            CreateSection("INFO 2B", "BSINFO", 2, info2);

            // 3rd Year
            var wmad3 = new[] {
                ("ITEP 308", "Mr. Manaloto"), ("ITEP 310", "Mr. Manzanero"), ("ITEL 304", "Mr. Del Rosario"),
                ("ITST 301", "Mr. Dorado"), ("ITST 302", "Ms. Viojan")
            };
            CreateSection("3WMAD1", "WMAD", 3, wmad3);
            CreateSection("3WMAD2", "WMAD", 3, wmad3);

            // 4th Year
            var wmad4 = new[] {
                ("ITEP 413", "Engr. Santos"), ("ITST 306", "Ms. Encanto"),
                ("ITEP 414", "Mr. Ual"), ("ITEP 415", "Mr. Dela Cruz")
            };
            CreateSection("4WMAD1", "WMAD", 4, wmad4);
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

                // Smart Lab Logic
                bool isPotentialLab = code.StartsWith("ITEC") || code.StartsWith("ITEL") ||
                                      code.StartsWith("ITEP") || code.StartsWith("CMSC") ||
                                      code.StartsWith("CSST");

                // Exception List: Subjects that are Lecture ONLY
                List<string> lectureOnlyExceptions = new List<string>
                {
                    "CMSC 202", "CMSC 203", "ITST 301", "ITST 302",
                    "ITST 306", "MATH 24", "ITEC 101"
                };

                bool isLabSubject = isPotentialLab && !lectureOnlyExceptions.Contains(code);
                bool isPE = code.StartsWith("PE") || code.StartsWith("PATHFIT");

                // Auto-Assign Qualifications
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
                    s.SubjectsToTake.Add(new Subject { Code = code + " (Lec)", Units = 2, IsLab = false });
                    s.SubjectsToTake.Add(new Subject { Code = code + " (Lab)", Units = 3, IsLab = true });
                }
                else
                {
                    s.SubjectsToTake.Add(new Subject { Code = code, Units = 3, IsLab = false });
                }
            }
            Sections.Add(s);
        }

        #endregion
    }
}