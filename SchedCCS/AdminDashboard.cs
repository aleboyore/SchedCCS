using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO; // Needed for File IO (CSV)
using iText.Kernel.Pdf; // Needed for PDF
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;

namespace SchedCCS
{
    public partial class AdminDashboard : Form
    {
        // =============================================================
        // 1. FIELDS & STATE MANAGEMENT (Encapsulation)
        // =============================================================

        // Flags for sorting logic in the Master Schedule Grid
        private string currentSortColumn = "";
        private bool isAscending = true;

        // Tracking IDs for the "Edit/Update" functionality
        private int editingTeacherId = -1;
        private int editingRoomId = -1;
        private int editingSectionId = -1;
        private string editingSubjectCode = "";

        // Optimization flag to prevent unnecessary schedule regeneration
        private bool isDataDirty = true;

        // Role property to manage access control (Admin vs Student view)
        public bool IsAdmin { get; set; } = true;

        // =============================================================
        // 2. CONSTRUCTOR & FORM LOAD
        // =============================================================

        public AdminDashboard()
        {
            InitializeComponent();
            // Initialize the Admin Lists immediately upon creation to ensure data visibility
            RefreshAdminLists();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            // Apply role-based UI restrictions
            if (IsAdmin == false)
            {
                button1.Visible = false; // Hide Generate button
                this.Text = "Student Schedule Viewer";
                MessageBox.Show("Student View: Please select your section from the dropdown.");
            }
            else
            {
                this.Text = "Admin Dashboard - Schedule Generator";
            }

            RefreshSectionDropdown();
        }

        // =============================================================
        // 3. CORE SCHEDULING LOGIC
        // =============================================================

        // Centralized method to execute the scheduling algorithm and update all views.
        private void RunScheduleGeneration()
        {
            // 1. State Reset: Clear previous 'Busy' flags for all resources
            foreach (var r in DataManager.Rooms) r.IsBusy = new bool[6, 13];
            foreach (var t in DataManager.Teachers) t.IsBusy = new bool[6, 13];
            foreach (var s in DataManager.Sections) s.IsBusy = new bool[6, 13];

            // 2. Algorithm Execution: Instantiate and run the Greedy Algorithm
            ScheduleGenerator generator = new ScheduleGenerator(DataManager.Rooms, DataManager.Teachers, DataManager.Sections);
            generator.Generate();

            // 3. Data Persistence: Save result to the Shared DataManager
            DataManager.MasterSchedule = generator.GeneratedSchedule;

            // 4. UI Synchronization: Refresh Dropdowns and Grids
            string currentSelection = comboBox1.SelectedItem?.ToString();
            comboBox1.Items.Clear();
            foreach (var section in DataManager.Sections) comboBox1.Items.Add(section.Name);

            if (currentSelection != null && comboBox1.Items.Contains(currentSelection))
            {
                comboBox1.SelectedItem = currentSelection;
            }

            // Update Master List Grid
            if (dgvMaster != null)
            {
                dgvMaster.DataSource = null;
                dgvMaster.DataSource = DataManager.MasterSchedule;
                dgvMaster.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }

            // Update Calendar View Grid
            if (comboBox1.SelectedItem != null)
            {
                string mode = cmbFilterType.SelectedItem?.ToString() ?? "Section";
                DisplayTimetable(comboBox1.SelectedItem.ToString(), mode);
            }

            // Reset dirty flag as data is now synchronized
            isDataDirty = false;
        }

        // Manual trigger for schedule generation
        private void button1_Click(object sender, EventArgs e)
        {
            RunScheduleGeneration();
            MessageBox.Show("Schedule Generated Successfully!");
        }

        // =============================================================
        // 4. VISUALIZATION & GRID RENDERING
        // =============================================================

