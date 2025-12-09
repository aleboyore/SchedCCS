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
            lblRoomName = new Label();
            dgvRoomSchedule = new DataGridView();
            btnClose = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvRoomSchedule).BeginInit();
            SuspendLayout();
            // 
            // lblRoomName
            // 
            lblRoomName.AutoSize = true;
            lblRoomName.Location = new Point(12, 16);
            lblRoomName.Name = "lblRoomName";
            lblRoomName.Size = new Size(119, 15);
            lblRoomName.TabIndex = 0;
            lblRoomName.Text = "Schedule for LEC-101";
            // 
            // dgvRoomSchedule
            // 
            dgvRoomSchedule.AllowUserToAddRows = false;
            dgvRoomSchedule.AllowUserToDeleteRows = false;
            dgvRoomSchedule.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvRoomSchedule.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRoomSchedule.Location = new Point(12, 52);
            dgvRoomSchedule.Name = "dgvRoomSchedule";
            dgvRoomSchedule.ReadOnly = true;
            dgvRoomSchedule.Size = new Size(1256, 656);
            dgvRoomSchedule.TabIndex = 1;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(1177, 12);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(75, 23);
            btnClose.TabIndex = 2;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // RoomScheduleForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 720);
            Controls.Add(btnClose);
            Controls.Add(dgvRoomSchedule);
            Controls.Add(lblRoomName);
            FormBorderStyle = FormBorderStyle.None;
            Name = "RoomScheduleForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Room Schedule Form";
            ((System.ComponentModel.ISupportInitialize)dgvRoomSchedule).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblRoomName;
        private DataGridView dgvRoomSchedule;
        private Button btnClose;
    }
}