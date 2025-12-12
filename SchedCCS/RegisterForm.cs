using System;
using System.Linq;
using System.Windows.Forms;

namespace SchedCCS
{
    public partial class RegisterForm : Form
    {
        #region 1. Initialization

        public RegisterForm()
        {
            InitializeComponent();
            PopulateSectionDropdown();
        }

        #endregion

        #region 2. UI Event Handlers

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
            btnClose.BackColor = System.Drawing.Color.Red;
            btnClose.ForeColor = System.Drawing.Color.White;
        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            btnClose.BackColor = System.Drawing.Color.Transparent;
            btnClose.ForeColor = System.Drawing.Color.Black;
        }

        #endregion

        #region 3. Core Logic & Helpers

        private void PopulateSectionDropdown()
        {
            cmbSection.Items.Clear();

            if (DataManager.Sections.Count > 0)
            {
                foreach (var s in DataManager.Sections)
                {
                    cmbSection.Items.Add(s.Name);
                }
            }
            else
            {
                MessageBox.Show("No sections available. Please contact Administrator.");
            }
        }

        private void RegisterStudent()
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtStudentID.Text))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            // Validate section selection
            if (cmbSection.SelectedItem == null)
            {
                MessageBox.Show("Please select a section.");
                return;
            }

            // Validate password matching
            if (txtPassword.Text != txtConfirm.Text)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            // Check for duplicate Student ID
            bool userExists = DataManager.Users.Any(u => u.Username == txtStudentID.Text);
            if (userExists)
            {
                MessageBox.Show("Student ID is already registered.");
                return;
            }

            // Create and persist new User
            User newUser = new User
            {
                Username = txtStudentID.Text,

                // Hashing the password before saving
                Password = SecurityHelper.HashPassword(txtPassword.Text),

                FullName = $"{txtFirstName.Text} {txtLastName.Text}",
                Role = "Student",
                StudentSection = cmbSection.SelectedItem.ToString()
            };

            DataManager.Users.Add(newUser);
            MessageBox.Show("Account created successfully. Please login.");

            this.Close();
        }

        #endregion

        
    }
}