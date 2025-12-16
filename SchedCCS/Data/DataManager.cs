using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;

namespace SchedCCS
{
    #region 1. Core Models

    /// <summary>
    /// Represents an authenticated system user with role-based access levels.
    /// </summary>
    public class User
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = "Student";
        public string? StudentSection { get; set; }
    }

    #endregion

    /// <summary>
    /// Orchestrates application data state. Manages in-memory repositories and 
    /// ensures synchronization between the volatile application state and the SQL persistence layer.
    /// </summary>
    public static class DataManager
    {
        #region 2. Repositories & Constants

        // In-memory collections synchronized with the persistence layer
        public static List<Room> Rooms { get; set; } = new List<Room>();
        public static List<Teacher> Teachers { get; set; } = new List<Teacher>();
        public static List<Section> Sections { get; set; } = new List<Section>();
        public static List<User> Users { get; set; } = new List<User>();
        public static List<ScheduleItem> MasterSchedule { get; set; } = new List<ScheduleItem>();
        public static List<FailedEntry> FailedAssignments { get; set; } = new List<FailedEntry>();

        // System-wide academic program identifiers
        public static readonly List<string> Programs = new List<string>
        {
            "BSCS", "BSINFO", "GAV", "IS", "SMP", "WMAD", "NA"
        };

        private const string ADMIN_HASH = "9922e395462d774d6b6377e868c5b9679f046b8565b93d09a05b38d33d596409";

        #endregion

        #region 3. Lifecycle & Initialization

        /// <summary>
        /// Synchronizes the application state with the database. 
        /// Prioritizes database loading and preserves current session data if the connection is unavailable.
        /// </summary>
        public static void Initialize()
        {
            try
            {
                // Verify database connectivity and retrieve data
                var dbUsers = DatabaseHelper.LoadUsers();
                var dbRooms = DatabaseHelper.LoadRooms();
                var dbTeachers = DatabaseHelper.LoadTeachers();
                var dbSections = DatabaseHelper.LoadSections();

                // On successful connection, refresh local memory with persistent data
                ClearMemory();

                Users = dbUsers;
                Rooms = dbRooms;
                Teachers = dbTeachers;
                Sections = dbSections;

                HandleAdminSecurity();

                if (Rooms.Count == 0) SeedDefaultRooms_ToDatabase();
            }
            catch (Exception)
            {
                // Fallback: Preserves current session data in memory if DB connection fails
                if (Users.Count == 0)
                {
                    LoadOfflineDefaults();
                }
                else
                {
                    MessageBox.Show("Database unreachable. Continuing with existing session data in offline mode.",
                        "System Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// Resets all in-memory collections to an empty state.
        /// </summary>
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

        #region 4. Security & Administration Logic

        /// <summary>
        /// Ensures the root administrator account exists and maintains password integrity.
        /// </summary>
        private static void HandleAdminSecurity()
        {
            string correctHash = ComputeLocalHash("@admin2025!");

            if (Users.Count == 0)
            {
                var admin = new User
                {
                    Username = "admin",
                    Password = correctHash,
                    Role = "Admin",
                    FullName = "System Administrator"
                };

                DatabaseHelper.SaveUser(admin);
                Users.Add(admin);
            }
            else
            {
                // Identity verification: synchronizes administrative credentials with system constants
                var admin = Users.FirstOrDefault(u => u.Username == "admin");
                if (admin != null && admin.Password != correctHash)
                {
                    DatabaseHelper.UpdateAdminPassword(correctHash);
                    admin.Password = correctHash;
                }
            }
        }

        #endregion

        #region 5. Data Seeding & Fallback Logic

        /// <summary>
        /// Generates standardized academic facilities for initial setup.
        /// </summary>
        private static List<Room> GetDefaultRooms()
        {
            var defaults = new List<Room>();

            for (int i = 1; i <= 12; i++)
                defaults.Add(new Room { Name = $"LEC {i}", Type = RoomType.Lecture });

            for (int i = 1; i <= 6; i++)
                defaults.Add(new Room { Name = $"LAB {i}", Type = RoomType.Laboratory });

            defaults.Add(new Room { Name = "GYM", Type = RoomType.Lecture });
            defaults.Add(new Room { Name = "FIELD", Type = RoomType.Lecture });

            return defaults;
        }

        /// <summary>
        /// Persists default facility data to the database.
        /// </summary>
        private static void SeedDefaultRooms_ToDatabase()
        {
            foreach (var r in GetDefaultRooms())
            {
                try { DatabaseHelper.CreateRoom(r.Name, r.Type.ToString()); }
                catch { /* Prevents termination on duplicate entries */ }
            }
            Rooms = DatabaseHelper.LoadRooms();
        }

        /// <summary>
        /// Initializes a default environment in memory when persistence is unavailable.
        /// </summary>
        private static void LoadOfflineDefaults()
        {
            if (Users.Any(u => u.Username == "admin")) return;

            string offlinePasswordHash = ComputeLocalHash("@admin2025!");

            Users.Add(new User
            {
                Username = "admin",
                Password = offlinePasswordHash,
                Role = "Admin",
                FullName = "Offline Administrator"
            });

            foreach (var room in GetDefaultRooms())
            {
                if (!Rooms.Any(r => r.Name == room.Name))
                {
                    room.Id = Rooms.Count + 1;
                    Rooms.Add(room);
                }
            }
        }

        #endregion

        #region 6. Resource Management (CRUD)

        /// <summary>
        /// Synchronizes section existence between the interface and the persistence layer.
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
                // Offline fallback logic
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

        #region 7. Utility Methods

        /// <summary>
        /// Generates a SHA-256 hexadecimal representation of a raw string.
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