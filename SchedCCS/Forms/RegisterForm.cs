using System;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace SchedCCS
{
    public partial class RegisterForm : Form
    {
        // Placeholder text constant
        private const string SectionPlaceholder = "Format: PROGRAM-YEAR-SECTION (e.g., BSCS 1A)";

        #region 1. Initialization

        public RegisterForm()
        {
            InitializeComponent();

            // --- DOUBLE BUFFERING (Anti-Flicker) ---
            SetDoubleBuffered(this); // Buffer the Form itself

            // If you have a main layout panel (like a sidebar), buffer it here too:
            // if (this.Controls.ContainsKey("pnlSidebar")) SetDoubleBuffered(this.Controls["pnlSidebar"]);

            InitializePlaceholder();
        }

        private void InitializePlaceholder()
        {
            // Set the initial state of the Section text box
            txtSection.Text = SectionPlaceholder;
            txtSection.ForeColor = Color.Gray;

            // Wire up the events manually
            txtSection.Enter += TxtSection_Enter;
            txtSection.Leave += TxtSection_Leave;
        }

        #endregion

        #region 2. Placeholder Logic (Ghost Text)

        private void TxtSection_Enter(object sender, EventArgs e)
        {
            // When user clicks the box, if it has the placeholder, clear it
            if (txtSection.Text == SectionPlaceholder)
            {
                txtSection.Text = "";
                txtSection.ForeColor = Color.Black;
            }
        }

        private void TxtSection_Leave(object sender, EventArgs e)
        {
            // When user leaves the box, if empty, put placeholder back
            if (string.IsNullOrWhiteSpace(txtSection.Text))
            {
                txtSection.Text = SectionPlaceholder;
                txtSection.ForeColor = Color.Gray;
            }
        }

        #endregion

        #region 3. UI Event Handlers

        private void btnRegister_Click(object sender, EventArgs e)
        {
            RegisterStudent();
        }

        private void lnkBackToLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnClose_MouseEnter(object sender, EventArgs e)
        {
            btnClose.BackColor = Color.Red;
            btnClose.ForeColor = Color.White;
        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            btnClose.BackColor = Color.Transparent;
            btnClose.ForeColor = Color.Black;
        }

        #endregion

        #region 4. Core Registration Logic

        private void RegisterStudent()
        {
            // 1. Validate Fields
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtStudentID.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            // Validate Section (Check if it's empty OR still has the placeholder)
            string sectionInput = txtSection.Text.Trim();
            if (string.IsNullOrWhiteSpace(sectionInput) || sectionInput == SectionPlaceholder)
            {
                MessageBox.Show("Please enter your section.");
                return;
            }

            if (txtPassword.Text != txtConfirm.Text)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            // 2. Check for Duplicate ID (RAM check)
            if (DataManager.Users.Any(u => u.Username == txtStudentID.Text))
            {
                MessageBox.Show("Student ID is already registered.");
                return;
            }

            // 3. CRITICAL: Ensure the Section Exists (Create if missing)
            // This solves the "Empty Database" problem
            try
            {
                DataManager.EnsureSectionExists(sectionInput);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing section: " + ex.Message);
                return;
            }

            // 4. Create User Object
            User newUser = new User
            {
                Username = txtStudentID.Text.Trim(),
                Password = ComputeSha256Hash(txtPassword.Text), // Hash locally
                FullName = $"{txtFirstName.Text.Trim()} {txtLastName.Text.Trim()}",
                Role = "Student",
                StudentSection = sectionInput.ToUpper() // Standardize to uppercase
            };

            // 5. Save to Database AND Memory
            try
            {
                DatabaseHelper.SaveUser(newUser); // Save to SQL
                DataManager.Users.Add(newUser);   // Update RAM immediately

                MessageBox.Show("Account created successfully! You can now login.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }

            DataManager.Users.Add(newUser);
            this.Close();
        }

        // Standard Hashing Helper
        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++) builder.Append(bytes[i].ToString("x2"));
                return builder.ToString();
            }
        }

        #endregion

        #region 5. Helpers

        // Helper to enable Double Buffering
        public static void SetDoubleBuffered(Control control)
        {
            if (System.Windows.Forms.SystemInformation.TerminalServerSession) return;

            typeof(Control).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null, control, new object[] { true });
        }

        #endregion
    }
}