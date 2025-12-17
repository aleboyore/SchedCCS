using System;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace SchedCCS
{
    public partial class RegisterForm : Form
    {
        #region 1. Initialization & UI Setup

        public RegisterForm()
        {
            InitializeComponent();
            SetDoubleBuffered(this);

            // Use the built-in PlaceholderText property (Available in .NET 8)
            // This provides a native placeholder that doesn't interfere with the Text property
            txtSection.PlaceholderText = "Ex: 'BSCS 1A' or '3GAV1'";
            txtStudentID.PlaceholderText = "03XX-XXXX";
            txtPassword.PlaceholderText = "Min 8 chars, 1 letter, 1 number";
        }

        public static void SetDoubleBuffered(Control control)
        {
            if (SystemInformation.TerminalServerSession) return;
            typeof(Control).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null, control, new object[] { true });
        }

        #endregion

        #region 2. User Interaction Logic

        private void btnRegister_Click(object sender, EventArgs e)
        {
            RegisterStudent();
        }

        private void chkShowRegPass_CheckedChanged(object sender, EventArgs e)
        {
            char mask = chkShowRegPass.Checked ? '\0' : '*';
            txtPassword.PasswordChar = mask;
            txtConfirm.PasswordChar = mask;
        }

        private void lnkBackToLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => this.Close();

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit application?", "Confirm Exit", MessageBoxButtons.YesNo) == DialogResult.Yes)
                Application.Exit();
        }

        private void btnClose_MouseEnter(object sender, EventArgs e) { btnClose.BackColor = Color.Red; btnClose.ForeColor = Color.White; }
        private void btnClose_MouseLeave(object sender, EventArgs e) { btnClose.BackColor = Color.Transparent; btnClose.ForeColor = Color.Black; }

        #endregion

        #region 3. Core Registration Logic

        private void RegisterStudent()
        {
            // 1. Validate Basic Fields
            // Removed checks against placeholder text since we use PlaceholderText property now
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) || 
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtStudentID.Text) || 
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("All fields are required.", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check Password Match
            if (txtPassword.Text != txtConfirm.Text)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check Student ID Format
            string studentID = txtStudentID.Text.Trim();
            if (!IsValidStudentId(studentID))
            {
                MessageBox.Show("Invalid Student ID Format.\n\nMust follow the college format: 03xx-xxxx\n(e.g., 0323-1234)",
                                "ID Format Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check Password Strength
            if (!IsValidPassword(txtPassword.Text))
            {
                MessageBox.Show("Password is too weak.\n\nRequirements:\n- Minimum 8 characters\n- At least 1 Letter\n- At least 1 Number",
                                "Security Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check Section Format
            string sectionInput = txtSection.Text.Trim();
            if (string.IsNullOrWhiteSpace(sectionInput))
            {
                MessageBox.Show("Please enter your section.", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsValidSectionFormat(sectionInput))
            {
                MessageBox.Show("Invalid Section Format.\n\nUse: 'BSCS 1A' (1st/2nd Yr) or '3GAV1' (3rd/4th Yr)",
                                "Format Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSection.Focus();
                return;
            }

            // Check Duplicates (RAM Check for Speed/Offline Safety)
            if (DataManager.Users.Any(u => u.Username == studentID))
            {
                MessageBox.Show("Student ID is already registered.", "Duplicate ID", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            // Sync Section (Auto-Uppercase)
            try { DataManager.EnsureSectionExists(sectionInput.ToUpper()); }
            catch (Exception ex) { Console.WriteLine("Section sync warning: " + ex.Message); }

            // Create Object
            User newUser = new User
            {
                Username = studentID,
                Password = ComputeSha256Hash(txtPassword.Text),
                FullName = $"{txtFirstName.Text.Trim()} {txtLastName.Text.Trim()}",
                Role = "Student",
                StudentSection = sectionInput.ToUpper()
            };

            // Save Logic (Database First, RAM Fallback)
            bool dbSuccess = false;
            try
            {
                DatabaseHelper.SaveUser(newUser);
                dbSuccess = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database unavailable (" + ex.Message + ").\nRegistering in Offline Mode (RAM only).",
                                "Offline Registration", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (!DataManager.Users.Any(u => u.Username == newUser.Username))
            {
                DataManager.Users.Add(newUser);
            }

            if (dbSuccess)
                MessageBox.Show("Account created successfully! You can now login.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }

        #endregion

        #region 4. Helper Utilities (Regex Validators)

        private bool IsValidStudentId(string id)
        {
            return Regex.IsMatch(id, @"^03\d{2}-\d{4}$");
        }

        private bool IsValidPassword(string password)
        {
            if (password.Length < 8) return false;
            if (!password.Any(char.IsLetter)) return false;
            if (!password.Any(char.IsDigit)) return false;
            return true;
        }

        private bool IsValidSectionFormat(string input)
        {
            string section = input.Trim().ToUpper();
            string patternBase = @"^(BSCS|BSINFO)\s[1-2][A-Z]$";
            string patternMajor = @"^[3-4](GAV|IS|SMP|WMAD|NA)[0-9]$";

            return Regex.IsMatch(section, patternBase) || Regex.IsMatch(section, patternMajor);
        }

        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes) builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }

        #endregion
    }
}