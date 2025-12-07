using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;

namespace SchedCCS
{
    public partial class AdminDashboard : Form
    {
        #region 1. Fields & Properties

        // Grid Sorting Flags
        private string currentSortColumn = "";
        private bool isAscending = true;

        // Edit Tracking IDs
        private int editingTeacherId = -1;
        private int editingRoomId = -1;
        private int editingSectionId = -1;
        private string editingSubjectCode = "";

        // State Flags
        private bool isDataDirty = true;
        public bool IsAdmin { get; set; } = true;

        #endregion

        #region 2. Constructor & Initialization

        public AdminDashboard()
        {
            InitializeComponent();
            RefreshAdminLists();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            // Apply role-based UI restrictions
            if (!IsAdmin)
            {
                btnGenerate.Visible = false;
                this.Text = "Student Schedule Viewer";
                MessageBox.Show("Student View: Please select your section from the dropdown.");
            }
            else
            {
                this.Text = "Admin Dashboard - Schedule Generator";
            }

            RefreshSectionDropdown();
        }

        #endregion

        #region 3. Core Scheduling Logic

        // Executes the scheduling algorithm and updates the UI
        private void RunScheduleGeneration()
        {
            Cursor.Current = Cursors.WaitCursor;

            // 1. SETUP THE "CHAMPION" (Current Best)
            // We assume the worst case (Infinite conflicts) initially.
            int lowestConflictCount = int.MaxValue;
            List<ScheduleItem> bestSchedule = new List<ScheduleItem>();
            List<FailedEntry> bestFailures = new List<FailedEntry>();

            // 2. SAFETY NET LOGIC:
            // If the data hasn't changed (isDataDirty == false) AND we have an existing schedule,
            // we make that existing schedule the "Defending Champion".
            if (!isDataDirty && DataManager.MasterSchedule.Count > 0)
            {
                // Keep the current one as the one to beat
                lowestConflictCount = DataManager.FailedAssignments.Count;
                bestSchedule = new List<ScheduleItem>(DataManager.MasterSchedule);
                bestFailures = new List<FailedEntry>(DataManager.FailedAssignments);
            }

            // 3. THE TOURNAMENT (50 Challengers)
            int attempts = 50;
            ScheduleGenerator generator = new ScheduleGenerator(DataManager.Rooms, DataManager.Teachers, DataManager.Sections);

            for (int i = 0; i < attempts; i++)
            {
                generator.Generate();

                int score = generator.FailedAssignments.Count;

                // CHALLENGER CHECK:
                // Does this new schedule have FEWER conflicts than the current best?
                if (score < lowestConflictCount)
                {
                    // We found a new winner!
                    lowestConflictCount = score;
                    bestSchedule = new List<ScheduleItem>(generator.GeneratedSchedule);
                    bestFailures = new List<FailedEntry>(generator.FailedAssignments);
                }

                // Optimization: If we hit 0 conflicts, we can't get better than perfection.
                if (score == 0) break;
            }

            // 4. APPLY THE WINNER
            // Whether it's the old Champion or a new Challenger, apply the best one we found.
            DataManager.MasterSchedule = bestSchedule;
            DataManager.FailedAssignments = bestFailures;

            // 5. HOUSEKEEPING
            RebuildBusyArrays(bestSchedule); // Re-lock the busy slots
            UpdateMasterGrid();
            UpdateTimetableView();

            // (Uncomment if you have this method)
            // LoadPendingList(); 

            isDataDirty = false; // Mark data as "Clean" and saved
            Cursor.Current = Cursors.Default;
        }

