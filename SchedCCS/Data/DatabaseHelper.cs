using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using Dapper;

namespace SchedCCS
{
    /// <summary>
    /// Static utility class for MySQL database operations using Dapper.
    /// Handles data persistence, resource synchronization, and transactions.
    /// </summary>
    public static class DatabaseHelper
    {
        private const string ConnectionString = "Server=localhost;Database=sched_ccs_db;User=root;Password=;";

        /// <summary>
        /// Creates and returns a new database connection.
        /// </summary>
        public static IDbConnection GetConnection() => new MySqlConnection(ConnectionString);

        #region 1. Identity & Access Management

        /// <summary>
        /// Loads all users from the database.
        /// </summary>
        public static List<User> LoadUsers()
        {
            using (IDbConnection db = GetConnection())
            {
                const string sql = "SELECT username, password_hash AS Password, full_name AS FullName, role, student_section AS StudentSection FROM users";
                return db.Query<User>(sql).ToList();
            }
        }

        /// <summary>
        /// Saves a new user to the database.
        /// </summary>
        public static void SaveUser(User user)
        {
            using (IDbConnection db = GetConnection())
            {
                const string sql = @"INSERT INTO users (username, password_hash, full_name, role, student_section) 
                                     VALUES (@Username, @Password, @FullName, @Role, @StudentSection)";
                db.Execute(sql, user);
            }
        }

