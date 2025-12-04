using System;
using System.Linq;
using System.Windows.Forms;

namespace SchedCCS
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        #region Event Handlers

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string inputID = txtStudentID.Text;
            string inputPass = txtPassword.Text;

            // [Encapsulation] Delegate authentication logic to a specific method
            var user = AuthenticateUser(inputID, inputPass);

            if (user != null)
            {
                ProceedToDashboard(user);
            }
            else
            {
                MessageBox.Show("Invalid Student ID or Password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCreateAccount_Click(object sender, EventArgs e)
        {
            OpenRegistration();
        }

        #endregion

        #region Helper Methods (Logic Encapsulation)

        // Queries the DataManager to validate credentials.
        private User AuthenticateUser(string username, string password)
        {
            return DataManager.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        // Handles the navigation flow based on User Role.
        private void ProceedToDashboard(User user)
        {
            this.Hide(); // Hide Login Form

            if (user.Role == "Admin")
            {
                MessageBox.Show("Welcome, Admin!");

                // Launch Admin Dashboard
                using (AdminDashboard adminPage = new AdminDashboard())
                {
                    adminPage.ShowDialog(); // Execution pauses here until AdminDashboard closes
                }

                // Admin Logout Logic: Full Wipe for security
                ClearFields(wipeUsername: true);
            }
            else
            {
                MessageBox.Show($"Welcome, {user.FullName}!");

                // Launch Student Dashboard with User Context
                using (StudentDashboard studentPage = new StudentDashboard(user))
                {
                    studentPage.ShowDialog(); // Execution pauses here until StudentDashboard closes
                }

                // Student Logout Logic: Keep ID for convenience, wipe password
                ClearFields(wipeUsername: false);
            }

            this.Show(); // Re-show Login Form
        }

        // Opens the registration dialog.
        private void OpenRegistration()
        {
            this.Hide();
            using (RegisterForm register = new RegisterForm())
            {
                register.ShowDialog();
            }
            this.Show();
        }

        // Resets input fields based on security needs.
        private void ClearFields(bool wipeUsername)
        {
            txtPassword.Clear();

            if (wipeUsername)
            {
                txtStudentID.Clear();
                txtStudentID.Focus(); // Reset cursor focus to top box
            }
        }

        #endregion
    }
}