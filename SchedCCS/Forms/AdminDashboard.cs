using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.Drawing; // Used for UI
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace SchedCCS
{
    public partial class AdminDashboard : Form
    {
        #region Fields

        private readonly ScheduleService _scheduleService;

        // Grid Sorting
        private string currentSortColumn = "";
        private bool isAscending = true;

        // Edit Tracking
        private int editingTeacherId = -1;
        private int editingRoomId = -1;
        private int editingSectionId = -1;
        private string editingSubjectCode = "";

        // Application State
        private bool isDataDirty = true;
        public bool IsAdmin { get; set; } = true;

        #endregion

        #region 1. Initialization

        public AdminDashboard()
        {
            InitializeComponent();

            // --- DOUBLE BUFFERING (Anti-Flicker) ---
            SetDoubleBuffered(pnlContent);      // Main Container
            SetDoubleBuffered(pnlViewSchedule); // Grid View
            SetDoubleBuffered(pnlViewMaster);   // Master List
            SetDoubleBuffered(pnlViewManage);   // Manage Data
            SetDoubleBuffered(pnlViewPending);  // Pending View

            // Buffer the NEW Sub-Panels (Manage Data Views)
            if (this.Controls.Find("pnlViewRooms", true).Length > 0)
                SetDoubleBuffered(this.Controls.Find("pnlViewRooms", true)[0]);
            if (this.Controls.Find("pnlViewTeachers", true).Length > 0)
                SetDoubleBuffered(this.Controls.Find("pnlViewTeachers", true)[0]);
            if (this.Controls.Find("pnlViewSections", true).Length > 0)
                SetDoubleBuffered(this.Controls.Find("pnlViewSections", true)[0]);

            // Initialize Logic
            _scheduleService = new ScheduleService();
            ClearManageInputs();
            RefreshAdminLists();

            // Set Default View
            ShowView(pnlViewSchedule);
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
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

        private void ClearManageInputs()
        {
            cmbSectionProgram.SelectedIndex = -1;
            cmbSectionYear.SelectedIndex = -1;
            cmbRoomType.SelectedIndex = -1;
            cmbBatchProgram.SelectedIndex = -1;
            cmbBatchYear.SelectedIndex = -1;
        }

        #endregion

        #region 2. Navigation Logic (Main & Sub-Nav)

        private void ShowView(Panel panelToShow)
        {
            // Hide all main panels
            pnlViewSchedule.Visible = false;
            pnlViewMaster.Visible = false;
            pnlViewManage.Visible = false;
            pnlViewPending.Visible = false;

            // Show target
            panelToShow.Visible = true;
            panelToShow.BringToFront();
        }

        private void btnNavSchedule_Click(object sender, EventArgs e)
        {
            ShowView(pnlViewSchedule);

            // Initialize filters if empty
            if (cmbFilterType.SelectedIndex == -1) cmbFilterType.SelectedIndex = 0;
            if (cmbScheduleView.Items.Count == 0) UpdateTimetableView();

            // Refresh grid if data exists
            if (DataManager.MasterSchedule != null && DataManager.MasterSchedule.Count > 0)
            {
                if (cmbScheduleView.SelectedItem != null)
                {
                    string mode = cmbFilterType.SelectedItem?.ToString() ?? "Section";
                    DisplayTimetable(cmbScheduleView.SelectedItem.ToString(), mode);
                }
            }
        }

        private void btnNavMaster_Click(object sender, EventArgs e)
        {
            ShowView(pnlViewMaster);
            UpdateMasterGrid();
        }

        private void btnNavManage_Click(object sender, EventArgs e)
        {
            ShowView(pnlViewManage);

            // Default to Rooms View when entering Manage Data
            // Ensure these controls exist before accessing
            if (this.Controls.Find("pnlViewRooms", true).Length > 0 && this.Controls.Find("btnSubNavRooms", true).Length > 0)
            {
                SwitchManageDataView((Panel)this.Controls.Find("pnlViewRooms", true)[0], (Button)this.Controls.Find("btnSubNavRooms", true)[0]);
            }

            RefreshAdminLists();
        }

        private void btnNavPending_Click(object sender, EventArgs e)
        {
            ShowView(pnlViewPending);
            LoadPendingList();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Log out and return to Login screen?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                this.Close();
        }

        // --- SUB-NAVIGATION LOGIC (Manage Data Panels) ---

        private void SwitchManageDataView(Panel panelToShow, Button activeBtn)
        {
            // 1. Hide all Sub-Panels (Using Find for safety if code-behind)
            var roomsPanel = this.Controls.Find("pnlViewRooms", true).FirstOrDefault() as Panel;
            var teachersPanel = this.Controls.Find("pnlViewTeachers", true).FirstOrDefault() as Panel;
            var sectionsPanel = this.Controls.Find("pnlViewSections", true).FirstOrDefault() as Panel;

            if (roomsPanel != null) roomsPanel.Visible = false;
            if (teachersPanel != null) teachersPanel.Visible = false;
            if (sectionsPanel != null) sectionsPanel.Visible = false;

            // 2. Show the selected one
            if (panelToShow != null)
            {
                panelToShow.Visible = true;
                panelToShow.Dock = DockStyle.Fill;
            }

            // 3. Update Button Styles
            ResetSubNavButtons();
            if (activeBtn != null) activeBtn.BackColor = System.Drawing.Color.LightBlue;
        }

        private void ResetSubNavButtons()
        {
            System.Drawing.Color defaultColor = System.Drawing.Color.WhiteSmoke;

            var btnRooms = this.Controls.Find("btnSubNavRooms", true).FirstOrDefault() as Button;
            var btnTeachers = this.Controls.Find("btnSubNavTeachers", true).FirstOrDefault() as Button;
            var btnSections = this.Controls.Find("btnSubNavSections", true).FirstOrDefault() as Button;

            if (btnRooms != null) btnRooms.BackColor = defaultColor;
            if (btnTeachers != null) btnTeachers.BackColor = defaultColor;
            if (btnSections != null) btnSections.BackColor = defaultColor;
        }

        private void btnSubNavRooms_Click(object sender, EventArgs e)
        {
            var pnl = this.Controls.Find("pnlViewRooms", true).FirstOrDefault() as Panel;
            SwitchManageDataView(pnl, (Button)sender);
        }

        private void btnSubNavTeachers_Click(object sender, EventArgs e)
        {
            var pnl = this.Controls.Find("pnlViewTeachers", true).FirstOrDefault() as Panel;
            SwitchManageDataView(pnl, (Button)sender);
        }

        private void btnSubNavSections_Click(object sender, EventArgs e)
        {
            var pnl = this.Controls.Find("pnlViewSections", true).FirstOrDefault() as Panel;
            SwitchManageDataView(pnl, (Button)sender);
        }

        #endregion

        #region 3. Schedule Generation Logic

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            // Force refresh from database
            DataManager.Initialize();

            // Validation
            if (DataManager.Rooms.Count == 0 || DataManager.Teachers.Count == 0 || DataManager.Sections.Count == 0)
            {
                MessageBox.Show("Cannot generate schedule: Missing Data.\nPlease check Rooms, Teachers, or Sections.", "Generation Failed");
                return;
            }

            // Existing schedule warning
            if (!isDataDirty && DataManager.FailedAssignments.Count == 0 && DataManager.MasterSchedule.Count > 0)
            {
                var result = MessageBox.Show("A valid schedule exists. Re-generating will attempt to find a better configuration.\n\nContinue?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No) return;
            }

            RunScheduleGeneration();
        }

        private async void RunScheduleGeneration()
        {
            Cursor.Current = Cursors.WaitCursor;
            btnGenerate.Enabled = false;
            btnGenerate.Text = "Processing...";
            Application.DoEvents();

            try
            {
                // 1. Run the Algorithm (Memory Only)
                string resultMsg = await _scheduleService.GenerateScheduleAsync();

                // 2. Refresh UI
                UpdateMasterGrid();
                UpdateTimetableView();
                LoadPendingList();

                isDataDirty = false;

                if (resultMsg.Contains("Success"))
                {
                    // 3. Save the result to MySQL Database
                    try
                    {
                        DatabaseHelper.SaveMasterSchedule(DataManager.MasterSchedule);
                        MessageBox.Show("Perfect Schedule Generated and Saved to Database!");
                    }
                    catch (Exception dbEx)
                    {
                        MessageBox.Show("Schedule generated in memory, but failed to save to DB: " + dbEx.Message);
                    }
                }
                else
                {
                    MessageBox.Show(resultMsg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating schedule: " + ex.Message);
            }
            finally
            {
                btnGenerate.Enabled = true;
                btnGenerate.Text = "Generate Schedule";
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        #region 4. Manual Operations & Grid Interaction

        private void UnassignSubject(ScheduleItem item)
        {
            _scheduleService.UnassignSubject(item);
            UpdateMasterGrid();
            UpdateTimetableView();
            LoadPendingList();
        }

        private void PerformSwap(ScheduleItem oldClass, FailedEntry newClass)
        {
            bool success = _scheduleService.PerformSwap(oldClass, newClass);

            if (!success)
            {
                MessageBox.Show("Swap failed: The new subject requirements conflict with availability.");
            }

            UpdateMasterGrid();
            UpdateTimetableView();
            LoadPendingList();
        }

        private void ContextMenu_PlaceClick(object sender, EventArgs e)
        {
            var menuItem = (ToolStripMenuItem)sender;
            dynamic data = menuItem.Tag;

            FailedEntry failEntry = data.FailEntry;
            int targetDay = data.Day;
            int targetTime = data.Time;
            string targetRoom = null;

            if (cmbFilterType.SelectedItem?.ToString() == "Room")
            {
                targetRoom = cmbScheduleView.SelectedItem.ToString();
            }
            else
            {
                var roomType = failEntry.Subject.IsLab ? RoomType.Laboratory : RoomType.Lecture;
                var validRoom = DataManager.Rooms.FirstOrDefault(r => r.Type == roomType && !r.IsBusy[targetDay, targetTime]);

                if (validRoom == null)
                {
                    MessageBox.Show($"No {roomType} rooms are free at this time.");
                    return;
                }
                targetRoom = validRoom.Name;
            }

            bool success = _scheduleService.PlaceBlockManual(failEntry, targetDay, targetTime, targetRoom);

            if (success)
            {
                UpdateMasterGrid();
                UpdateTimetableView();
                LoadPendingList();
            }
            else
            {
                MessageBox.Show("Cannot place here. The teacher is busy, the room is occupied, or the class duration exceeds the day.");
            }
        }

        private void btnFindSlots_Click(object sender, EventArgs e)
        {
            if (dgvPending.SelectedRows.Count == 0) return;

            var failEntry = (FailedEntry)dgvPending.SelectedRows[0].Tag;
            string subjectName = CleanSubjectName(failEntry.Subject.Code);
            var teacher = DataManager.Teachers.FirstOrDefault(t => t.QualifiedSubjects.Contains(subjectName));

            if (teacher == null)
            {
                MessageBox.Show("No qualified teacher found for " + subjectName);
                return;
            }

            string report = $"-- Valid Slots for {failEntry.Subject.Code} ({failEntry.Subject.Units} hrs) --\n";
            report += $"Teacher: {teacher.Name}\n\n";
            int optionsFound = 0;

            for (int d = 1; d <= 6; d++)
            {
                for (int t = 0; t <= 11 - failEntry.Subject.Units; t++)
                {
                    bool isFree = true;
                    for (int i = 0; i < failEntry.Subject.Units; i++)
                    {
                        if (teacher.IsBusy[d, t + i] || failEntry.Section.IsBusy[d, t + i])
                        {
                            isFree = false;
                            break;
                        }
                    }

                    if (isFree)
                    {
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

        private void dgvTimetable_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right || e.RowIndex < 0 || e.ColumnIndex <= 0) return;

            dgvTimetable.ClearSelection();
            dgvTimetable.CurrentCell = dgvTimetable[e.ColumnIndex, e.RowIndex];
            dgvTimetable.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;

            int dayIndex = e.ColumnIndex;
            int timeIndex = e.RowIndex;
            string currentSectionName = cmbScheduleView.SelectedItem?.ToString();

            var existingClass = DataManager.MasterSchedule.FirstOrDefault(s =>
                (s.Section == currentSectionName || cmbFilterType.Text != "Section") &&
                s.DayIndex == dayIndex &&
                s.TimeIndex == timeIndex &&
                (s.Room == dgvTimetable.Rows[timeIndex].Cells[e.ColumnIndex].Value?.ToString().Split('\n').LastOrDefault()
                 || cmbFilterType.Text == "Section")
            );

            if (cmbFilterType.Text == "Section" && !string.IsNullOrEmpty(currentSectionName))
            {
                existingClass = DataManager.MasterSchedule.FirstOrDefault(s =>
                    s.Section == currentSectionName && s.DayIndex == dayIndex && s.TimeIndex == timeIndex);
            }

            ctxMenuSchedule.Items.Clear();
            ctxMenuSchedule.Items.Add(new ToolStripMenuItem($"Slot: {GetDayName(dayIndex)} @ {ToSimple12Hour(GetTimeLabel(timeIndex))}") { Enabled = false, BackColor = System.Drawing.Color.LightGray });
            ctxMenuSchedule.Items.Add(new ToolStripSeparator());

            if (existingClass != null)
            {
                ctxMenuSchedule.Items.Add(new ToolStripMenuItem($"Current: {existingClass.Subject} ({existingClass.Room})") { Enabled = false });

                var unassignItem = new ToolStripMenuItem("Unassign Entire Block (Move to Pending)");
                unassignItem.Click += (s, args) => UnassignSubject(existingClass);
                ctxMenuSchedule.Items.Add(unassignItem);

                ctxMenuSchedule.Items.Add(new ToolStripSeparator());

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
            else
            {
                string currentMode = cmbFilterType.SelectedItem?.ToString();
                var relevantPending = new List<FailedEntry>();

                if (currentMode == "Section")
                {
                    relevantPending = DataManager.FailedAssignments.Where(f => f.Section.Name == currentSectionName).ToList();
                }
                else if (currentMode == "Teacher")
                {
                    relevantPending = DataManager.FailedAssignments.Where(f =>
                    {
                        var t = DataManager.Teachers.FirstOrDefault(teacher => teacher.Name == currentSectionName);
                        return t != null && t.QualifiedSubjects.Contains(CleanSubjectName(f.Subject.Code));
                    }).ToList();
                }
                else
                {
                    relevantPending = DataManager.FailedAssignments.ToList();
                }

                if (relevantPending.Count == 0)
                {
                    ctxMenuSchedule.Items.Add("No pending subjects found.");
                }
                else
                {
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

            Rectangle cellRect = dgvTimetable.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
            ctxMenuSchedule.Show(dgvTimetable, cellRect.Left + e.X, cellRect.Top + e.Y);
        }

        #endregion

        #region 5. Data Management (CRUD with Database Sync)

        // --- Teacher Management ---
        private void btnAddTeacher_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTeacherName.Text)) { MessageBox.Show("Please enter a teacher name."); return; }

            string tName = txtTeacherName.Text.Trim();

            // 1. Database Insert
            int newId = DataManager.Teachers.Count + 1; // Temporary ID
            try
            {
                DatabaseHelper.ExecuteQuery("INSERT INTO teachers (teacher_name) VALUES (@Name)", new { Name = tName });

                // Reload to get the real ID
                var dbTeachers = DatabaseHelper.LoadTeachers();
                if (dbTeachers.Count > 0) newId = dbTeachers.Last().Id;
            }
            catch { /* Offline: Keep the temp ID */ }

            // 2. Memory Update
            Teacher newTeacher = new Teacher { Id = newId, Name = tName };

            if (!string.IsNullOrWhiteSpace(txtTeacherSubjects.Text))
            {
                foreach (var s in txtTeacherSubjects.Text.Split(','))
                {
                    string sub = s.Trim();
                    newTeacher.QualifiedSubjects.Add(sub);
                    try { DatabaseHelper.ExecuteQuery("INSERT INTO teacher_subjects (teacher_id, subject_code) VALUES (@TId, @Sub)", new { TId = newId, Sub = sub }); } catch { }
                }
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

                // 1. Database Update
                try
                {
                    // Update Name
                    DatabaseHelper.ExecuteQuery("UPDATE teachers SET teacher_name = @Name WHERE teacher_id = @Id", new { Name = teacher.Name, Id = teacher.Id });

                    // Update Subjects (Wipe and Re-add)
                    DatabaseHelper.ExecuteQuery("DELETE FROM teacher_subjects WHERE teacher_id = @Id", new { Id = teacher.Id });
                }
                catch { }

                if (!string.IsNullOrWhiteSpace(txtTeacherSubjects.Text))
                {
                    foreach (var s in txtTeacherSubjects.Text.Split(','))
                    {
                        string sub = s.Trim();
                        teacher.QualifiedSubjects.Add(sub);
                        try { DatabaseHelper.ExecuteQuery("INSERT INTO teacher_subjects (teacher_id, subject_code) VALUES (@TId, @Sub)", new { TId = teacher.Id, Sub = sub }); } catch { }
                    }
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

            if (teacher != null && MessageBox.Show($"Delete {name} and their schedule?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DataManager.RemoveTeacher(teacher);
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

        // --- Room Management ---
        private void btnAddRoom_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRoomName.Text) || cmbRoomType.SelectedItem == null)
            {
                MessageBox.Show("Please enter valid room details.");
                return;
            }

            string rName = txtRoomName.Text;
            RoomType rType = cmbRoomType.SelectedItem.ToString() == "Laboratory" ? RoomType.Laboratory : RoomType.Lecture;

            // 1. Database Insert
            try { DatabaseHelper.CreateRoom(rName, rType.ToString()); } catch { }

            // 2. Memory Update
            Room newRoom = new Room
            {
                Id = DataManager.Rooms.Count + 1, // Temp ID
                Name = rName,
                Type = rType
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

                // Database Update
                try
                {
                    DatabaseHelper.ExecuteQuery("UPDATE rooms SET room_name = @Name, room_type = @Type WHERE room_id = @Id",
                        new { Name = room.Name, Type = room.Type.ToString(), Id = room.Id });
                }
                catch { }

                MessageBox.Show("Room Updated!");
                ResetRoomForm();
            }
        }

        private void btnDeleteRoom_Click(object sender, EventArgs e)
        {
            if (lstRooms.SelectedItem == null) return;
            string name = lstRooms.SelectedItem.ToString().Split('|')[0].Trim();
            var room = DataManager.Rooms.FirstOrDefault(r => r.Name == name);

            if (room != null && MessageBox.Show($"Delete {name} and its schedule?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DataManager.RemoveRoom(room);
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

        // --- Section Management ---
        private void btnCreateSection_Click(object sender, EventArgs e)
        {
            // 1. Validate Inputs
            if (string.IsNullOrWhiteSpace(txtSectionName.Text) || cmbSectionProgram.SelectedItem == null || cmbSectionYear.SelectedItem == null)
            {
                MessageBox.Show("Please complete all section fields.");
                return;
            }

            string sName = txtSectionName.Text;
            string program = cmbSectionProgram.SelectedItem.ToString();
            int yearLevel = int.Parse(cmbSectionYear.SelectedItem.ToString());

            // 2. LOGIC SPLIT: Are we Updating or Creating?
            if (editingSectionId != -1)
            {
                // === UPDATE EXISTING SECTION ===
                var section = DataManager.Sections.FirstOrDefault(s => s.Id == editingSectionId);
                if (section != null)
                {
                    section.Name = sName;
                    section.Program = program;
                    section.YearLevel = yearLevel;

                    try
                    {
                        DatabaseHelper.ExecuteQuery("UPDATE sections SET section_name = @Name, program = @Prog, year_level = @Year WHERE section_id = @Id",
                            new { Name = sName, Prog = program, Year = yearLevel, Id = section.Id });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Offline Update: " + ex.Message);
                    }

                    MessageBox.Show("Section Updated!");
                }

                // Reset UI
                editingSectionId = -1;
                btnCreateSection.Text = "Create";
            }
            else
            {
                // === CREATE NEW SECTION ===
                try
                {
                    // A. Insert into Database
                    string sql = "INSERT INTO sections (section_name, program, year_level) VALUES (@Name, @Prog, @Year)";
                    DatabaseHelper.ExecuteQuery(sql, new { Name = sName, Prog = program, Year = yearLevel });

                    // B. CRITICAL: Reload from DB to get the REAL ID (Auto-Increment)
                    // This ensures the ID in memory matches the ID in the database!
                    DataManager.Sections = DatabaseHelper.LoadSections();

                    MessageBox.Show($"Section {sName} created successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database Error: " + ex.Message + "\nAdding to local memory only.");

                    // C. Offline Fallback
                    Section offlineSection = new Section
                    {
                        Id = DataManager.Sections.Count + 1,
                        Name = sName,
                        Program = program,
                        YearLevel = yearLevel
                    };
                    DataManager.Sections.Add(offlineSection);
                }
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

        private void btnDeleteSection_Click(object sender, EventArgs e)
        {
            if (lstSections.SelectedItem == null) return;
            string name = lstSections.SelectedItem.ToString();
            var section = DataManager.Sections.FirstOrDefault(s => s.Name == name);

            if (section != null && MessageBox.Show($"Delete {name} and its schedule?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DataManager.RemoveSection(section);
                MessageBox.Show("Section deleted.");
                RefreshAdminLists();
                isDataDirty = true;
            }
        }

        private void btnCancelSection_Click(object sender, EventArgs e)
        {
            ResetSectionForm();
        }

        private void ResetSectionForm()
        {
            // --- 1. RESET LEFT SIDE (Create/Edit Section) ---
            txtSectionName.Clear();

            // Explicitly clear selection so RefreshAdminLists doesn't "restore" it
            cmbSectionProgram.SelectedIndex = -1;
            cmbSectionProgram.Text = "";

            cmbSectionYear.SelectedIndex = -1;
            cmbSectionYear.Text = "";

            // Reset Edit State
            editingSectionId = -1;
            btnCreateSection.Text = "Create";

            // --- 2. RESET RIGHT SIDE (Batch Add & Selected Section) ---

            // Clear Selected Section
            cmbSectionList.SelectedIndex = -1;
            cmbSectionList.Text = "";
            lstSectionSubjects.Items.Clear(); // Wipe the subject list view

            // Clear Batch Add Dropdowns
            // Important: Clear these BEFORE RefreshAdminLists runs!
            cmbBatchProgram.SelectedIndex = -1;
            cmbBatchProgram.Text = "";

            cmbBatchYear.SelectedIndex = -1;
            cmbBatchYear.Text = "";

            // Clear Subject Textboxes (Code, Units, etc.)
            ResetSubjectInputs();

            // --- 3. REFRESH DATA ---
            // Now when this runs, it sees the selections are null/empty, so it won't restore old values.
            RefreshAdminLists();
            RefreshSectionDropdown();
        }

        // --- SUBJECT ADDITION (DB Enabled) ---

        private void btnAddSubject_Click(object sender, EventArgs e)
        {
            ProcessSubjectAddition(false);
        }

        private void btnBatchAdd_Click(object sender, EventArgs e)
        {
            ProcessSubjectAddition(true);
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

                // DB Deletion Logic
                try
                {
                    // Note: Deleting by code alone might be ambiguous if duplicate codes exist in section
                    // ideally we'd have a unique link ID, but here we do best effort:
                    string sql = "DELETE FROM section_subjects WHERE section_id = @SId AND subject_code = @Code";
                    DatabaseHelper.ExecuteQuery(sql, new { SId = section.Id, Code = subject.Code });
                }
                catch { }

                MessageBox.Show("Subject removed.");
                RefreshSubjectList();
                isDataDirty = true;
            }
        }

        private void btnCancelSubject_Click(object sender, EventArgs e)
        {
            ResetSubjectInputs();
        }

        private void ResetSubjectInputs()
        {
            txtSubjCode.Clear();
            txtUnits.Clear();
            chkIsLab.Checked = false;
            editingSubjectCode = "";
            btnAddSubject.Enabled = true;
        }

        private void ProcessSubjectAddition(bool isBatchMode)
        {
            // --- 1. GATHER INPUTS ---
            string rawCode = txtSubjCode.Text.Trim();
            if (string.IsNullOrWhiteSpace(rawCode) || !int.TryParse(txtUnits.Text, out int units))
            {
                MessageBox.Show("Please enter a valid Subject Code and Units.");
                return;
            }
            bool isLab = chkIsLab.Checked;

            // --- 2. IDENTIFY TARGETS ---
            List<Section> targetSections = new List<Section>();

            if (isBatchMode)
            {
                if (cmbBatchProgram.SelectedItem == null || cmbBatchYear.SelectedItem == null)
                {
                    MessageBox.Show("Please select a Target Program and Year Level first.");
                    return;
                }

                string prog = cmbBatchProgram.SelectedItem.ToString();
                int year = int.Parse(cmbBatchYear.SelectedItem.ToString());

                // Trim() ensures we ignore accidental spaces (e.g. "BSCS " vs "BSCS")
                targetSections = DataManager.Sections
                    .Where(s => s.Program.Trim() == prog.Trim() && s.YearLevel == year)
                    .ToList();

                if (targetSections.Count == 0)
                {
                    MessageBox.Show($"No sections found for {prog} - Year {year}.");
                    return;
                }
            }
            else
            {
                if (cmbSectionList.SelectedItem == null)
                {
                    MessageBox.Show("Please select a specific Section first.");
                    return;
                }

                var sec = DataManager.Sections.FirstOrDefault(s => s.Name == cmbSectionList.SelectedItem.ToString());
                if (sec != null) targetSections.Add(sec);
            }

            // --- 3. EXECUTE ADDITION ---
            int successCount = 0;

            foreach (var section in targetSections)
            {
                // Skip duplicate subjects
                if (section.SubjectsToTake.Any(s => s.Code.StartsWith(rawCode))) continue;

                List<Subject> subjectsToAdd = new List<Subject>();

                if (isLab)
                {
                    subjectsToAdd.Add(new Subject { Code = rawCode + " (Lec)", Units = units - 1, IsLab = false });
                    subjectsToAdd.Add(new Subject { Code = rawCode + " (Lab)", Units = 3, IsLab = true });
                }
                else
                {
                    subjectsToAdd.Add(new Subject { Code = rawCode, Units = units, IsLab = false });
                }

                foreach (var sub in subjectsToAdd)
                {
                    // A. Update Memory
                    section.SubjectsToTake.Add(sub);

                    // B. Update Database
                    try
                    {
                        string sql = "INSERT INTO section_subjects (section_id, subject_code, units, is_lab) VALUES (@SId, @Code, @Units, @IsLab)";
                        DatabaseHelper.ExecuteQuery(sql, new
                        {
                            SId = section.Id,
                            Code = sub.Code,
                            Units = sub.Units,
                            IsLab = sub.IsLab
                        });
                    }
                    catch { /* Silent fail in production/offline is acceptable here */ }
                }
                successCount++;
            }

            // --- 4. FINISH ---
            if (successCount > 0)
            {
                // Optional: Keep this small toast if you want confirmation, or remove it for total silence.
                MessageBox.Show(isBatchMode
                    ? $"Successfully added {rawCode} to {successCount} sections!"
                    : "Subject added successfully.");

                RefreshSubjectList();
                isDataDirty = true;
            }
            else
            {
                MessageBox.Show("No changes made. The subject might already exist in the target section(s).");
            }
        }

        #endregion

        #region 6. Visualization & Helpers (Same as before)

        // ... (Keep existing visualization logic) ...
        // Note: The rest of the file (UpdateMasterGrid, etc.) remains unchanged from previous version
        // as it deals with memory display, which is updated by the lists modified above.

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

        private void RefreshAdminLists()
        {
            lstTeachers.Items.Clear();
            foreach (var t in DataManager.Teachers) lstTeachers.Items.Add($"{t.Name} ({string.Join(", ", t.QualifiedSubjects)})");

            lstRooms.Items.Clear();
            foreach (var r in DataManager.Rooms) lstRooms.Items.Add($"{r.Name} | {r.Type}");

            lstSections.Items.Clear();
            foreach (var s in DataManager.Sections) lstSections.Items.Add(s.Name);

            RefreshSectionDropdown();

            string currentBatchProg = cmbBatchProgram.SelectedItem?.ToString();
            string currentSectProg = cmbSectionProgram.SelectedItem?.ToString();

            cmbBatchProgram.Items.Clear();
            cmbSectionProgram.Items.Clear();

            foreach (var prog in DataManager.Programs)
            {
                cmbBatchProgram.Items.Add(prog);
                cmbSectionProgram.Items.Add(prog);
            }

            if (currentBatchProg != null && cmbBatchProgram.Items.Contains(currentBatchProg))
                cmbBatchProgram.SelectedItem = currentBatchProg;

            if (currentSectProg != null && cmbSectionProgram.Items.Contains(currentSectProg))
                cmbSectionProgram.SelectedItem = currentSectProg;
        }

        private void RefreshSectionDropdown()
        {
            cmbSectionList.Items.Clear();
            foreach (var s in DataManager.Sections) cmbSectionList.Items.Add(s.Name);
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

        // List & Combo Selection Events
        private void cmbFilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbScheduleView.Items.Clear();
            cmbScheduleView.Text = "";
            string mode = cmbFilterType.SelectedItem?.ToString() ?? "Section";

            if (mode == "Section") foreach (var s in DataManager.Sections) cmbScheduleView.Items.Add(s.Name);
            else if (mode == "Teacher") foreach (var t in DataManager.Teachers) cmbScheduleView.Items.Add(t.Name);
            else if (mode == "Room") foreach (var r in DataManager.Rooms) cmbScheduleView.Items.Add(r.Name);
        }

        private void cmbScheduleView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbScheduleView.SelectedItem != null)
            {
                string mode = cmbFilterType.SelectedItem?.ToString() ?? "Section";
                DisplayTimetable(cmbScheduleView.SelectedItem.ToString(), mode);
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
                btnCreateSection.Text = "Update";
                if (cmbSectionList.Items.Contains(section.Name)) cmbSectionList.SelectedItem = section.Name;
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

        private void cmbSectionList_SelectedIndexChanged(object sender, EventArgs e) => RefreshSubjectList();

        private void UpdateMasterGrid()
        {
            dgvMaster.DataSource = null;

            var sortedSchedule = DataManager.MasterSchedule
                .OrderBy(x => x.DayIndex)
                .ThenBy(x => x.TimeIndex)
                .ThenBy(x => x.Section)
                .ToList();

            dgvMaster.DataSource = sortedSchedule;

            dgvMaster.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMaster.AllowUserToAddRows = false;
            dgvMaster.RowHeadersVisible = false;
            dgvMaster.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMaster.ReadOnly = true;

            if (dgvMaster.Columns["DayIndex"] != null) dgvMaster.Columns["DayIndex"].Visible = false;
            if (dgvMaster.Columns["TimeIndex"] != null) dgvMaster.Columns["TimeIndex"].Visible = false;
            if (dgvMaster.Columns["RoomObj"] != null) dgvMaster.Columns["RoomObj"].Visible = false;

            if (dgvMaster.Columns["Section"] != null) dgvMaster.Columns["Section"].HeaderText = "SECTION";
            if (dgvMaster.Columns["Subject"] != null) dgvMaster.Columns["Subject"].HeaderText = "SUBJECT CODE";
            if (dgvMaster.Columns["Teacher"] != null) dgvMaster.Columns["Teacher"].HeaderText = "INSTRUCTOR";
            if (dgvMaster.Columns["Room"] != null) dgvMaster.Columns["Room"].HeaderText = "ROOM";
            if (dgvMaster.Columns["Day"] != null) dgvMaster.Columns["Day"].HeaderText = "DAY";
            if (dgvMaster.Columns["Time"] != null) dgvMaster.Columns["Time"].HeaderText = "TIME SLOT";

            if (dgvMaster.Columns["Day"] != null) dgvMaster.Columns["Day"].DisplayIndex = 0;
            if (dgvMaster.Columns["Time"] != null) dgvMaster.Columns["Time"].DisplayIndex = 1;
            if (dgvMaster.Columns["Section"] != null) dgvMaster.Columns["Section"].DisplayIndex = 2;
            if (dgvMaster.Columns["Subject"] != null) dgvMaster.Columns["Subject"].DisplayIndex = 3;
            if (dgvMaster.Columns["Room"] != null) dgvMaster.Columns["Room"].DisplayIndex = 4;
            if (dgvMaster.Columns["Teacher"] != null) dgvMaster.Columns["Teacher"].DisplayIndex = 5;
        }

        private void UpdateTimetableView()
        {
            string currentSelection = cmbScheduleView.SelectedItem?.ToString();
            cmbScheduleView.Items.Clear();
            foreach (var section in DataManager.Sections) cmbScheduleView.Items.Add(section.Name);

            if (currentSelection != null && cmbScheduleView.Items.Contains(currentSelection))
            {
                cmbScheduleView.SelectedItem = currentSelection;
            }
            else if (cmbScheduleView.Items.Count > 0)
            {
                cmbScheduleView.SelectedIndex = 0;
            }

            if (cmbScheduleView.SelectedItem != null)
            {
                string mode = cmbFilterType.SelectedItem?.ToString() ?? "Section";
                DisplayTimetable(cmbScheduleView.SelectedItem.ToString(), mode);
            }
            dgvTimetable.Refresh();
        }

        private void DisplayTimetable(string filterValue, string filterMode)
        {
            dgvTimetable.DataSource = null;
            dgvTimetable.Rows.Clear();
            dgvTimetable.Columns.Clear();

            dgvTimetable.Columns.Add("Time", "TIME");
            string[] days = { "MONDAY", "TUESDAY", "WEDNESDAY", "THURSDAY", "FRIDAY", "SATURDAY", "SUNDAY" };
            foreach (var day in days) dgvTimetable.Columns.Add(day, day);

            ConfigureGridStyles();

            int exactRowHeight = CalculateRowHeight();
            for (int hour = 7; hour < 18; hour++)
            {
                string niceTime = ToSimple12Hour($"{hour}:00 - {hour + 1}:00");
                int rowIndex = dgvTimetable.Rows.Add(niceTime, "", "", "", "", "", "", "");
                dgvTimetable.Rows[rowIndex].Height = exactRowHeight;
            }

            List<ScheduleItem> filteredList = new List<ScheduleItem>();
            if (filterMode == "Section") filteredList = DataManager.MasterSchedule.Where(x => x.Section == filterValue).ToList();
            else if (filterMode == "Teacher") filteredList = DataManager.MasterSchedule.Where(x => x.Teacher == filterValue).ToList();
            else if (filterMode == "Room") filteredList = DataManager.MasterSchedule.Where(x => x.Room == filterValue).ToList();

            RenderScheduleItems(filteredList, filterMode);
            dgvTimetable.ClearSelection();
        }

        private void RenderScheduleItems(List<ScheduleItem> items, string filterMode)
        {
            foreach (var item in items)
            {
                int startHour = int.Parse(item.Time.Split(':')[0]);
                int rowIndex = startHour - 7;
                int colIndex = GetDayColumnIndex(item.Day);

                if (rowIndex >= 0 && rowIndex < dgvTimetable.Rows.Count && colIndex > 0)
                {
                    string cellText = "";
                    if (filterMode == "Section") cellText = $"{item.Subject}\n{item.Teacher}\n{item.Room}";
                    else if (filterMode == "Teacher") cellText = $"{item.Subject}\n{item.Section}\n{item.Room}";
                    else if (filterMode == "Room") cellText = $"{item.Subject}\n{item.Section}\n{item.Teacher}";

                    var cell = dgvTimetable.Rows[rowIndex].Cells[colIndex];
                    cell.Value = cellText;

                    if (item.Subject.Contains("(Lab)"))
                        cell.Style.BackColor = System.Drawing.Color.LightSalmon;
                    else
                        cell.Style.BackColor = GetSubjectColor(item.Subject);
                }
            }
        }

        private void ConfigureGridStyles()
        {
            dgvTimetable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTimetable.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvTimetable.Columns["Time"].FillWeight = 50;
            dgvTimetable.ScrollBars = ScrollBars.None;
            dgvTimetable.DefaultCellStyle.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular);
            dgvTimetable.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            dgvTimetable.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTimetable.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTimetable.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvTimetable.AllowUserToAddRows = false;
            dgvTimetable.RowHeadersVisible = false;
            foreach (DataGridViewColumn col in dgvTimetable.Columns) col.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private int CalculateRowHeight()
        {
            int availableHeight = dgvTimetable.Height - dgvTimetable.ColumnHeadersHeight;
            int height = availableHeight / 11;
            return height < 20 ? 20 : height;
        }

        private void dgvTimetable_Resize(object sender, EventArgs e)
        {
            if (dgvTimetable.Rows.Count == 0 || !dgvTimetable.Visible) return;
            try
            {
                int newRowHeight = CalculateRowHeight();
                foreach (DataGridViewRow row in dgvTimetable.Rows) row.Height = newRowHeight;
            }
            catch { }
        }

        private void dgvMaster_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string columnClicked = dgvMaster.Columns[e.ColumnIndex].Name;

            if (currentSortColumn == columnClicked)
                isAscending = !isAscending;
            else
            {
                currentSortColumn = columnClicked;
                isAscending = true;
            }

            List<ScheduleItem> sortedList = null;

            if (isAscending)
            {
                switch (columnClicked)
                {
                    case "Day": sortedList = DataManager.MasterSchedule.OrderBy(x => x.DayIndex).ToList(); break;
                    case "Time": sortedList = DataManager.MasterSchedule.OrderBy(x => x.TimeIndex).ToList(); break;
                    case "Section": sortedList = DataManager.MasterSchedule.OrderBy(x => x.Section).ToList(); break;
                    case "Subject": sortedList = DataManager.MasterSchedule.OrderBy(x => x.Subject).ToList(); break;
                    case "Teacher": sortedList = DataManager.MasterSchedule.OrderBy(x => x.Teacher).ToList(); break;
                    case "Room": sortedList = DataManager.MasterSchedule.OrderBy(x => x.Room).ToList(); break;
                    default: return;
                }
            }
            else
            {
                switch (columnClicked)
                {
                    case "Day": sortedList = DataManager.MasterSchedule.OrderByDescending(x => x.DayIndex).ToList(); break;
                    case "Time": sortedList = DataManager.MasterSchedule.OrderByDescending(x => x.TimeIndex).ToList(); break;
                    case "Section": sortedList = DataManager.MasterSchedule.OrderByDescending(x => x.Section).ToList(); break;
                    case "Subject": sortedList = DataManager.MasterSchedule.OrderByDescending(x => x.Subject).ToList(); break;
                    case "Teacher": sortedList = DataManager.MasterSchedule.OrderByDescending(x => x.Teacher).ToList(); break;
                    case "Room": sortedList = DataManager.MasterSchedule.OrderByDescending(x => x.Room).ToList(); break;
                    default: return;
                }
            }

            dgvMaster.DataSource = sortedList;

            foreach (DataGridViewColumn col in dgvMaster.Columns)
            {
                col.HeaderText = col.HeaderText.Replace(" ▲", "").Replace(" ▼", "");
            }
            dgvMaster.Columns[e.ColumnIndex].HeaderText += isAscending ? " ▲" : " ▼";
        }

        private int GetDayColumnIndex(string day)
        {
            if (day.StartsWith("Mon")) return 1;
            if (day.StartsWith("Tue")) return 2;
            if (day.StartsWith("Wed")) return 3;
            if (day.StartsWith("Thu")) return 4;
            if (day.StartsWith("Fri")) return 5;
            if (day.StartsWith("Sat")) return 6;
            if (day.StartsWith("Sun")) return 7;
            return 0;
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

        private string CleanSubjectName(string s) => s.Replace(" (Lec)", "").Replace(" (Lab)", "").Trim();

        private string GetDayName(int d)
        {
            string[] shortNames = { "", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            if (d >= 1 && d <= 7) return shortNames[d];
            return "Err";
        }

        private string GetTimeLabel(int t) => $"{7 + t}:00 - {8 + t}:00";

        // Helper to enable Double Buffering
        public static void SetDoubleBuffered(System.Windows.Forms.Control control)
        {
            if (System.Windows.Forms.SystemInformation.TerminalServerSession) return;

            typeof(System.Windows.Forms.Control).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null, control, new object[] { true });
        }

        #endregion

        #region 7. Backup & Restore (JSON)

        private void btnRefreshData_Click(object sender, EventArgs e)
        {
            DataManager.Initialize();
            RefreshAdminLists();
            MessageBox.Show("Data refreshed from database!", "System Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnBackupDatabase_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "System Backup File|*.json";
            save.FileName = $"SchedCCS_Backup_{DateTime.Now:yyyyMMdd_HHmm}.json";

            if (save.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var backup = new SystemBackup
                    {
                        Rooms = DataManager.Rooms,
                        Teachers = DataManager.Teachers,
                        Sections = DataManager.Sections,
                        MasterSchedule = DataManager.MasterSchedule,
                        FailedAssignments = DataManager.FailedAssignments
                    };

                    string jsonString = JsonSerializer.Serialize(backup, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(save.FileName, jsonString);

                    MessageBox.Show("System Database Backed up Successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Backup Failed: " + ex.Message);
                }
            }
        }

        private void btnRestoreDatabase_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "System Backup File|*.json";

            if (open.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string jsonString = File.ReadAllText(open.FileName);
                    SystemBackup backup = JsonSerializer.Deserialize<SystemBackup>(jsonString);

                    if (backup != null)
                    {
                        DataManager.Rooms = backup.Rooms ?? new List<Room>();
                        DataManager.Teachers = backup.Teachers ?? new List<Teacher>();
                        DataManager.Sections = backup.Sections ?? new List<Section>();
                        DataManager.MasterSchedule = backup.MasterSchedule ?? new List<ScheduleItem>();
                        DataManager.FailedAssignments = backup.FailedAssignments ?? new List<FailedEntry>();

                        _scheduleService.RebuildBusyArrays();
                        RefreshAdminLists();
                        UpdateMasterGrid();
                        UpdateTimetableView();

                        MessageBox.Show("System Data Restored Successfully!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Restore Failed: " + ex.Message);
                }
            }
        }

        #endregion

        #region 8. PDF Export

        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            if (cmbScheduleView.SelectedItem == null) { MessageBox.Show("Please select a view first."); return; }

            string filterMode = cmbFilterType.SelectedItem?.ToString() ?? "Section";
            string filterValue = cmbScheduleView.SelectedItem.ToString();

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

                    // 1. Header (Standardized)
                    document.Add(GeneratePdfHeader());

                    // 2. Context Info Table (Adapts to Section/Room/Teacher)
                    document.Add(GenerateContextInfoTable(filterMode, filterValue));

                    // 3. Schedule Grid
                    document.Add(GenerateAdminScheduleTable(filterMode, filterValue));

                    // 4. Footer
                    document.Add(GeneratePdfFooter());

                    document.Close();
                    MessageBox.Show("PDF Exported Successfully!");
                }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            }
        }

        private Paragraph GeneratePdfHeader()
        {
            var term = GetCurrentAcademicTerm();

            return new Paragraph()
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(9)
                .SetMultipliedLeading(1.0f)
                .Add("Republic of the Philippines\n")
                .Add(new Text("Laguna State Polytechnic University\n").SetFontSize(11).SetBold())
                .Add("Province of Laguna\n")
                .Add("College of Computer Studies\n\n")
                .Add(new Text("CLASS SCHEDULE\n").SetFontSize(13).SetBold())
                .Add($"{term.Semester}, Academic Year {term.AcadYear}");
        }

        /// <summary>
        /// Extracts the program code (letters) from a section name (e.g., "3GAV1" -> "GAV").
        /// </summary>
        private string ExtractProgramFromSection(string sectionName)
        {
            if (string.IsNullOrWhiteSpace(sectionName)) return "N/A";

            string upperName = sectionName.ToUpper();

            // Priority 1: Check for known Program Codes explicitly
            // This ensures we capture the correct code even if the section name format varies (e.g. "BSCS 2A" or "2A BSCS")
            string[] knownCodes = { "BSCS", "BSINFO", "WMAD", "GAV", "SMP", "INFO", "IS", "CS", "NA" };

            foreach (var code in knownCodes)
            {
                if (upperName.Contains(code))
                {
                    return code;
                }
            }

            // Priority 2: Fallback to Regex (First sequence of letters)
            var match = System.Text.RegularExpressions.Regex.Match(sectionName, @"[A-Za-z]+");
            if (match.Success)
            {
                return match.Value.ToUpper();
            }

            return "N/A";
        }

        /// <summary>
        /// Maps short codes to full degree titles based on college structure.
        /// </summary>
        private string GetFullDegreeName(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return "N/A";

            code = code.ToUpper().Trim();

            // Computer Science Group
            if (code == "BSCS" || code == "GAV" || code == "IS" || code == "CS")
            {
                return "Bachelor of Science in Computer Science";
            }

            // Info Tech Group
            if (code == "BSINFO" || code == "INFO" || code == "WMAD" || code == "SMP" || code == "NA")
            {
                return "Bachelor of Science in Information Technology";
            }

            return code; // Default if unknown
        }

        private Table GenerateContextInfoTable(string mode, string value)
        {
            // Changed column ratio from { 2, 1, 1 } to { 2, 1, 2 } (40% - 20% - 40%)
            // This ensures the middle column is exactly in the center of the page.
            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 2, 1, 2 }));
            table.SetWidth(UnitValue.CreatePercentValue(100));
            table.SetBorder(iText.Layout.Borders.Border.NO_BORDER);
            table.SetMarginTop(5);
            table.SetMarginBottom(10);

            if (mode == "Section")
            {
                string rawCode = ExtractProgramFromSection(value);
                string fullProgramName = GetFullDegreeName(rawCode);

                string year = "N/A";
                if (char.IsDigit(value[0])) year = value[0].ToString();
                else
                {
                    var yearMatch = System.Text.RegularExpressions.Regex.Match(value, @"\d");
                    if (yearMatch.Success) year = yearMatch.Value;
                }

                string formattedYear = GetOrdinalYear(year);

                table.AddCell(new Cell().Add(new Paragraph($"Program: {fullProgramName}").SetBold())
                    .SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetFontSize(9));

                table.AddCell(new Cell().Add(new Paragraph($"Year Level: {formattedYear}").SetBold())
                    .SetBorder(iText.Layout.Borders.Border.NO_BORDER)
                    .SetTextAlignment(TextAlignment.CENTER).SetFontSize(10));

                table.AddCell(new Cell().Add(new Paragraph($"Section: {value}").SetBold())
                    .SetBorder(iText.Layout.Borders.Border.NO_BORDER)
                    .SetTextAlignment(TextAlignment.RIGHT).SetFontSize(10));
            }
            else if (mode == "Room")
            {
                var room = DataManager.Rooms.FirstOrDefault(r => r.Name == value);
                string type = room?.Type.ToString() ?? "Lecture";

                table.AddCell(new Cell().Add(new Paragraph($"Room: {value}").SetBold())
                    .SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetFontSize(10));

                table.AddCell(new Cell().Add(new Paragraph($"Type: {type}").SetBold())
                    .SetBorder(iText.Layout.Borders.Border.NO_BORDER)
                    .SetTextAlignment(TextAlignment.CENTER).SetFontSize(10));

                table.AddCell(new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER));
            }
            else // Teacher
            {
                table.AddCell(new Cell().Add(new Paragraph($"Instructor: {value}").SetBold())
                    .SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetFontSize(10));

                table.AddCell(new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER));
                table.AddCell(new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER));
            }

            return table;
        }

        // Add this helper to AdminDashboard.cs as well
        private string GetOrdinalYear(string year)
        {
            if (int.TryParse(year, out int y))
            {
                switch (y)
                {
                    case 1: return "1st Year";
                    case 2: return "2nd Year";
                    case 3: return "3rd Year";
                    case 4: return "4th Year";
                    default: return $"{y}th Year";
                }
            }
            return $"{year} Year";
        }

        private Table GenerateAdminScheduleTable(string filterMode, string filterValue)
        {
            float[] colWidths = { 1.2f, 2, 2, 2, 2, 2, 2, 2 };
            Table table = new Table(UnitValue.CreatePercentArray(colWidths));
            table.SetWidth(UnitValue.CreatePercentValue(100));

            string[] headers = { "TIME", "MONDAY", "TUESDAY", "WEDNESDAY", "THURSDAY", "FRIDAY", "SATURDAY", "SUNDAY" };
            foreach (string h in headers)
            {
                table.AddCell(new Cell().Add(new Paragraph(h).SetBold().SetFontSize(7))
                    .SetBackgroundColor(ColorConstants.WHITE) // Clean white header
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .SetPadding(0).SetHeight(15));
            }

            for (int t = 0; t < 11; t++)
            {
                string timeLabel = $"{7 + t}:00 - {8 + t}:00";

                // Time Column (Same style as student)
                table.AddCell(new Cell().Add(new Paragraph(timeLabel).SetFontSize(7).SetBold())
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .SetPadding(0).SetHeight(25));

                for (int d = 1; d <= 7; d++)
                {
                    var item = DataManager.MasterSchedule.FirstOrDefault(s =>
                        s.DayIndex == d && s.TimeIndex == t &&
                        ((filterMode == "Section" && s.Section == filterValue) ||
                         (filterMode == "Teacher" && s.Teacher == filterValue) ||
                         (filterMode == "Room" && s.Room == filterValue))
                    );

                    Cell cell = new Cell().SetHeight(25).SetPadding(1)
                        .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                        .SetTextAlignment(TextAlignment.CENTER);

                    if (item != null)
                    {
                        string content = "";
                        // Smart content based on what we are looking at
                        if (filterMode == "Section") content = $"{item.Subject}\n{item.Teacher}\n{item.Room}";
                        else if (filterMode == "Teacher") content = $"{item.Subject}\n{item.Section}\n{item.Room}";
                        else content = $"{item.Subject}\n{item.Section}\n{item.Teacher}";

                        cell.Add(new Paragraph(content).SetFontSize(7).SetMultipliedLeading(0.9f));

                        // Color Logic
                        if (item.Subject.Contains("(Lab)"))
                        {
                            cell.SetBackgroundColor(new DeviceRgb(255, 160, 122)); // Salmon
                        }
                        else
                        {
                            System.Drawing.Color rndColor = GetSubjectColor(item.Subject);
                            cell.SetBackgroundColor(new DeviceRgb(rndColor.R, rndColor.G, rndColor.B));
                        }
                    }
                    table.AddCell(cell);
                }
            }
            return table;
        }

        private Table GeneratePdfFooter()
        {
            Table footerTable = new Table(UnitValue.CreatePercentArray(new float[] { 2, 1 }));
            footerTable.SetWidth(UnitValue.CreatePercentValue(100));
            footerTable.SetBorder(iText.Layout.Borders.Border.NO_BORDER);
            footerTable.SetMarginTop(25);

            // SYSTEM NAME
            string systemName = "Generated by: Let's Sched Started v1.0\n(College of Computer Studies)";

            Cell left = new Cell().Add(new Paragraph(systemName).SetFontSize(8).SetItalic())
                .SetBorder(iText.Layout.Borders.Border.NO_BORDER);

            // APPROVAL
            Cell right = new Cell().Add(new Paragraph("Approved by: ______________________\nCollege Dean")
                .SetFontSize(10).SetBold())
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetBorder(iText.Layout.Borders.Border.NO_BORDER);

            footerTable.AddCell(left);
            footerTable.AddCell(right);
            return footerTable;
        }

        private (string Semester, string AcadYear) GetCurrentAcademicTerm()
        {
            DateTime now = DateTime.Now;
            int month = now.Month; // 1 = Jan, 12 = Dec
            int year = now.Year;

            string semester;
            string acadYear;

            // Logic based on LSPU Calendar 2025-2026
            if (month >= 8) // Aug (8) to Dec (12) -> First Semester
            {
                semester = "First Semester";
                // If today is Aug 2025, AY is "2025-2026"
                acadYear = $"{year}-{year + 1}";
            }
            else if (month <= 5) // Jan (1) to May (5) -> Second Semester
            {
                semester = "Second Semester";
                // If today is Jan 2026, AY is still "2025-2026" (Started prev year)
                acadYear = $"{year - 1}-{year}";
            }
            else // June (6) to July (7) -> Inter-Semester
            {
                semester = "Inter-Semester";
                // If today is June 2026, AY is still "2025-2026"
                acadYear = $"{year - 1}-{year}";
            }

            return (semester, acadYear);
        }

        

        #endregion
    }

    public class SystemBackup
    {
        public List<Room> Rooms { get; set; }
        public List<Teacher> Teachers { get; set; }
        public List<Section> Sections { get; set; }
        public List<ScheduleItem> MasterSchedule { get; set; }
        public List<FailedEntry> FailedAssignments { get; set; }
    }
}