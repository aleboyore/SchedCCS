using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;

namespace SchedCCS
{
    /// <summary>
    /// Represents a system user, either an Administrator or a Student.
    /// </summary>
    public class User
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = "Student";
        public string? StudentSection { get; set; }
    }

    /// <summary>
    /// Centralized data management class. Handles in-memory repositories, 
    /// application initialization, and database synchronization logic.
    /// </summary>
    public static class DataManager
    {
        #region 1. Data Repositories

        // In-memory collections synchronized with the database
        public static List<Room> Rooms { get; set; } = new List<Room>();
        public static List<Teacher> Teachers { get; set; } = new List<Teacher>();
        public static List<Section> Sections { get; set; } = new List<Section>();
        public static List<User> Users { get; set; } = new List<User>();
        public static List<ScheduleItem> MasterSchedule { get; set; } = new List<ScheduleItem>();
        public static List<FailedEntry> FailedAssignments { get; set; } = new List<FailedEntry>();

        // Pre-defined academic programs
        public static readonly List<string> Programs = new List<string>
        {
            "BSCS", "BSINFO", "GAV", "IS", "SMP", "WMAD", "NA"
        };

        #endregion

        #region 2. Smart Initialization

        /// <summary>
        /// Initializes the application data by attempting to load from the database.
        /// Implements self-healing for Admin accounts and falls back to offline mode on connection failure.
        /// </summary>
        public static void Initialize()
        {
            ClearMemory();

            try
            {
                // Attempt to populate repositories from MySQL
                Users = DatabaseHelper.LoadUsers();
                Rooms = DatabaseHelper.LoadRooms();
                Teachers = DatabaseHelper.LoadTeachers();
                Sections = DatabaseHelper.LoadSections();

                HandleAdminSecurity();

                // Seed default rooms if the database table is empty
                if (Rooms.Count == 0)
                {
                    SeedDefaultRooms_ToDatabase();
                }
            }
            catch (Exception ex)
            {
                // Handle database connection failure and proceed in offline mode
                MessageBox.Show($"Database connection failed: {ex.Message}\n\nLaunching in Offline Mode.",
                    "Offline Mode active", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                LoadOfflineDefaults();
            }
        }

        /// <summary>
        /// Validates the existence and integrity of the Admin account.
        /// </summary>
        private static void HandleAdminSecurity()
        {
            string correctHash = ComputeLocalHash("@admin2025!");

            if (Users.Count == 0)
            {
                // Create initial admin if no users exist
                var admin = new User
                {
                    Username = "admin",
                    Password = correctHash,
                    Role = "Admin",
                    FullName = "System Admin"
                };

                DatabaseHelper.SaveUser(admin);
                Users.Add(admin);
            }
            else
            {
                // Self-healing: Reset admin password in DB if it does not match the system constant
                var admin = Users.FirstOrDefault(u => u.Username == "admin");
                if (admin != null && admin.Password != correctHash)
                {
                    DatabaseHelper.UpdateAdminPassword(correctHash);
                    admin.Password = correctHash;
                }
            }
        }

        private static void ClearMemory()
        {
            Rooms.Clear();
            Teachers.Clear();
            Sections.Clear();
            Users.Clear();
            MasterSchedule.Clear();
            FailedAssignments.Clear();
        }

        #endregion

        #region 3. Seeding Logic (Pre-Set Data)

        private const string ADMIN_HASH = "9922e395462d774d6b6377e868c5b9679f046b8565b93d09a05b38d33d596409";

        /// <summary>
        /// Returns a standardized list of rooms used for initial system setup.
        /// </summary>
        private static List<Room> GetDefaultRooms()
        {
            var defaults = new List<Room>();

            // Generate Building A Lecture Rooms
            for (int i = 1; i <= 12; i++)
                defaults.Add(new Room { Name = $"LEC {i}", Type = RoomType.Lecture });

            // Generate Building B Laboratory Rooms
            for (int i = 1; i <= 6; i++)
                defaults.Add(new Room { Name = $"LAB {i}", Type = RoomType.Laboratory });

            // Specialty rooms
            defaults.Add(new Room { Name = "GYM", Type = RoomType.Lecture });
            defaults.Add(new Room { Name = "FIELD", Type = RoomType.Lecture });

            return defaults;
        }

        private static void SeedDefaultRooms_ToDatabase()
        {
            foreach (var r in GetDefaultRooms())
            {
                try { DatabaseHelper.CreateRoom(r.Name, r.Type.ToString()); }
                catch { /* Ignore duplicates or schema errors during seeding */ }
            }
            // Refresh memory after DB insert
            Rooms = DatabaseHelper.LoadRooms();
        }

        private static void LoadOfflineDefaults()
        {
            // Seed Admin and Rooms into RAM only
            Users.Add(new User { Username = "admin", Password = ADMIN_HASH, Role = "Admin", FullName = "Offline Admin" });

            var defaults = GetDefaultRooms();
            for (int i = 0; i < defaults.Count; i++)
            {
                defaults[i].Id = i + 1;
                Rooms.Add(defaults[i]);
            }
        }

        #endregion

        #region 4. CRUD Operations (Memory & DB Wrappers)

        /// <summary>
        /// Ensures a section exists in both memory and the database.
        /// </summary>
        public static void EnsureSectionExists(string sectionName)
        {
            string cleanName = sectionName.Trim().ToUpper();
            if (Sections.Any(s => s.Name.ToUpper() == cleanName)) return;

            try
            {
                DatabaseHelper.CreateSection(cleanName);
                Sections = DatabaseHelper.LoadSections();
            }
            catch
            {
                // Fallback for offline mode
                Sections.Add(new Section { Id = Sections.Count + 1, Name = cleanName, Program = "N/A", YearLevel = 1 });
            }
        }

        public static void RemoveTeacher(Teacher t)
        {
            try { DatabaseHelper.DeleteTeacher(t.Id, t.Name); } catch { }
            Teachers.Remove(t);
            MasterSchedule.RemoveAll(x => x.Teacher == t.Name);
        }

        public static void RemoveRoom(Room r)
        {
            try { DatabaseHelper.DeleteRoom(r.Id, r.Name); } catch { }
            Rooms.Remove(r);
            MasterSchedule.RemoveAll(x => x.Room == r.Name);
        }

        public static void RemoveSection(Section s)
        {
            try { DatabaseHelper.DeleteSection(s.Id, s.Name); } catch { }
            Sections.Remove(s);
            MasterSchedule.RemoveAll(x => x.Section == s.Name);
        }

        #endregion

        #region 5. Security Utilities

        /// <summary>
        /// Computes a SHA-256 hash for a given string.
        /// </summary>
        private static string ComputeLocalHash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        #endregion
    }
}