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
            // 1. Wipe Memory Clean
            Rooms.Clear();
            Teachers.Clear();
            Sections.Clear();
            Users.Clear();

            try
            {
                // 2. Load from Database
                Users = DatabaseHelper.LoadUsers();
                Rooms = DatabaseHelper.LoadRooms();
                Teachers = DatabaseHelper.LoadTeachers();
                Sections = DatabaseHelper.LoadSections();

                // 3. Fallback: If DB is empty, Seed ONLY Users and Rooms
                // We deliberately leave Teachers/Sections empty for testing Add/Edit functions.
                if (Users.Count == 0) LoadUsers_Hardcoded();
                if (Rooms.Count == 0) LoadRooms_Hardcoded();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Database Error: " + ex.Message + "\n\nSwitching to Offline Mode.");
                LoadUsers_Hardcoded();
                LoadRooms_Hardcoded();
                // Teachers and Sections start empty in offline mode
            }

            // 4. Ensure Virtual Room "FIELD" exists for PE subjects
            // This allows scheduling PE even if no physical rooms are in the DB.
            if (!Rooms.Any(r => r.Name.ToUpper().Contains("FIELD") || r.Name.ToUpper().Contains("GYM")))
            {
                Rooms.Add(new Room
                {
                    Id = 9999,
                    Name = "FIELD",
                    Type = RoomType.Lecture
                });
            }
        }

        #endregion

        #region 3. Data Management (CRUD & Safety)

        public static void EnsureSectionExists(string sectionName)
        {
            // 1. Clean the input
            string cleanName = sectionName.Trim().ToUpper();

            // 2. Check if it already exists in memory
            if (Sections.Any(s => s.Name.ToUpper() == cleanName)) return;

            // 3. If not, Create it in Database
            try
            {
                DatabaseHelper.CreateSection(cleanName);

                // 4. Reload Sections to sync ID and data
                Sections = DatabaseHelper.LoadSections();
            }
            catch
            {
                // Offline Fallback: Add to local memory only
                Sections.Add(new Section
                {
                    Id = Sections.Count + 1,
                    Name = cleanName,
                    Program = "N/A",
                    YearLevel = 1
                });
            }
        }

        public static void RemoveTeacher(Teacher t)
        {
            try { DatabaseHelper.DeleteTeacher(t.Id, t.Name); } catch { /* Continue if offline */ }

            // Remove from Memory Lists
            Teachers.Remove(t);

            // Remove any schedule associated with this teacher
            MasterSchedule.RemoveAll(x => x.Teacher == t.Name);
        }

        public static void RemoveRoom(Room r)
        {
            try { DatabaseHelper.DeleteRoom(r.Id, r.Name); } catch { /* Continue if offline */ }

            Rooms.Remove(r);
            MasterSchedule.RemoveAll(x => x.Room == r.Name);
        }

        public static void RemoveSection(Section s)
        {
            try { DatabaseHelper.DeleteSection(s.Id, s.Name); } catch { /* Continue if offline */ }

            Sections.Remove(s);
            MasterSchedule.RemoveAll(x => x.Section == s.Name);
        }

        #endregion

        #region 4. Hardcoded Data Loaders (Fallback/Seeding)

        private static void LoadUsers_Hardcoded()
        {
            Users.Add(new User { Username = "admin", Password = "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3", Role = "Admin", FullName = "System Admin" });
            Users.Add(new User { Username = "student", Password = "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3", Role = "Student", StudentSection = null, FullName = "Student User" });
        }

        private static void LoadRooms_Hardcoded()
        {
            // Building A
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
            Rooms.Add(new Room { Id = 20, Name = "LEC 1", Type = RoomType.Lecture });

            // Building B (Labs)
            Rooms.Add(new Room { Id = 21, Name = "LAB 1", Type = RoomType.Laboratory });
            Rooms.Add(new Room { Id = 22, Name = "LAB 2", Type = RoomType.Laboratory });
            Rooms.Add(new Room { Id = 23, Name = "LAB 3", Type = RoomType.Laboratory });
            Rooms.Add(new Room { Id = 24, Name = "LAB 4", Type = RoomType.Laboratory });
            Rooms.Add(new Room { Id = 25, Name = "LAB 5", Type = RoomType.Laboratory });
            Rooms.Add(new Room { Id = 26, Name = "LAB 6", Type = RoomType.Laboratory });
        }

        private static void LoadTeachers_Hardcoded()
        {
            // Intentionally empty for manual testing
        }

        private static void LoadSections_Hardcoded()
        {
            // Intentionally empty for manual testing
        }

        #endregion

        #region 5. Helper Methods (Internal)

        private static void AddTeacher(int id, string name, params string[] subjects)
        {
            Teacher t = new Teacher { Id = id, Name = name };
            foreach (var s in subjects) t.QualifiedSubjects.Add(s);
            Teachers.Add(t);
        }

        private static void CreateSection(string name, string program, int year, (string, string)[] subjects)
        {
            Section s = new Section { Id = Sections.Count + 1, Name = name, Program = program, YearLevel = year };
            // Note: Subject adding logic is preserved in comments if needed later
            Sections.Add(s);
        }

        #endregion
    }
}