        private void RebuildBusyArrays(List<ScheduleItem> acceptedSchedule)
        {
            // 1. Wipe everything clean first (Reset all to false)
            foreach (var r in DataManager.Rooms) r.IsBusy = new bool[7, 13];
            foreach (var t in DataManager.Teachers) t.IsBusy = new bool[7, 13];
            foreach (var s in DataManager.Sections) s.IsBusy = new bool[7, 13];

            // 2. Mark slots from the Best Schedule as busy
            foreach (var slot in acceptedSchedule)
            {
                // Find the actual objects in memory
                var room = DataManager.Rooms.FirstOrDefault(r => r.Name == slot.Room);
                var teacher = DataManager.Teachers.FirstOrDefault(t => t.Name == slot.Teacher);
                var section = DataManager.Sections.FirstOrDefault(s => s.Name == slot.Section);

                // Mark them busy if they exist
                if (room != null && teacher != null && section != null)
                {
                    room.IsBusy[slot.DayIndex, slot.TimeIndex] = true;
                    teacher.IsBusy[slot.DayIndex, slot.TimeIndex] = true;
                    section.IsBusy[slot.DayIndex, slot.TimeIndex] = true;

                    // Re-link the object in the schedule item to ensure consistency
                    slot.RoomObj = room;
                }
            }
        }

