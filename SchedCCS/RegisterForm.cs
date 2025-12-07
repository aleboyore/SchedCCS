using System;
using System.Windows.Forms;
using System.Linq;

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

        #region 2. Core Logic

        // Retrieves available sections from the DataManager and populates the ComboBox.
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
                MessageBox.Show("No sections found. Administrator action required to generate data.");
            }
        }

        // Validates input fields and creates a new Student entity.
        private void RegisterStudent()
        {
            // 1. Validate required text fields
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtStudentID.Text))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            // 2. Validate section selection
            if (cmbSection.SelectedItem == null)
            {
                MessageBox.Show("Please select a section.");
                return;
            }

            // 3. Validate password matching
            if (txtPassword.Text != txtConfirm.Text)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            // 4. Check for duplicate Student ID
            bool userExists = DataManager.Users.Any(u => u.Username == txtStudentID.Text);
            if (userExists)
            {
                MessageBox.Show("The provided Student ID is already registered.");
                return;
            }

            // 5. Create and persist new User object
            User newUser = new User
            {
                Username = txtStudentID.Text,
                Password = txtPassword.Text,
                FullName = $"{txtFirstName.Text} {txtLastName.Text}",
                Role = "Student",
                StudentSection = cmbSection.SelectedItem.ToString()
            };

            DataManager.Users.Add(newUser);
            MessageBox.Show("Account created successfully. Please login.");

            this.Close();
        }

        #endregion

        #region 3. Event Handlers

        private void btnRegister_Click(object sender, EventArgs e)
        {
            RegisterStudent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}