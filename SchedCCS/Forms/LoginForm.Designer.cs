namespace SchedCCS
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            label1 = new Label();
            txtStudentID = new TextBox();
            label2 = new Label();
            txtPassword = new TextBox();
            btnLogin = new Button();
            btnClose = new Button();
            lnkCreateAccount = new LinkLabel();
            label3 = new Label();
            chkShowLoginPass = new CheckBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Ebrima", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(479, 323);
            label1.Name = "label1";
            label1.Size = new Size(68, 17);
            label1.TabIndex = 0;
            label1.Text = "Student ID";
            // 
            // txtStudentID
            // 
            txtStudentID.Font = new Font("Ebrima", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtStudentID.Location = new Point(479, 347);
            txtStudentID.Name = "txtStudentID";
            txtStudentID.Size = new Size(319, 24);
            txtStudentID.TabIndex = 1;
            txtStudentID.TextAlign = HorizontalAlignment.Center;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Ebrima", 9.75F);
            label2.Location = new Point(479, 386);
            label2.Name = "label2";
            label2.Size = new Size(64, 17);
            label2.TabIndex = 2;
            label2.Text = "Password";
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Ebrima", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPassword.Location = new Point(479, 406);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(319, 24);
            txtPassword.TabIndex = 2;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.FromArgb(64, 0, 0);
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Ebrima", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(479, 484);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(319, 34);
            btnLogin.TabIndex = 3;
            btnLogin.Text = "LOGIN NOW";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
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
            btnClose.TabIndex = 6;
            btnClose.Text = "✕";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            btnClose.MouseEnter += btnClose_MouseEnter;
            btnClose.MouseLeave += btnClose_MouseLeave;
            // 
            // lnkCreateAccount
            // 
            lnkCreateAccount.ActiveLinkColor = Color.FromArgb(198, 168, 86);
            lnkCreateAccount.AutoSize = true;
            lnkCreateAccount.BackColor = Color.Transparent;
            lnkCreateAccount.Font = new Font("Ebrima", 9.75F);
            lnkCreateAccount.LinkArea = new LinkArea(23, 33);
            lnkCreateAccount.LinkBehavior = LinkBehavior.HoverUnderline;
            lnkCreateAccount.LinkColor = Color.FromArgb(0, 0, 64);
            lnkCreateAccount.Location = new Point(536, 531);
            lnkCreateAccount.Name = "lnkCreateAccount";
            lnkCreateAccount.Size = new Size(211, 23);
            lnkCreateAccount.TabIndex = 4;
            lnkCreateAccount.TabStop = true;
            lnkCreateAccount.Text = "Don't have an account? Create one";
            lnkCreateAccount.UseCompatibleTextRendering = true;
            lnkCreateAccount.VisitedLinkColor = Color.Purple;
            lnkCreateAccount.LinkClicked += lnkCreateAccount_LinkClicked;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Ebrima", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.White;
            label3.Location = new Point(521, 250);
            label3.Name = "label3";
            label3.Size = new Size(234, 21);
            label3.TabIndex = 8;
            label3.Text = "Welcome to LetSched Started\r\n";
            // 
            // chkShowLoginPass
            // 
            chkShowLoginPass.AutoSize = true;
            chkShowLoginPass.BackColor = Color.Transparent;
            chkShowLoginPass.Font = new Font("Ebrima", 9.75F);
            chkShowLoginPass.Location = new Point(480, 444);
            chkShowLoginPass.Name = "chkShowLoginPass";
            chkShowLoginPass.Size = new Size(118, 21);
            chkShowLoginPass.TabIndex = 9;
            chkShowLoginPass.Text = "Show Password";
            chkShowLoginPass.UseVisualStyleBackColor = false;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(1280, 720);
            Controls.Add(chkShowLoginPass);
            Controls.Add(label3);
            Controls.Add(lnkCreateAccount);
            Controls.Add(btnClose);
            Controls.Add(btnLogin);
            Controls.Add(txtPassword);
            Controls.Add(label2);
            Controls.Add(txtStudentID);
            Controls.Add(label1);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login Form";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtStudentID;
        private Label label2;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnClose;
        private LinkLabel lnkCreateAccount;
        private Label label3;
        private CheckBox chkShowLoginPass;
    }
}