        private void UpdateMasterGrid()
        {
            dgvMaster.DataSource = null;
            dgvMaster.DataSource = DataManager.MasterSchedule;
            dgvMaster.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void UpdateTimetableView()
        {
            // Refresh dropdown items
            string currentSelection = comboBox1.SelectedItem?.ToString();
            comboBox1.Items.Clear();
            foreach (var section in DataManager.Sections) comboBox1.Items.Add(section.Name);

            // Restore selection if possible, or select first item
            if (currentSelection != null && comboBox1.Items.Contains(currentSelection))
            {
                comboBox1.SelectedItem = currentSelection;
            }
            else if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }

            // Trigger grid redraw
            if (comboBox1.SelectedItem != null)
            {
                string mode = cmbFilterType.SelectedItem?.ToString() ?? "Section";
                DisplayTimetable(comboBox1.SelectedItem.ToString(), mode);
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            // Extra Check: If we already have perfection, ask before running the CPU hard.
            if (!isDataDirty && DataManager.FailedAssignments.Count == 0 && DataManager.MasterSchedule.Count > 0)
            {
                var result = MessageBox.Show("You already have a Perfect Schedule!\n\nRe-generating will try to find another one, but won't lose this one.\n\nContinue anyway?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No) return;
            }

            RunScheduleGeneration();

            // Optional: Show how many conflicts remain (if any)
            int conflicts = DataManager.FailedAssignments.Count;
            if (conflicts == 0)
                MessageBox.Show("Perfect Schedule Generated Successfully!");
            else
                MessageBox.Show($"Schedule Generated. Note: {conflicts} subjects could not be placed (check Pending tab).");
        }

        #endregion

        #region 4. Visualization & Grid Rendering

        private void DisplayTimetable(string filterValue, string filterMode)
        {
            // Reset Grid
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            // Setup Columns
            dataGridView1.Columns.Add("Time", "TIME");
            string[] days = { "MONDAY", "TUESDAY", "WEDNESDAY", "THURSDAY", "FRIDAY", "SATURDAY", "SUNDAY" };
            foreach (var day in days) dataGridView1.Columns.Add(day, day);

            // Style Configuration
            ConfigureGridStyles();

            // Populate Time Rows (7:00 AM - 6:00 PM)
            int exactRowHeight = CalculateRowHeight();
            for (int hour = 7; hour < 18; hour++)
            {
                string niceTime = ToSimple12Hour($"{hour}:00 - {hour + 1}:00");
                int rowIndex = dataGridView1.Rows.Add(niceTime, "", "", "", "", "", "", "");
                dataGridView1.Rows[rowIndex].Height = exactRowHeight;
            }

            // Filter Data
            List<ScheduleItem> filteredList = new List<ScheduleItem>();
            if (filterMode == "Section") filteredList = DataManager.MasterSchedule.Where(x => x.Section == filterValue).ToList();
            else if (filterMode == "Teacher") filteredList = DataManager.MasterSchedule.Where(x => x.Teacher == filterValue).ToList();
            else if (filterMode == "Room") filteredList = DataManager.MasterSchedule.Where(x => x.Room == filterValue).ToList();

            // Render Cells
            RenderScheduleItems(filteredList, filterMode);

            dataGridView1.ClearSelection();
        }

        private void ConfigureGridStyles()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView1.Columns["Time"].FillWeight = 50;
            dataGridView1.ScrollBars = ScrollBars.None;
            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            foreach (DataGridViewColumn col in dataGridView1.Columns) col.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private int CalculateRowHeight()
        {
            int availableHeight = dataGridView1.Height - dataGridView1.ColumnHeadersHeight;
            int height = availableHeight / 11;
            return height < 20 ? 20 : height;
        }

        private void RenderScheduleItems(List<ScheduleItem> items, string filterMode)
        {
            foreach (var item in items)
            {
                int startHour = int.Parse(item.Time.Split(':')[0]);
                int rowIndex = startHour - 7;
                int colIndex = GetDayColumnIndex(item.Day);

                if (rowIndex >= 0 && rowIndex < dataGridView1.Rows.Count && colIndex > 0)
                {
                    string cellText = "";
                    if (filterMode == "Section") cellText = $"{item.Subject}\n{item.Teacher}\n{item.Room}";
                    else if (filterMode == "Teacher") cellText = $"{item.Subject}\n{item.Section}\n{item.Room}";
                    else if (filterMode == "Room") cellText = $"{item.Subject}\n{item.Section}\n{item.Teacher}";

                    var cell = dataGridView1.Rows[rowIndex].Cells[colIndex];
                    cell.Value = cellText;

                    if (item.Subject.Contains("(Lab)"))
                        cell.Style.BackColor = System.Drawing.Color.LightSalmon;
                    else
                        cell.Style.BackColor = GetSubjectColor(item.Subject);
                }
            }
        }

        private int GetDayColumnIndex(string day)
        {
            switch (day)
            {
                case "Mon": return 1;
                case "Tue": return 2;
                case "Wed": return 3;
                case "Thu": return 4;
                case "Fri": return 5;
                case "Sat": return 6;
                case "Sun": return 7;
                default: return 0;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabControl1.TabPages["tabSchedule"])
            {
                // Ensure dropdowns are populated to prevent blank screens
                if (cmbFilterType.SelectedIndex == -1) cmbFilterType.SelectedIndex = 0; // Default to Section

                if (comboBox1.Items.Count == 0) UpdateTimetableView();

                if (DataManager.MasterSchedule != null && DataManager.MasterSchedule.Count > 0)
                {
                    if (comboBox1.SelectedItem != null)
                    {
                        string mode = cmbFilterType.SelectedItem?.ToString() ?? "Section";
                        DisplayTimetable(comboBox1.SelectedItem.ToString(), mode);
                    }
                }
            }
            else if (tabControl1.SelectedTab == tabControl1.TabPages["tabMaster"])
            {
                UpdateMasterGrid();
                if (isDataDirty) RunScheduleGeneration();
            }
            else if (tabControl1.SelectedTab == tabControl1.TabPages["tabManage"])
            {
                RefreshAdminLists();
            }
            else if (tabControl1.SelectedTab.Text == "Pending Subjects")
            {
                LoadPendingList();
            }
        }

        private void dataGridView1_Resize(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0 || !dataGridView1.Visible) return;
            try
            {
                int newRowHeight = CalculateRowHeight();
                foreach (DataGridViewRow row in dataGridView1.Rows) row.Height = newRowHeight;
            }
            catch { }
        }

        #endregion

        #region 5. Data Management (CRUD)

        // Teacher Management
        private void btnAddTeacher_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTeacherName.Text)) { MessageBox.Show("Please enter a teacher name."); return; }

            Teacher newTeacher = new Teacher
            {
                Id = DataManager.Teachers.Count + 1,
                Name = txtTeacherName.Text
            };

            if (!string.IsNullOrWhiteSpace(txtTeacherSubjects.Text))
            {
                foreach (var s in txtTeacherSubjects.Text.Split(',')) newTeacher.QualifiedSubjects.Add(s.Trim());
            }

