namespace SchedCCS
{
    partial class RegisterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegisterForm));
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            txtFirstName = new TextBox();
            txtLastName = new TextBox();
            txtStudentID = new TextBox();
            txtPassword = new TextBox();
            txtConfirm = new TextBox();
            cmbSection = new ComboBox();
            btnRegister = new Button();
            btnClose = new Button();
            lnkBackToLogin = new LinkLabel();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            label1.Location = new Point(682, 187);
            label1.Name = "label1";
            label1.Size = new Size(240, 30);
            label1.TabIndex = 0;
            label1.Text = "CREATE AN ACCOUNT";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Location = new Point(638, 239);
            label2.Name = "label2";
            label2.Size = new Size(64, 15);
            label2.TabIndex = 1;
            label2.Text = "First Name";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Location = new Point(809, 239);
            label3.Name = "label3";
            label3.Size = new Size(63, 15);
            label3.TabIndex = 2;
            label3.Text = "Last Name";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Location = new Point(638, 289);
            label4.Name = "label4";
            label4.Size = new Size(62, 15);
            label4.TabIndex = 3;
            label4.Text = "Student ID";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Location = new Point(638, 339);
            label5.Name = "label5";
            label5.Size = new Size(46, 15);
            label5.TabIndex = 4;
            label5.Text = "Section";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.Transparent;
            label6.Location = new Point(638, 389);
            label6.Name = "label6";
            label6.Size = new Size(57, 15);
            label6.TabIndex = 5;
            label6.Text = "Password";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.Transparent;
            label7.Location = new Point(638, 439);
            label7.Name = "label7";
            label7.Size = new Size(104, 15);
            label7.TabIndex = 6;
            label7.Text = "Confirm Password";
            // 
            // txtFirstName
            // 
            txtFirstName.Location = new Point(638, 260);
            txtFirstName.Name = "txtFirstName";
            txtFirstName.Size = new Size(153, 23);
            txtFirstName.TabIndex = 7;
            // 
            // txtLastName
            // 
            txtLastName.Location = new Point(809, 259);
            txtLastName.Name = "txtLastName";
            txtLastName.Size = new Size(158, 23);
            txtLastName.TabIndex = 8;
            // 
            // txtStudentID
            // 
            txtStudentID.Location = new Point(638, 310);
            txtStudentID.Name = "txtStudentID";
            txtStudentID.Size = new Size(329, 23);
            txtStudentID.TabIndex = 9;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(638, 410);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(329, 23);
            txtPassword.TabIndex = 10;
            // 
            // txtConfirm
            // 
            txtConfirm.Location = new Point(638, 460);
            txtConfirm.Name = "txtConfirm";
            txtConfirm.PasswordChar = '*';
            txtConfirm.Size = new Size(329, 23);
            txtConfirm.TabIndex = 11;
            // 
            // cmbSection
            // 
            cmbSection.FormattingEnabled = true;
            cmbSection.Location = new Point(638, 360);
            cmbSection.Name = "cmbSection";
            cmbSection.Size = new Size(329, 23);
            cmbSection.TabIndex = 12;
            // 
            // btnRegister
            // 
            btnRegister.BackColor = Color.FromArgb(12, 35, 64);
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.ForeColor = Color.White;
            btnRegister.Location = new Point(638, 512);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(329, 34);
            btnRegister.TabIndex = 13;
            btnRegister.Text = "Sign Up";
            btnRegister.UseVisualStyleBackColor = false;
            btnRegister.Click += btnRegister_Click;
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
            btnClose.TabIndex = 15;
            btnClose.Text = "✕";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // lnkBackToLogin
            // 
            lnkBackToLogin.ActiveLinkColor = Color.FromArgb(198, 168, 86);
            lnkBackToLogin.AutoSize = true;
            lnkBackToLogin.BackColor = Color.Transparent;
            lnkBackToLogin.LinkArea = new LinkArea(25, 31);
            lnkBackToLogin.LinkBehavior = LinkBehavior.HoverUnderline;
            lnkBackToLogin.LinkColor = Color.FromArgb(0, 0, 64);
            lnkBackToLogin.Location = new Point(712, 575);
            lnkBackToLogin.Name = "lnkBackToLogin";
            lnkBackToLogin.Size = new Size(181, 21);
            lnkBackToLogin.TabIndex = 16;
            lnkBackToLogin.TabStop = true;
            lnkBackToLogin.Text = "Already have an account? Log in";
            lnkBackToLogin.UseCompatibleTextRendering = true;
            lnkBackToLogin.LinkClicked += lnkBackToLogin_LinkClicked;
            // 
            // RegisterForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(1280, 720);
            Controls.Add(lnkBackToLogin);
            Controls.Add(btnClose);
            Controls.Add(btnRegister);
            Controls.Add(cmbSection);
            Controls.Add(txtConfirm);
            Controls.Add(txtPassword);
            Controls.Add(txtStudentID);
            Controls.Add(txtLastName);
            Controls.Add(txtFirstName);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Name = "RegisterForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Register Form";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private TextBox txtFirstName;
        private TextBox txtLastName;
        private TextBox txtStudentID;
        private TextBox txtPassword;
        private TextBox txtConfirm;
        private ComboBox cmbSection;
        private Button btnRegister;
        private Button btnClose;
        private LinkLabel lnkBackToLogin;
    }
}