namespace SchedCCS
{
    partial class RoomScheduleForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RoomScheduleForm));
            dgvRoomSchedule = new DataGridView();
            btnClose = new Button();
            lblSemesterYear = new Label();
            lblRoomName = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvRoomSchedule).BeginInit();
            SuspendLayout();
            // 
            // dgvRoomSchedule
            // 
            dgvRoomSchedule.AllowUserToAddRows = false;
            dgvRoomSchedule.AllowUserToDeleteRows = false;
            dgvRoomSchedule.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvRoomSchedule.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRoomSchedule.Location = new Point(12, 177);
            dgvRoomSchedule.Name = "dgvRoomSchedule";
            dgvRoomSchedule.ReadOnly = true;
            dgvRoomSchedule.Size = new Size(1256, 531);
            dgvRoomSchedule.TabIndex = 1;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.BackColor = Color.Transparent;
            btnClose.Cursor = Cursors.Hand;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.ForeColor = Color.Black;
            btnClose.Location = new Point(1233, 12);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(35, 30);
            btnClose.TabIndex = 2;
            btnClose.Text = "✕";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // lblSemesterYear
            // 
            lblSemesterYear.AutoSize = true;
            lblSemesterYear.BackColor = Color.Transparent;
            lblSemesterYear.Font = new Font("Calibri", 9F);
            lblSemesterYear.Location = new Point(562, 100);
            lblSemesterYear.Name = "lblSemesterYear";
            lblSemesterYear.Size = new Size(154, 14);
            lblSemesterYear.TabIndex = 3;
            lblSemesterYear.Text = "1st Semester, A.Y. 2025-2026";
            // 
            // lblRoomName
            // 
            lblRoomName.AutoSize = true;
            lblRoomName.BackColor = Color.Transparent;
            lblRoomName.Font = new Font("Calibri", 9F, FontStyle.Bold);
            lblRoomName.ForeColor = Color.Black;
            lblRoomName.Location = new Point(12, 160);
            lblRoomName.Name = "lblRoomName";
            lblRoomName.Size = new Size(110, 14);
            lblRoomName.TabIndex = 5;
            lblRoomName.Text = "Schedule for LEC-101";
            // 
            // RoomScheduleForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(1280, 720);
            Controls.Add(lblRoomName);
            Controls.Add(lblSemesterYear);
            Controls.Add(btnClose);
            Controls.Add(dgvRoomSchedule);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Name = "RoomScheduleForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Room Schedule Form";
            ((System.ComponentModel.ISupportInitialize)dgvRoomSchedule).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private DataGridView dgvRoomSchedule;
        private Button btnClose;
        private Label lblSemesterYear;
        private Label lblRoomName;
    }
}