using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using Dapper;

namespace SchedCCS
{
    public static class DatabaseHelper
    {
        // Connection String for XAMPP
        private static string connectionString = "Server=localhost;Database=sched_ccs_db;User=root;Password=;";

        public static IDbConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        #region 1. Users & Auth

        public static List<User> LoadUsers()
        {
            using (IDbConnection db = GetConnection())
            {
                string sql = "SELECT username, password_hash AS Password, full_name AS FullName, role, student_section AS StudentSection FROM users";
                return db.Query<User>(sql).ToList();
            }
        }

        public static void SaveUser(User user)
        {
            using (IDbConnection db = GetConnection())
            {
                string sql = @"INSERT INTO users (username, password_hash, full_name, role, student_section) 
                               VALUES (@Username, @Password, @FullName, @Role, @StudentSection)";
                db.Execute(sql, user);
            }
        }

        public static bool DeleteUser(string username)
        {
            if (username.Trim().ToLower() == "admin")
                throw new InvalidOperationException("Security Alert: The Root Administrator account cannot be deleted.");

            using (IDbConnection db = GetConnection())
            {
                string sql = "DELETE FROM users WHERE username = @Username";
                int rowsAffected = db.Execute(sql, new { Username = username });
                return rowsAffected > 0;
            }
        }

        #endregion

        #region 2. Resource Loading (Read Only)

        public static List<Room> LoadRooms()
        {
            using (IDbConnection db = GetConnection())
            {
                string sql = "SELECT room_id AS Id, room_name AS Name, room_type AS TypeStr FROM rooms";
                var rawRooms = db.Query<dynamic>(sql).ToList();
                var roomList = new List<Room>();

                foreach (var r in rawRooms)
                {
                    Room newRoom = new Room { Id = (int)r.Id, Name = (string)r.Name };
                    string typeStr = (string)r.TypeStr;
                    if (Enum.TryParse(typeStr, true, out RoomType parsedType)) newRoom.Type = parsedType;
                    else newRoom.Type = RoomType.Lecture;
                    roomList.Add(newRoom);
                }
                return roomList;
            }
        }

        public static List<Teacher> LoadTeachers()
        {
            using (IDbConnection db = GetConnection())
            {
                string sqlTeachers = "SELECT teacher_id AS Id, teacher_name AS Name FROM teachers";
                var teachers = db.Query<Teacher>(sqlTeachers).ToList();

                string sqlSubjects = "SELECT teacher_id, subject_code FROM teacher_subjects";
                var links = db.Query<dynamic>(sqlSubjects).ToList();

                foreach (var t in teachers)
                {
                    var mySubjects = links.Where(l => (int)l.teacher_id == t.Id)
                                          .Select(l => (string)l.subject_code).ToList();
                    t.QualifiedSubjects = mySubjects;
                }
                return teachers;
            }
        }

        public static List<Section> LoadSections()
        {
            using (IDbConnection db = GetConnection())
            {
                string sqlSections = "SELECT section_id AS Id, section_name AS Name, program, year_level AS YearLevel FROM sections";
                var sections = db.Query<Section>(sqlSections).ToList();

                string sqlSubj = "SELECT section_id, subject_code, units, is_lab FROM section_subjects";
                var rawSubjects = db.Query<dynamic>(sqlSubj).ToList();

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

        #region 3. Deletion & Transactions (The Fix)

        public static bool DeleteTeacher(int teacherId, string teacherName)
        {
            using (IDbConnection db = GetConnection())
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        // 1. Delete Schedules
                        db.Execute("DELETE FROM master_schedule WHERE teacher_name = @Name", new { Name = teacherName }, transaction);
                        // 2. Delete Qualifications
                        db.Execute("DELETE FROM teacher_subjects WHERE teacher_id = @Id", new { Id = teacherId }, transaction);
                        // 3. Delete Teacher
                        int rows = db.Execute("DELETE FROM teachers WHERE teacher_id = @Id", new { Id = teacherId }, transaction);

                        transaction.Commit();
                        return rows > 0;
                    }
                    catch { transaction.Rollback(); throw; }
                }
            }
        }

        public static bool DeleteRoom(int roomId, string roomName)
        {
            using (IDbConnection db = GetConnection())
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        // 1. Delete Schedules
                        db.Execute("DELETE FROM master_schedule WHERE room_name = @Name", new { Name = roomName }, transaction);
                        // 2. Delete Room
                        int rows = db.Execute("DELETE FROM rooms WHERE room_id = @Id", new { Id = roomId }, transaction);

                        transaction.Commit();
                        return rows > 0;
                    }
                    catch { transaction.Rollback(); throw; }
                }
            }
        }

        public static bool DeleteSection(int sectionId, string sectionName)
        {
            using (IDbConnection db = GetConnection())
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        // 1. Delete Schedules
                        db.Execute("DELETE FROM master_schedule WHERE section_name = @Name", new { Name = sectionName }, transaction);
                        // 2. Delete Subjects
                        db.Execute("DELETE FROM section_subjects WHERE section_id = @Id", new { Id = sectionId }, transaction);
                        // 3. Delete Section
                        int rows = db.Execute("DELETE FROM sections WHERE section_id = @Id", new { Id = sectionId }, transaction);

                        transaction.Commit();
                        return rows > 0;
                    }
                    catch { transaction.Rollback(); throw; }
                }
            }
        }

        public static void CreateSection(string sectionName)
        {
            using (IDbConnection db = GetConnection())
            {
                // Default Program to 'N/A' and Year to 1. Admin fixes this later.
                string sql = @"INSERT INTO sections (section_name, program, year_level) 
                               VALUES (@Name, 'N/A', 1)";
                db.Execute(sql, new { Name = sectionName });
            }
        }

        #endregion
    }
}