        // Renders the schedule into a readable timetable format (Calendar View).
        private void DisplayTimetable(string filterValue, string filterMode)
        {
            // Reset Grid
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            // Setup Columns (Time + 7 Days)
            dataGridView1.Columns.Add("Time", "TIME");
            string[] days = { "MONDAY", "TUESDAY", "WEDNESDAY", "THURSDAY", "FRIDAY", "SATURDAY", "SUNDAY" };
            foreach (var day in days) dataGridView1.Columns.Add(day, day);

            // Styling configuration
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.Columns["Time"].FillWeight = 50;
            dataGridView1.ScrollBars = ScrollBars.None; // Fixed layout (No scroll)

            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            // Dynamic Height Calculation: Fits rows to screen height
            int availableHeight = dataGridView1.Height - dataGridView1.ColumnHeadersHeight;
            int exactRowHeight = availableHeight / 11;

            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView1.RowTemplate.Height = exactRowHeight;

            // Cleanup UI
            foreach (DataGridViewColumn col in dataGridView1.Columns) col.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;

            // Setup Rows (7:00 AM - 6:00 PM)
            for (int hour = 7; hour < 18; hour++)
            {
                string rawTime = $"{hour}:00 - {hour + 1}:00";
                string niceTime = ToSimple12Hour(rawTime);

                int rowIndex = dataGridView1.Rows.Add(niceTime, "", "", "", "", "", "", "");
                dataGridView1.Rows[rowIndex].Height = exactRowHeight;
            }

            // Data Filtering based on Admin Selection
            List<ScheduleItem> filteredList = new List<ScheduleItem>();
            if (filterMode == "Section")
                filteredList = DataManager.MasterSchedule.Where(x => x.Section == filterValue).ToList();
            else if (filterMode == "Teacher")
                filteredList = DataManager.MasterSchedule.Where(x => x.Teacher == filterValue).ToList();
            else if (filterMode == "Room")
                filteredList = DataManager.MasterSchedule.Where(x => x.Room == filterValue).ToList();

            // Populate Cells
            foreach (var item in filteredList)
            {
                int startHour = int.Parse(item.Time.Split(':')[0]);
                int rowIndex = startHour - 7;

                int colIndex = 0;
                switch (item.Day)
                {
                    case "Mon": colIndex = 1; break;
                    case "Tue": colIndex = 2; break;
                    case "Wed": colIndex = 3; break;
                    case "Thu": colIndex = 4; break;
                    case "Fri": colIndex = 5; break;
                    case "Sat": colIndex = 6; break;
                    case "Sun": colIndex = 7; break;
                }

                if (rowIndex >= 0 && rowIndex < dataGridView1.Rows.Count)
                {
                    string cellText = "";
                    if (filterMode == "Section") cellText = $"{item.Subject}\n{item.Teacher}\n{item.Room}";
                    else if (filterMode == "Teacher") cellText = $"{item.Subject}\n{item.Section}\n{item.Room}";
                    else if (filterMode == "Room") cellText = $"{item.Subject}\n{item.Section}\n{item.Teacher}";

                    dataGridView1.Rows[rowIndex].Cells[colIndex].Value = cellText;

                    // --- THE FIX: Use 'System.Drawing.Color' explicitly ---
                    if (item.Subject.Contains("(Lab)"))
                        dataGridView1.Rows[rowIndex].Cells[colIndex].Style.BackColor = System.Drawing.Color.LightSalmon;
                    else
                        dataGridView1.Rows[rowIndex].Cells[colIndex].Style.BackColor = GetSubjectColor(item.Subject);
                }
            }
            dataGridView1.ClearSelection();
        }

