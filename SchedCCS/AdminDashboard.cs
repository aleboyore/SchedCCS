using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Text.Json;

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
            // 1. Reset the Grid
            dgvMaster.DataSource = null;

            // 2. Sort the Data (Crucial for Admins)
            // We sort by Day (Mon->Sun), then Time (7am->6pm), then Section.
            // This turns the random list into a readable timeline.
            var sortedSchedule = DataManager.MasterSchedule
                .OrderBy(x => x.DayIndex)       // Group by Day (Mon=1, Tue=2...)
                .ThenBy(x => x.TimeIndex)       // Then by Time (7am, 8am...)
                .ThenBy(x => x.Section)         // Then by Section name
                .ToList();

            dgvMaster.DataSource = sortedSchedule;

            // 3. Grid Styling
            dgvMaster.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMaster.AllowUserToAddRows = false;       // Disable extra empty row at bottom
            dgvMaster.RowHeadersVisible = false;        // Hide the ugly left-side selector block
            dgvMaster.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Click selects whole row
            dgvMaster.ReadOnly = true;                  // Admin shouldn't type directly here (use Right-Click)

            // 4. Hide "Developer" Columns (The Ugly Data)
            if (dgvMaster.Columns["DayIndex"] != null) dgvMaster.Columns["DayIndex"].Visible = false;
            if (dgvMaster.Columns["TimeIndex"] != null) dgvMaster.Columns["TimeIndex"].Visible = false;
            if (dgvMaster.Columns["RoomObj"] != null) dgvMaster.Columns["RoomObj"].Visible = false;

            // 5. Rename Headers (Professional Look)
            // We use ALL CAPS to distinguish this as a formal report.
            if (dgvMaster.Columns["Section"] != null) dgvMaster.Columns["Section"].HeaderText = "SECTION";
            if (dgvMaster.Columns["Subject"] != null) dgvMaster.Columns["Subject"].HeaderText = "SUBJECT CODE";
            if (dgvMaster.Columns["Teacher"] != null) dgvMaster.Columns["Teacher"].HeaderText = "INSTRUCTOR";
            if (dgvMaster.Columns["Room"] != null) dgvMaster.Columns["Room"].HeaderText = "ROOM";
            if (dgvMaster.Columns["Day"] != null) dgvMaster.Columns["Day"].HeaderText = "DAY";
            if (dgvMaster.Columns["Time"] != null) dgvMaster.Columns["Time"].HeaderText = "TIME SLOT";

            // 6. Optional: Column Ordering (Put Day/Time first for easier reading)
            if (dgvMaster.Columns["Day"] != null) dgvMaster.Columns["Day"].DisplayIndex = 0;
            if (dgvMaster.Columns["Time"] != null) dgvMaster.Columns["Time"].DisplayIndex = 1;
            if (dgvMaster.Columns["Section"] != null) dgvMaster.Columns["Section"].DisplayIndex = 2;
            if (dgvMaster.Columns["Subject"] != null) dgvMaster.Columns["Subject"].DisplayIndex = 3;
            if (dgvMaster.Columns["Room"] != null) dgvMaster.Columns["Room"].DisplayIndex = 4;
            if (dgvMaster.Columns["Teacher"] != null) dgvMaster.Columns["Teacher"].DisplayIndex = 5;
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
            dataGridView1.Refresh();
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

        // UPDATE THIS METHOD in AdminDashboard.cs
        private int GetDayColumnIndex(string day)
        {
            // Handle both Long and Short names just in case
            if (day.StartsWith("Mon")) return 1;
            if (day.StartsWith("Tue")) return 2;
            if (day.StartsWith("Wed")) return 3;
            if (day.StartsWith("Thu")) return 4;
            if (day.StartsWith("Fri")) return 5;
            if (day.StartsWith("Sat")) return 6;
            if (day.StartsWith("Sun")) return 7;

            return 0; // Error/Unknown
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
            // Identify which column was clicked
            string columnClicked = dgvMaster.Columns[e.ColumnIndex].Name;

            // Toggle Sort Direction
            if (currentSortColumn == columnClicked)
            {
                isAscending = !isAscending; // Flip direction if clicking the same column
            }
            else
            {
                currentSortColumn = columnClicked;
                isAscending = true; // Default to Ascending for a new column
            }

            // Perform the Sort
            // We use a List variable to hold the sorted results
            List<ScheduleItem> sortedList = null;

            if (isAscending)
            {
                switch (columnClicked)
                {
                    case "Day":
                        sortedList = DataManager.MasterSchedule.OrderBy(x => x.DayIndex).ToList();
                        break;
                    case "Time":
                        sortedList = DataManager.MasterSchedule.OrderBy(x => x.TimeIndex).ToList();
                        break;
                    case "Section":
                        sortedList = DataManager.MasterSchedule.OrderBy(x => x.Section).ToList();
                        break;
                    case "Subject":
                        sortedList = DataManager.MasterSchedule.OrderBy(x => x.Subject).ToList();
                        break;
                    case "Teacher":
                        sortedList = DataManager.MasterSchedule.OrderBy(x => x.Teacher).ToList();
                        break;
                    case "Room":
                        sortedList = DataManager.MasterSchedule.OrderBy(x => x.Room).ToList();
                        break;
                    default:
                        return; // Do nothing if clicking a hidden/unknown column
                }
            }
            else // Descending (Z-A)
            {
                switch (columnClicked)
                {
                    case "Day":
                        sortedList = DataManager.MasterSchedule.OrderByDescending(x => x.DayIndex).ToList();
                        break;
                    case "Time":
                        sortedList = DataManager.MasterSchedule.OrderByDescending(x => x.TimeIndex).ToList();
                        break;
                    case "Section":
                        sortedList = DataManager.MasterSchedule.OrderByDescending(x => x.Section).ToList();
                        break;
                    case "Subject":
                        sortedList = DataManager.MasterSchedule.OrderByDescending(x => x.Subject).ToList();
                        break;
                    case "Teacher":
                        sortedList = DataManager.MasterSchedule.OrderByDescending(x => x.Teacher).ToList();
                        break;
                    case "Room":
                        sortedList = DataManager.MasterSchedule.OrderByDescending(x => x.Room).ToList();
                        break;
                    default:
                        return;
                }
            }

            // Apply the sorted list to the grid
            dgvMaster.DataSource = sortedList;

            // Optional: Add a visual indicator (like an arrow) to the header text
            foreach (DataGridViewColumn col in dgvMaster.Columns)
            {
                // Clear arrows from other columns
                col.HeaderText = col.HeaderText.Replace(" ▲", "").Replace(" ▼", "");
            }

            // Add arrow to current column
            dgvMaster.Columns[e.ColumnIndex].HeaderText += isAscending ? " ▲" : " ▼";

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

        //create backup
        private void btnBackupDatabase_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "System Backup File|*.json";
            save.FileName = $"SchedCCS_Backup_{DateTime.Now:yyyyMMdd_HHmm}.json";

            if (save.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // 1. Pack data into the container
                    var backup = new SystemBackup
                    {
                        Rooms = DataManager.Rooms,
                        Teachers = DataManager.Teachers,
                        Sections = DataManager.Sections,
                        MasterSchedule = DataManager.MasterSchedule,
                        FailedAssignments = DataManager.FailedAssignments
                    };

                    // 2. Serialize to JSON (Make it readable with WriteIndented)
                    string jsonString = JsonSerializer.Serialize(backup, new JsonSerializerOptions { WriteIndented = true });

                    // 3. Save to file
                    File.WriteAllText(save.FileName, jsonString);

                    MessageBox.Show("System Database Backed up Successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Backup Failed: " + ex.Message);
                }
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

        // 1. EVENT: Handles the Right-Click on the Grid
        // Updated Right-Click Logic to handle SWAPPING/UNASSIGNING
        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // 1. Basic Validation (Right click on a valid cell)
            if (e.Button != MouseButtons.Right || e.RowIndex < 0 || e.ColumnIndex <= 0) return;

            // 2. CRITICAL FIX: Force selection to the cell you clicked
            dataGridView1.ClearSelection();
            dataGridView1.CurrentCell = dataGridView1[e.ColumnIndex, e.RowIndex];
            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;

            // 3. Calculate Indices
            int dayIndex = e.ColumnIndex;
            int timeIndex = e.RowIndex;

            // 4. Identify Context (What is currently in this slot?)
            string currentSectionName = comboBox1.SelectedItem?.ToString();

            // Find the class in this slot (if any)
            // Note: We search the MasterSchedule to get the REAL object, not just the text in the grid.
            var existingClass = DataManager.MasterSchedule.FirstOrDefault(s =>
                (s.Section == currentSectionName || cmbFilterType.Text != "Section") &&
                s.DayIndex == dayIndex &&
                s.TimeIndex == timeIndex &&
                (s.Room == dataGridView1.Rows[timeIndex].Cells[e.ColumnIndex].Value?.ToString().Split('\n').LastOrDefault()
                 || cmbFilterType.Text == "Section") // Logic varies slightly based on view, assuming Section view for now
            );

            // If viewing by Section, specific logic to find the exact class item
            if (cmbFilterType.Text == "Section" && !string.IsNullOrEmpty(currentSectionName))
            {
                existingClass = DataManager.MasterSchedule.FirstOrDefault(s =>
                    s.Section == currentSectionName && s.DayIndex == dayIndex && s.TimeIndex == timeIndex);
            }

            // 5. Build the Menu
            ctxMenuSchedule.Items.Clear();
            ctxMenuSchedule.Items.Add(new ToolStripMenuItem($"Slot: {GetDayName(dayIndex)} @ {ToSimple12Hour(GetTimeLabel(timeIndex))}") { Enabled = false, BackColor = System.Drawing.Color.LightGray });
            ctxMenuSchedule.Items.Add(new ToolStripSeparator());

            // === SCENARIO A: SLOT IS FULL (Options: Unassign or Swap) ===
            if (existingClass != null)
            {
                ctxMenuSchedule.Items.Add(new ToolStripMenuItem($"Current: {existingClass.Subject} ({existingClass.Room})") { Enabled = false });

                // OPTION 1: UNASSIGN (Removes ALL units of this block)
                var unassignItem = new ToolStripMenuItem("Unassign Entire Block (Move to Pending)");
                unassignItem.Click += (s, args) => UnassignSubject(existingClass);
                ctxMenuSchedule.Items.Add(unassignItem);

                ctxMenuSchedule.Items.Add(new ToolStripSeparator());

                // OPTION 2: SWAP (Replace this class with a Pending one)
                // Only show Pending subjects that MATCH the type (Lab/Lec)
                bool isLabSlot = existingClass.Subject.Contains("(Lab)");
                var validSwaps = DataManager.FailedAssignments
                    .Where(f => f.Section.Name == existingClass.Section && f.Subject.IsLab == isLabSlot)
                    .ToList();

                if (validSwaps.Count > 0)
                {
                    var swapRoot = new ToolStripMenuItem("Swap With...");
                    foreach (var pending in validSwaps)
                    {
                        var swapItem = new ToolStripMenuItem($"{pending.Subject.Code} ({pending.Subject.Units} units)");
                        swapItem.Click += (s, args) => PerformSwap(existingClass, pending);
                        swapRoot.DropDownItems.Add(swapItem);
                    }
                    ctxMenuSchedule.Items.Add(swapRoot);
                }
            }
            // === SCENARIO B: SLOT IS EMPTY (Option: Place Pending) ===
            else
            {
                string currentMode = cmbFilterType.SelectedItem?.ToString();
                var relevantPending = new List<FailedEntry>();

                // LOGIC FIX: Handle different views (Section vs Room vs Teacher)
                if (currentMode == "Section")
                {
                    // Only show pending items for THIS section
                    relevantPending = DataManager.FailedAssignments
                        .Where(f => f.Section.Name == currentSectionName)
                        .ToList();
                }
                else if (currentMode == "Teacher")
                {
                    // Show subjects this teacher can actually teach
                    relevantPending = DataManager.FailedAssignments
                        .Where(f =>
                        {
                            var t = DataManager.Teachers.FirstOrDefault(teacher => teacher.Name == currentSectionName);
                            return t != null && t.QualifiedSubjects.Contains(CleanSubjectName(f.Subject.Code));
                        })
                        .ToList();
                }
                else // View is "Room"
                {
                    // Show ALL pending items (because any section *could* potentially use this room)
                    // Optional: You could filter to only show "Lab" subjects if the room is a Laboratory
                    relevantPending = DataManager.FailedAssignments.ToList();
                }

                if (relevantPending.Count == 0)
                {
                    ctxMenuSchedule.Items.Add("No pending subjects found.");
                }
                else
                {
                    // Sort them so the menu isn't a mess
                    foreach (var fail in relevantPending.OrderBy(f => f.Section.Name))
                    {
                        string label = $"Place {fail.Section.Name} - {fail.Subject.Code} ({fail.Subject.Units}u)";
                        var item = new ToolStripMenuItem(label);

                        item.Tag = new { FailEntry = fail, Day = dayIndex, Time = timeIndex };
                        item.Click += ContextMenu_PlaceClick;

                        ctxMenuSchedule.Items.Add(item);
                    }
                }
            }

            // Show Menu
            Rectangle cellRect = dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
            ctxMenuSchedule.Show(dataGridView1, cellRect.Left + e.X, cellRect.Top + e.Y);
        }

        private void UnassignSubject(ScheduleItem item)
        {
            // 1. Identify the Target Type (Lec or Lab?)
            // We only want to remove the matching type. If unassigning Lab, keep the Lecture.
            bool isLabTarget = item.Subject.Contains("(Lab)");

            // 2. Find ALL slots for this specific Section + Subject + Type
            var relatedItems = DataManager.MasterSchedule
                .Where(s => s.Section == item.Section &&
                            s.Subject.Contains("(Lab)") == isLabTarget &&
                            CleanSubjectName(s.Subject) == CleanSubjectName(item.Subject))
                .ToList();

            if (relatedItems.Count == 0) return;

            // 3. Clear Busy Flags for ALL of them
            foreach (var part in relatedItems)
            {
                ClearBusyFlag(part);
            }

            // 4. Remove ALL of them from the schedule
            DataManager.MasterSchedule.RemoveAll(x => relatedItems.Contains(x));

            // 5. Create ONE Pending Entry for the WHOLE subject
            var sec = DataManager.Sections.First(x => x.Name == item.Section);
            // Find the subject definition to get the correct Total Units
            var sub = sec.SubjectsToTake.First(x =>
                CleanSubjectName(x.Code) == CleanSubjectName(item.Subject) && x.IsLab == isLabTarget);

            // Only add to pending if not already there
            if (!DataManager.FailedAssignments.Any(f => f.Section == sec && f.Subject == sub))
            {
                DataManager.FailedAssignments.Add(new FailedEntry
                {
                    Section = sec,
                    Subject = sub,
                    Reason = "Manually Unassigned"
                });
            }

            // 6. Refresh UI
            RebuildBusyArrays(DataManager.MasterSchedule);
            UpdateMasterGrid();
            UpdateTimetableView();
            LoadPendingList();
        }

        private void PerformSwap(ScheduleItem oldClass, FailedEntry newClass)
        {
            // 1. Validate: Does the new class fit?
            // The new class might have different units (e.g. swapping a 3-unit Lab with a 3-unit Major is easy. 
            // Swapping 3-unit Lab with 1-unit Lec is messy but allowed).

            // 2. Unassign the OLD class (Move it to pending)
            UnassignSubject(oldClass);

            // 3. Assign the NEW class (Move it from pending to schedule)
            // We try to place it starting at the same time index.
            bool success = PlaceBlockManual(newClass, oldClass.DayIndex, oldClass.TimeIndex, oldClass.Room);

            if (!success)
            {
                MessageBox.Show("Swap failed: The new subject requires more time/units than available, or the teacher/room is busy for the full duration.");
                // OPTIONAL: Undo the unassign if you want to be fancy, 
                // but for now, the old class just stays in Pending, which is safe.
            }

            // 4. Refresh
            UpdateMasterGrid();
            UpdateTimetableView();
            LoadPendingList();
        }

        // Event Handler for "Place Here" menu item
        private void ContextMenu_PlaceClick(object sender, EventArgs e)
        {
            // 1. Retrieve Data from the Menu Item
            var menuItem = (ToolStripMenuItem)sender;
            dynamic data = menuItem.Tag; // Unpack the anonymous object

            FailedEntry failEntry = data.FailEntry;
            int targetDay = data.Day;
            int targetTime = data.Time;

            // 2. Validation: We need a Room and Teacher
            // Since "Place Here" relies on the FailedEntry, we check if we have a teacher capable.
            // For the Room, we use the one associated with the "Slot" (if viewing by Room)
            // OR we pick the first available valid room.

            string targetRoom = null;

            // Smart Room Finder
            if (cmbFilterType.SelectedItem?.ToString() == "Room")
            {
                // If viewing a specific room, assume we want to place it in THAT room.
                targetRoom = comboBox1.SelectedItem.ToString();
            }
            else
            {
                // Otherwise, find the first free room that fits the type (Lab/Lec)
                var roomType = failEntry.Subject.IsLab ? RoomType.Laboratory : RoomType.Lecture;
                var validRoom = DataManager.Rooms.FirstOrDefault(r =>
                    r.Type == roomType &&
                    !r.IsBusy[targetDay, targetTime]); // Simple check for start time

                if (validRoom == null)
                {
                    MessageBox.Show($"No {roomType} rooms are free at this time.");
                    return;
                }
                targetRoom = validRoom.Name;
            }

            // 3. Execute the Placement
            // We use the same helper we wrote for Swapping
            bool success = PlaceBlockManual(failEntry, targetDay, targetTime, targetRoom);

            if (success)
            {
                UpdateMasterGrid();
                UpdateTimetableView();
                LoadPendingList();
            }
            else
            {
                MessageBox.Show("Cannot place here. The teacher is busy, the room is occupied for the full duration, or the class exceeds the day's hours.");
            }
        }

        // Helper to place a multi-hour block manually
        // Update this method in AdminDashboard.cs
        private bool PlaceBlockManual(FailedEntry fail, int day, int startInfo, string roomName)
        {
            int duration = fail.Subject.Units;
            var teacher = DataManager.Teachers.FirstOrDefault(t => t.QualifiedSubjects.Contains(CleanSubjectName(fail.Subject.Code)));
            var room = DataManager.Rooms.First(r => r.Name == roomName);

            if (teacher == null) return false;

            // 1. COLLISION SCANNING (The Fix)
            // We must check if the Teacher OR the Room is occupied for the FULL duration.
            // AND we must check if there are existing classes in the way (to move to pending).

            List<ScheduleItem> obstacles = new List<ScheduleItem>();

            for (int i = 0; i < duration; i++)
            {
                int t = startInfo + i;
                if (t > 12) return false; // Exceeds day limits

                // A. Check Teacher Availability (Hard Constraint)
                // If the teacher is busy elsewhere, we CANNOT proceed.
                // We exclude the current slot if we are swapping, but for "Place" we just check strict busy-ness.
                // Note: We perform a deeper check against MasterSchedule to ignore the class we are currently replacing if needed.
                if (teacher.IsBusy[day, t])
                {
                    // Check if the teacher is busy because of a class we are about to overwrite?
                    // If yes, that's fine. If no (busy elsewhere), return false.
                    bool busyWithOthers = DataManager.MasterSchedule.Any(s =>
                        s.Teacher == teacher.Name && s.DayIndex == day && s.TimeIndex == t &&
                        s.Section != fail.Section.Name); // Busy with a DIFFERENT section

                    if (busyWithOthers) return false;
                }

                // B. Identify Obstacles (Classes currently sitting in this slot)
                // We look for anything in this Room OR anything this Section is doing.
                var existing = DataManager.MasterSchedule.FirstOrDefault(s =>
                    s.DayIndex == day && s.TimeIndex == t &&
                    (s.Room == roomName || s.Section == fail.Section.Name));

                if (existing != null)
                {
                    obstacles.Add(existing);
                }
            }

            // 2. BULLDOZER PHASE (Move obstacles to Pending)
            foreach (var obstacle in obstacles)
            {
                // Avoid duplicate logic if the obstacle takes up 2 slots (don't unassign twice)
                if (DataManager.MasterSchedule.Contains(obstacle))
                {
                    UnassignSubject(obstacle); // Use the Helper we wrote earlier!
                }
            }

            // 3. COMMIT PLACEMENT
            for (int i = 0; i < duration; i++)
            {
                int t = startInfo + i;
                ScheduleItem newItem = new ScheduleItem
                {
                    Section = fail.Section.Name,
                    Subject = fail.Subject.Code,
                    Teacher = teacher.Name,
                    Room = room.Name,
                    Day = GetDayName(day), // Ensure this uses your fixed 1=Mon helper
                    Time = GetTimeLabel(t),
                    DayIndex = day,
                    TimeIndex = t,
                    RoomObj = room
                };
                DataManager.MasterSchedule.Add(newItem);

                // Update flags
                teacher.IsBusy[day, t] = true;
                fail.Section.IsBusy[day, t] = true;
                room.IsBusy[day, t] = true;
            }

            // Remove the one we just placed from Pending
            if (DataManager.FailedAssignments.Contains(fail))
                DataManager.FailedAssignments.Remove(fail);

            return true;
        }

        // 2. LOGIC: Executes when you click a menu item
        private void ContextMenu_AssignClick(object sender, EventArgs e)
        {
            // Retrieve the data we attached to the tag
            var menuItem = (ToolStripMenuItem)sender;
            dynamic data = menuItem.Tag;
            FailedEntry fail = data.FailEntry;
            int day = data.Day;
            int time = data.Time;

            // Smart Validation: Do we need a Room or Teacher?
            // Since we are forcing, we might need to Auto-Assign a room if one isn't specified.
            // For this basic version, we will try to find the *first available* room/teacher for this slot.

            // 1. Find a Teacher (If not already defined in the pending entry, pick one who is free)
            // Note: In your current logic, Pending subjects don't store a "Teacher", so we pick one capable.
            var teacher = DataManager.Teachers.FirstOrDefault(t =>
                t.QualifiedSubjects.Contains(CleanSubjectName(fail.Subject.Code)) &&
                !t.IsBusy[day, time]);

            if (teacher == null)
            {
                MessageBox.Show("Cannot place here: No qualified teachers are free at this time.");
                return;
            }

            // 2. Find a Room
            var roomType = fail.Subject.IsLab ? RoomType.Laboratory : RoomType.Lecture;
            var room = DataManager.Rooms.FirstOrDefault(r =>
                r.Type == roomType &&
                !r.IsBusy[day, time]);

            if (room == null)
            {
                MessageBox.Show($"Cannot place here: No {roomType} rooms are free at this time.");
                return;
            }

            // 3. Check if Section is free (Don't double book the students!)
            if (fail.Section.IsBusy[day, time])
            {
                if (MessageBox.Show("Students already have a class here. Overwrite?", "Conflict", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;

                // (Optional: Code to remove the conflicting class goes here if needed)
            }

            // 4. COMMIT THE ASSIGNMENT
            ScheduleItem newItem = new ScheduleItem
            {
                Section = fail.Section.Name,
                Subject = fail.Subject.Code,
                Teacher = teacher.Name,
                Room = room.Name,
                Day = GetDayName(day),
                Time = GetTimeLabel(time),
                DayIndex = day,
                TimeIndex = time,
                RoomObj = room
            };

            // Update Data
            DataManager.MasterSchedule.Add(newItem);
            DataManager.FailedAssignments.Remove(fail);

            // Update Flags
            teacher.IsBusy[day, time] = true;
            fail.Section.IsBusy[day, time] = true;
            room.IsBusy[day, time] = true;

            // Refresh UI
            UpdateMasterGrid();
            UpdateTimetableView();
            LoadPendingList();
        }

        // 3. HELPERS (Add these if missing)
        // CHANGE THIS METHOD at the bottom of AdminDashboard.cs
        private string GetDayName(int d)
        {
            // Fix: Make Index 1 = Monday to match the Grid Column Index
            string[] shortNames = { "", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };

            if (d >= 1 && d <= 7) return shortNames[d];
            return "Err";
        }
        private string GetTimeLabel(int t) => $"{7 + t}:00 - {8 + t}:00";
        private string CleanSubjectName(string s) => s.Replace(" (Lec)", "").Replace(" (Lab)", "").Trim();
        #endregion

        private void btnFindSlots_Click(object sender, EventArgs e)
        {
            // 1. Validation
            if (dgvPending.SelectedRows.Count == 0) return;

            var failEntry = (FailedEntry)dgvPending.SelectedRows[0].Tag;
            string subjectName = CleanSubjectName(failEntry.Subject.Code);

            // Find the teacher responsible for this subject
            var teacher = DataManager.Teachers.FirstOrDefault(t => t.QualifiedSubjects.Contains(subjectName));

            if (teacher == null)
            {
                MessageBox.Show("No qualified teacher found in the database for " + subjectName);
                return;
            }

            // 2. Scan for Openings
            string report = $"-- Valid Slots for {failEntry.Subject.Code} ({failEntry.Subject.Units} hrs) --\n";
            report += $"Teacher: {teacher.Name}\n\n";

            int optionsFound = 0;

            // Loop Days (1=Mon to 6=Sat)
            for (int d = 1; d <= 6; d++)
            {
                // Loop Hours (Stop early enough so the block fits)
                for (int t = 0; t <= 11 - failEntry.Subject.Units; t++)
                {
                    bool isFree = true;

                    // Check the whole block (e.g. 3 hours)
                    for (int i = 0; i < failEntry.Subject.Units; i++)
                    {
                        // Conflict Check: Teacher Busy OR Section Busy
                        if (teacher.IsBusy[d, t + i] || failEntry.Section.IsBusy[d, t + i])
                        {
                            isFree = false;
                            break;
                        }
                    }

                    if (isFree)
                    {
                        // Optional: Find which room is free here
                        var roomType = failEntry.Subject.IsLab ? RoomType.Laboratory : RoomType.Lecture;
                        var freeRoom = DataManager.Rooms.FirstOrDefault(r => r.Type == roomType && !r.IsBusy[d, t]);
                        string roomNote = freeRoom != null ? $"({freeRoom.Name})" : "(No Rooms)";

                        report += $"• {GetDayName(d)} @ {GetTimeLabel(t)} {roomNote}\n";
                        optionsFound++;
                    }
                }
            }

            if (optionsFound == 0)
                MessageBox.Show($"Impossible to place. {teacher.Name} and Section {failEntry.Section.Name} have no common free time.");
            else
                MessageBox.Show(report, "Availability Cheat Sheet");
        }

        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null) { MessageBox.Show("Please select a view first."); return; }

            string filterMode = cmbFilterType.SelectedItem?.ToString() ?? "Section";
            string filterValue = comboBox1.SelectedItem.ToString();

            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "PDF File|*.pdf";
            save.FileName = $"{filterMode}_{filterValue}_Schedule.pdf";

            if (save.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    PdfWriter writer = new PdfWriter(save.FileName);
                    PdfDocument pdf = new PdfDocument(writer);
                    pdf.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4.Rotate());
                    Document document = new Document(pdf);
                    document.SetMargins(15, 20, 10, 20);

                    // 1. Header
                    document.Add(GeneratePdfHeader());

                    // 2. Context Title
                    document.Add(new Paragraph($"\nOFFICIAL SCHEDULE: {filterValue} ({filterMode})")
                        .SetTextAlignment(TextAlignment.CENTER).SetBold().SetFontSize(14));

                    // 3. The Colorful Grid
                    document.Add(GenerateAdminScheduleTable(filterMode, filterValue));

                    // 4. The "Approved By" Footer (Added!)
                    document.Add(GeneratePdfFooter());

                    document.Close();
                    MessageBox.Show("PDF Exported Successfully!");
                }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            }
        }

        // --- HELPER 1: HEADER ---
        private Paragraph GeneratePdfHeader()
        {
            return new Paragraph()
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(9)
                .SetMultipliedLeading(1.0f)
                .Add("Republic of the Philippines\n")
                .Add(new Text("Laguna State Polytechnic University\n").SetFontSize(11).SetBold())
                .Add("College of Computer Studies\n\n");
        }

        // --- HELPER 2: FOOTER (New!) ---
        private Table GeneratePdfFooter()
        {
            Table footerTable = new Table(UnitValue.CreatePercentArray(new float[] { 1, 1 }));
            footerTable.SetWidth(UnitValue.CreatePercentValue(100));
            footerTable.SetBorder(iText.Layout.Borders.Border.NO_BORDER);
            footerTable.SetMarginTop(20); // Space above signature

            Cell left = new Cell().Add(new Paragraph("Generated by: SchedCCS Admin System").SetFontSize(8))
                .SetBorder(iText.Layout.Borders.Border.NO_BORDER);

            Cell right = new Cell().Add(new Paragraph("Approved by: ______________________\nCollege Dean")
                .SetFontSize(10).SetBold())
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetBorder(iText.Layout.Borders.Border.NO_BORDER);

            footerTable.AddCell(left);
            footerTable.AddCell(right);
            return footerTable;
        }

        // --- HELPER 3: COLORFUL GRID ---
        private Table GenerateAdminScheduleTable(string filterMode, string filterValue)
        {
            float[] colWidths = { 1.2f, 2, 2, 2, 2, 2, 2, 2 };
            Table table = new Table(UnitValue.CreatePercentArray(colWidths));
            table.SetWidth(UnitValue.CreatePercentValue(100));

            // Headers
            string[] headers = { "TIME", "MONDAY", "TUESDAY", "WEDNESDAY", "THURSDAY", "FRIDAY", "SATURDAY", "SUNDAY" };
            foreach (string h in headers)
            {
                table.AddCell(new Cell().Add(new Paragraph(h).SetBold().SetFontSize(7))
                    .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                    .SetTextAlignment(TextAlignment.CENTER));
            }

            // Rows
            for (int t = 0; t < 11; t++)
            {
                string timeLabel = $"{7 + t}:00 - {8 + t}:00";
                table.AddCell(new Cell().Add(new Paragraph(timeLabel).SetFontSize(7).SetBold())
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .SetHeight(25));

                for (int d = 1; d <= 7; d++)
                {
                    var item = DataManager.MasterSchedule.FirstOrDefault(s =>
                        s.DayIndex == d && s.TimeIndex == t &&
                        ((filterMode == "Section" && s.Section == filterValue) ||
                         (filterMode == "Teacher" && s.Teacher == filterValue) ||
                         (filterMode == "Room" && s.Room == filterValue))
                    );

                    Cell cell = new Cell().SetHeight(25).SetPadding(1);

                    if (item != null)
                    {
                        string content = "";
                        if (filterMode == "Section") content = $"{item.Subject}\n{item.Teacher}\n{item.Room}";
                        else if (filterMode == "Teacher") content = $"{item.Subject}\n{item.Section}\n{item.Room}";
                        else content = $"{item.Subject}\n{item.Section}\n{item.Teacher}";

                        cell.Add(new Paragraph(content).SetFontSize(7).SetMultipliedLeading(0.9f).SetTextAlignment(TextAlignment.CENTER));

                        // --- COLOR LOGIC (FIXED) ---
                        if (item.Subject.Contains("(Lab)"))
                        {
                            cell.SetBackgroundColor(new DeviceRgb(255, 160, 122)); // Light Salmon
                        }
                        else
                        {
                            // Generate Random Pastel Color based on Subject Name
                            System.Drawing.Color rndColor = GetSubjectColor(item.Subject);
                            cell.SetBackgroundColor(new DeviceRgb(rndColor.R, rndColor.G, rndColor.B));
                        }
                    }
                    table.AddCell(cell);
                }
            }
            return table;
        }

        // --- HELPER 4: COLOR GENERATOR (Same as Student Dashboard) ---

        private void btnRestoreDatabase_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "System Backup File|*.json";

            if (open.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string jsonString = File.ReadAllText(open.FileName);

                    // Deserialize (Convert text back to Objects)
                    SystemBackup backup = JsonSerializer.Deserialize<SystemBackup>(jsonString);

                    if (backup != null)
                    {
                        // 1. Restore Data
                        DataManager.Rooms = backup.Rooms ?? new List<Room>();
                        DataManager.Teachers = backup.Teachers ?? new List<Teacher>();
                        DataManager.Sections = backup.Sections ?? new List<Section>();
                        DataManager.MasterSchedule = backup.MasterSchedule ?? new List<ScheduleItem>();
                        DataManager.FailedAssignments = backup.FailedAssignments ?? new List<FailedEntry>();

                        // 2. Critical: Re-Link the "RoomObj" references
                        // JSON saves data, but it breaks the "Links" between objects. We must rebuild them.
                        RebuildBusyArrays(DataManager.MasterSchedule);

                        // 3. Refresh UI
                        RefreshAdminLists();     // Updates the side lists
                        UpdateMasterGrid();      // Updates the main view
                        UpdateTimetableView();   // Updates the calendar view

                        MessageBox.Show("System Data Restored Successfully!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Restore Failed: " + ex.Message);
                }
            }
        }
    }

    // Simple container for all system data
    public class SystemBackup
    {
        public List<Room> Rooms { get; set; }
        public List<Teacher> Teachers { get; set; }
        public List<Section> Sections { get; set; }
        public List<ScheduleItem> MasterSchedule { get; set; }
        public List<FailedEntry> FailedAssignments { get; set; }
    }
}