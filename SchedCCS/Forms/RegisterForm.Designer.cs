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
            btnRegister = new Button();
            btnClose = new Button();
            lnkBackToLogin = new LinkLabel();
            txtSection = new TextBox();
            chkShowRegPass = new CheckBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Ebrima", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(547, 160);
            label1.Name = "label1";
            label1.Size = new Size(203, 21);
            label1.TabIndex = 0;
            label1.Text = "Create your User Account";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Ebrima", 9.75F);
            label2.Location = new Point(478, 229);
            label2.Name = "label2";
            label2.Size = new Size(71, 17);
            label2.TabIndex = 1;
            label2.Text = "First Name";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Ebrima", 9.75F);
            label3.Location = new Point(649, 229);
            label3.Name = "label3";
            label3.Size = new Size(70, 17);
            label3.TabIndex = 2;
            label3.Text = "Last Name";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Ebrima", 9.75F);
            label4.Location = new Point(478, 291);
            label4.Name = "label4";
            label4.Size = new Size(68, 17);
            label4.TabIndex = 3;
            label4.Text = "Student ID";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Ebrima", 9.75F);
            label5.Location = new Point(478, 354);
            label5.Name = "label5";
            label5.Size = new Size(50, 17);
            label5.TabIndex = 4;
            label5.Text = "Section";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.Transparent;
            label6.Font = new Font("Ebrima", 9.75F);
            label6.Location = new Point(478, 416);
            label6.Name = "label6";
            label6.Size = new Size(64, 17);
            label6.TabIndex = 5;
            label6.Text = "Password";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.Transparent;
            label7.Font = new Font("Ebrima", 9.75F);
            label7.Location = new Point(478, 478);
            label7.Name = "label7";
            label7.Size = new Size(114, 17);
            label7.TabIndex = 6;
            label7.Text = "Confirm Password";
            // 
            // txtFirstName
            // 
            txtFirstName.Font = new Font("Ebrima", 9F);
            txtFirstName.Location = new Point(478, 250);
            txtFirstName.Name = "txtFirstName";
            txtFirstName.Size = new Size(153, 24);
            txtFirstName.TabIndex = 1;
            // 
            // txtLastName
            // 
            txtLastName.Font = new Font("Ebrima", 9F);
            txtLastName.Location = new Point(649, 250);
            txtLastName.Name = "txtLastName";
            txtLastName.Size = new Size(158, 24);
            txtLastName.TabIndex = 2;
            // 
            // txtStudentID
            // 
            txtStudentID.Font = new Font("Ebrima", 9F);
            txtStudentID.Location = new Point(478, 311);
            txtStudentID.Name = "txtStudentID";
            txtStudentID.Size = new Size(329, 24);
            txtStudentID.TabIndex = 3;
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Ebrima", 9F);
            txtPassword.Location = new Point(478, 436);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(329, 24);
            txtPassword.TabIndex = 5;
            // 
            // txtConfirm
            // 
            txtConfirm.Font = new Font("Ebrima", 9F);
            txtConfirm.Location = new Point(478, 498);
            txtConfirm.Name = "txtConfirm";
            txtConfirm.PasswordChar = '*';
            txtConfirm.Size = new Size(329, 24);
            txtConfirm.TabIndex = 6;
            // 
            // btnRegister
            // 
            btnRegister.BackColor = Color.FromArgb(64, 0, 0);
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.Font = new Font("Ebrima", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRegister.ForeColor = Color.White;
            btnRegister.Location = new Point(478, 582);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(329, 34);
            btnRegister.TabIndex = 13;
            btnRegister.Text = "CREATE ACCOUNT";
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
            btnClose.MouseEnter += btnClose_MouseEnter;
            btnClose.MouseLeave += btnClose_MouseLeave;
            // 
            // lnkBackToLogin
            // 
            lnkBackToLogin.ActiveLinkColor = Color.FromArgb(198, 168, 86);
            lnkBackToLogin.AutoSize = true;
            lnkBackToLogin.BackColor = Color.Transparent;
            lnkBackToLogin.Font = new Font("Ebrima", 9.75F);
            lnkBackToLogin.LinkArea = new LinkArea(25, 31);
            lnkBackToLogin.LinkBehavior = LinkBehavior.HoverUnderline;
            lnkBackToLogin.LinkColor = Color.FromArgb(0, 0, 64);
            lnkBackToLogin.Location = new Point(547, 628);
            lnkBackToLogin.Name = "lnkBackToLogin";
            lnkBackToLogin.Size = new Size(196, 23);
            lnkBackToLogin.TabIndex = 16;
            lnkBackToLogin.TabStop = true;
            lnkBackToLogin.Text = "Already have an account? Log in";
            lnkBackToLogin.UseCompatibleTextRendering = true;
            lnkBackToLogin.LinkClicked += lnkBackToLogin_LinkClicked;
            // 
            // txtSection
            // 
            txtSection.Location = new Point(478, 374);
            txtSection.Name = "txtSection";
            txtSection.Size = new Size(329, 23);
            txtSection.TabIndex = 17;
            // 
            // chkShowRegPass
            // 
            chkShowRegPass.AutoSize = true;
            chkShowRegPass.BackColor = Color.Transparent;
            chkShowRegPass.Font = new Font("Ebrima", 9.75F);
            chkShowRegPass.Location = new Point(478, 543);
            chkShowRegPass.Name = "chkShowRegPass";
            chkShowRegPass.Size = new Size(118, 21);
            chkShowRegPass.TabIndex = 18;
            chkShowRegPass.Text = "Show Password";
            chkShowRegPass.UseVisualStyleBackColor = false;
            chkShowRegPass.CheckedChanged += chkShowRegPass_CheckedChanged;
            // 
            // RegisterForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(1280, 720);
            Controls.Add(chkShowRegPass);
            Controls.Add(txtSection);
            Controls.Add(lnkBackToLogin);
            Controls.Add(btnClose);
            Controls.Add(btnRegister);
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
        private Button btnRegister;
        private Button btnClose;
        private LinkLabel lnkBackToLogin;
        private TextBox txtSection;
        private CheckBox chkShowRegPass;
    }
}