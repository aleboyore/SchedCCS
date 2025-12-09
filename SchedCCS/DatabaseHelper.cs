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
        // Connection String for XAMPP (Default)
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

        // SECURITY: Delete User with Admin Protection
        public static bool DeleteUser(string username)
        {
            // SECURITY FAILSAFE: Never delete the Root Admin
            if (username.Trim().ToLower() == "admin")
            {
                // We throw an exception so the UI knows exactly why it failed
                throw new InvalidOperationException("Security Alert: The Root Administrator account cannot be deleted.");
            }

            using (IDbConnection db = GetConnection())
            {
                string sql = "DELETE FROM users WHERE username = @Username";
                int rowsAffected = db.Execute(sql, new { Username = username });

                return rowsAffected > 0;
            }
        }

        #endregion

        #region 2. Resource Loading

        public static List<Room> LoadRooms()
        {
            using (IDbConnection db = GetConnection())
            {
                // Fetch raw string for 'Type' and convert manually
                string sql = "SELECT room_id AS Id, room_name AS Name, room_type AS TypeStr FROM rooms";

                var rawRooms = db.Query<dynamic>(sql).ToList();
                var roomList = new List<Room>();

                foreach (var r in rawRooms)
                {
                    Room newRoom = new Room
                    {
                        Id = (int)r.Id,
                        Name = (string)r.Name
                    };

                    // Convert String "Lecture" -> Enum RoomType.Lecture
                    string typeStr = (string)r.TypeStr;
                    if (Enum.TryParse(typeStr, true, out RoomType parsedType))
                    {
                        newRoom.Type = parsedType;
                    }
                    else
                    {
                        newRoom.Type = RoomType.Lecture; // Default fallback
                    }

                    roomList.Add(newRoom);
                }
                return roomList;
            }
        }

        #endregion

        #region 3. Teachers & Subjects

        public static List<Teacher> LoadTeachers()
        {
            using (IDbConnection db = GetConnection())
            {
                // 1. Get all Teachers
                string sqlTeachers = "SELECT teacher_id AS Id, teacher_name AS Name FROM teachers";
                var teachers = db.Query<Teacher>(sqlTeachers).ToList();

                // 2. Get all Subjects links
                string sqlSubjects = "SELECT teacher_id, subject_code FROM teacher_subjects";
                var links = db.Query<dynamic>(sqlSubjects).ToList();

                // 3. Match them up
                foreach (var t in teachers)
                {
                    var mySubjects = links.Where(l => (int)l.teacher_id == t.Id)
                                          .Select(l => (string)l.subject_code)
                                          .ToList();

                    t.QualifiedSubjects = mySubjects;
                }

                return teachers;
            }
        }

        #endregion

        #region 4. Sections & Curriculum

        public static List<Section> LoadSections()
        {
            using (IDbConnection db = GetConnection())
            {
                // 1. Get Sections
                string sqlSections = "SELECT section_id AS Id, section_name AS Name, program, year_level AS YearLevel FROM sections";
                var sections = db.Query<Section>(sqlSections).ToList();

                // 2. Get Subjects
                string sqlSubj = "SELECT section_id, subject_code, units, is_lab FROM section_subjects";
                var rawSubjects = db.Query<dynamic>(sqlSubj).ToList();

                // 3. Match
                foreach (var s in sections)
                {
                    var mySubjs = rawSubjects.Where(x => (int)x.section_id == s.Id).ToList();

                    foreach (var row in mySubjs)
                    {
                        s.SubjectsToTake.Add(new Subject
                        {
                            Code = (string)row.subject_code,
                            Units = (int)row.units,
                            IsLab = (bool)row.is_lab // 1/0 maps to Bool
                        });
                    }
                }

                return sections;
            }
        }

        #endregion
    }
}