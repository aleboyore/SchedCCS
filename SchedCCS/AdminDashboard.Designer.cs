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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            btnLogout = new Button();
            cmbFilterType = new ComboBox();
            button1 = new Button();
            dataGridView1 = new DataGridView();
            btnExport = new Button();
            comboBox1 = new ComboBox();
            tabMaster = new TabPage();
            dgvMaster = new DataGridView();
            tabPage2 = new TabPage();
            tabControl2 = new TabControl();
            tabPage3 = new TabPage();
            groupBox4 = new GroupBox();
            btnDeleteTeacher = new Button();
            lstTeachers = new ListBox();
            groupBox2 = new GroupBox();
            btnCancelTeacher = new Button();
            btnUpdateTeacher = new Button();
            btnAddTeacher = new Button();
            txtTeacherSubjects = new TextBox();
            txtTeacherName = new TextBox();
            label4 = new Label();
            label3 = new Label();
            tabPage4 = new TabPage();
            groupBox5 = new GroupBox();
            lstRooms = new ListBox();
            btnDeleteRoom = new Button();
            groupBox1 = new GroupBox();
            btnCancelRoom = new Button();
            btnUpdateRoom = new Button();
            btnAddRoom = new Button();
            label1 = new Label();
            cmbRoomType = new ComboBox();
            label2 = new Label();
            txtRoomName = new TextBox();
            tabPage5 = new TabPage();
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
            tabPending = new TabPage();
            btnForceAssign = new Button();
            cmbManualTeacher = new ComboBox();
            label15 = new Label();
            dgvPending = new DataGridView();
            Section = new DataGridViewTextBoxColumn();
            Subject = new DataGridViewTextBoxColumn();
            Reason = new DataGridViewTextBoxColumn();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            tabMaster.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMaster).BeginInit();
            tabPage2.SuspendLayout();
            tabControl2.SuspendLayout();
            tabPage3.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox2.SuspendLayout();
            tabPage4.SuspendLayout();
            groupBox5.SuspendLayout();
            groupBox1.SuspendLayout();
            tabPage5.SuspendLayout();
            groupBox8.SuspendLayout();
            groupBox6.SuspendLayout();
            groupBox7.SuspendLayout();
            groupBox3.SuspendLayout();
            tabPending.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPending).BeginInit();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabMaster);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPending);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1264, 681);
            tabControl1.TabIndex = 4;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(btnLogout);
            tabPage1.Controls.Add(cmbFilterType);
            tabPage1.Controls.Add(button1);
            tabPage1.Controls.Add(dataGridView1);
            tabPage1.Controls.Add(btnExport);
            tabPage1.Controls.Add(comboBox1);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1256, 653);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Schedule View";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnLogout
            // 
            btnLogout.Location = new Point(6, 7);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(75, 23);
            btnLogout.TabIndex = 10;
            btnLogout.Text = "Log Out";
            btnLogout.UseVisualStyleBackColor = true;
            btnLogout.Click += btnLogout_Click;
            // 
            // cmbFilterType
            // 
            cmbFilterType.FormattingEnabled = true;
            cmbFilterType.Items.AddRange(new object[] { "Section", "Teacher", "Room" });
            cmbFilterType.Location = new Point(889, 7);
            cmbFilterType.Name = "cmbFilterType";
            cmbFilterType.Size = new Size(121, 23);
            cmbFilterType.TabIndex = 9;
            cmbFilterType.Text = "Section";
            cmbFilterType.SelectedIndexChanged += cmbFilterType_SelectedIndexChanged;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            button1.Location = new Point(425, 618);
            button1.Name = "button1";
            button1.Size = new Size(407, 23);
            button1.TabIndex = 4;
            button1.Text = "Generate Schedule";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.Location = new Point(6, 36);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(1244, 571);
            dataGridView1.TabIndex = 8;
            dataGridView1.Resize += dataGridView1_Resize;
            // 
            // btnExport
            // 
            btnExport.Location = new Point(1135, 7);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(113, 23);
            btnExport.TabIndex = 7;
            btnExport.Text = "Export to CSV";
            btnExport.UseVisualStyleBackColor = true;
            btnExport.Click += btnExport_Click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(1016, 7);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(113, 23);
            comboBox1.TabIndex = 6;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // tabMaster
            // 
            tabMaster.Controls.Add(dgvMaster);
            tabMaster.Location = new Point(4, 24);
            tabMaster.Name = "tabMaster";
            tabMaster.Size = new Size(1256, 653);
            tabMaster.TabIndex = 2;
            tabMaster.Text = "Master Schedule";
            tabMaster.UseVisualStyleBackColor = true;
            // 
            // dgvMaster
            // 
            dgvMaster.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvMaster.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMaster.Location = new Point(8, 10);
            dgvMaster.Name = "dgvMaster";
            dgvMaster.ReadOnly = true;
            dgvMaster.Size = new Size(1240, 635);
            dgvMaster.TabIndex = 0;
            dgvMaster.ColumnHeaderMouseClick += dgvMaster_ColumnHeaderMouseClick;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(tabControl2);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1256, 653);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Manage Data";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            tabControl2.Alignment = TabAlignment.Left;
            tabControl2.Controls.Add(tabPage3);
            tabControl2.Controls.Add(tabPage4);
            tabControl2.Controls.Add(tabPage5);
            tabControl2.Dock = DockStyle.Fill;
            tabControl2.Location = new Point(3, 3);
            tabControl2.Multiline = true;
            tabControl2.Name = "tabControl2";
            tabControl2.SelectedIndex = 0;
            tabControl2.Size = new Size(1250, 647);
            tabControl2.TabIndex = 0;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(groupBox4);
            tabPage3.Controls.Add(groupBox2);
            tabPage3.Location = new Point(27, 4);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(1219, 639);
            tabPage3.TabIndex = 0;
            tabPage3.Text = "Manage Teacher";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(btnDeleteTeacher);
            groupBox4.Controls.Add(lstTeachers);
            groupBox4.Location = new Point(386, 221);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(447, 352);
            groupBox4.TabIndex = 11;
            groupBox4.TabStop = false;
            groupBox4.Text = "Existing Teachers";
            // 
            // btnDeleteTeacher
            // 
            btnDeleteTeacher.Location = new Point(175, 317);
            btnDeleteTeacher.Name = "btnDeleteTeacher";
            btnDeleteTeacher.Size = new Size(96, 23);
            btnDeleteTeacher.TabIndex = 2;
            btnDeleteTeacher.Text = "Delete Selected";
            btnDeleteTeacher.UseVisualStyleBackColor = true;
            // 
            // lstTeachers
            // 
            lstTeachers.FormattingEnabled = true;
            lstTeachers.ItemHeight = 15;
            lstTeachers.Location = new Point(6, 22);
            lstTeachers.Name = "lstTeachers";
            lstTeachers.Size = new Size(435, 289);
            lstTeachers.TabIndex = 0;
            lstTeachers.SelectedIndexChanged += lstTeachers_SelectedIndexChanged;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btnCancelTeacher);
            groupBox2.Controls.Add(btnUpdateTeacher);
            groupBox2.Controls.Add(btnAddTeacher);
            groupBox2.Controls.Add(txtTeacherSubjects);
            groupBox2.Controls.Add(txtTeacherName);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(label3);
            groupBox2.Location = new Point(386, 66);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(447, 149);
            groupBox2.TabIndex = 9;
            groupBox2.TabStop = false;
            groupBox2.Text = "Add New Teacher";
            // 
            // btnCancelTeacher
            // 
            btnCancelTeacher.Location = new Point(81, 118);
            btnCancelTeacher.Name = "btnCancelTeacher";
            btnCancelTeacher.Size = new Size(75, 23);
            btnCancelTeacher.TabIndex = 6;
            btnCancelTeacher.Text = "Cancel";
            btnCancelTeacher.UseVisualStyleBackColor = true;
            btnCancelTeacher.Click += btnCancelTeacher_Click;
            // 
            // btnUpdateTeacher
            // 
            btnUpdateTeacher.Enabled = false;
            btnUpdateTeacher.Location = new Point(269, 118);
            btnUpdateTeacher.Name = "btnUpdateTeacher";
            btnUpdateTeacher.Size = new Size(101, 23);
            btnUpdateTeacher.TabIndex = 5;
            btnUpdateTeacher.Text = "Update Teacher";
            btnUpdateTeacher.UseVisualStyleBackColor = true;
            btnUpdateTeacher.Click += btnUpdateTeacher_Click;
            // 
            // btnAddTeacher
            // 
            btnAddTeacher.Location = new Point(162, 118);
            btnAddTeacher.Name = "btnAddTeacher";
            btnAddTeacher.Size = new Size(101, 23);
            btnAddTeacher.TabIndex = 4;
            btnAddTeacher.Text = "Add Teacher";
            btnAddTeacher.UseVisualStyleBackColor = true;
            btnAddTeacher.Click += btnAddTeacher_Click;
            // 
            // txtTeacherSubjects
            // 
            txtTeacherSubjects.Location = new Point(6, 89);
            txtTeacherSubjects.Name = "txtTeacherSubjects";
            txtTeacherSubjects.Size = new Size(435, 23);
            txtTeacherSubjects.TabIndex = 3;
            // 
            // txtTeacherName
            // 
            txtTeacherName.Location = new Point(6, 39);
            txtTeacherName.Name = "txtTeacherName";
            txtTeacherName.Size = new Size(435, 23);
            txtTeacherName.TabIndex = 2;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 71);
            label4.Name = "label4";
            label4.Size = new Size(160, 15);
            label4.TabIndex = 1;
            label4.Text = "Subjects (comma separated):";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 21);
            label3.Name = "label3";
            label3.Size = new Size(39, 15);
            label3.TabIndex = 0;
            label3.Text = "Name";
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(groupBox5);
            tabPage4.Controls.Add(groupBox1);
            tabPage4.Location = new Point(27, 4);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(1219, 639);
            tabPage4.TabIndex = 1;
            tabPage4.Text = "Manage Rooms";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(lstRooms);
            groupBox5.Controls.Add(btnDeleteRoom);
            groupBox5.Location = new Point(625, 116);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(361, 406);
            groupBox5.TabIndex = 13;
            groupBox5.TabStop = false;
            groupBox5.Text = "Existing Rooms";
            // 
            // lstRooms
            // 
            lstRooms.FormattingEnabled = true;
            lstRooms.ItemHeight = 15;
            lstRooms.Location = new Point(6, 22);
            lstRooms.Name = "lstRooms";
            lstRooms.Size = new Size(349, 319);
            lstRooms.TabIndex = 1;
            lstRooms.SelectedIndexChanged += lstRooms_SelectedIndexChanged;
            // 
            // btnDeleteRoom
            // 
            btnDeleteRoom.Location = new Point(132, 376);
            btnDeleteRoom.Name = "btnDeleteRoom";
            btnDeleteRoom.Size = new Size(96, 23);
            btnDeleteRoom.TabIndex = 0;
            btnDeleteRoom.Text = "Delete Selected";
            btnDeleteRoom.UseVisualStyleBackColor = true;
            btnDeleteRoom.Click += btnDeleteRoom_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnCancelRoom);
            groupBox1.Controls.Add(btnUpdateRoom);
            groupBox1.Controls.Add(btnAddRoom);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(cmbRoomType);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(txtRoomName);
            groupBox1.Location = new Point(232, 116);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(382, 406);
            groupBox1.TabIndex = 9;
            groupBox1.TabStop = false;
            groupBox1.Text = "Add New Room";
            // 
            // btnCancelRoom
            // 
            btnCancelRoom.Location = new Point(52, 376);
            btnCancelRoom.Name = "btnCancelRoom";
            btnCancelRoom.Size = new Size(75, 23);
            btnCancelRoom.TabIndex = 7;
            btnCancelRoom.Text = "Cancel";
            btnCancelRoom.UseVisualStyleBackColor = true;
            btnCancelRoom.Click += btnCancelRoom_Click;
            // 
            // btnUpdateRoom
            // 
            btnUpdateRoom.Enabled = false;
            btnUpdateRoom.Location = new Point(230, 376);
            btnUpdateRoom.Name = "btnUpdateRoom";
            btnUpdateRoom.Size = new Size(91, 23);
            btnUpdateRoom.TabIndex = 6;
            btnUpdateRoom.Text = "Update Room";
            btnUpdateRoom.UseVisualStyleBackColor = true;
            btnUpdateRoom.Click += btnUpdateRoom_Click;
            // 
            // btnAddRoom
            // 
            btnAddRoom.Location = new Point(133, 376);
            btnAddRoom.Name = "btnAddRoom";
            btnAddRoom.Size = new Size(91, 23);
            btnAddRoom.TabIndex = 5;
            btnAddRoom.Text = "Add Room";
            btnAddRoom.UseVisualStyleBackColor = true;
            btnAddRoom.Click += btnAddRoom_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 19);
            label1.Name = "label1";
            label1.Size = new Size(74, 15);
            label1.TabIndex = 1;
            label1.Text = "Room Name";
            // 
            // cmbRoomType
            // 
            cmbRoomType.FormattingEnabled = true;
            cmbRoomType.Items.AddRange(new object[] { "Lecture", "Laboratory" });
            cmbRoomType.Location = new Point(6, 87);
            cmbRoomType.Name = "cmbRoomType";
            cmbRoomType.Size = new Size(370, 23);
            cmbRoomType.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 69);
            label2.Name = "label2";
            label2.Size = new Size(66, 15);
            label2.TabIndex = 2;
            label2.Text = "Room Type";
            // 
            // txtRoomName
            // 
            txtRoomName.Location = new Point(6, 37);
            txtRoomName.Name = "txtRoomName";
            txtRoomName.Size = new Size(370, 23);
            txtRoomName.TabIndex = 3;
            // 
            // tabPage5
            // 
            tabPage5.Controls.Add(groupBox8);
            tabPage5.Controls.Add(groupBox6);
            tabPage5.Controls.Add(groupBox7);
            tabPage5.Controls.Add(groupBox3);
            tabPage5.Location = new Point(27, 4);
            tabPage5.Name = "tabPage5";
            tabPage5.Size = new Size(1219, 639);
            tabPage5.TabIndex = 2;
            tabPage5.Text = "Sections & Subjects";
            tabPage5.UseVisualStyleBackColor = true;
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
            btnBatchAdd.Click += btnBatchAdd_Click;
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
            btnRemoveSubject.Click += btnRemoveSubject_Click;
            // 
            // lstSectionSubjects
            // 
            lstSectionSubjects.FormattingEnabled = true;
            lstSectionSubjects.ItemHeight = 15;
            lstSectionSubjects.Location = new Point(6, 31);
            lstSectionSubjects.Name = "lstSectionSubjects";
            lstSectionSubjects.Size = new Size(346, 139);
            lstSectionSubjects.TabIndex = 0;
            lstSectionSubjects.SelectedIndexChanged += lstSectionSubjects_SelectedIndexChanged;
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
            lstSections.Size = new Size(346, 124);
            lstSections.TabIndex = 1;
            lstSections.SelectedIndexChanged += lstSections_SelectedIndexChanged;
            // 
            // btnDeleteSection
            // 
            btnDeleteSection.Location = new Point(131, 167);
            btnDeleteSection.Name = "btnDeleteSection";
            btnDeleteSection.Size = new Size(96, 23);
            btnDeleteSection.TabIndex = 0;
            btnDeleteSection.Text = "Delete Selected";
            btnDeleteSection.UseVisualStyleBackColor = true;
            btnDeleteSection.Click += btnDeleteSection_Click;
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
            btnSaveChanges.Click += btnSaveChanges_Click;
            // 
            // btnCancelSubject
            // 
            btnCancelSubject.Location = new Point(43, 401);
            btnCancelSubject.Name = "btnCancelSubject";
            btnCancelSubject.Size = new Size(75, 23);
            btnCancelSubject.TabIndex = 11;
            btnCancelSubject.Text = "Cancel";
            btnCancelSubject.UseVisualStyleBackColor = true;
            btnCancelSubject.Click += btnCancelSubject_Click;
            // 
            // btnAddSubject
            // 
            btnAddSubject.Location = new Point(205, 401);
            btnAddSubject.Name = "btnAddSubject";
            btnAddSubject.Size = new Size(145, 23);
            btnAddSubject.TabIndex = 10;
            btnAddSubject.Text = "Add Subject to Section";
            btnAddSubject.UseVisualStyleBackColor = true;
            btnAddSubject.Click += btnAddSubject_Click;
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
            cmbSectionList.SelectedIndexChanged += cmbSectionList_SelectedIndexChanged;
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
            btnCreateSection.Click += btnCreateSection_Click;
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
            // tabPending
            // 
            tabPending.Controls.Add(btnForceAssign);
            tabPending.Controls.Add(cmbManualTeacher);
            tabPending.Controls.Add(label15);
            tabPending.Controls.Add(dgvPending);
            tabPending.Location = new Point(4, 24);
            tabPending.Name = "tabPending";
            tabPending.Size = new Size(1256, 653);
            tabPending.TabIndex = 3;
            tabPending.Text = "Pending Subjects";
            tabPending.UseVisualStyleBackColor = true;
            // 
            // btnForceAssign
            // 
            btnForceAssign.Location = new Point(682, 117);
            btnForceAssign.Name = "btnForceAssign";
            btnForceAssign.Size = new Size(122, 23);
            btnForceAssign.TabIndex = 3;
            btnForceAssign.Text = "Force Assign & Retry";
            btnForceAssign.UseVisualStyleBackColor = true;
            btnForceAssign.Click += btnForceAssign_Click;
            // 
            // cmbManualTeacher
            // 
            cmbManualTeacher.FormattingEnabled = true;
            cmbManualTeacher.Location = new Point(682, 72);
            cmbManualTeacher.Name = "cmbManualTeacher";
            cmbManualTeacher.Size = new Size(121, 23);
            cmbManualTeacher.TabIndex = 2;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(654, 30);
            label15.Name = "label15";
            label15.Size = new Size(115, 15);
            label15.TabIndex = 1;
            label15.Text = "Assign New Teacher:";
            // 
            // dgvPending
            // 
            dgvPending.AllowUserToAddRows = false;
            dgvPending.AllowUserToDeleteRows = false;
            dgvPending.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPending.Columns.AddRange(new DataGridViewColumn[] { Section, Subject, Reason });
            dgvPending.Dock = DockStyle.Left;
            dgvPending.Location = new Point(0, 0);
            dgvPending.Name = "dgvPending";
            dgvPending.ReadOnly = true;
            dgvPending.Size = new Size(635, 653);
            dgvPending.TabIndex = 0;
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
            // AdminDashboard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1264, 681);
            Controls.Add(tabControl1);
            Name = "AdminDashboard";
            Text = "Admin Dashboard";
            Load += Form1_Load_1;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            tabMaster.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMaster).EndInit();
            tabPage2.ResumeLayout(false);
            tabControl2.ResumeLayout(false);
            tabPage3.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            tabPage4.ResumeLayout(false);
            groupBox5.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabPage5.ResumeLayout(false);
            groupBox8.ResumeLayout(false);
            groupBox8.PerformLayout();
            groupBox6.ResumeLayout(false);
            groupBox7.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            tabPending.ResumeLayout(false);
            tabPending.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPending).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private Button btnExport;
        private ComboBox comboBox1;
        private Button button1;
        private TabPage tabPage2;
        private DataGridView dataGridView1;
        private TabPage tabMaster;
        private DataGridView dgvMaster;
        private TabControl tabControl2;
        private TabPage tabPage3;
        private GroupBox groupBox4;
        private ListBox lstTeachers;
        private GroupBox groupBox2;
        private Button btnAddTeacher;
        private TextBox txtTeacherSubjects;
        private TextBox txtTeacherName;
        private Label label4;
        private Label label3;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private GroupBox groupBox5;
        private ListBox lstRooms;
        private Button btnDeleteRoom;
        private GroupBox groupBox1;
        private Button btnAddRoom;
        private Label label1;
        private ComboBox cmbRoomType;
        private Label label2;
        private TextBox txtRoomName;
        private GroupBox groupBox6;
        private Button btnRemoveSubject;
        private ListBox lstSectionSubjects;
        private GroupBox groupBox7;
        private ListBox lstSections;
        private Button btnDeleteSection;
        private GroupBox groupBox3;
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
        private ComboBox cmbFilterType;
        private Button btnUpdateTeacher;
        private Button btnUpdateRoom;
        private Button btnCancelTeacher;
        private Button btnCancelRoom;
        private Button btnCancelSubject;
        private Button btnSaveChanges;
        private Button btnLogout;
        private Button btnDeleteTeacher;
        private GroupBox groupBox8;
        private Label label9;
        private ComboBox cmbBatchProgram;
        private Label label10;
        private ComboBox cmbBatchYear;
        private Label label11;
        private TextBox txtBatchCode;
        private Label label12;
        private TextBox txtBatchUnits;
        private CheckBox chkBatchLab;
        private Button btnBatchAdd;
        private Label label13;
        private ComboBox cmbSectionProgram;
        private Label label14;
        private ComboBox cmbSectionYear;
        private TabPage tabPending;
        private DataGridView dgvPending;
        private DataGridViewTextBoxColumn Section;
        private DataGridViewTextBoxColumn Subject;
        private DataGridViewTextBoxColumn Reason;
        private Label label15;
        private ComboBox cmbManualTeacher;
        private Button btnForceAssign;
    }
}
