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
            label1 = new Label();
            txtStudentID = new TextBox();
            label2 = new Label();
            txtPassword = new TextBox();
            btnLogin = new Button();
            btnCreateAccount = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(580, 268);
            label1.Name = "label1";
            label1.Size = new Size(62, 15);
            label1.TabIndex = 0;
            label1.Text = "Student ID";
            // 
            // txtStudentID
            // 
            txtStudentID.Location = new Point(580, 286);
            txtStudentID.Name = "txtStudentID";
            txtStudentID.Size = new Size(104, 23);
            txtStudentID.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(580, 316);
            label2.Name = "label2";
            label2.Size = new Size(57, 15);
            label2.TabIndex = 2;
            label2.Text = "Password";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(580, 334);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(104, 23);
            txtPassword.TabIndex = 3;
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(580, 363);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(104, 23);
            btnLogin.TabIndex = 4;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // btnCreateAccount
            // 
            btnCreateAccount.Location = new Point(580, 392);
            btnCreateAccount.Name = "btnCreateAccount";
            btnCreateAccount.Size = new Size(104, 23);
            btnCreateAccount.TabIndex = 5;
            btnCreateAccount.Text = "Create Account";
            btnCreateAccount.UseVisualStyleBackColor = true;
            btnCreateAccount.Click += btnCreateAccount_Click;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1264, 681);
            Controls.Add(btnCreateAccount);
            Controls.Add(btnLogin);
            Controls.Add(txtPassword);
            Controls.Add(label2);
            Controls.Add(txtStudentID);
            Controls.Add(label1);
            Name = "LoginForm";
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
        private Button btnCreateAccount;
    }
}