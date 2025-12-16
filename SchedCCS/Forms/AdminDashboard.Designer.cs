namespace SchedCCS
{
    partial class AdminDashboard
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminDashboard));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            ctxMenuSchedule = new ContextMenuStrip(components);
            pnlContent = new Panel();
            pnlViewManage = new Panel();
            tabDataManagers = new TabControl();
            tabTeachers = new TabPage();
            btnCancelTeacher = new Button();
            btnDeleteTeacher = new Button();
            btnUpdateTeacher = new Button();
            lstTeachers = new ListBox();
            btnAddTeacher = new Button();
            txtTeacherSubjects = new TextBox();
            txtTeacherName = new TextBox();
            label17 = new Label();
            label16 = new Label();
            tabRooms = new TabPage();
            lstRooms = new ListBox();
            btnDeleteRoom = new Button();
            btnCancelRoom = new Button();
            btnUpdateRoom = new Button();
            btnAddRoom = new Button();
            label1 = new Label();
            txtRoomName = new TextBox();
            cmbRoomType = new ComboBox();
            label2 = new Label();
            tabSections = new TabPage();
            groupBox8 = new GroupBox();
            btnBatchAdd = new Button();
            chkBatchLab = new CheckBox();
            txtBatchUnits = new TextBox();
            label12 = new Label();
            txtBatchCode = new TextBox();
            label11 = new Label();
            cmbBatchYear = new ComboBox();
            label10 = new Label();
            cmbBatchProgram = new ComboBox();
            label9 = new Label();
            groupBox6 = new GroupBox();
            btnRemoveSubject = new Button();
            lstSectionSubjects = new ListBox();
            groupBox7 = new GroupBox();
            lstSections = new ListBox();
            btnDeleteSection = new Button();
            groupBox3 = new GroupBox();
            cmbSectionYear = new ComboBox();
            label14 = new Label();
            cmbSectionProgram = new ComboBox();
            label13 = new Label();
            btnSaveChanges = new Button();
            btnCancelSubject = new Button();
            btnAddSubject = new Button();
            chkIsLab = new CheckBox();
            txtUnits = new TextBox();
            txtSubjCode = new TextBox();
            label8 = new Label();
            label7 = new Label();
            cmbSectionList = new ComboBox();
            label6 = new Label();
            btnCreateSection = new Button();
            txtSectionName = new TextBox();
            label5 = new Label();
            pnlViewSchedule = new Panel();
            btnExportPdf = new Button();
            label15 = new Label();
            dgvTimetable = new DataGridView();
            btnRestoreDatabase = new Button();
            cmbFilterType = new ComboBox();
            btnGenerate = new Button();
            btnBackupDatabase = new Button();
            cmbScheduleView = new ComboBox();
            pnlViewPending = new Panel();
            btnFindSlots = new Button();
            dgvPending = new DataGridView();
            Section = new DataGridViewTextBoxColumn();
            Subject = new DataGridViewTextBoxColumn();
            Reason = new DataGridViewTextBoxColumn();
            pnlViewMaster = new Panel();
            dgvMaster = new DataGridView();
            btnLogout = new Button();
            pnlNavBar = new Panel();
            btnNavPending = new Button();
            btnNavManage = new Button();
            btnNavMaster = new Button();
            btnNavSchedule = new Button();
            pnlContent.SuspendLayout();
            pnlViewManage.SuspendLayout();
            tabDataManagers.SuspendLayout();
            tabTeachers.SuspendLayout();
            tabRooms.SuspendLayout();
            tabSections.SuspendLayout();
            groupBox8.SuspendLayout();
            groupBox6.SuspendLayout();
            groupBox7.SuspendLayout();
            groupBox3.SuspendLayout();
            pnlViewSchedule.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTimetable).BeginInit();
            pnlViewPending.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPending).BeginInit();
            pnlViewMaster.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMaster).BeginInit();
            pnlNavBar.SuspendLayout();
            SuspendLayout();
            // 
            // ctxMenuSchedule
            // 
            ctxMenuSchedule.Name = "ctxMenuSchedule";
            ctxMenuSchedule.Size = new Size(61, 4);
            // 
            // pnlContent
            // 
            pnlContent.Controls.Add(pnlViewSchedule);
            pnlContent.Controls.Add(pnlViewManage);
            pnlContent.Controls.Add(pnlViewPending);
            pnlContent.Controls.Add(pnlViewMaster);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(0, 64);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new Size(1280, 656);
            pnlContent.TabIndex = 5;
            // 
            // pnlViewManage
            // 
            pnlViewManage.BackColor = Color.FromArgb(215, 216, 216);
            pnlViewManage.Controls.Add(tabDataManagers);
            pnlViewManage.Dock = DockStyle.Fill;
            pnlViewManage.Location = new Point(0, 0);
            pnlViewManage.Name = "pnlViewManage";
            pnlViewManage.Size = new Size(1280, 656);
            pnlViewManage.TabIndex = 2;
            // 
            // tabDataManagers
            // 
            tabDataManagers.Controls.Add(tabTeachers);
            tabDataManagers.Controls.Add(tabRooms);
            tabDataManagers.Controls.Add(tabSections);
            tabDataManagers.Location = new Point(84, 16);
            tabDataManagers.Multiline = true;
            tabDataManagers.Name = "tabDataManagers";
            tabDataManagers.SelectedIndex = 0;
            tabDataManagers.Size = new Size(1098, 592);
            tabDataManagers.TabIndex = 1;
            // 
            // tabTeachers
            // 
            tabTeachers.BackColor = Color.FromArgb(215, 216, 216);
            tabTeachers.BackgroundImage = (Image)resources.GetObject("tabTeachers.BackgroundImage");
            tabTeachers.Controls.Add(btnCancelTeacher);
            tabTeachers.Controls.Add(btnDeleteTeacher);
            tabTeachers.Controls.Add(btnUpdateTeacher);
            tabTeachers.Controls.Add(lstTeachers);
            tabTeachers.Controls.Add(btnAddTeacher);
            tabTeachers.Controls.Add(txtTeacherSubjects);
            tabTeachers.Controls.Add(txtTeacherName);
            tabTeachers.Controls.Add(label17);
            tabTeachers.Controls.Add(label16);
            tabTeachers.Location = new Point(4, 24);
            tabTeachers.Name = "tabTeachers";
            tabTeachers.Padding = new Padding(3);
            tabTeachers.Size = new Size(1090, 564);
            tabTeachers.TabIndex = 0;
            tabTeachers.Text = "Manage Teacher";
            // 
            // btnCancelTeacher
            // 
            btnCancelTeacher.FlatAppearance.BorderSize = 0;
            btnCancelTeacher.FlatStyle = FlatStyle.Flat;
            btnCancelTeacher.Font = new Font("Ebrima", 9.75F);
            btnCancelTeacher.Location = new Point(928, 186);
            btnCancelTeacher.Name = "btnCancelTeacher";
            btnCancelTeacher.Size = new Size(89, 25);
            btnCancelTeacher.TabIndex = 13;
            btnCancelTeacher.Text = "Cancel";
            btnCancelTeacher.UseVisualStyleBackColor = true;
            btnCancelTeacher.Click += btnCancelTeacher_Click;
            // 
            // btnDeleteTeacher
            // 
            btnDeleteTeacher.FlatAppearance.BorderSize = 0;
            btnDeleteTeacher.FlatStyle = FlatStyle.Flat;
            btnDeleteTeacher.Font = new Font("Ebrima", 9.75F);
            btnDeleteTeacher.Location = new Point(571, 516);
            btnDeleteTeacher.Name = "btnDeleteTeacher";
            btnDeleteTeacher.Size = new Size(137, 25);
            btnDeleteTeacher.TabIndex = 4;
            btnDeleteTeacher.Text = "Delete Selected";
            btnDeleteTeacher.UseVisualStyleBackColor = true;
            btnDeleteTeacher.Click += btnDeleteTeacher_Click;
            // 
            // btnUpdateTeacher
            // 
            btnUpdateTeacher.BackColor = Color.Silver;
            btnUpdateTeacher.Enabled = false;
            btnUpdateTeacher.FlatAppearance.BorderSize = 0;
            btnUpdateTeacher.FlatStyle = FlatStyle.Popup;
            btnUpdateTeacher.Font = new Font("Ebrima", 9.75F);
            btnUpdateTeacher.Location = new Point(415, 516);
            btnUpdateTeacher.Name = "btnUpdateTeacher";
            btnUpdateTeacher.Size = new Size(137, 25);
            btnUpdateTeacher.TabIndex = 12;
            btnUpdateTeacher.Text = "Update Teacher";
            btnUpdateTeacher.UseVisualStyleBackColor = false;
            btnUpdateTeacher.Click += btnUpdateTeacher_Click;
            // 
            // lstTeachers
            // 
            lstTeachers.Font = new Font("Ebrima", 9.75F);
            lstTeachers.FormattingEnabled = true;
            lstTeachers.ItemHeight = 17;
            lstTeachers.Location = new Point(85, 233);
            lstTeachers.Name = "lstTeachers";
            lstTeachers.Size = new Size(932, 259);
            lstTeachers.TabIndex = 3;
            lstTeachers.SelectedIndexChanged += lstTeachers_SelectedIndexChanged;
            // 
            // btnAddTeacher
            // 
            btnAddTeacher.BackColor = Color.FromArgb(64, 0, 0);
            btnAddTeacher.FlatAppearance.BorderSize = 0;
            btnAddTeacher.FlatStyle = FlatStyle.Popup;
            btnAddTeacher.Font = new Font("Ebrima", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAddTeacher.ForeColor = Color.White;
            btnAddTeacher.Location = new Point(785, 186);
            btnAddTeacher.Name = "btnAddTeacher";
            btnAddTeacher.Size = new Size(137, 25);
            btnAddTeacher.TabIndex = 11;
            btnAddTeacher.Text = "Add Teacher";
            btnAddTeacher.UseVisualStyleBackColor = false;
            btnAddTeacher.Click += btnAddTeacher_Click;
            // 
            // txtTeacherSubjects
            // 
            txtTeacherSubjects.Font = new Font("Ebrima", 9.75F);
            txtTeacherSubjects.Location = new Point(406, 187);
            txtTeacherSubjects.Name = "txtTeacherSubjects";
            txtTeacherSubjects.Size = new Size(344, 25);
            txtTeacherSubjects.TabIndex = 10;
            // 
            // txtTeacherName
            // 
            txtTeacherName.Font = new Font("Ebrima", 9.75F);
            txtTeacherName.Location = new Point(85, 187);
            txtTeacherName.Name = "txtTeacherName";
            txtTeacherName.Size = new Size(288, 25);
            txtTeacherName.TabIndex = 9;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.BackColor = Color.Transparent;
            label17.Font = new Font("Ebrima", 9.75F);
            label17.Location = new Point(85, 158);
            label17.Name = "label17";
            label17.Size = new Size(43, 17);
            label17.TabIndex = 7;
            label17.Text = "Name";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.BackColor = Color.Transparent;
            label16.Font = new Font("Ebrima", 9.75F);
            label16.Location = new Point(406, 158);
            label16.Name = "label16";
            label16.Size = new Size(177, 17);
            label16.TabIndex = 8;
            label16.Text = "Subjects (comma separated):";
            // 
            // tabRooms
            // 
            tabRooms.BackgroundImage = (Image)resources.GetObject("tabRooms.BackgroundImage");
            tabRooms.Controls.Add(lstRooms);
            tabRooms.Controls.Add(btnDeleteRoom);
            tabRooms.Controls.Add(btnCancelRoom);
            tabRooms.Controls.Add(btnUpdateRoom);
            tabRooms.Controls.Add(btnAddRoom);
            tabRooms.Controls.Add(label1);
            tabRooms.Controls.Add(txtRoomName);
            tabRooms.Controls.Add(cmbRoomType);
            tabRooms.Controls.Add(label2);
            tabRooms.Location = new Point(4, 24);
            tabRooms.Name = "tabRooms";
            tabRooms.Padding = new Padding(3);
            tabRooms.Size = new Size(1090, 564);
            tabRooms.TabIndex = 1;
            tabRooms.Text = "Manage Rooms";
            tabRooms.UseVisualStyleBackColor = true;
            // 
            // lstRooms
            // 
            lstRooms.Font = new Font("Ebrima", 9.75F);
            lstRooms.FormattingEnabled = true;
            lstRooms.ItemHeight = 17;
            lstRooms.Location = new Point(91, 222);
            lstRooms.Name = "lstRooms";
            lstRooms.Size = new Size(919, 276);
            lstRooms.TabIndex = 1;
            lstRooms.SelectedIndexChanged += lstRooms_SelectedIndexChanged;
            // 
            // btnDeleteRoom
            // 
            btnDeleteRoom.BackColor = Color.FromArgb(215, 216, 216);
            btnDeleteRoom.FlatAppearance.BorderSize = 0;
            btnDeleteRoom.FlatStyle = FlatStyle.Popup;
            btnDeleteRoom.Font = new Font("Ebrima", 9.75F);
            btnDeleteRoom.Location = new Point(587, 520);
            btnDeleteRoom.Name = "btnDeleteRoom";
            btnDeleteRoom.Size = new Size(120, 25);
            btnDeleteRoom.TabIndex = 0;
            btnDeleteRoom.Text = "Delete Selected";
            btnDeleteRoom.UseVisualStyleBackColor = false;
            btnDeleteRoom.Click += btnDeleteRoom_Click;
            // 
            // btnCancelRoom
            // 
            btnCancelRoom.Font = new Font("Ebrima", 9.75F);
            btnCancelRoom.Location = new Point(935, 180);
            btnCancelRoom.Name = "btnCancelRoom";
            btnCancelRoom.Size = new Size(75, 25);
            btnCancelRoom.TabIndex = 7;
            btnCancelRoom.Text = "Cancel";
            btnCancelRoom.UseVisualStyleBackColor = true;
            btnCancelRoom.Click += btnCancelRoom_Click;
            // 
            // btnUpdateRoom
            // 
            btnUpdateRoom.BackColor = Color.Silver;
            btnUpdateRoom.Enabled = false;
            btnUpdateRoom.FlatAppearance.BorderSize = 0;
            btnUpdateRoom.FlatStyle = FlatStyle.Popup;
            btnUpdateRoom.Font = new Font("Ebrima", 9.75F);
            btnUpdateRoom.Location = new Point(445, 520);
            btnUpdateRoom.Name = "btnUpdateRoom";
            btnUpdateRoom.Size = new Size(122, 25);
            btnUpdateRoom.TabIndex = 6;
            btnUpdateRoom.Text = "Update Room";
            btnUpdateRoom.UseVisualStyleBackColor = false;
            btnUpdateRoom.Click += btnUpdateRoom_Click;
            // 
            // btnAddRoom
            // 
            btnAddRoom.BackColor = Color.FromArgb(64, 0, 0);
            btnAddRoom.FlatAppearance.BorderSize = 0;
            btnAddRoom.FlatStyle = FlatStyle.Popup;
            btnAddRoom.Font = new Font("Ebrima", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAddRoom.ForeColor = Color.White;
            btnAddRoom.Location = new Point(802, 180);
            btnAddRoom.Name = "btnAddRoom";
            btnAddRoom.Size = new Size(117, 25);
            btnAddRoom.TabIndex = 5;
            btnAddRoom.Text = "Add Room";
            btnAddRoom.UseVisualStyleBackColor = false;
            btnAddRoom.Click += btnAddRoom_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Ebrima", 9.75F);
            label1.Location = new Point(91, 165);
            label1.Name = "label1";
            label1.Size = new Size(82, 17);
            label1.TabIndex = 1;
            label1.Text = "Room Name";
            // 
            // txtRoomName
            // 
            txtRoomName.Font = new Font("Ebrima", 9.75F);
            txtRoomName.Location = new Point(91, 183);
            txtRoomName.Name = "txtRoomName";
            txtRoomName.Size = new Size(336, 25);
            txtRoomName.TabIndex = 3;
            // 
            // cmbRoomType
            // 
            cmbRoomType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRoomType.Font = new Font("Ebrima", 9.75F);
            cmbRoomType.FormattingEnabled = true;
            cmbRoomType.Items.AddRange(new object[] { "Lecture", "Laboratory" });
            cmbRoomType.Location = new Point(459, 182);
            cmbRoomType.Name = "cmbRoomType";
            cmbRoomType.Size = new Size(324, 25);
            cmbRoomType.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Ebrima", 9.75F);
            label2.Location = new Point(459, 164);
            label2.Name = "label2";
            label2.Size = new Size(75, 17);
            label2.TabIndex = 2;
            label2.Text = "Room Type";
            // 
            // tabSections
            // 
            tabSections.Controls.Add(groupBox8);
            tabSections.Controls.Add(groupBox6);
            tabSections.Controls.Add(groupBox7);
            tabSections.Controls.Add(groupBox3);
            tabSections.Location = new Point(4, 24);
            tabSections.Name = "tabSections";
            tabSections.Size = new Size(1090, 564);
            tabSections.TabIndex = 2;
            tabSections.Text = "Sections & Subjects";
            tabSections.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            groupBox8.Controls.Add(btnBatchAdd);
            groupBox8.Controls.Add(chkBatchLab);
            groupBox8.Controls.Add(txtBatchUnits);
            groupBox8.Controls.Add(label12);
            groupBox8.Controls.Add(txtBatchCode);
            groupBox8.Controls.Add(label11);
            groupBox8.Controls.Add(cmbBatchYear);
            groupBox8.Controls.Add(label10);
            groupBox8.Controls.Add(cmbBatchProgram);
            groupBox8.Controls.Add(label9);
            groupBox8.Location = new Point(787, 25);
            groupBox8.Name = "groupBox8";
            groupBox8.Size = new Size(409, 394);
            groupBox8.TabIndex = 17;
            groupBox8.TabStop = false;
            groupBox8.Text = "Batch Subject Entry";
            // 
            // btnBatchAdd
            // 
            btnBatchAdd.Location = new Point(143, 294);
            btnBatchAdd.Name = "btnBatchAdd";
            btnBatchAdd.Size = new Size(123, 23);
            btnBatchAdd.TabIndex = 9;
            btnBatchAdd.Text = "Add to ALL Sections";
            btnBatchAdd.UseVisualStyleBackColor = true;
            // 
            // chkBatchLab
            // 
            chkBatchLab.AutoSize = true;
            chkBatchLab.Location = new Point(29, 215);
            chkBatchLab.Name = "chkBatchLab";
            chkBatchLab.Size = new Size(61, 19);
            chkBatchLab.TabIndex = 8;
            chkBatchLab.Text = "Is Lab?";
            chkBatchLab.UseVisualStyleBackColor = true;
            // 
            // txtBatchUnits
            // 
            txtBatchUnits.Location = new Point(134, 162);
            txtBatchUnits.Name = "txtBatchUnits";
            txtBatchUnits.Size = new Size(255, 23);
            txtBatchUnits.TabIndex = 7;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(29, 165);
            label12.Name = "label12";
            label12.Size = new Size(37, 15);
            label12.TabIndex = 6;
            label12.Text = "Units:";
            // 
            // txtBatchCode
            // 
            txtBatchCode.Location = new Point(134, 118);
            txtBatchCode.Name = "txtBatchCode";
            txtBatchCode.Size = new Size(255, 23);
            txtBatchCode.TabIndex = 5;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(29, 120);
            label11.Name = "label11";
            label11.Size = new Size(80, 15);
            label11.TabIndex = 4;
            label11.Text = "Subject Code:";
            // 
            // cmbBatchYear
            // 
            cmbBatchYear.FormattingEnabled = true;
            cmbBatchYear.Items.AddRange(new object[] { "1", "2", "3", "4" });
            cmbBatchYear.Location = new Point(134, 74);
            cmbBatchYear.Name = "cmbBatchYear";
            cmbBatchYear.Size = new Size(255, 23);
            cmbBatchYear.TabIndex = 3;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(29, 75);
            label10.Name = "label10";
            label10.Size = new Size(67, 15);
            label10.TabIndex = 2;
            label10.Text = "Target Year:";
            // 
            // cmbBatchProgram
            // 
            cmbBatchProgram.FormattingEnabled = true;
            cmbBatchProgram.Items.AddRange(new object[] { "BSCS", "BSIT" });
            cmbBatchProgram.Location = new Point(134, 30);
            cmbBatchProgram.Name = "cmbBatchProgram";
            cmbBatchProgram.Size = new Size(255, 23);
            cmbBatchProgram.TabIndex = 1;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(29, 30);
            label9.Name = "label9";
            label9.Size = new Size(91, 15);
            label9.TabIndex = 0;
            label9.Text = "Target Program:";
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(btnRemoveSubject);
            groupBox6.Controls.Add(lstSectionSubjects);
            groupBox6.Location = new Point(415, 209);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(355, 210);
            groupBox6.TabIndex = 16;
            groupBox6.TabStop = false;
            groupBox6.Text = "Existing Subjects";
            // 
            // btnRemoveSubject
            // 
            btnRemoveSubject.Location = new Point(127, 181);
            btnRemoveSubject.Name = "btnRemoveSubject";
            btnRemoveSubject.Size = new Size(104, 23);
            btnRemoveSubject.TabIndex = 1;
            btnRemoveSubject.Text = "Remove Subject";
            btnRemoveSubject.UseVisualStyleBackColor = true;
            // 
            // lstSectionSubjects
            // 
            lstSectionSubjects.FormattingEnabled = true;
            lstSectionSubjects.ItemHeight = 15;
            lstSectionSubjects.Location = new Point(6, 31);
            lstSectionSubjects.Name = "lstSectionSubjects";
            lstSectionSubjects.Size = new Size(346, 109);
            lstSectionSubjects.TabIndex = 0;
            // 
            // groupBox7
            // 
            groupBox7.Controls.Add(lstSections);
            groupBox7.Controls.Add(btnDeleteSection);
            groupBox7.Location = new Point(415, 13);
            groupBox7.Name = "groupBox7";
            groupBox7.Size = new Size(355, 210);
            groupBox7.TabIndex = 15;
            groupBox7.TabStop = false;
            groupBox7.Text = "Existing Sections";
            // 
            // lstSections
            // 
            lstSections.FormattingEnabled = true;
            lstSections.ItemHeight = 15;
            lstSections.Location = new Point(6, 31);
            lstSections.Name = "lstSections";
            lstSections.Size = new Size(346, 109);
            lstSections.TabIndex = 1;
            // 
            // btnDeleteSection
            // 
            btnDeleteSection.Location = new Point(131, 167);
            btnDeleteSection.Name = "btnDeleteSection";
            btnDeleteSection.Size = new Size(96, 23);
            btnDeleteSection.TabIndex = 0;
            btnDeleteSection.Text = "Delete Selected";
            btnDeleteSection.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(cmbSectionYear);
            groupBox3.Controls.Add(label14);
            groupBox3.Controls.Add(cmbSectionProgram);
            groupBox3.Controls.Add(label13);
            groupBox3.Controls.Add(btnSaveChanges);
            groupBox3.Controls.Add(btnCancelSubject);
            groupBox3.Controls.Add(btnAddSubject);
            groupBox3.Controls.Add(chkIsLab);
            groupBox3.Controls.Add(txtUnits);
            groupBox3.Controls.Add(txtSubjCode);
            groupBox3.Controls.Add(label8);
            groupBox3.Controls.Add(label7);
            groupBox3.Controls.Add(cmbSectionList);
            groupBox3.Controls.Add(label6);
            groupBox3.Controls.Add(btnCreateSection);
            groupBox3.Controls.Add(txtSectionName);
            groupBox3.Controls.Add(label5);
            groupBox3.Location = new Point(16, 13);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(393, 562);
            groupBox3.TabIndex = 11;
            groupBox3.TabStop = false;
            groupBox3.Text = "Add Section & Subjects";
            // 
            // cmbSectionYear
            // 
            cmbSectionYear.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSectionYear.FormattingEnabled = true;
            cmbSectionYear.Items.AddRange(new object[] { "1", "2", "3", "4" });
            cmbSectionYear.Location = new Point(74, 99);
            cmbSectionYear.Name = "cmbSectionYear";
            cmbSectionYear.Size = new Size(313, 23);
            cmbSectionYear.TabIndex = 16;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(6, 103);
            label14.Name = "label14";
            label14.Size = new Size(62, 15);
            label14.TabIndex = 15;
            label14.Text = "Year Level:";
            // 
            // cmbSectionProgram
            // 
            cmbSectionProgram.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSectionProgram.FormattingEnabled = true;
            cmbSectionProgram.Items.AddRange(new object[] { "BSCS", "BSIT" });
            cmbSectionProgram.Location = new Point(74, 71);
            cmbSectionProgram.Name = "cmbSectionProgram";
            cmbSectionProgram.Size = new Size(313, 23);
            cmbSectionProgram.TabIndex = 14;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(6, 75);
            label13.Name = "label13";
            label13.Size = new Size(56, 15);
            label13.TabIndex = 13;
            label13.Text = "Program:";
            // 
            // btnSaveChanges
            // 
            btnSaveChanges.Location = new Point(124, 401);
            btnSaveChanges.Name = "btnSaveChanges";
            btnSaveChanges.Size = new Size(75, 23);
            btnSaveChanges.TabIndex = 12;
            btnSaveChanges.Text = "Update";
            btnSaveChanges.UseVisualStyleBackColor = true;
            // 
            // btnCancelSubject
            // 
            btnCancelSubject.Location = new Point(43, 401);
            btnCancelSubject.Name = "btnCancelSubject";
            btnCancelSubject.Size = new Size(75, 23);
            btnCancelSubject.TabIndex = 11;
            btnCancelSubject.Text = "Cancel";
            btnCancelSubject.UseVisualStyleBackColor = true;
            // 
            // btnAddSubject
            // 
            btnAddSubject.Location = new Point(205, 401);
            btnAddSubject.Name = "btnAddSubject";
            btnAddSubject.Size = new Size(145, 23);
            btnAddSubject.TabIndex = 10;
            btnAddSubject.Text = "Add Subject to Section";
            btnAddSubject.UseVisualStyleBackColor = true;
            // 
            // chkIsLab
            // 
            chkIsLab.AutoSize = true;
            chkIsLab.Location = new Point(6, 369);
            chkIsLab.Name = "chkIsLab";
            chkIsLab.Size = new Size(61, 19);
            chkIsLab.TabIndex = 9;
            chkIsLab.Text = "Is Lab?";
            chkIsLab.UseVisualStyleBackColor = true;
            // 
            // txtUnits
            // 
            txtUnits.Location = new Point(6, 333);
            txtUnits.Name = "txtUnits";
            txtUnits.Size = new Size(381, 23);
            txtUnits.TabIndex = 8;
            // 
            // txtSubjCode
            // 
            txtSubjCode.Location = new Point(6, 269);
            txtSubjCode.Name = "txtSubjCode";
            txtSubjCode.Size = new Size(381, 23);
            txtSubjCode.TabIndex = 7;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(6, 305);
            label8.Name = "label8";
            label8.Size = new Size(37, 15);
            label8.TabIndex = 6;
            label8.Text = "Units:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(6, 241);
            label7.Name = "label7";
            label7.Size = new Size(64, 15);
            label7.TabIndex = 5;
            label7.Text = "Subj Code:";
            // 
            // cmbSectionList
            // 
            cmbSectionList.FormattingEnabled = true;
            cmbSectionList.Location = new Point(6, 205);
            cmbSectionList.Name = "cmbSectionList";
            cmbSectionList.Size = new Size(381, 23);
            cmbSectionList.TabIndex = 4;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(6, 177);
            label6.Name = "label6";
            label6.Size = new Size(83, 15);
            label6.TabIndex = 3;
            label6.Text = "Select Section:";
            // 
            // btnCreateSection
            // 
            btnCreateSection.Location = new Point(159, 132);
            btnCreateSection.Name = "btnCreateSection";
            btnCreateSection.Size = new Size(75, 23);
            btnCreateSection.TabIndex = 2;
            btnCreateSection.Text = "Create Section";
            btnCreateSection.UseVisualStyleBackColor = true;
            // 
            // txtSectionName
            // 
            txtSectionName.Location = new Point(6, 42);
            txtSectionName.Name = "txtSectionName";
            txtSectionName.Size = new Size(381, 23);
            txtSectionName.TabIndex = 1;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(6, 18);
            label5.Name = "label5";
            label5.Size = new Size(111, 15);
            label5.TabIndex = 0;
            label5.Text = "New Section Name:";
            // 
            // pnlViewSchedule
            // 
            pnlViewSchedule.Controls.Add(btnExportPdf);
            pnlViewSchedule.Controls.Add(label15);
            pnlViewSchedule.Controls.Add(dgvTimetable);
            pnlViewSchedule.Controls.Add(btnRestoreDatabase);
            pnlViewSchedule.Controls.Add(cmbFilterType);
            pnlViewSchedule.Controls.Add(btnGenerate);
            pnlViewSchedule.Controls.Add(btnBackupDatabase);
            pnlViewSchedule.Controls.Add(cmbScheduleView);
            pnlViewSchedule.Dock = DockStyle.Fill;
            pnlViewSchedule.Location = new Point(0, 0);
            pnlViewSchedule.Name = "pnlViewSchedule";
            pnlViewSchedule.Size = new Size(1280, 656);
            pnlViewSchedule.TabIndex = 0;
            // 
            // btnExportPdf
            // 
            btnExportPdf.Font = new Font("Ebrima", 9.75F);
            btnExportPdf.Location = new Point(1094, 43);
            btnExportPdf.Name = "btnExportPdf";
            btnExportPdf.Size = new Size(84, 23);
            btnExportPdf.TabIndex = 20;
            btnExportPdf.Text = "Export PDF";
            btnExportPdf.UseVisualStyleBackColor = true;
            btnExportPdf.Click += btnExportPdf_Click;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Font = new Font("Ebrima", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label15.Location = new Point(84, 46);
            label15.Name = "label15";
            label15.Size = new Size(54, 17);
            label15.TabIndex = 22;
            label15.Text = "Filter by";
            // 
            // dgvTimetable
            // 
            dgvTimetable.AllowUserToAddRows = false;
            dgvTimetable.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvTimetable.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvTimetable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvTimetable.DefaultCellStyle = dataGridViewCellStyle1;
            dgvTimetable.Location = new Point(84, 89);
            dgvTimetable.Name = "dgvTimetable";
            dgvTimetable.Size = new Size(1098, 474);
            dgvTimetable.TabIndex = 17;
            // 
            // btnRestoreDatabase
            // 
            btnRestoreDatabase.Font = new Font("Ebrima", 9.75F);
            btnRestoreDatabase.Location = new Point(988, 43);
            btnRestoreDatabase.Name = "btnRestoreDatabase";
            btnRestoreDatabase.Size = new Size(90, 23);
            btnRestoreDatabase.TabIndex = 21;
            btnRestoreDatabase.Text = "Load Backup";
            btnRestoreDatabase.UseVisualStyleBackColor = true;
            btnRestoreDatabase.Click += btnRestoreDatabase_Click;
            // 
            // cmbFilterType
            // 
            cmbFilterType.Font = new Font("Ebrima", 9.75F);
            cmbFilterType.FormattingEnabled = true;
            cmbFilterType.Items.AddRange(new object[] { "Section", "Teacher", "Room" });
            cmbFilterType.Location = new Point(151, 43);
            cmbFilterType.Name = "cmbFilterType";
            cmbFilterType.Size = new Size(265, 25);
            cmbFilterType.TabIndex = 18;
            cmbFilterType.Text = "Section";
            cmbFilterType.SelectedIndexChanged += cmbFilterType_SelectedIndexChanged;
            // 
            // btnGenerate
            // 
            btnGenerate.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnGenerate.BackColor = Color.FromArgb(64, 0, 0);
            btnGenerate.FlatAppearance.BorderSize = 0;
            btnGenerate.FlatStyle = FlatStyle.Popup;
            btnGenerate.Font = new Font("Ebrima", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnGenerate.ForeColor = Color.White;
            btnGenerate.Location = new Point(513, 590);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(217, 37);
            btnGenerate.TabIndex = 14;
            btnGenerate.Text = "Generate Schedule";
            btnGenerate.UseVisualStyleBackColor = false;
            btnGenerate.Click += btnGenerate_Click;
            // 
            // btnBackupDatabase
            // 
            btnBackupDatabase.Font = new Font("Ebrima", 9.75F);
            btnBackupDatabase.Location = new Point(851, 43);
            btnBackupDatabase.Name = "btnBackupDatabase";
            btnBackupDatabase.Size = new Size(113, 23);
            btnBackupDatabase.TabIndex = 16;
            btnBackupDatabase.Text = "Create Backup";
            btnBackupDatabase.UseVisualStyleBackColor = true;
            btnBackupDatabase.Click += btnBackupDatabase_Click;
            // 
            // cmbScheduleView
            // 
            cmbScheduleView.Font = new Font("Ebrima", 9.75F);
            cmbScheduleView.FormattingEnabled = true;
            cmbScheduleView.Location = new Point(457, 43);
            cmbScheduleView.Name = "cmbScheduleView";
            cmbScheduleView.Size = new Size(247, 25);
            cmbScheduleView.TabIndex = 15;
            // 
            // pnlViewPending
            // 
            pnlViewPending.Controls.Add(btnFindSlots);
            pnlViewPending.Controls.Add(dgvPending);
            pnlViewPending.Dock = DockStyle.Fill;
            pnlViewPending.Location = new Point(0, 0);
            pnlViewPending.Name = "pnlViewPending";
            pnlViewPending.Size = new Size(1280, 656);
            pnlViewPending.TabIndex = 3;
            // 
            // btnFindSlots
            // 
            btnFindSlots.BackColor = Color.FromArgb(64, 0, 0);
            btnFindSlots.FlatAppearance.BorderSize = 0;
            btnFindSlots.FlatStyle = FlatStyle.Popup;
            btnFindSlots.Font = new Font("Ebrima", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnFindSlots.ForeColor = Color.White;
            btnFindSlots.Location = new Point(84, 24);
            btnFindSlots.Name = "btnFindSlots";
            btnFindSlots.Size = new Size(169, 32);
            btnFindSlots.TabIndex = 3;
            btnFindSlots.Text = "Find Valid Slots";
            btnFindSlots.UseVisualStyleBackColor = false;
            // 
            // dgvPending
            // 
            dgvPending.AllowUserToAddRows = false;
            dgvPending.AllowUserToDeleteRows = false;
            dgvPending.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPending.Columns.AddRange(new DataGridViewColumn[] { Section, Subject, Reason });
            dgvPending.Location = new Point(84, 67);
            dgvPending.Name = "dgvPending";
            dgvPending.ReadOnly = true;
            dgvPending.Size = new Size(1098, 529);
            dgvPending.TabIndex = 2;
            // 
            // Section
            // 
            Section.HeaderText = "Section";
            Section.Name = "Section";
            Section.ReadOnly = true;
            // 
            // Subject
            // 
            Subject.HeaderText = "Subject";
            Subject.Name = "Subject";
            Subject.ReadOnly = true;
            // 
            // Reason
            // 
            Reason.HeaderText = "Reason";
            Reason.Name = "Reason";
            Reason.ReadOnly = true;
            // 
            // pnlViewMaster
            // 
            pnlViewMaster.Controls.Add(dgvMaster);
            pnlViewMaster.Dock = DockStyle.Fill;
            pnlViewMaster.Location = new Point(0, 0);
            pnlViewMaster.Name = "pnlViewMaster";
            pnlViewMaster.Size = new Size(1280, 656);
            pnlViewMaster.TabIndex = 1;
            // 
            // dgvMaster
            // 
            dgvMaster.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvMaster.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMaster.Location = new Point(84, 40);
            dgvMaster.Name = "dgvMaster";
            dgvMaster.ReadOnly = true;
            dgvMaster.Size = new Size(1098, 568);
            dgvMaster.TabIndex = 1;
            // 
            // btnLogout
            // 
            btnLogout.BackColor = Color.Transparent;
            btnLogout.BackgroundImage = (Image)resources.GetObject("btnLogout.BackgroundImage");
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatStyle = FlatStyle.Popup;
            btnLogout.Font = new Font("Ebrima", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLogout.ForeColor = Color.White;
            btnLogout.Location = new Point(1073, 17);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(109, 31);
            btnLogout.TabIndex = 19;
            btnLogout.Text = "Logout";
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += btnLogout_Click;
            // 
            // pnlNavBar
            // 
            pnlNavBar.BackColor = Color.FromArgb(90, 36, 36);
            pnlNavBar.Controls.Add(btnNavPending);
            pnlNavBar.Controls.Add(btnNavManage);
            pnlNavBar.Controls.Add(btnNavMaster);
            pnlNavBar.Controls.Add(btnLogout);
            pnlNavBar.Controls.Add(btnNavSchedule);
            pnlNavBar.Dock = DockStyle.Top;
            pnlNavBar.Location = new Point(0, 0);
            pnlNavBar.Name = "pnlNavBar";
            pnlNavBar.Size = new Size(1280, 64);
            pnlNavBar.TabIndex = 6;
            // 
            // btnNavPending
            // 
            btnNavPending.FlatAppearance.BorderSize = 0;
            btnNavPending.FlatStyle = FlatStyle.Flat;
            btnNavPending.Font = new Font("Ebrima", 9.75F);
            btnNavPending.ForeColor = Color.White;
            btnNavPending.Location = new Point(576, 19);
            btnNavPending.Name = "btnNavPending";
            btnNavPending.Size = new Size(99, 23);
            btnNavPending.TabIndex = 3;
            btnNavPending.Text = "Pending";
            btnNavPending.UseVisualStyleBackColor = true;
            btnNavPending.Click += btnNavPending_Click;
            // 
            // btnNavManage
            // 
            btnNavManage.FlatAppearance.BorderSize = 0;
            btnNavManage.FlatStyle = FlatStyle.Flat;
            btnNavManage.Font = new Font("Ebrima", 9.75F);
            btnNavManage.ForeColor = Color.White;
            btnNavManage.Location = new Point(408, 19);
            btnNavManage.Name = "btnNavManage";
            btnNavManage.Size = new Size(99, 23);
            btnNavManage.TabIndex = 2;
            btnNavManage.Text = "Manage Data";
            btnNavManage.UseVisualStyleBackColor = true;
            btnNavManage.Click += btnNavManage_Click;
            // 
            // btnNavMaster
            // 
            btnNavMaster.FlatAppearance.BorderSize = 0;
            btnNavMaster.FlatStyle = FlatStyle.Flat;
            btnNavMaster.Font = new Font("Ebrima", 9.75F);
            btnNavMaster.ForeColor = Color.White;
            btnNavMaster.Location = new Point(242, 19);
            btnNavMaster.Name = "btnNavMaster";
            btnNavMaster.Size = new Size(99, 23);
            btnNavMaster.TabIndex = 1;
            btnNavMaster.Text = "Master List";
            btnNavMaster.UseVisualStyleBackColor = true;
            btnNavMaster.Click += btnNavMaster_Click;
            // 
            // btnNavSchedule
            // 
            btnNavSchedule.FlatAppearance.BorderSize = 0;
            btnNavSchedule.FlatStyle = FlatStyle.Flat;
            btnNavSchedule.Font = new Font("Ebrima", 9.75F);
            btnNavSchedule.ForeColor = Color.White;
            btnNavSchedule.Location = new Point(84, 19);
            btnNavSchedule.Name = "btnNavSchedule";
            btnNavSchedule.Size = new Size(99, 23);
            btnNavSchedule.TabIndex = 0;
            btnNavSchedule.Text = "Schedule View";
            btnNavSchedule.UseVisualStyleBackColor = true;
            btnNavSchedule.Click += btnNavSchedule_Click;
            // 
            // AdminDashboard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 720);
            Controls.Add(pnlContent);
            Controls.Add(pnlNavBar);
            FormBorderStyle = FormBorderStyle.None;
            Name = "AdminDashboard";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Admin Dashboard";
            Load += Form1_Load_1;
            pnlContent.ResumeLayout(false);
            pnlViewManage.ResumeLayout(false);
            tabDataManagers.ResumeLayout(false);
            tabTeachers.ResumeLayout(false);
            tabTeachers.PerformLayout();
            tabRooms.ResumeLayout(false);
            tabRooms.PerformLayout();
            tabSections.ResumeLayout(false);
            groupBox8.ResumeLayout(false);
            groupBox8.PerformLayout();
            groupBox6.ResumeLayout(false);
            groupBox7.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            pnlViewSchedule.ResumeLayout(false);
            pnlViewSchedule.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTimetable).EndInit();
            pnlViewPending.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvPending).EndInit();
            pnlViewMaster.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMaster).EndInit();
            pnlNavBar.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private ContextMenuStrip ctxMenuSchedule;
        private Panel pnlContent;
        private Panel pnlViewMaster;
        private Panel pnlViewSchedule;
        private Panel pnlViewManage;
        private Panel pnlViewPending;
        private Button btnRestoreDatabase;
        private Button btnExportPdf;
        private Button btnLogout;
        private ComboBox cmbFilterType;
        private Button btnGenerate;
        private DataGridView dgvTimetable;
        private Button btnBackupDatabase;
        private ComboBox cmbScheduleView;
        private TabControl tabDataManagers;
        private TabPage tabTeachers;
        private TabPage tabRooms;
        private ListBox lstRooms;
        private Button btnDeleteRoom;
        private TabPage tabSections;
        private GroupBox groupBox8;
        private Button btnBatchAdd;
        private CheckBox chkBatchLab;
        private TextBox txtBatchUnits;
        private Label label12;
        private TextBox txtBatchCode;
        private Label label11;
        private ComboBox cmbBatchYear;
        private Label label10;
        private ComboBox cmbBatchProgram;
        private Label label9;
        private GroupBox groupBox6;
        private Button btnRemoveSubject;
        private ListBox lstSectionSubjects;
        private GroupBox groupBox7;
        private ListBox lstSections;
        private Button btnDeleteSection;
        private GroupBox groupBox3;
        private ComboBox cmbSectionYear;
        private Label label14;
        private ComboBox cmbSectionProgram;
        private Label label13;
        private Button btnSaveChanges;
        private Button btnCancelSubject;
        private Button btnAddSubject;
        private CheckBox chkIsLab;
        private TextBox txtUnits;
        private TextBox txtSubjCode;
        private Label label8;
        private Label label7;
        private ComboBox cmbSectionList;
        private Label label6;
        private Button btnCreateSection;
        private TextBox txtSectionName;
        private Label label5;
        private DataGridView dgvMaster;
        private Button btnFindSlots;
        private DataGridView dgvPending;
        private DataGridViewTextBoxColumn Section;
        private DataGridViewTextBoxColumn Subject;
        private DataGridViewTextBoxColumn Reason;
        private Panel pnlNavBar;
        private Button btnNavMaster;
        private Button btnNavSchedule;
        private Button btnNavManage;
        private Button btnNavPending;
        private Label label15;
        private Button btnCancelTeacher;
        private Button btnDeleteTeacher;
        private Button btnUpdateTeacher;
        private ListBox lstTeachers;
        private Button btnAddTeacher;
        private TextBox txtTeacherSubjects;
        private TextBox txtTeacherName;
        private Label label17;
        private Label label16;
        private Button btnCancelRoom;
        private Button btnUpdateRoom;
        private Button btnAddRoom;
        private Label label1;
        private TextBox txtRoomName;
        private ComboBox cmbRoomType;
        private Label label2;
    }
}