            DataManager.Teachers.Add(newTeacher);
            MessageBox.Show($"Teacher {newTeacher.Name} added!");
            ResetTeacherForm();
        }

        private void btnUpdateTeacher_Click(object sender, EventArgs e)
        {
            var teacher = DataManager.Teachers.FirstOrDefault(t => t.Id == editingTeacherId);
            if (teacher != null)
            {
                teacher.Name = txtTeacherName.Text;
                teacher.QualifiedSubjects.Clear();
                if (!string.IsNullOrWhiteSpace(txtTeacherSubjects.Text))
                {
                    foreach (var s in txtTeacherSubjects.Text.Split(',')) teacher.QualifiedSubjects.Add(s.Trim());
                }

                MessageBox.Show("Teacher Updated!");
                ResetTeacherForm();
            }
        }

        private void btnDeleteTeacher_Click(object sender, EventArgs e)
        {
            if (lstTeachers.SelectedItem == null) return;
            string name = lstTeachers.SelectedItem.ToString().Split('(')[0].Trim();
            var teacher = DataManager.Teachers.FirstOrDefault(t => t.Name == name);

            if (teacher != null)
            {
                DataManager.Teachers.Remove(teacher);
                MessageBox.Show("Teacher removed.");
                RefreshAdminLists();
            }
        }

        private void btnCancelTeacher_Click(object sender, EventArgs e) => ResetTeacherForm();

        private void ResetTeacherForm()
        {
            txtTeacherName.Clear();
            txtTeacherSubjects.Clear();
            editingTeacherId = -1;
            btnUpdateTeacher.Enabled = false;
            btnAddTeacher.Enabled = true;
            RefreshAdminLists();
            isDataDirty = true;
        }

        // Room Management
        private void btnAddRoom_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRoomName.Text) || cmbRoomType.SelectedItem == null)
            {
                MessageBox.Show("Please enter valid room details.");
                return;
            }

            Room newRoom = new Room
            {
                Id = DataManager.Rooms.Count + 1,
                Name = txtRoomName.Text,
                Type = cmbRoomType.SelectedItem.ToString() == "Laboratory" ? RoomType.Laboratory : RoomType.Lecture
            };

            DataManager.Rooms.Add(newRoom);
            MessageBox.Show($"Room '{newRoom.Name}' added!");
            ResetRoomForm();
        }

        private void btnUpdateRoom_Click(object sender, EventArgs e)
        {
            var room = DataManager.Rooms.FirstOrDefault(r => r.Id == editingRoomId);
            if (room != null)
            {
                room.Name = txtRoomName.Text;
                room.Type = cmbRoomType.SelectedItem.ToString() == "Laboratory" ? RoomType.Laboratory : RoomType.Lecture;
                MessageBox.Show("Room Updated!");
                ResetRoomForm();
            }
        }

        private void btnDeleteRoom_Click(object sender, EventArgs e)
        {
            if (lstRooms.SelectedItem == null) return;
            string name = lstRooms.SelectedItem.ToString().Split('|')[0].Trim();
            var room = DataManager.Rooms.FirstOrDefault(r => r.Name == name);

            if (room != null)
            {
                DataManager.Rooms.Remove(room);
                MessageBox.Show("Room deleted.");
                ResetRoomForm();
            }
        }

        private void btnCancelRoom_Click(object sender, EventArgs e) => ResetRoomForm();

        private void ResetRoomForm()
        {
            txtRoomName.Clear();
            cmbRoomType.SelectedIndex = -1;
            editingRoomId = -1;
            btnUpdateRoom.Enabled = false;
            btnAddRoom.Enabled = true;
            RefreshAdminLists();
            isDataDirty = true;
        }

        // Section & Subject Management
        private void btnCreateSection_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSectionName.Text) || cmbSectionProgram.SelectedItem == null || cmbSectionYear.SelectedItem == null)
            {
                MessageBox.Show("Please complete all section fields.");
                return;
            }

            string program = cmbSectionProgram.SelectedItem.ToString();
            int yearLevel = int.Parse(cmbSectionYear.SelectedItem.ToString());

            if (editingSectionId != -1)
            {
                var section = DataManager.Sections.FirstOrDefault(s => s.Id == editingSectionId);
                if (section != null)
                {
                    section.Name = txtSectionName.Text;
                    section.Program = program;
                    section.YearLevel = yearLevel;
                    MessageBox.Show("Section Updated!");
                }
                editingSectionId = -1;
                btnCreateSection.Text = "Create";
            }
            else
            {
                Section newSection = new Section
                {
                    Id = DataManager.Sections.Count + 1,
                    Name = txtSectionName.Text,
                    Program = program,
                    YearLevel = yearLevel
                };
                DataManager.Sections.Add(newSection);
                MessageBox.Show($"Section {newSection.Name} created!");
            }

            ResetSectionForm();
        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            if (editingSectionId != -1)
            {
                var section = DataManager.Sections.FirstOrDefault(s => s.Id == editingSectionId);
                if (section != null)
                {
                    section.Name = txtSectionName.Text;

                    if (!string.IsNullOrEmpty(editingSubjectCode))
                    {
                        var subject = section.SubjectsToTake.FirstOrDefault(s => s.Code.Contains(editingSubjectCode));
                        if (subject != null)
                        {
                            string newCode = txtSubjCode.Text.Trim();
                            subject.Code = chkIsLab.Checked && !newCode.Contains("(Lab)") ? newCode + " (Lab)" : newCode;
                            int.TryParse(txtUnits.Text, out int u);
                            subject.Units = u;
                            subject.IsLab = chkIsLab.Checked;
                        }
                    }

                    cmbSectionList.SelectedItem = section.Name;
                    MessageBox.Show("Changes Saved!");
                }
            }
            RefreshAdminLists();
            RefreshSubjectList();
            isDataDirty = true;
        }

        private void btnAddSubject_Click(object sender, EventArgs e)
        {
            if (cmbSectionList.SelectedItem == null) return;
            if (!int.TryParse(txtUnits.Text, out int inputUnits) || string.IsNullOrWhiteSpace(txtSubjCode.Text)) return;

            var section = DataManager.Sections.FirstOrDefault(s => s.Name == cmbSectionList.SelectedItem.ToString());
            if (section == null) return;

            if (chkIsLab.Checked)
            {
                section.SubjectsToTake.Add(new Subject { Code = txtSubjCode.Text + " (Lec)", IsLab = false, Units = inputUnits - 1 });
                section.SubjectsToTake.Add(new Subject { Code = txtSubjCode.Text + " (Lab)", IsLab = true, Units = 3 });
                MessageBox.Show($"Added {txtSubjCode.Text} (Lec/Lab split).");
            }
            else
            {
                section.SubjectsToTake.Add(new Subject { Code = txtSubjCode.Text, IsLab = false, Units = inputUnits });
                MessageBox.Show($"Added {txtSubjCode.Text}.");
            }

            ResetSubjectInputs();
            RefreshSubjectList();
            isDataDirty = true;
        }

        private void btnRemoveSubject_Click(object sender, EventArgs e)
        {
            if (cmbSectionList.SelectedItem == null || lstSectionSubjects.SelectedItem == null) return;

            var section = DataManager.Sections.FirstOrDefault(s => s.Name == cmbSectionList.SelectedItem.ToString());
            string selectedText = lstSectionSubjects.SelectedItem.ToString();
            var subject = section?.SubjectsToTake.FirstOrDefault(s => selectedText.Contains(s.Code));

            if (subject != null)
            {
                section.SubjectsToTake.Remove(subject);
                MessageBox.Show("Subject removed.");
                RefreshSubjectList();
                isDataDirty = true;
            }
        }

        private void btnDeleteSection_Click(object sender, EventArgs e)
        {
            if (lstSections.SelectedItem == null) return;
            string name = lstSections.SelectedItem.ToString();
            var section = DataManager.Sections.FirstOrDefault(s => s.Name == name);

            if (section != null && MessageBox.Show($"Delete {name}?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DataManager.Sections.Remove(section);
                MessageBox.Show("Section deleted.");
                RefreshAdminLists();
                isDataDirty = true;
            }
        }

        private void btnCancelSubject_Click(object sender, EventArgs e)
        {
            ResetSubjectInputs();
            ResetSectionForm();
        }

        private void ResetSectionForm()
        {
            txtSectionName.Clear();
            cmbSectionProgram.SelectedIndex = -1;
            cmbSectionYear.SelectedIndex = -1;
            RefreshAdminLists();
            RefreshSectionDropdown();
        }

        private void ResetSubjectInputs()
        {
            txtSubjCode.Clear();
            txtUnits.Clear();
            chkIsLab.Checked = false;
            editingSubjectCode = "";
            btnAddSubject.Enabled = true;
        }

        #endregion

        #region 6. Helper Functions

        private void RefreshSectionDropdown()
        {
            cmbSectionList.Items.Clear();
            foreach (var s in DataManager.Sections) cmbSectionList.Items.Add(s.Name);
        }

        private void RefreshAdminLists()
        {
            lstTeachers.Items.Clear();
            foreach (var t in DataManager.Teachers) lstTeachers.Items.Add($"{t.Name} ({string.Join(", ", t.QualifiedSubjects)})");

            lstRooms.Items.Clear();
            foreach (var r in DataManager.Rooms) lstRooms.Items.Add($"{r.Name} | {r.Type}");

            lstSections.Items.Clear();
            foreach (var s in DataManager.Sections) lstSections.Items.Add(s.Name);

            RefreshSectionDropdown();
        }

        private void RefreshSubjectList()
        {
            lstSectionSubjects.Items.Clear();
            if (cmbSectionList.SelectedItem == null) return;

            var section = DataManager.Sections.FirstOrDefault(s => s.Name == cmbSectionList.SelectedItem.ToString());
            if (section != null)
            {
                foreach (var sub in section.SubjectsToTake)
                {
                    string type = sub.IsLab ? "Lab" : "Lec";
                    lstSectionSubjects.Items.Add($"{sub.Code} - {type} ({sub.Units} units)");
                }
            }
        }

        private string ToSimple12Hour(string timeRange)
        {
            try
            {
                var parts = timeRange.Split('-');
                string start = DateTime.Parse(parts[0].Trim()).ToString("h:mm");
                string end = DateTime.Parse(parts[1].Trim()).ToString("h:mm");
                return $"{start} - {end}";
            }
            catch { return timeRange; }
        }

        private System.Drawing.Color GetSubjectColor(string subjectName)
        {
            string baseName = subjectName.Replace(" (Lec)", "").Replace(" (Lab)", "").Trim();
            int seed = baseName.GetHashCode();
            Random r = new Random(seed);
            return System.Drawing.Color.FromArgb(r.Next(160, 255), r.Next(160, 255), r.Next(160, 255));
        }

        #endregion

        #region 7. Event Listeners (UI Interaction)

        private void cmbFilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Text = "";
            string mode = cmbFilterType.SelectedItem?.ToString() ?? "Section";

            if (mode == "Section") foreach (var s in DataManager.Sections) comboBox1.Items.Add(s.Name);
            else if (mode == "Teacher") foreach (var t in DataManager.Teachers) comboBox1.Items.Add(t.Name);
            else if (mode == "Room") foreach (var r in DataManager.Rooms) comboBox1.Items.Add(r.Name);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                string mode = cmbFilterType.SelectedItem?.ToString() ?? "Section";
                DisplayTimetable(comboBox1.SelectedItem.ToString(), mode);
            }
        }

        private void lstTeachers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstTeachers.SelectedItem == null) return;
            string name = lstTeachers.SelectedItem.ToString().Split('(')[0].Trim();
            var teacher = DataManager.Teachers.FirstOrDefault(t => t.Name == name);

            if (teacher != null)
            {
                editingTeacherId = teacher.Id;
                txtTeacherName.Text = teacher.Name;
                txtTeacherSubjects.Text = string.Join(", ", teacher.QualifiedSubjects);
                btnUpdateTeacher.Enabled = true;
                btnAddTeacher.Enabled = false;
            }
        }

        private void lstRooms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstRooms.SelectedItem == null) return;
            string name = lstRooms.SelectedItem.ToString().Split('|')[0].Trim();
            var room = DataManager.Rooms.FirstOrDefault(r => r.Name == name);

            if (room != null)
            {
                editingRoomId = room.Id;
                txtRoomName.Text = room.Name;
                cmbRoomType.SelectedItem = room.Type == RoomType.Laboratory ? "Laboratory" : "Lecture";
                btnUpdateRoom.Enabled = true;
                btnAddRoom.Enabled = false;
            }
        }

        private void lstSectionSubjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstSectionSubjects.SelectedItem == null || cmbSectionList.SelectedItem == null) return;

            string code = lstSectionSubjects.SelectedItem.ToString().Split('-')[0].Trim();
            editingSubjectCode = code;

            var section = DataManager.Sections.FirstOrDefault(s => s.Name == cmbSectionList.SelectedItem.ToString());
            var subject = section?.SubjectsToTake.FirstOrDefault(s => s.Code.Contains(code));

            if (subject != null)
            {
                txtSubjCode.Text = subject.Code.Replace(" (Lec)", "").Replace(" (Lab)", "").Trim();
                txtUnits.Text = subject.Units.ToString();
                chkIsLab.Checked = subject.IsLab;
                btnAddSubject.Enabled = false;
            }
        }

        private void lstSections_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstSections.SelectedItem == null) return;
            string name = lstSections.SelectedItem.ToString();
            var section = DataManager.Sections.FirstOrDefault(s => s.Name == name);

            if (section != null)
            {
                editingSectionId = section.Id;
                txtSectionName.Text = section.Name;
                if (cmbSectionProgram.Items.Contains(section.Program)) cmbSectionProgram.SelectedItem = section.Program;
                if (cmbSectionYear.Items.Contains(section.YearLevel.ToString())) cmbSectionYear.SelectedItem = section.YearLevel.ToString();
                btnCreateSection.Text = "Update Section";
                if (cmbSectionList.Items.Contains(section.Name)) cmbSectionList.SelectedItem = section.Name;
            }
        }

        private void cmbSectionList_SelectedIndexChanged(object sender, EventArgs e) => RefreshSubjectList();

        private void dgvMaster_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string columnClicked = dgvMaster.Columns[e.ColumnIndex].Name;
            isAscending = (currentSortColumn == columnClicked) ? !isAscending : true;
            currentSortColumn = columnClicked;

            if (columnClicked == "Day")
                DataManager.MasterSchedule = isAscending ? DataManager.MasterSchedule.OrderBy(x => GetDayIndex(x.Day)).ToList() : DataManager.MasterSchedule.OrderByDescending(x => GetDayIndex(x.Day)).ToList();
            else if (columnClicked == "Time")
                DataManager.MasterSchedule = isAscending ? DataManager.MasterSchedule.OrderBy(x => GetTimeIndex(x.Time)).ToList() : DataManager.MasterSchedule.OrderByDescending(x => GetTimeIndex(x.Time)).ToList();
            else
            {
                switch (columnClicked)
                {
                    case "Section": DataManager.MasterSchedule = isAscending ? DataManager.MasterSchedule.OrderBy(x => x.Section).ToList() : DataManager.MasterSchedule.OrderByDescending(x => x.Section).ToList(); break;
                    case "Subject": DataManager.MasterSchedule = isAscending ? DataManager.MasterSchedule.OrderBy(x => x.Subject).ToList() : DataManager.MasterSchedule.OrderByDescending(x => x.Subject).ToList(); break;
                    case "Teacher": DataManager.MasterSchedule = isAscending ? DataManager.MasterSchedule.OrderBy(x => x.Teacher).ToList() : DataManager.MasterSchedule.OrderByDescending(x => x.Teacher).ToList(); break;
                    case "Room": DataManager.MasterSchedule = isAscending ? DataManager.MasterSchedule.OrderBy(x => x.Room).ToList() : DataManager.MasterSchedule.OrderByDescending(x => x.Room).ToList(); break;
                }
            }

            UpdateMasterGrid();
        }

        private int GetDayIndex(string dayName)
        {
            // Fix: Ensure all returned values are between 0 and 6
            switch (dayName)
            {
                case "Monday": return 1;
                case "Tuesday": return 2;
                case "Wednesday": return 3;
                case "Thursday": return 4;
                case "Friday": return 5;
                case "Saturday": return 6;
                case "Sunday": return 0; // CHANGED FROM 7 TO 0
                default: return 0; // Safe fallback
            }
        }

        private int GetTimeIndex(string timeRange)
        {
            try
            {
                string startHour = timeRange.Split(':')[0];
                int hour = int.Parse(startHour);
                int index = hour - 7;

                // Safety Clamp: Ensure it stays within [0, 12]
                if (index < 0) return 0;
                if (index > 12) return 12;

                return index;
            }
            catch
            {
                return 0;
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Log out and return to Login screen?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                this.Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (DataManager.MasterSchedule.Count == 0)
            {
                MessageBox.Show("No schedule to export! Please generate one first.");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog { Filter = "CSV File|*.csv", FileName = "Schedule.csv" };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(sfd.FileName))
                    {
                        sw.WriteLine("Section,Subject,Teacher,Room,Day,Time");
                        foreach (var item in DataManager.MasterSchedule)
                            sw.WriteLine($"{item.Section},{item.Subject},{item.Teacher},{item.Room},{item.Day},{item.Time}");
                    }
                    MessageBox.Show("Export Successful!");
                }
                catch (Exception ex) { MessageBox.Show("Error saving file: " + ex.Message); }
            }
        }

        private void btnBatchAdd_Click(object sender, EventArgs e)
        {
            if (cmbBatchProgram.SelectedItem == null || cmbBatchYear.SelectedItem == null || string.IsNullOrWhiteSpace(txtBatchCode.Text))
            {
                MessageBox.Show("Please enter all subject details.");
                return;
            }

            string program = cmbBatchProgram.SelectedItem.ToString();
            int year = int.Parse(cmbBatchYear.SelectedItem.ToString());
            string code = txtBatchCode.Text.Trim();
            int.TryParse(txtBatchUnits.Text, out int units);
            bool isLab = chkBatchLab.Checked;

            var targetSections = DataManager.Sections.Where(s => s.Program == program && s.YearLevel == year).ToList();

            if (targetSections.Count == 0)
            {
                MessageBox.Show("No sections found.");
                return;
            }

            int count = 0;
            foreach (var s in targetSections)
            {
                if (s.SubjectsToTake.Any(sub => sub.Code.Contains(code))) continue;

                if (isLab)
                {
                    s.SubjectsToTake.Add(new Subject { Code = code + " (Lec)", Units = units - 1, IsLab = false });
                    s.SubjectsToTake.Add(new Subject { Code = code + " (Lab)", Units = 3, IsLab = true });
                }
                else
                {
                    s.SubjectsToTake.Add(new Subject { Code = code, Units = units, IsLab = false });
                }
                count++;
            }

            MessageBox.Show($"Added {code} to {count} sections.");
            ResetSubjectInputs();
            RefreshSubjectList();
            isDataDirty = true;
        }

        private void LoadPendingList()
        {
            dgvPending.DataSource = null;
            dgvPending.Rows.Clear();
            dgvPending.Columns.Clear();
            dgvPending.Columns.Add("Section", "Section");
            dgvPending.Columns.Add("Subject", "Subject");
            dgvPending.Columns.Add("Reason", "Reason");
            dgvPending.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (DataManager.FailedAssignments != null)
            {
                foreach (var fail in DataManager.FailedAssignments)
                {
                    int rowIndex = dgvPending.Rows.Add(fail.Section.Name, fail.Subject.Code, fail.Reason);
                    dgvPending.Rows[rowIndex].Tag = fail;
                }
            }
        }

        // --- HELPER TO CLEAR FLAGS ---
        private void ClearBusyFlag(ScheduleItem item)
        {
            var t = DataManager.Teachers.FirstOrDefault(x => x.Name == item.Teacher);
            var s = DataManager.Sections.FirstOrDefault(x => x.Name == item.Section);
            var r = DataManager.Rooms.FirstOrDefault(x => x.Name == item.Room);

            if (t != null) t.IsBusy[item.DayIndex, item.TimeIndex] = false;
            if (s != null) s.IsBusy[item.DayIndex, item.TimeIndex] = false;
            if (r != null) r.IsBusy[item.DayIndex, item.TimeIndex] = false;
        }

        #endregion
    }
}