        /// <summary>
        /// Deletes a user by username. Prevents deletion of the 'admin' account.
        /// </summary>
        public static bool DeleteUser(string username)
        {
            if (string.Equals(username.Trim(), "admin", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Security Violation: The Root Administrator account cannot be removed.");

            using (IDbConnection db = GetConnection())
            {
                const string sql = "DELETE FROM users WHERE username = @Username";
                return db.Execute(sql, new { Username = username }) > 0;
            }
        }

        /// <summary>
        /// Updates the password for the root administrator.
        /// </summary>
        public static void UpdateAdminPassword(string newHash)
        {
            using (IDbConnection db = GetConnection())
            {
                const string sql = "UPDATE users SET password_hash = @Hash WHERE username = 'admin'";
                db.Execute(sql, new { Hash = newHash });
            }
        }

        #endregion

        #region 2. Institutional Resource Loading

        /// <summary>
        /// Loads all rooms and parses their types.
        /// </summary>
        public static List<Room> LoadRooms()
        {
            using (IDbConnection db = GetConnection())
            {
                const string sql = "SELECT room_id AS Id, room_name AS Name, room_type AS TypeStr FROM rooms";
                var rawRooms = db.Query<dynamic>(sql).ToList();
                var roomList = new List<Room>();

                foreach (var r in rawRooms)
                {
                    var newRoom = new Room { Id = (int)r.Id, Name = (string)r.Name };
                    if (Enum.TryParse((string)r.TypeStr, true, out RoomType parsedType))
                        newRoom.Type = parsedType;
                    else
                        newRoom.Type = RoomType.Lecture;

                    roomList.Add(newRoom);
                }
                return roomList;
            }
        }

        /// <summary>
        /// Loads teachers and their qualified subjects.
        /// </summary>
        public static List<Teacher> LoadTeachers()
        {
            using (IDbConnection db = GetConnection())
            {
                var teachers = db.Query<Teacher>("SELECT teacher_id AS Id, teacher_name AS Name FROM teachers").ToList();
                var links = db.Query<dynamic>("SELECT teacher_id, subject_code FROM teacher_subjects").ToList();

                foreach (var t in teachers)
                {
                    t.QualifiedSubjects = links.Where(l => (int)l.teacher_id == t.Id)
                                               .Select(l => (string)l.subject_code).ToList();
                }
                return teachers;
            }
        }

        /// <summary>
        /// Loads sections and their required subjects.
        /// </summary>
        public static List<Section> LoadSections()
        {
            using (IDbConnection db = GetConnection())
            {
                var sections = db.Query<Section>("SELECT section_id AS Id, section_name AS Name, program, year_level AS YearLevel FROM sections").ToList();
                var rawSubjects = db.Query<dynamic>("SELECT section_id, subject_code, units, is_lab FROM section_subjects").ToList();

                foreach (var s in sections)
                {
                    var mySubjs = rawSubjects.Where(x => (int)x.section_id == s.Id).ToList();
                    foreach (var row in mySubjs)
                    {
                        s.SubjectsToTake.Add(new Subject
                        {
                            Code = (string)row.subject_code,
                            Units = (int)row.units,
                            IsLab = (bool)row.is_lab
                        });
                    }
                }
                return sections;
            }
        }

        #endregion

        #region 3. Resource Creation

        /// <summary>
        /// Creates a new room record.
        /// </summary>
        public static void CreateRoom(string name, string type)
        {
            using (IDbConnection db = GetConnection())
            {
                db.Execute("INSERT INTO rooms (room_name, room_type) VALUES (@Name, @Type)", new { Name = name, Type = type });
            }
        }

        /// <summary>
        /// Creates a new section record.
        /// </summary>
        public static void CreateSection(string sectionName)
        {
            using (IDbConnection db = GetConnection())
            {
                db.Execute("INSERT INTO sections (section_name, program, year_level) VALUES (@Name, 'N/A', 1)", new { Name = sectionName });
            }
        }

        #endregion

        #region 4. Transactional Deletion Logic

        /// <summary>
        /// Deletes a teacher and cascades deletion to related records.
        /// </summary>
        public static bool DeleteTeacher(int teacherId, string teacherName)
        {
            using (var db = GetConnection())
            {
                db.Open();
                using (var trans = db.BeginTransaction())
                {
                    try
                    {
                        db.Execute("DELETE FROM master_schedule WHERE teacher_name = @Name", new { Name = teacherName }, trans);
                        db.Execute("DELETE FROM teacher_subjects WHERE teacher_id = @Id", new { Id = teacherId }, trans);
                        int rows = db.Execute("DELETE FROM teachers WHERE teacher_id = @Id", new { Id = teacherId }, trans);
                        trans.Commit();
                        return rows > 0;
                    }
                    catch { trans.Rollback(); throw; }
                }
            }
        }

        /// <summary>
        /// Deletes a room and cascades deletion to related records.
        /// </summary>
        public static bool DeleteRoom(int roomId, string roomName)
        {
            using (var db = GetConnection())
            {
                db.Open();
                using (var trans = db.BeginTransaction())
                {
                    try
                    {
                        db.Execute("DELETE FROM master_schedule WHERE room_name = @Name", new { Name = roomName }, trans);
                        int rows = db.Execute("DELETE FROM rooms WHERE room_id = @Id", new { Id = roomId }, trans);
                        trans.Commit();
                        return rows > 0;
                    }
                    catch { trans.Rollback(); throw; }
                }
            }
        }

        /// <summary>
        /// Deletes a section and cascades deletion to related records.
        /// </summary>
        public static bool DeleteSection(int sectionId, string sectionName)
        {
            using (var db = GetConnection())
            {
                db.Open();
                using (var trans = db.BeginTransaction())
                {
                    try
                    {
                        db.Execute("DELETE FROM master_schedule WHERE section_name = @Name", new { Name = sectionName }, trans);
                        db.Execute("DELETE FROM section_subjects WHERE section_id = @Id", new { Id = sectionId }, trans);
                        int rows = db.Execute("DELETE FROM sections WHERE section_id = @Id", new { Id = sectionId }, trans);
                        trans.Commit();
                        return rows > 0;
                    }
                    catch { trans.Rollback(); throw; }
                }
            }
        }

        #endregion

        #region 5. Administrative Utilities

        /// <summary>
        /// Executes a raw SQL command.
        /// </summary>
        public static void ExecuteQuery(string sql, object parameters)
        {
            using (IDbConnection db = GetConnection())
            {
                db.Execute(sql, parameters);
            }
        }

        /// <summary>
        /// Replaces the master schedule with a new set of items within a transaction.
        /// </summary>
        public static void SaveMasterSchedule(List<ScheduleItem> schedule)
        {
            using (var db = GetConnection())
            {
                db.Open();
                using (var trans = db.BeginTransaction())
                {
                    try
                    {
                        db.Execute("DELETE FROM master_schedule", transaction: trans);
                        const string sql = @"INSERT INTO master_schedule 
                                             (section_name, subject_code, teacher_name, room_name, day, start_time, day_index, time_index) 
                                             VALUES 
                                             (@Section, @Subject, @Teacher, @Room, @Day, @Time, @DayIndex, @TimeIndex)";
                        db.Execute(sql, schedule, transaction: trans);
                        trans.Commit();
                    }
                    catch { trans.Rollback(); throw; }
                }
            }
        }

        #endregion
    }
}