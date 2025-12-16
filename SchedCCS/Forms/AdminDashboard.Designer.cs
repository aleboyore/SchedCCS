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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminDashboard));
            ctxMenuSchedule = new ContextMenuStrip(components);
            pnlContent = new Panel();
            pnlViewSchedule = new Panel();
            panel1 = new Panel();
            btnExportPdf = new Button();
            btnBackupDatabase = new Button();
            btnRestoreDatabase = new Button();
            label19 = new Label();
            label15 = new Label();
            dgvTimetable = new DataGridView();
            cmbFilterType = new ComboBox();
            btnGenerate = new Button();
            cmbScheduleView = new ComboBox();
            pnlViewMaster = new Panel();
            dgvMaster = new DataGridView();
            pnlViewManage = new Panel();
            pnlViewRooms = new Panel();
            label12 = new Label();
            label1 = new Label();
            label11 = new Label();
            lstRooms = new ListBox();
            btnDeleteRoom = new Button();
            btnUpdateRoom = new Button();
            label2 = new Label();
            btnCancelRoom = new Button();
            btnAddRoom = new Button();
            txtRoomName = new TextBox();
            cmbRoomType = new ComboBox();
            pnlViewSections = new Panel();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            cmbSectionYear = new ComboBox();
            label14 = new Label();
            lstSections = new ListBox();
            cmbSectionProgram = new ComboBox();
            btnRemoveSubject = new Button();
            label13 = new Label();
            btnDeleteSection = new Button();
            btnSaveChanges = new Button();
            btnBatchAdd = new Button();
            btnCancelSubject = new Button();
            btnAddSubject = new Button();
            lstSectionSubjects = new ListBox();
            chkIsLab = new CheckBox();
            txtUnits = new TextBox();
            txtSubjCode = new TextBox();
            label8 = new Label();
            label7 = new Label();
            cmbBatchYear = new ComboBox();
            cmbSectionList = new ComboBox();
            label9 = new Label();
            label6 = new Label();
            label10 = new Label();
            btnCreateSection = new Button();
            cmbBatchProgram = new ComboBox();
            txtSectionName = new TextBox();
            pnlViewTeachers = new Panel();
            label18 = new Label();
            lstTeachers = new ListBox();
            btnUpdateTeacher = new Button();
            btnDeleteTeacher = new Button();
            btnCancelTeacher = new Button();
            btnAddTeacher = new Button();
            txtTeacherSubjects = new TextBox();
            txtTeacherName = new TextBox();
            label17 = new Label();
            label16 = new Label();
            pnlManageDataNav = new Panel();
            btnSubNavSections = new Button();
            btnSubNavTeachers = new Button();
            btnSubNavRooms = new Button();
            pnlViewPending = new Panel();
            btnFindSlots = new Button();
            dgvPending = new DataGridView();
            Section = new DataGridViewTextBoxColumn();
            Subject = new DataGridViewTextBoxColumn();
            Reason = new DataGridViewTextBoxColumn();
            btnLogout = new Button();
            pnlNavBar = new Panel();
            btnNavPending = new Button();
            btnNavManage = new Button();
            btnNavMaster = new Button();
            btnNavSchedule = new Button();
            pnlContent.SuspendLayout();
            pnlViewSchedule.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTimetable).BeginInit();
            pnlViewMaster.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMaster).BeginInit();
            pnlViewManage.SuspendLayout();
            pnlViewRooms.SuspendLayout();
            pnlViewSections.SuspendLayout();
            pnlViewTeachers.SuspendLayout();
            pnlManageDataNav.SuspendLayout();
            pnlViewPending.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPending).BeginInit();
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
            pnlContent.Controls.Add(pnlViewManage);
            pnlContent.Controls.Add(pnlViewSchedule);
            pnlContent.Controls.Add(pnlViewMaster);
            pnlContent.Controls.Add(pnlViewPending);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(0, 64);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new Size(1280, 656);
            pnlContent.TabIndex = 5;
            // 
            // pnlViewSchedule
            // 
            pnlViewSchedule.Controls.Add(panel1);
            pnlViewSchedule.Controls.Add(label19);
            pnlViewSchedule.Controls.Add(label15);
            pnlViewSchedule.Controls.Add(dgvTimetable);
            pnlViewSchedule.Controls.Add(cmbFilterType);
            pnlViewSchedule.Controls.Add(btnGenerate);
            pnlViewSchedule.Controls.Add(cmbScheduleView);
            pnlViewSchedule.Dock = DockStyle.Fill;
            pnlViewSchedule.Location = new Point(0, 0);
            pnlViewSchedule.Name = "pnlViewSchedule";
            pnlViewSchedule.Size = new Size(1280, 656);
            pnlViewSchedule.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(245, 243, 244);
            panel1.Controls.Add(btnExportPdf);
            panel1.Controls.Add(btnBackupDatabase);
            panel1.Controls.Add(btnRestoreDatabase);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1280, 56);
            panel1.TabIndex = 4;
            // 
            // btnExportPdf
            // 
            btnExportPdf.BackColor = Color.Transparent;
            btnExportPdf.FlatAppearance.BorderSize = 0;
            btnExportPdf.FlatStyle = FlatStyle.Flat;
            btnExportPdf.Font = new Font("Ebrima", 12F);
            btnExportPdf.ForeColor = Color.Maroon;
            btnExportPdf.Location = new Point(724, 9);
            btnExportPdf.Name = "btnExportPdf";
            btnExportPdf.Size = new Size(113, 37);
            btnExportPdf.TabIndex = 20;
            btnExportPdf.Text = "Export PDF";
            btnExportPdf.UseVisualStyleBackColor = false;
            btnExportPdf.Click += btnExportPdf_Click;
            // 
            // btnBackupDatabase
            // 
            btnBackupDatabase.BackColor = Color.Transparent;
            btnBackupDatabase.FlatAppearance.BorderSize = 0;
            btnBackupDatabase.FlatStyle = FlatStyle.Flat;
            btnBackupDatabase.Font = new Font("Ebrima", 12F);
            btnBackupDatabase.ForeColor = Color.Maroon;
            btnBackupDatabase.Location = new Point(402, 9);
            btnBackupDatabase.Name = "btnBackupDatabase";
            btnBackupDatabase.Size = new Size(142, 37);
            btnBackupDatabase.TabIndex = 16;
            btnBackupDatabase.Text = "Create Backup";
            btnBackupDatabase.UseVisualStyleBackColor = false;
            btnBackupDatabase.Click += btnBackupDatabase_Click;
            // 
            // btnRestoreDatabase
            // 
            btnRestoreDatabase.BackColor = Color.Transparent;
            btnRestoreDatabase.FlatAppearance.BorderSize = 0;
            btnRestoreDatabase.FlatStyle = FlatStyle.Flat;
            btnRestoreDatabase.Font = new Font("Ebrima", 12F);
            btnRestoreDatabase.ForeColor = Color.Maroon;
            btnRestoreDatabase.Location = new Point(571, 9);
            btnRestoreDatabase.Name = "btnRestoreDatabase";
            btnRestoreDatabase.Size = new Size(119, 37);
            btnRestoreDatabase.TabIndex = 21;
            btnRestoreDatabase.Text = "Load Backup";
            btnRestoreDatabase.UseVisualStyleBackColor = false;
            btnRestoreDatabase.Click += btnRestoreDatabase_Click;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Font = new Font("Ebrima", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label19.Location = new Point(493, 106);
            label19.Name = "label19";
            label19.Size = new Size(57, 17);
            label19.TabIndex = 23;
            label19.Text = "Selected";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Font = new Font("Ebrima", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label15.Location = new Point(115, 105);
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
            dgvTimetable.BackgroundColor = Color.FromArgb(224, 224, 224);
            dgvTimetable.BorderStyle = BorderStyle.None;
            dgvTimetable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvTimetable.DefaultCellStyle = dataGridViewCellStyle1;
            dgvTimetable.Location = new Point(116, 133);
            dgvTimetable.Name = "dgvTimetable";
            dgvTimetable.Size = new Size(1066, 463);
            dgvTimetable.TabIndex = 17;
            dgvTimetable.CellMouseClick += dgvTimetable_CellMouseClick;
            dgvTimetable.Resize += dgvTimetable_Resize;
            // 
            // cmbFilterType
            // 
            cmbFilterType.Font = new Font("Ebrima", 9.75F);
            cmbFilterType.FormattingEnabled = true;
            cmbFilterType.Items.AddRange(new object[] { "Section", "Teacher", "Room" });
            cmbFilterType.Location = new Point(187, 102);
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
            btnGenerate.Location = new Point(866, 102);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(316, 25);
            btnGenerate.TabIndex = 14;
            btnGenerate.Text = "Generate Schedule";
            btnGenerate.UseVisualStyleBackColor = false;
            btnGenerate.Click += btnGenerate_Click;
            // 
            // cmbScheduleView
            // 
            cmbScheduleView.Font = new Font("Ebrima", 9.75F);
            cmbScheduleView.FormattingEnabled = true;
            cmbScheduleView.Location = new Point(577, 103);
            cmbScheduleView.Name = "cmbScheduleView";
            cmbScheduleView.Size = new Size(274, 25);
            cmbScheduleView.TabIndex = 15;
            cmbScheduleView.SelectedIndexChanged += cmbScheduleView_SelectedIndexChanged;
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
            dgvMaster.BackgroundColor = Color.FromArgb(224, 224, 224);
            dgvMaster.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMaster.Location = new Point(106, 56);
            dgvMaster.Name = "dgvMaster";
            dgvMaster.ReadOnly = true;
            dgvMaster.Size = new Size(1076, 527);
            dgvMaster.TabIndex = 1;
            dgvMaster.ColumnHeaderMouseClick += dgvMaster_ColumnHeaderMouseClick;
            // 
            // pnlViewManage
            // 
            pnlViewManage.BackColor = Color.FromArgb(215, 216, 216);
            pnlViewManage.Controls.Add(pnlViewSections);
            pnlViewManage.Controls.Add(pnlViewRooms);
            pnlViewManage.Controls.Add(pnlViewTeachers);
            pnlViewManage.Controls.Add(pnlManageDataNav);
            pnlViewManage.Dock = DockStyle.Fill;
            pnlViewManage.Location = new Point(0, 0);
            pnlViewManage.Name = "pnlViewManage";
            pnlViewManage.Size = new Size(1280, 656);
            pnlViewManage.TabIndex = 2;
            // 
            // pnlViewRooms
            // 
            pnlViewRooms.BackgroundImage = (Image)resources.GetObject("pnlViewRooms.BackgroundImage");
            pnlViewRooms.Controls.Add(label12);
            pnlViewRooms.Controls.Add(label1);
            pnlViewRooms.Controls.Add(label11);
            pnlViewRooms.Controls.Add(lstRooms);
            pnlViewRooms.Controls.Add(btnDeleteRoom);
            pnlViewRooms.Controls.Add(btnUpdateRoom);
            pnlViewRooms.Controls.Add(label2);
            pnlViewRooms.Controls.Add(btnCancelRoom);
            pnlViewRooms.Controls.Add(btnAddRoom);
            pnlViewRooms.Controls.Add(txtRoomName);
            pnlViewRooms.Controls.Add(cmbRoomType);
            pnlViewRooms.Dock = DockStyle.Fill;
            pnlViewRooms.Location = new Point(0, 56);
            pnlViewRooms.Name = "pnlViewRooms";
            pnlViewRooms.Size = new Size(1280, 600);
            pnlViewRooms.TabIndex = 3;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.BackColor = Color.Transparent;
            label12.Font = new Font("Ebrima", 9.75F);
            label12.Location = new Point(160, 202);
            label12.Name = "label12";
            label12.Size = new Size(96, 17);
            label12.TabIndex = 19;
            label12.Text = "- Room Name-";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Ebrima", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Maroon;
            label1.Location = new Point(591, 140);
            label1.Name = "label1";
            label1.Size = new Size(99, 20);
            label1.TabIndex = 18;
            label1.Text = "Room(s) List";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.BackColor = Color.Transparent;
            label11.Font = new Font("Ebrima", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label11.ForeColor = Color.Maroon;
            label11.Location = new Point(164, 155);
            label11.Name = "label11";
            label11.Size = new Size(120, 20);
            label11.TabIndex = 17;
            label11.Text = "Add New Room";
            // 
            // lstRooms
            // 
            lstRooms.BackColor = Color.FromArgb(224, 224, 224);
            lstRooms.BorderStyle = BorderStyle.None;
            lstRooms.Font = new Font("Ebrima", 9.75F);
            lstRooms.FormattingEnabled = true;
            lstRooms.ItemHeight = 17;
            lstRooms.Location = new Point(594, 174);
            lstRooms.Name = "lstRooms";
            lstRooms.Size = new Size(540, 255);
            lstRooms.TabIndex = 8;
            lstRooms.SelectedIndexChanged += lstRooms_SelectedIndexChanged;
            // 
            // btnDeleteRoom
            // 
            btnDeleteRoom.BackColor = Color.Maroon;
            btnDeleteRoom.FlatAppearance.BorderSize = 0;
            btnDeleteRoom.FlatStyle = FlatStyle.Popup;
            btnDeleteRoom.Font = new Font("Ebrima", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDeleteRoom.ForeColor = Color.White;
            btnDeleteRoom.Location = new Point(993, 140);
            btnDeleteRoom.Name = "btnDeleteRoom";
            btnDeleteRoom.Size = new Size(141, 25);
            btnDeleteRoom.TabIndex = 13;
            btnDeleteRoom.Text = "Delete Selected";
            btnDeleteRoom.UseVisualStyleBackColor = false;
            btnDeleteRoom.Click += btnDeleteRoom_Click;
            // 
            // btnUpdateRoom
            // 
            btnUpdateRoom.BackColor = Color.FromArgb(64, 0, 0);
            btnUpdateRoom.Enabled = false;
            btnUpdateRoom.FlatAppearance.BorderSize = 0;
            btnUpdateRoom.FlatStyle = FlatStyle.Flat;
            btnUpdateRoom.Font = new Font("Ebrima", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnUpdateRoom.ForeColor = Color.White;
            btnUpdateRoom.Location = new Point(866, 140);
            btnUpdateRoom.Name = "btnUpdateRoom";
            btnUpdateRoom.Size = new Size(121, 25);
            btnUpdateRoom.TabIndex = 16;
            btnUpdateRoom.Text = "Update";
            btnUpdateRoom.UseVisualStyleBackColor = false;
            btnUpdateRoom.Click += btnUpdateRoom_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Ebrima", 9.75F);
            label2.Location = new Point(160, 280);
            label2.Name = "label2";
            label2.Size = new Size(93, 17);
            label2.TabIndex = 15;
            label2.Text = "- Room Type -";
            // 
            // btnCancelRoom
            // 
            btnCancelRoom.BackColor = Color.Maroon;
            btnCancelRoom.FlatAppearance.BorderSize = 0;
            btnCancelRoom.FlatStyle = FlatStyle.Flat;
            btnCancelRoom.Font = new Font("Ebrima", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancelRoom.ForeColor = Color.White;
            btnCancelRoom.Location = new Point(164, 398);
            btnCancelRoom.Name = "btnCancelRoom";
            btnCancelRoom.Size = new Size(303, 25);
            btnCancelRoom.TabIndex = 12;
            btnCancelRoom.Text = "Cancel";
            btnCancelRoom.UseVisualStyleBackColor = false;
            btnCancelRoom.Click += btnCancelRoom_Click;
            // 
            // btnAddRoom
            // 
            btnAddRoom.BackColor = Color.FromArgb(64, 0, 0);
            btnAddRoom.FlatAppearance.BorderSize = 0;
            btnAddRoom.FlatStyle = FlatStyle.Popup;
            btnAddRoom.Font = new Font("Ebrima", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAddRoom.ForeColor = Color.White;
            btnAddRoom.Location = new Point(164, 360);
            btnAddRoom.Name = "btnAddRoom";
            btnAddRoom.Size = new Size(303, 25);
            btnAddRoom.TabIndex = 11;
            btnAddRoom.Text = "Add Room";
            btnAddRoom.UseVisualStyleBackColor = false;
            btnAddRoom.Click += btnAddRoom_Click;
            // 
            // txtRoomName
            // 
            txtRoomName.Font = new Font("Ebrima", 9.75F);
            txtRoomName.Location = new Point(164, 229);
            txtRoomName.Name = "txtRoomName";
            txtRoomName.Size = new Size(299, 25);
            txtRoomName.TabIndex = 9;
            // 
            // cmbRoomType
            // 
            cmbRoomType.Font = new Font("Ebrima", 9.75F);
            cmbRoomType.FormattingEnabled = true;
            cmbRoomType.Items.AddRange(new object[] { "Lecture", "Laboratory" });
            cmbRoomType.Location = new Point(161, 307);
            cmbRoomType.Name = "cmbRoomType";
            cmbRoomType.Size = new Size(302, 25);
            cmbRoomType.TabIndex = 10;
            // 
            // pnlViewSections
            // 
            pnlViewSections.BackgroundImage = (Image)resources.GetObject("pnlViewSections.BackgroundImage");
            pnlViewSections.Controls.Add(label5);
            pnlViewSections.Controls.Add(label4);
            pnlViewSections.Controls.Add(label3);
            pnlViewSections.Controls.Add(cmbSectionYear);
            pnlViewSections.Controls.Add(label14);
            pnlViewSections.Controls.Add(lstSections);
            pnlViewSections.Controls.Add(cmbSectionProgram);
            pnlViewSections.Controls.Add(btnRemoveSubject);
            pnlViewSections.Controls.Add(label13);
            pnlViewSections.Controls.Add(btnDeleteSection);
            pnlViewSections.Controls.Add(btnSaveChanges);
            pnlViewSections.Controls.Add(btnBatchAdd);
            pnlViewSections.Controls.Add(btnCancelSubject);
            pnlViewSections.Controls.Add(btnAddSubject);
            pnlViewSections.Controls.Add(lstSectionSubjects);
            pnlViewSections.Controls.Add(chkIsLab);
            pnlViewSections.Controls.Add(txtUnits);
            pnlViewSections.Controls.Add(txtSubjCode);
            pnlViewSections.Controls.Add(label8);
            pnlViewSections.Controls.Add(label7);
            pnlViewSections.Controls.Add(cmbBatchYear);
            pnlViewSections.Controls.Add(cmbSectionList);
            pnlViewSections.Controls.Add(label9);
            pnlViewSections.Controls.Add(label6);
            pnlViewSections.Controls.Add(label10);
            pnlViewSections.Controls.Add(btnCreateSection);
            pnlViewSections.Controls.Add(cmbBatchProgram);
            pnlViewSections.Controls.Add(txtSectionName);
            pnlViewSections.Dock = DockStyle.Fill;
            pnlViewSections.Font = new Font("Ebrima", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            pnlViewSections.Location = new Point(0, 56);
            pnlViewSections.Name = "pnlViewSections";
            pnlViewSections.Size = new Size(1280, 600);
            pnlViewSections.TabIndex = 5;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Ebrima", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.Maroon;
            label5.Location = new Point(121, 51);
            label5.Name = "label5";
            label5.Size = new Size(145, 20);
            label5.TabIndex = 22;
            label5.Text = "Create New Section";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Ebrima", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.Maroon;
            label4.Location = new Point(664, 332);
            label4.Name = "label4";
            label4.Size = new Size(95, 17);
            label4.TabIndex = 21;
            label4.Text = "Subject(s) List";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Ebrima", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.Maroon;
            label3.Location = new Point(128, 332);
            label3.Name = "label3";
            label3.Size = new Size(95, 17);
            label3.TabIndex = 20;
            label3.Text = "Section(s) List";
            // 
            // cmbSectionYear
            // 
            cmbSectionYear.BackColor = SystemColors.Window;
            cmbSectionYear.FormattingEnabled = true;
            cmbSectionYear.Items.AddRange(new object[] { "1", "2", "3", "4" });
            cmbSectionYear.Location = new Point(124, 187);
            cmbSectionYear.Name = "cmbSectionYear";
            cmbSectionYear.Size = new Size(287, 25);
            cmbSectionYear.TabIndex = 16;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.BackColor = Color.Transparent;
            label14.Location = new Point(126, 163);
            label14.Name = "label14";
            label14.Size = new Size(123, 17);
            label14.TabIndex = 15;
            label14.Text = "- Select Year Level -";
            // 
            // lstSections
            // 
            lstSections.BackColor = Color.FromArgb(224, 224, 224);
            lstSections.BorderStyle = BorderStyle.None;
            lstSections.FormattingEnabled = true;
            lstSections.ItemHeight = 17;
            lstSections.Location = new Point(131, 366);
            lstSections.Name = "lstSections";
            lstSections.Size = new Size(470, 187);
            lstSections.TabIndex = 19;
            lstSections.SelectedIndexChanged += lstSections_SelectedIndexChanged;
            // 
            // cmbSectionProgram
            // 
            cmbSectionProgram.BackColor = SystemColors.Window;
            cmbSectionProgram.FormattingEnabled = true;
            cmbSectionProgram.Items.AddRange(new object[] { "BSCS", "BSIT" });
            cmbSectionProgram.Location = new Point(128, 119);
            cmbSectionProgram.Name = "cmbSectionProgram";
            cmbSectionProgram.Size = new Size(283, 25);
            cmbSectionProgram.TabIndex = 14;
            // 
            // btnRemoveSubject
            // 
            btnRemoveSubject.BackColor = Color.Maroon;
            btnRemoveSubject.FlatAppearance.BorderSize = 0;
            btnRemoveSubject.FlatStyle = FlatStyle.Flat;
            btnRemoveSubject.Font = new Font("Ebrima", 9.75F, FontStyle.Bold);
            btnRemoveSubject.ForeColor = Color.White;
            btnRemoveSubject.Location = new Point(993, 324);
            btnRemoveSubject.Name = "btnRemoveSubject";
            btnRemoveSubject.Size = new Size(143, 25);
            btnRemoveSubject.TabIndex = 1;
            btnRemoveSubject.Text = "Remove Subject";
            btnRemoveSubject.UseVisualStyleBackColor = false;
            btnRemoveSubject.Click += btnRemoveSubject_Click;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.BackColor = Color.Transparent;
            label13.Location = new Point(126, 93);
            label13.Name = "label13";
            label13.Size = new Size(115, 17);
            label13.TabIndex = 13;
            label13.Text = "- Select Program -";
            // 
            // btnDeleteSection
            // 
            btnDeleteSection.BackColor = Color.Maroon;
            btnDeleteSection.FlatAppearance.BorderSize = 0;
            btnDeleteSection.FlatStyle = FlatStyle.Flat;
            btnDeleteSection.Font = new Font("Ebrima", 9.75F, FontStyle.Bold);
            btnDeleteSection.ForeColor = Color.White;
            btnDeleteSection.Location = new Point(469, 326);
            btnDeleteSection.Name = "btnDeleteSection";
            btnDeleteSection.Size = new Size(129, 25);
            btnDeleteSection.TabIndex = 0;
            btnDeleteSection.Text = "Delete Section";
            btnDeleteSection.UseVisualStyleBackColor = false;
            btnDeleteSection.Click += btnDeleteSection_Click;
            // 
            // btnSaveChanges
            // 
            btnSaveChanges.BackColor = Color.FromArgb(64, 0, 0);
            btnSaveChanges.FlatAppearance.BorderSize = 0;
            btnSaveChanges.FlatStyle = FlatStyle.Flat;
            btnSaveChanges.Font = new Font("Ebrima", 9.75F, FontStyle.Bold);
            btnSaveChanges.ForeColor = Color.White;
            btnSaveChanges.Location = new Point(974, 143);
            btnSaveChanges.Name = "btnSaveChanges";
            btnSaveChanges.Size = new Size(148, 25);
            btnSaveChanges.TabIndex = 12;
            btnSaveChanges.Text = "Update";
            btnSaveChanges.UseVisualStyleBackColor = false;
            btnSaveChanges.Click += btnSaveChanges_Click;
            // 
            // btnBatchAdd
            // 
            btnBatchAdd.BackColor = Color.FromArgb(64, 0, 0);
            btnBatchAdd.FlatAppearance.BorderSize = 0;
            btnBatchAdd.FlatStyle = FlatStyle.Flat;
            btnBatchAdd.Font = new Font("Ebrima", 9.75F, FontStyle.Bold);
            btnBatchAdd.ForeColor = Color.White;
            btnBatchAdd.Location = new Point(974, 97);
            btnBatchAdd.Name = "btnBatchAdd";
            btnBatchAdd.Size = new Size(148, 25);
            btnBatchAdd.TabIndex = 9;
            btnBatchAdd.Text = "Add Multiple";
            btnBatchAdd.UseVisualStyleBackColor = false;
            btnBatchAdd.Click += btnBatchAdd_Click;
            // 
            // btnCancelSubject
            // 
            btnCancelSubject.BackColor = Color.Maroon;
            btnCancelSubject.FlatAppearance.BorderSize = 0;
            btnCancelSubject.FlatStyle = FlatStyle.Flat;
            btnCancelSubject.Font = new Font("Ebrima", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancelSubject.ForeColor = Color.White;
            btnCancelSubject.Location = new Point(628, 235);
            btnCancelSubject.Name = "btnCancelSubject";
            btnCancelSubject.Size = new Size(494, 25);
            btnCancelSubject.TabIndex = 11;
            btnCancelSubject.Text = "Cancel";
            btnCancelSubject.UseVisualStyleBackColor = false;
            btnCancelSubject.Click += btnCancelSection_Click;
            // 
            // btnAddSubject
            // 
            btnAddSubject.BackColor = Color.FromArgb(64, 0, 0);
            btnAddSubject.FlatAppearance.BorderSize = 0;
            btnAddSubject.FlatStyle = FlatStyle.Flat;
            btnAddSubject.Font = new Font("Ebrima", 9.75F, FontStyle.Bold);
            btnAddSubject.ForeColor = Color.White;
            btnAddSubject.Location = new Point(974, 52);
            btnAddSubject.Name = "btnAddSubject";
            btnAddSubject.Size = new Size(148, 25);
            btnAddSubject.TabIndex = 10;
            btnAddSubject.Text = "Add Subject";
            btnAddSubject.UseVisualStyleBackColor = false;
            btnAddSubject.Click += btnAddSubject_Click;
            // 
            // lstSectionSubjects
            // 
            lstSectionSubjects.BackColor = Color.FromArgb(224, 224, 224);
            lstSectionSubjects.BorderStyle = BorderStyle.None;
            lstSectionSubjects.FormattingEnabled = true;
            lstSectionSubjects.ItemHeight = 17;
            lstSectionSubjects.Location = new Point(666, 367);
            lstSectionSubjects.Name = "lstSectionSubjects";
            lstSectionSubjects.Size = new Size(470, 187);
            lstSectionSubjects.TabIndex = 0;
            lstSectionSubjects.SelectedIndexChanged += lstSectionSubjects_SelectedIndexChanged;
            // 
            // chkIsLab
            // 
            chkIsLab.AutoSize = true;
            chkIsLab.BackColor = Color.Transparent;
            chkIsLab.Location = new Point(501, 239);
            chkIsLab.Name = "chkIsLab";
            chkIsLab.Size = new Size(67, 21);
            chkIsLab.TabIndex = 9;
            chkIsLab.Text = "Is Lab?";
            chkIsLab.UseVisualStyleBackColor = false;
            // 
            // txtUnits
            // 
            txtUnits.Location = new Point(974, 191);
            txtUnits.Name = "txtUnits";
            txtUnits.Size = new Size(148, 25);
            txtUnits.TabIndex = 8;
            // 
            // txtSubjCode
            // 
            txtSubjCode.Location = new Point(628, 192);
            txtSubjCode.Name = "txtSubjCode";
            txtSubjCode.Size = new Size(180, 25);
            txtSubjCode.TabIndex = 7;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.BackColor = Color.Transparent;
            label8.Location = new Point(852, 194);
            label8.Name = "label8";
            label8.Size = new Size(96, 17);
            label8.TabIndex = 6;
            label8.Text = "- No. of Units -";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.Transparent;
            label7.Location = new Point(495, 196);
            label7.Name = "label7";
            label7.Size = new Size(102, 17);
            label7.TabIndex = 5;
            label7.Text = "- Course Code -";
            // 
            // cmbBatchYear
            // 
            cmbBatchYear.FormattingEnabled = true;
            cmbBatchYear.Items.AddRange(new object[] { "1", "2", "3", "4" });
            cmbBatchYear.Location = new Point(628, 144);
            cmbBatchYear.Name = "cmbBatchYear";
            cmbBatchYear.Size = new Size(326, 25);
            cmbBatchYear.TabIndex = 3;
            // 
            // cmbSectionList
            // 
            cmbSectionList.FormattingEnabled = true;
            cmbSectionList.Location = new Point(628, 51);
            cmbSectionList.Name = "cmbSectionList";
            cmbSectionList.Size = new Size(326, 25);
            cmbSectionList.TabIndex = 4;
            cmbSectionList.SelectedIndexChanged += cmbSectionList_SelectedIndexChanged;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.BackColor = Color.Transparent;
            label9.Location = new Point(495, 100);
            label9.Name = "label9";
            label9.Size = new Size(119, 17);
            label9.TabIndex = 0;
            label9.Text = "- Target Program -";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.Transparent;
            label6.Font = new Font("Ebrima", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.Maroon;
            label6.Location = new Point(493, 52);
            label6.Name = "label6";
            label6.Size = new Size(122, 20);
            label6.TabIndex = 3;
            label6.Text = "Selected Section";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.BackColor = Color.Transparent;
            label10.Location = new Point(496, 148);
            label10.Name = "label10";
            label10.Size = new Size(94, 17);
            label10.TabIndex = 2;
            label10.Text = "- Target Year -";
            // 
            // btnCreateSection
            // 
            btnCreateSection.BackColor = Color.FromArgb(64, 0, 0);
            btnCreateSection.FlatAppearance.BorderSize = 0;
            btnCreateSection.FlatStyle = FlatStyle.Flat;
            btnCreateSection.Font = new Font("Ebrima", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCreateSection.ForeColor = Color.White;
            btnCreateSection.Location = new Point(125, 235);
            btnCreateSection.Name = "btnCreateSection";
            btnCreateSection.Size = new Size(285, 25);
            btnCreateSection.TabIndex = 2;
            btnCreateSection.Text = "Create Section";
            btnCreateSection.UseVisualStyleBackColor = false;
            btnCreateSection.Click += btnCreateSection_Click;
            // 
            // cmbBatchProgram
            // 
            cmbBatchProgram.FormattingEnabled = true;
            cmbBatchProgram.Items.AddRange(new object[] { "BSCS", "BSIT" });
            cmbBatchProgram.Location = new Point(628, 97);
            cmbBatchProgram.Name = "cmbBatchProgram";
            cmbBatchProgram.Size = new Size(326, 25);
            cmbBatchProgram.TabIndex = 1;
            // 
            // txtSectionName
            // 
            txtSectionName.BackColor = Color.White;
            txtSectionName.BorderStyle = BorderStyle.FixedSingle;
            txtSectionName.Location = new Point(272, 49);
            txtSectionName.Name = "txtSectionName";
            txtSectionName.PlaceholderText = "  Input section here";
            txtSectionName.Size = new Size(138, 25);
            txtSectionName.TabIndex = 1;
            // 
            // pnlViewTeachers
            // 
            pnlViewTeachers.BackgroundImage = (Image)resources.GetObject("pnlViewTeachers.BackgroundImage");
            pnlViewTeachers.Controls.Add(label18);
            pnlViewTeachers.Controls.Add(lstTeachers);
            pnlViewTeachers.Controls.Add(btnUpdateTeacher);
            pnlViewTeachers.Controls.Add(btnDeleteTeacher);
            pnlViewTeachers.Controls.Add(btnCancelTeacher);
            pnlViewTeachers.Controls.Add(btnAddTeacher);
            pnlViewTeachers.Controls.Add(txtTeacherSubjects);
            pnlViewTeachers.Controls.Add(txtTeacherName);
            pnlViewTeachers.Controls.Add(label17);
            pnlViewTeachers.Controls.Add(label16);
            pnlViewTeachers.Dock = DockStyle.Fill;
            pnlViewTeachers.Location = new Point(0, 56);
            pnlViewTeachers.Name = "pnlViewTeachers";
            pnlViewTeachers.Size = new Size(1280, 600);
            pnlViewTeachers.TabIndex = 4;
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.BackColor = Color.Transparent;
            label18.Font = new Font("Ebrima", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label18.ForeColor = Color.Maroon;
            label18.Location = new Point(154, 153);
            label18.Name = "label18";
            label18.Size = new Size(97, 20);
            label18.TabIndex = 23;
            label18.Text = "Add Teacher";
            // 
            // lstTeachers
            // 
            lstTeachers.BackColor = Color.FromArgb(224, 224, 224);
            lstTeachers.BorderStyle = BorderStyle.None;
            lstTeachers.Font = new Font("Ebrima", 9.75F);
            lstTeachers.FormattingEnabled = true;
            lstTeachers.ItemHeight = 17;
            lstTeachers.Location = new Point(604, 177);
            lstTeachers.Name = "lstTeachers";
            lstTeachers.Size = new Size(518, 255);
            lstTeachers.TabIndex = 14;
            lstTeachers.SelectedIndexChanged += lstTeachers_SelectedIndexChanged;
            // 
            // btnUpdateTeacher
            // 
            btnUpdateTeacher.BackColor = Color.FromArgb(64, 0, 0);
            btnUpdateTeacher.Enabled = false;
            btnUpdateTeacher.FlatAppearance.BorderSize = 0;
            btnUpdateTeacher.FlatStyle = FlatStyle.Flat;
            btnUpdateTeacher.Font = new Font("Ebrima", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnUpdateTeacher.ForeColor = Color.White;
            btnUpdateTeacher.Location = new Point(854, 144);
            btnUpdateTeacher.Name = "btnUpdateTeacher";
            btnUpdateTeacher.Size = new Size(121, 25);
            btnUpdateTeacher.TabIndex = 22;
            btnUpdateTeacher.Text = "Update";
            btnUpdateTeacher.UseVisualStyleBackColor = false;
            btnUpdateTeacher.Click += btnUpdateTeacher_Click;
            // 
            // btnDeleteTeacher
            // 
            btnDeleteTeacher.BackColor = Color.Maroon;
            btnDeleteTeacher.FlatAppearance.BorderSize = 0;
            btnDeleteTeacher.FlatStyle = FlatStyle.Flat;
            btnDeleteTeacher.Font = new Font("Ebrima", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDeleteTeacher.ForeColor = Color.White;
            btnDeleteTeacher.Location = new Point(983, 143);
            btnDeleteTeacher.Name = "btnDeleteTeacher";
            btnDeleteTeacher.Size = new Size(137, 25);
            btnDeleteTeacher.TabIndex = 21;
            btnDeleteTeacher.Text = "Delete Selected";
            btnDeleteTeacher.UseVisualStyleBackColor = false;
            btnDeleteTeacher.Click += btnDeleteTeacher_Click;
            // 
            // btnCancelTeacher
            // 
            btnCancelTeacher.BackColor = Color.Maroon;
            btnCancelTeacher.FlatAppearance.BorderSize = 0;
            btnCancelTeacher.FlatStyle = FlatStyle.Flat;
            btnCancelTeacher.Font = new Font("Ebrima", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancelTeacher.ForeColor = Color.White;
            btnCancelTeacher.Location = new Point(158, 404);
            btnCancelTeacher.Name = "btnCancelTeacher";
            btnCancelTeacher.Size = new Size(309, 25);
            btnCancelTeacher.TabIndex = 20;
            btnCancelTeacher.Text = "Cancel";
            btnCancelTeacher.UseVisualStyleBackColor = false;
            btnCancelTeacher.Click += btnCancelTeacher_Click;
            // 
            // btnAddTeacher
            // 
            btnAddTeacher.BackColor = Color.FromArgb(64, 0, 0);
            btnAddTeacher.FlatAppearance.BorderSize = 0;
            btnAddTeacher.FlatStyle = FlatStyle.Flat;
            btnAddTeacher.Font = new Font("Ebrima", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAddTeacher.ForeColor = Color.White;
            btnAddTeacher.Location = new Point(158, 367);
            btnAddTeacher.Name = "btnAddTeacher";
            btnAddTeacher.Size = new Size(309, 25);
            btnAddTeacher.TabIndex = 19;
            btnAddTeacher.Text = "Add Teacher";
            btnAddTeacher.UseVisualStyleBackColor = false;
            btnAddTeacher.Click += btnAddTeacher_Click;
            // 
            // txtTeacherSubjects
            // 
            txtTeacherSubjects.Font = new Font("Ebrima", 9.75F);
            txtTeacherSubjects.Location = new Point(159, 310);
            txtTeacherSubjects.Name = "txtTeacherSubjects";
            txtTeacherSubjects.PlaceholderText = "   Separate using comma(s)";
            txtTeacherSubjects.Size = new Size(308, 25);
            txtTeacherSubjects.TabIndex = 18;
            // 
            // txtTeacherName
            // 
            txtTeacherName.Font = new Font("Ebrima", 9.75F);
            txtTeacherName.Location = new Point(158, 226);
            txtTeacherName.Name = "txtTeacherName";
            txtTeacherName.Size = new Size(309, 25);
            txtTeacherName.TabIndex = 17;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.BackColor = Color.Transparent;
            label17.Font = new Font("Ebrima", 9.75F);
            label17.Location = new Point(160, 199);
            label17.Name = "label17";
            label17.Size = new Size(43, 17);
            label17.TabIndex = 15;
            label17.Text = "Name";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.BackColor = Color.Transparent;
            label16.Font = new Font("Ebrima", 9.75F);
            label16.Location = new Point(158, 279);
            label16.Name = "label16";
            label16.Size = new Size(95, 17);
            label16.TabIndex = 16;
            label16.Text = "Subject Course";
            // 
            // pnlManageDataNav
            // 
            pnlManageDataNav.BackColor = Color.FromArgb(245, 243, 244);
            pnlManageDataNav.Controls.Add(btnSubNavSections);
            pnlManageDataNav.Controls.Add(btnSubNavTeachers);
            pnlManageDataNav.Controls.Add(btnSubNavRooms);
            pnlManageDataNav.Dock = DockStyle.Top;
            pnlManageDataNav.Location = new Point(0, 0);
            pnlManageDataNav.Name = "pnlManageDataNav";
            pnlManageDataNav.Size = new Size(1280, 56);
            pnlManageDataNav.TabIndex = 2;
            // 
            // btnSubNavSections
            // 
            btnSubNavSections.BackColor = Color.Transparent;
            btnSubNavSections.FlatAppearance.BorderSize = 0;
            btnSubNavSections.FlatStyle = FlatStyle.Flat;
            btnSubNavSections.Font = new Font("Ebrima", 12F);
            btnSubNavSections.ForeColor = Color.Maroon;
            btnSubNavSections.Location = new Point(560, 13);
            btnSubNavSections.Name = "btnSubNavSections";
            btnSubNavSections.Size = new Size(172, 29);
            btnSubNavSections.TabIndex = 2;
            btnSubNavSections.Text = "Sections and Subjects";
            btnSubNavSections.UseVisualStyleBackColor = false;
            btnSubNavSections.Click += btnSubNavSections_Click;
            // 
            // btnSubNavTeachers
            // 
            btnSubNavTeachers.BackColor = Color.Transparent;
            btnSubNavTeachers.FlatAppearance.BorderSize = 0;
            btnSubNavTeachers.FlatStyle = FlatStyle.Flat;
            btnSubNavTeachers.Font = new Font("Ebrima", 12F);
            btnSubNavTeachers.ForeColor = Color.Maroon;
            btnSubNavTeachers.Location = new Point(766, 14);
            btnSubNavTeachers.Name = "btnSubNavTeachers";
            btnSubNavTeachers.Size = new Size(106, 26);
            btnSubNavTeachers.TabIndex = 1;
            btnSubNavTeachers.Text = "Teachers";
            btnSubNavTeachers.UseVisualStyleBackColor = false;
            btnSubNavTeachers.Click += btnSubNavTeachers_Click;
            // 
            // btnSubNavRooms
            // 
            btnSubNavRooms.BackColor = Color.Transparent;
            btnSubNavRooms.FlatAppearance.BorderSize = 0;
            btnSubNavRooms.FlatStyle = FlatStyle.Flat;
            btnSubNavRooms.Font = new Font("Ebrima", 12F);
            btnSubNavRooms.ForeColor = Color.Maroon;
            btnSubNavRooms.Location = new Point(448, 13);
            btnSubNavRooms.Name = "btnSubNavRooms";
            btnSubNavRooms.Size = new Size(78, 29);
            btnSubNavRooms.TabIndex = 0;
            btnSubNavRooms.Text = "Room";
            btnSubNavRooms.UseVisualStyleBackColor = false;
            btnSubNavRooms.Click += btnSubNavRooms_Click;
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
            btnFindSlots.Location = new Point(84, 29);
            btnFindSlots.Name = "btnFindSlots";
            btnFindSlots.Size = new Size(1098, 32);
            btnFindSlots.TabIndex = 3;
            btnFindSlots.Text = "Find Valid Slots";
            btnFindSlots.UseVisualStyleBackColor = false;
            btnFindSlots.Click += btnFindSlots_Click;
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
            btnNavPending.Font = new Font("Ebrima", 12F);
            btnNavPending.ForeColor = Color.White;
            btnNavPending.Location = new Point(654, 19);
            btnNavPending.Name = "btnNavPending";
            btnNavPending.Size = new Size(113, 29);
            btnNavPending.TabIndex = 3;
            btnNavPending.Text = "Pending";
            btnNavPending.UseVisualStyleBackColor = true;
            btnNavPending.Click += btnNavPending_Click;
            // 
            // btnNavManage
            // 
            btnNavManage.FlatAppearance.BorderSize = 0;
            btnNavManage.FlatStyle = FlatStyle.Flat;
            btnNavManage.Font = new Font("Ebrima", 12F);
            btnNavManage.ForeColor = Color.White;
            btnNavManage.Location = new Point(455, 19);
            btnNavManage.Name = "btnNavManage";
            btnNavManage.Size = new Size(113, 29);
            btnNavManage.TabIndex = 2;
            btnNavManage.Text = "Manage Data";
            btnNavManage.UseVisualStyleBackColor = true;
            btnNavManage.Click += btnNavManage_Click;
            // 
            // btnNavMaster
            // 
            btnNavMaster.FlatAppearance.BorderSize = 0;
            btnNavMaster.FlatStyle = FlatStyle.Flat;
            btnNavMaster.Font = new Font("Ebrima", 12F);
            btnNavMaster.ForeColor = Color.White;
            btnNavMaster.Location = new Point(262, 19);
            btnNavMaster.Name = "btnNavMaster";
            btnNavMaster.Size = new Size(113, 29);
            btnNavMaster.TabIndex = 1;
            btnNavMaster.Text = "Master List";
            btnNavMaster.UseVisualStyleBackColor = true;
            btnNavMaster.Click += btnNavMaster_Click;
            // 
            // btnNavSchedule
            // 
            btnNavSchedule.FlatAppearance.BorderSize = 0;
            btnNavSchedule.FlatStyle = FlatStyle.Flat;
            btnNavSchedule.Font = new Font("Ebrima", 12F);
            btnNavSchedule.ForeColor = Color.White;
            btnNavSchedule.Location = new Point(84, 19);
            btnNavSchedule.Name = "btnNavSchedule";
            btnNavSchedule.Size = new Size(113, 29);
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
            pnlViewSchedule.ResumeLayout(false);
            pnlViewSchedule.PerformLayout();
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvTimetable).EndInit();
            pnlViewMaster.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMaster).EndInit();
            pnlViewManage.ResumeLayout(false);
            pnlViewRooms.ResumeLayout(false);
            pnlViewRooms.PerformLayout();
            pnlViewSections.ResumeLayout(false);
            pnlViewSections.PerformLayout();
            pnlViewTeachers.ResumeLayout(false);
            pnlViewTeachers.PerformLayout();
            pnlManageDataNav.ResumeLayout(false);
            pnlViewPending.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvPending).EndInit();
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
        private Button btnLogout;
        private ComboBox cmbFilterType;
        private Button btnGenerate;
        private DataGridView dgvTimetable;
        private ComboBox cmbScheduleView;
        private TabPage tabRooms;
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
        private Panel pnlManageDataNav;
        private Button btnSubNavRooms;
        private Button btnSubNavTeachers;
        private Button btnSubNavSections;
        private Panel pnlViewRooms;
        private Panel pnlViewTeachers;
        private Button btnDeleteRoom;
        private Button btnUpdateRoom;
        private Label label2;
        private ListBox lstRooms;
        private Button btnCancelRoom;
        private Button btnAddRoom;
        private TextBox txtRoomName;
        private ComboBox cmbRoomType;
        private Button btnUpdateTeacher;
        private Button btnDeleteTeacher;
        private Button btnCancelTeacher;
        private ListBox lstTeachers;
        private Button btnAddTeacher;
        private TextBox txtTeacherSubjects;
        private TextBox txtTeacherName;
        private Label label17;
        private Label label16;
        private Panel pnlViewSections;
        private Button btnBatchAdd;
        private ComboBox cmbBatchYear;
        private Label label10;
        private ComboBox cmbBatchProgram;
        private Label label9;
        private Button btnRemoveSubject;
        private ListBox lstSectionSubjects;
        private Button btnDeleteSection;
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
        private ListBox lstSections;
        private Label label4;
        private Label label3;
        private Label label5;
        private Label label11;
        private Label label1;
        private Label label12;
        private Label label18;
        private Label label19;
        private Panel panel1;
        private Button btnExportPdf;
        private Button btnBackupDatabase;
        private Button btnRestoreDatabase;
    }
}
