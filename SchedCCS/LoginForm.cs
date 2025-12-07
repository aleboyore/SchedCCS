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

        #region 2. Event Handlers

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string inputID = txtStudentID.Text;
            string inputPass = txtPassword.Text;

            // Attempt to retrieve user from DataManager
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

        #endregion

        #region 3. Helper Methods

        // Validates credentials against the in-memory user list
        private User AuthenticateUser(string username, string password)
        {
            return DataManager.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        // Directs the user to the appropriate dashboard based on role
        private void ProceedToDashboard(User user)
        {
            this.Hide();

            if (user.Role == "Admin")
            {
                MessageBox.Show("Welcome, Admin!");

                using (AdminDashboard adminPage = new AdminDashboard())
                {
                    // ShowDialog pauses execution here until the dashboard closes
                    adminPage.ShowDialog();
                }

                // Security: Wipe all fields on Admin logout
                ClearFields(wipeUsername: true);
            }
            else
            {
                MessageBox.Show($"Welcome, {user.FullName}!");

                // Pass the specific User object to the Student Dashboard context
                using (StudentDashboard studentPage = new StudentDashboard(user))
                {
                    studentPage.ShowDialog();
                }

                // UX: Keep Student ID populated for convenience on logout
                ClearFields(wipeUsername: false);
            }

            this.Show(); // Re-open Login window
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