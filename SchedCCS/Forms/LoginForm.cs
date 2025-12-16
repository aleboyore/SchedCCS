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

            // --- DOUBLE BUFFERING (Anti-Flicker) ---
            SetDoubleBuffered(this); // Buffer the Form itself

            // Buffer Sidebar if it exists
            if (this.Controls.ContainsKey("panelSidebar"))
                SetDoubleBuffered(this.Controls["panelSidebar"]);
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

        private void lnkCreateAccount_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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
            // Smoothly transition by hiding Login first
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

            // Re-show Login when dashboard closes
            this.Show();
        }

        private void OpenRegistration()
        {
            RegisterForm regForm = new RegisterForm();

            // 1. Hide Login (Smooth transition)
            this.Hide();

            // 2. Wait for Registration to finish
            regForm.ShowDialog();

            // 3. Re-show Login
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

        #region 5. Helpers

        // Helper to enable Double Buffering (Anti-Flicker)
        public static void SetDoubleBuffered(System.Windows.Forms.Control control)
        {
            if (System.Windows.Forms.SystemInformation.TerminalServerSession) return;

            typeof(System.Windows.Forms.Control).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null, control, new object[] { true });
        }

        #endregion
    }
}