        // Handles Tab Switching logic for Smart Updates
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabControl1.TabPages["tabSchedule"] ||
                tabControl1.SelectedTab == tabControl1.TabPages["tabMaster"])
            {
                if (isDataDirty) RunScheduleGeneration();
            }
            else if (tabControl1.SelectedTab == tabControl1.TabPages["tabManage"])
            {
                RefreshAdminLists();
            }
        }

        // Handle Grid Resizing (Responsive Design)
        private void dataGridView1_Resize(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0 || !dataGridView1.Visible) return;
            try
            {
                int availableHeight = dataGridView1.Height - dataGridView1.ColumnHeadersHeight;
                int newRowHeight = availableHeight / 11;
                if (newRowHeight < 20) newRowHeight = 20;

                foreach (DataGridViewRow row in dataGridView1.Rows) row.Height = newRowHeight;
            }
            catch { }
        }

        // =============================================================
        // 5. DATA MANAGEMENT (CRUD OPERATIONS)
        // =============================================================

        #region Teacher Management
        private void btnAddTeacher_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTeacherName.Text)) { MessageBox.Show("Please enter a teacher name."); return; }

            Teacher newTeacher = new Teacher();
            newTeacher.Id = DataManager.Teachers.Count + 1;
            newTeacher.Name = txtTeacherName.Text;

            if (!string.IsNullOrWhiteSpace(txtTeacherSubjects.Text))
            {
                string[] subs = txtTeacherSubjects.Text.Split(',');
                foreach (var s in subs) newTeacher.QualifiedSubjects.Add(s.Trim());
            }

            DataManager.Teachers.Add(newTeacher);
            MessageBox.Show($"Teacher {newTeacher.Name} added!");

            txtTeacherName.Clear();
            txtTeacherSubjects.Clear();
            RefreshAdminLists();
            isDataDirty = true;
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
                    string[] subs = txtTeacherSubjects.Text.Split(',');
                    foreach (var s in subs) teacher.QualifiedSubjects.Add(s.Trim());
                }

                RefreshAdminLists();
                MessageBox.Show("Teacher Updated!");

                // Reset
                txtTeacherName.Clear();
                txtTeacherSubjects.Clear();
                editingTeacherId = -1;
                btnUpdateTeacher.Enabled = false;
                btnAddTeacher.Enabled = true;
            }
            isDataDirty = true;
        }

        private void btnDeleteTeacher_Click(object sender, EventArgs e)
        {
            if (lstTeachers.SelectedItem == null) return;

            string selectedText = lstTeachers.SelectedItem.ToString();
            string name = selectedText.Split('(')[0].Trim();

            var teacherToRemove = DataManager.Teachers.FirstOrDefault(t => t.Name == name);
            if (teacherToRemove != null)
            {
                DataManager.Teachers.Remove(teacherToRemove);
                RefreshAdminLists();
                MessageBox.Show("Teacher removed.");
            }
            isDataDirty = true;
        }

        private void btnCancelTeacher_Click(object sender, EventArgs e)
        {
            txtTeacherName.Clear();
            txtTeacherSubjects.Clear();
            editingTeacherId = -1;
            btnUpdateTeacher.Enabled = false;
            btnAddTeacher.Enabled = true;
        }
        #endregion

        #region Room Management
        private void btnAddRoom_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRoomName.Text)) { MessageBox.Show("Please enter a room name."); return; }
            if (cmbRoomType.SelectedItem == null) { MessageBox.Show("Please select a room type."); return; }

            Room newRoom = new Room();
            newRoom.Id = DataManager.Rooms.Count + 1;
            newRoom.Name = txtRoomName.Text;
            newRoom.Type = cmbRoomType.SelectedItem.ToString() == "Laboratory" ? RoomType.Laboratory : RoomType.Lecture;

            DataManager.Rooms.Add(newRoom);
            MessageBox.Show($"Room '{newRoom.Name}' added successfully!");

            txtRoomName.Clear();
            cmbRoomType.SelectedIndex = -1;
            RefreshAdminLists();
            isDataDirty = true;
        }

        private void btnUpdateRoom_Click(object sender, EventArgs e)
        {
            if (editingRoomId == -1) return;
            var room = DataManager.Rooms.FirstOrDefault(r => r.Id == editingRoomId);

            if (room != null)
            {
                room.Name = txtRoomName.Text;
                room.Type = cmbRoomType.SelectedItem.ToString() == "Laboratory" ? RoomType.Laboratory : RoomType.Lecture;

                RefreshAdminLists();
                MessageBox.Show("Room Updated!");

                txtRoomName.Clear();
                cmbRoomType.SelectedIndex = -1;
                editingRoomId = -1;
                btnUpdateRoom.Enabled = false;
                btnAddRoom.Enabled = true;
            }
            isDataDirty = true;
        }

        private void btnDeleteRoom_Click(object sender, EventArgs e)
        {
            if (lstRooms.SelectedItem == null) { MessageBox.Show("Please select a room to delete."); return; }

            string selectedText = lstRooms.SelectedItem.ToString();
            string name = selectedText.Split('|')[0].Trim(); // Split by Pipe

            var roomToRemove = DataManager.Rooms.FirstOrDefault(r => r.Name == name);
            if (roomToRemove != null)
            {
                DataManager.Rooms.Remove(roomToRemove);
                RefreshAdminLists();

                txtRoomName.Clear();
                editingRoomId = -1;
                btnUpdateRoom.Enabled = false;
                btnAddRoom.Enabled = true;
                MessageBox.Show("Room deleted.");
            }
            isDataDirty = true;
        }

        private void btnCancelRoom_Click(object sender, EventArgs e)
        {
            txtRoomName.Clear();
            cmbRoomType.SelectedIndex = -1;
            editingRoomId = -1;
            btnUpdateRoom.Enabled = false;
            btnAddRoom.Enabled = true;
        }
        #endregion

        #region Section & Subject Management
        private void btnCreateSection_Click(object sender, EventArgs e)
        {
            // 1. Validation
            if (string.IsNullOrWhiteSpace(txtSectionName.Text))
            {
                MessageBox.Show("Please enter a Section Name.");
                return;
            }
            if (cmbSectionProgram.SelectedItem == null || cmbSectionYear.SelectedItem == null)
            {
                MessageBox.Show("Please select a Program and Year Level.");
                return;
            }

            // 2. Capture Inputs
            string program = cmbSectionProgram.SelectedItem.ToString();
            int yearLevel = int.Parse(cmbSectionYear.SelectedItem.ToString());

            // SCENARIO A: UPDATE EXISTING SECTION
            if (editingSectionId != -1)
            {
                var section = DataManager.Sections.FirstOrDefault(s => s.Id == editingSectionId);
                if (section != null)
                {
                    section.Name = txtSectionName.Text;
                    section.Program = program;     // Update Program
                    section.YearLevel = yearLevel; // Update Year
                    MessageBox.Show("Section Updated!");
                }

                // Reset Mode
                editingSectionId = -1;
                btnCreateSection.Text = "Create";
            }
            // SCENARIO B: CREATE NEW SECTION
            else
            {
                Section newSection = new Section();
                newSection.Id = DataManager.Sections.Count + 1;
                newSection.Name = txtSectionName.Text;
                newSection.Program = program;      // Save Program
                newSection.YearLevel = yearLevel;  // Save Year

                DataManager.Sections.Add(newSection);
                MessageBox.Show($"Section {newSection.Name} created!");
            }

            // 3. Cleanup
            txtSectionName.Clear();
            cmbSectionProgram.SelectedIndex = -1;
            cmbSectionYear.SelectedIndex = -1;

            RefreshAdminLists();
            RefreshSectionDropdown();
            isDataDirty = true;
        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            Section targetSection = null;

            // 1. Update Section Name
            if (editingSectionId != -1)
            {
                targetSection = DataManager.Sections.FirstOrDefault(s => s.Id == editingSectionId);
                if (targetSection != null && !string.IsNullOrWhiteSpace(txtSectionName.Text))
                {
                    targetSection.Name = txtSectionName.Text;
                }
            }

            // 2. Update Subject Details
            if (targetSection != null && !string.IsNullOrEmpty(editingSubjectCode))
            {
                var subject = targetSection.SubjectsToTake.FirstOrDefault(s => s.Code.Contains(editingSubjectCode));

                if (subject != null)
                {
                    string newCode = txtSubjCode.Text.Trim();

                    // Logic to maintain suffix
                    if (chkIsLab.Checked && !newCode.Contains("(Lab)")) subject.Code = newCode + " (Lab)";
                    else if (!chkIsLab.Checked && !newCode.Contains("(Lec)")) subject.Code = newCode;
                    else subject.Code = newCode;

                    int.TryParse(txtUnits.Text, out int u);
                    subject.Units = u;
                    subject.IsLab = chkIsLab.Checked;
                }
            }

            RefreshAdminLists();
            RefreshSectionDropdown();

            if (targetSection != null)
            {
                cmbSectionList.SelectedItem = targetSection.Name;
                RefreshSubjectList();
            }

            MessageBox.Show("Changes Saved Successfully!");
            isDataDirty = true;
        }

        private void btnAddSubject_Click(object sender, EventArgs e)
        {
            if (cmbSectionList.SelectedItem == null) { MessageBox.Show("Please select a Section first!"); return; }
            if (!int.TryParse(txtUnits.Text, out int inputUnits) || string.IsNullOrWhiteSpace(txtSubjCode.Text))
            {
                MessageBox.Show("Invalid Code or Units.");
                return;
            }

            string selectedSectionName = cmbSectionList.SelectedItem.ToString();
            Section targetSection = DataManager.Sections.FirstOrDefault(s => s.Name == selectedSectionName);

            if (targetSection == null) return;

            if (chkIsLab.Checked)
            {
                Subject lecPart = new Subject { Code = txtSubjCode.Text + " (Lec)", IsLab = false, Units = inputUnits - 1 };
                Subject labPart = new Subject { Code = txtSubjCode.Text + " (Lab)", IsLab = true, Units = 3 };

                targetSection.SubjectsToTake.Add(lecPart);
                targetSection.SubjectsToTake.Add(labPart);
                MessageBox.Show($"Added {txtSubjCode.Text} as:\n- Lecture ({lecPart.Units} hrs)\n- Lab ({labPart.Units} hrs)");
            }
            else
            {
                Subject newSub = new Subject { Code = txtSubjCode.Text, IsLab = false, Units = inputUnits };
                targetSection.SubjectsToTake.Add(newSub);
                MessageBox.Show($"Added {newSub.Code} ({newSub.Units} hrs)");
            }

            txtSubjCode.Clear();
            txtUnits.Clear();
            chkIsLab.Checked = false;

            RefreshSubjectList();
            isDataDirty = true;
        }

        private void btnRemoveSubject_Click(object sender, EventArgs e)
        {
            if (cmbSectionList.SelectedItem == null || lstSectionSubjects.SelectedItem == null)
            {
                MessageBox.Show("Please select a subject to remove.");
                return;
            }

            string sectionName = cmbSectionList.SelectedItem.ToString();
            Section selectedSection = DataManager.Sections.FirstOrDefault(s => s.Name == sectionName);

            if (selectedSection != null)
            {
                string selectedText = lstSectionSubjects.SelectedItem.ToString();
                string subjectCode = selectedText.Split('-')[0].Trim();

                var subjectToRemove = selectedSection.SubjectsToTake.FirstOrDefault(s => selectedText.Contains(s.Code));

                if (subjectToRemove != null)
                {
                    selectedSection.SubjectsToTake.Remove(subjectToRemove);
                    RefreshSubjectList();
                    MessageBox.Show("Subject removed.");
                }
            }
            isDataDirty = true;
        }

        private void btnDeleteSection_Click(object sender, EventArgs e)
        {
            if (lstSections.SelectedItem == null) return;
            string name = lstSections.SelectedItem.ToString();

            var sectionToRemove = DataManager.Sections.FirstOrDefault(s => s.Name == name);
            if (sectionToRemove != null)
            {
                if (MessageBox.Show($"Delete {name} and all subjects?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DataManager.Sections.Remove(sectionToRemove);
                    RefreshAdminLists();
                    MessageBox.Show("Section deleted.");
                }
            }
            isDataDirty = true;
        }

        private void btnCancelSubject_Click(object sender, EventArgs e)
        {
            txtSubjCode.Clear();
            txtUnits.Clear();
            chkIsLab.Checked = false;
            editingSubjectCode = "";
            btnAddSubject.Enabled = true;

            txtSectionName.Clear();
            cmbSectionProgram.SelectedIndex = -1; // Clear Program
            cmbSectionYear.SelectedIndex = -1;    // Clear Year
            editingSectionId = -1;
            btnCreateSection.Text = "Create";
        }
        #endregion

        // =============================================================
        // 6. HELPER FUNCTIONS
        // =============================================================

        private void RefreshSectionDropdown()
        {
            cmbSectionList.Items.Clear();
            foreach (var s in DataManager.Sections) cmbSectionList.Items.Add(s.Name);
        }

        private void RefreshAdminLists()
        {
            lstTeachers.Items.Clear();
            foreach (var t in DataManager.Teachers)
                lstTeachers.Items.Add($"{t.Name} ({string.Join(", ", t.QualifiedSubjects)})");

            lstRooms.Items.Clear();
            foreach (var r in DataManager.Rooms)
                lstRooms.Items.Add($"{r.Name} | {r.Type}");

            lstSections.Items.Clear();
            foreach (var s in DataManager.Sections)
                lstSections.Items.Add(s.Name);

            RefreshSectionDropdown();
        }

        private void RefreshSubjectList()
        {
            if (cmbSectionList.SelectedItem == null) { lstSectionSubjects.Items.Clear(); return; }

            string sectionName = cmbSectionList.SelectedItem.ToString();
            Section selectedSection = DataManager.Sections.FirstOrDefault(s => s.Name == sectionName);

            lstSectionSubjects.Items.Clear();
            if (selectedSection != null)
            {
                foreach (var sub in selectedSection.SubjectsToTake)
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

            int red = r.Next(160, 255);
            int green = r.Next(160, 255);
            int blue = r.Next(160, 255);

            return System.Drawing.Color.FromArgb(red, green, blue);
        }

        // =============================================================
        // 7. EVENT LISTENERS (List Selections & Sorting)
        // =============================================================

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
            string selectedText = lstTeachers.SelectedItem.ToString();
            string name = selectedText.Split('(')[0].Trim();

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
            string selectedText = lstRooms.SelectedItem.ToString();
            string name = selectedText.Split('|')[0].Trim();

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

            string selectedText = lstSectionSubjects.SelectedItem.ToString();
            string code = selectedText.Split('-')[0].Trim();
            editingSubjectCode = code;

            string sectionName = cmbSectionList.SelectedItem.ToString();
            var section = DataManager.Sections.FirstOrDefault(s => s.Name == sectionName);
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
                // 1. Load ID and Name
                editingSectionId = section.Id;
                txtSectionName.Text = section.Name;

                // 2. Load Program and Year into Dropdowns
                if (cmbSectionProgram.Items.Contains(section.Program))
                    cmbSectionProgram.SelectedItem = section.Program;

                if (cmbSectionYear.Items.Contains(section.YearLevel.ToString()))
                    cmbSectionYear.SelectedItem = section.YearLevel.ToString();

                // 3. Switch Button Mode
                btnCreateSection.Text = "Update Section";

                // 4. Sync with middle dropdown
                if (cmbSectionList.Items.Contains(section.Name))
                    cmbSectionList.SelectedItem = section.Name;
            }
        }

        private void cmbSectionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshSubjectList();
        }

        private void dgvMaster_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string columnClicked = dgvMaster.Columns[e.ColumnIndex].Name;
            if (currentSortColumn == columnClicked) isAscending = !isAscending;
            else { currentSortColumn = columnClicked; isAscending = true; }

            if (columnClicked == "Day")
                DataManager.MasterSchedule = isAscending ? DataManager.MasterSchedule.OrderBy(x => GetDayIndex(x.Day)).ToList() : DataManager.MasterSchedule.OrderByDescending(x => GetDayIndex(x.Day)).ToList();
            else if (columnClicked == "Time")
                DataManager.MasterSchedule = isAscending ? DataManager.MasterSchedule.OrderBy(x => GetTimeIndex(x.Time)).ToList() : DataManager.MasterSchedule.OrderByDescending(x => GetTimeIndex(x.Time)).ToList();
            else
            {
                // Default sorting logic
                switch (columnClicked)
                {
                    case "Section": DataManager.MasterSchedule = isAscending ? DataManager.MasterSchedule.OrderBy(x => x.Section).ToList() : DataManager.MasterSchedule.OrderByDescending(x => x.Section).ToList(); break;
                    case "Subject": DataManager.MasterSchedule = isAscending ? DataManager.MasterSchedule.OrderBy(x => x.Subject).ToList() : DataManager.MasterSchedule.OrderByDescending(x => x.Subject).ToList(); break;
                    case "Teacher": DataManager.MasterSchedule = isAscending ? DataManager.MasterSchedule.OrderBy(x => x.Teacher).ToList() : DataManager.MasterSchedule.OrderByDescending(x => x.Teacher).ToList(); break;
                    case "Room": DataManager.MasterSchedule = isAscending ? DataManager.MasterSchedule.OrderBy(x => x.Room).ToList() : DataManager.MasterSchedule.OrderByDescending(x => x.Room).ToList(); break;
                }
            }

            dgvMaster.DataSource = null;
            dgvMaster.DataSource = DataManager.MasterSchedule;
        }

        private int GetDayIndex(string day)
        {
            switch (day) { case "Mon": return 1; case "Tue": return 2; case "Wed": return 3; case "Thu": return 4; case "Fri": return 5; case "Sat": return 6; default: return 7; }
        }

        private int GetTimeIndex(string timeRange)
        {
            string startHour = timeRange.Split(':')[0];
            if (int.TryParse(startHour, out int h)) return h;
            return 0;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Log out and return to Login screen?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0 || DataManager.MasterSchedule.Count == 0)
            {
                MessageBox.Show("No schedule to export! Please generate one first.");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV File|*.csv";
            saveFileDialog.Title = "Save Schedule";
            saveFileDialog.FileName = "Schedule.csv";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                    {
                        writer.WriteLine("Section,Subject,Teacher,Room,Day,Time");
                        foreach (var item in DataManager.MasterSchedule)
                            writer.WriteLine($"{item.Section},{item.Subject},{item.Teacher},{item.Room},{item.Day},{item.Time}");
                    }
                    MessageBox.Show("Export Successful!");
                }
                catch (Exception ex) { MessageBox.Show("Error saving file: " + ex.Message); }
            }
        }

        private void btnBatchAdd_Click(object sender, EventArgs e)
        {
            // 1. Validation
            if (cmbBatchProgram.SelectedItem == null || cmbBatchYear.SelectedItem == null)
            {
                MessageBox.Show("Please select a Program and Year Level.");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtBatchCode.Text) || string.IsNullOrWhiteSpace(txtBatchUnits.Text))
            {
                MessageBox.Show("Please enter subject details.");
                return;
            }

            // 2. Get Target Criteria
            string targetProgram = cmbBatchProgram.SelectedItem.ToString();
            int targetYear = int.Parse(cmbBatchYear.SelectedItem.ToString());

            // 3. Parse Subject Details
            string baseCode = txtBatchCode.Text.Trim();
            int.TryParse(txtBatchUnits.Text, out int units);
            bool isLab = chkBatchLab.Checked;

            // 4. FIND MATCHING SECTIONS (The Algorithm)
            // We look for all sections that match the Program AND Year
            var targetSections = DataManager.Sections
                .Where(s => s.Program == targetProgram && s.YearLevel == targetYear)
                .ToList();

            if (targetSections.Count == 0)
            {
                MessageBox.Show($"No sections found for {targetProgram} Year {targetYear}.");
                return;
            }

            // 5. LOOP AND ADD
            int count = 0;
            foreach (var section in targetSections)
            {
                // Check if subject already exists to prevent duplicates
                if (section.SubjectsToTake.Any(sub => sub.Code.Contains(baseCode)))
                    continue; // Skip this section

                if (isLab)
                {
                    // Split Logic (2 Units Lec + 3 Units Lab)
                    section.SubjectsToTake.Add(new Subject { Code = baseCode + " (Lec)", Units = units - 1, IsLab = false });
                    section.SubjectsToTake.Add(new Subject { Code = baseCode + " (Lab)", Units = 3, IsLab = true });
                }
                else
                {
                    // Regular Logic
                    section.SubjectsToTake.Add(new Subject { Code = baseCode, Units = units, IsLab = false });
                }
                count++;
            }

            // 6. Success Message
            MessageBox.Show($"Success! Added {baseCode} to {count} sections.");

            // Clear inputs
            txtBatchCode.Clear();
            txtBatchUnits.Clear();
            chkBatchLab.Checked = false;

            // Refresh lists to show changes
            RefreshSubjectList();
            isDataDirty = true;
        }
    }
}