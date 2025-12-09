using System;
using System.Linq;
using System.Windows.Forms;

namespace SchedCCS
{
    public partial class LoginForm : Form
    {
        #region 1. Constructor

        public LoginForm()
        {
            InitializeComponent();
        }

        #endregion

        #region 2. UI Event Handlers

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string inputID = txtStudentID.Text;
            string inputPass = txtPassword.Text;

            var user = AuthenticateUser(inputID, inputPass);

            if (user != null)
            {
                ProceedToDashboard(user);
            }
            else
            {
                MessageBox.Show("Invalid Student ID or Password.", "Login Failed",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCreateAccount_Click(object sender, EventArgs e)
        {
            OpenRegistration();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnClose_MouseEnter(object sender, EventArgs e)
        {
            btnClose.BackColor = System.Drawing.Color.Red;
            btnClose.ForeColor = System.Drawing.Color.White;
        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            btnClose.BackColor = System.Drawing.Color.Transparent;
            btnClose.ForeColor = System.Drawing.Color.Black;
        }

        #endregion

        #region 3. Authentication Logic

        private User AuthenticateUser(string username, string password)
        {
            // 1. Hash the input immediately
            string hashedInput = SecurityHelper.HashPassword(password);

            // 2. Compare the HASHED input with the HASHED database password
            return DataManager.Users.FirstOrDefault(u => u.Username == username && u.Password == hashedInput);
        }

        #endregion

        #region 4. Navigation & State Management

        private void ProceedToDashboard(User user)
        {
            this.Hide();

            if (user.Role == "Admin")
            {
                MessageBox.Show("Welcome, Admin!");
                using (AdminDashboard adminPage = new AdminDashboard())
                {
                    adminPage.ShowDialog();
                }
                ClearFields(wipeUsername: true);
            }
            else
            {
                MessageBox.Show($"Welcome, {user.FullName}!");
                using (StudentDashboard studentPage = new StudentDashboard(user))
                {
                    studentPage.ShowDialog();
                }
                ClearFields(wipeUsername: false);
            }

            this.Show();
        }

        private void OpenRegistration()
        {
            this.Hide();
            using (RegisterForm register = new RegisterForm())
            {
                register.ShowDialog();
            }
            this.Show();
        }

        private void ClearFields(bool wipeUsername)
        {
            txtPassword.Clear();

            if (wipeUsername)
            {
                txtStudentID.Clear();
                txtStudentID.Focus();
            }
        }

        #endregion
    }
}