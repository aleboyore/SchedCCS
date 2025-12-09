using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;

// iText7 Imports
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.Kernel.Geom;

namespace SchedCCS
{
    public partial class StudentDashboard : Form
    {
        private readonly User currentUser;

        #region 1. Initialization

        public StudentDashboard(User user)
        {
            InitializeComponent();
            currentUser = user;

            // Initialize UI Components
            InitializeDashboardUI();

            // Setup and Load Data
            SetupGrid();
            LoadMySchedule();

            // Initialize Interactive Map
            InitializeMapHotspots();
            InitializeSettingsUI();
        }

        private void InitializeDashboardUI()
        {
            try
            {
                // Update Sidebar Name
                Control lbl = this.Controls.Find("lblStudentName", true).FirstOrDefault();
                if (lbl != null) lbl.Text = currentUser.FullName;

                // Set Default Title
                UpdatePageTitle("Dashboard Home");
            }
            catch { /* Suppress minor UI lookup errors during initialization */ }
        }

        private void InitializeMapHotspots()
        {
            // Register Building A Hotspots
            SetupMapHotspot(pic_LEC9);
            SetupMapHotspot(pic_LEC10);
            SetupMapHotspot(pic_FACULTY);
            SetupMapHotspot(pic_LEC11);
            SetupMapHotspot(pic_LEC12);
            SetupMapHotspot(pic_LEC4);
            SetupMapHotspot(pic_LEC5);
            SetupMapHotspot(pic_LEC6);
            SetupMapHotspot(pic_LEC7);
            SetupMapHotspot(pic_LEC8);
            SetupMapHotspot(pic_DEAN);
            SetupMapHotspot(pic_ACCRED);
            SetupMapHotspot(pic_LEC2);
            SetupMapHotspot(pic_LEC3);
            SetupMapHotspot(pic_OCTA);

            // Register Building B Hotspots
            SetupMapHotspot(pic_LAB1);
            SetupMapHotspot(pic_LAB2);
            SetupMapHotspot(pic_LAB3);

            // Set Initial Map State
            if (cmbBuilding.Items.Count > 0)
            {
                cmbBuilding.SelectedIndex = 0;
                cmbBuilding_SelectedIndexChanged(this, EventArgs.Empty);
            }
        }

        private void SetupMapHotspot(PictureBox hotspot)
        {
            if (hotspot == null) return;

            // Configure Transparency and Events
            hotspot.Parent = picBlueprint;
            hotspot.BackColor = System.Drawing.Color.Transparent;

            string roomName = hotspot.Tag?.ToString() ?? "Unknown Room";
            toolTip1.SetToolTip(hotspot, roomName);

            hotspot.Click += Room_Click;
            hotspot.MouseEnter += Room_MouseEnter;
            hotspot.MouseLeave += Room_MouseLeave;
        }

        #endregion

        #region 2. Navigation & View Logic

        private void ShowView(Panel panelToShow)
        {
            pnlViewSchedule.Visible = false;
            pnlViewSettings.Visible = false;
            if (pnlViewHome != null) pnlViewHome.Visible = false;

            panelToShow.Visible = true;
            panelToShow.BringToFront();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            UpdatePageTitle("Dashboard Home");
            ShowView(pnlViewHome);
        }

        private void btnMySchedule_Click(object sender, EventArgs e)
        {
            UpdatePageTitle("My Class Schedule");
            ShowView(pnlViewSchedule);
            LoadMySchedule();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            UpdatePageTitle("Account Settings");
            ShowView(pnlViewSettings);

            // Pre-fill current user data
            txtEditName.Text = currentUser.FullName;
            txtEditPass.Text = currentUser.Password;
            txtEditConfirm.Text = currentUser.Password;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Log out?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void UpdatePageTitle(string titleText)
        {
            Control title = this.Controls.Find("lblPageTitle", true).FirstOrDefault();
            if (title != null) title.Text = titleText;
        }

        #endregion

        #region 3. Map Interaction Logic

        private void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBuilding.SelectedItem == null) return;

            string selected = cmbBuilding.SelectedItem.ToString();

            if (selected == "Building A")
            {
                picBlueprint.BackColor = System.Drawing.Color.LightBlue;
                SetBuildingA_Visibility(true);
                SetBuildingB_Visibility(false);
            }
            else if (selected == "Building B")
            {
                picBlueprint.BackColor = System.Drawing.Color.LightSalmon;
                SetBuildingA_Visibility(false);
                SetBuildingB_Visibility(true);
            }
        }

        private void SetBuildingA_Visibility(bool isVisible)
        {
            if (pic_LEC9 != null) pic_LEC9.Visible = isVisible;
            if (pic_LEC10 != null) pic_LEC10.Visible = isVisible;
            if (pic_FACULTY != null) pic_FACULTY.Visible = isVisible;
            if (pic_LEC11 != null) pic_LEC11.Visible = isVisible;
            if (pic_LEC12 != null) pic_LEC12.Visible = isVisible;
            if (pic_LEC4 != null) pic_LEC4.Visible = isVisible;
            if (pic_LEC5 != null) pic_LEC5.Visible = isVisible;
            if (pic_LEC6 != null) pic_LEC6.Visible = isVisible;
            if (pic_LEC7 != null) pic_LEC7.Visible = isVisible;
            if (pic_LEC8 != null) pic_LEC8.Visible = isVisible;
            if (pic_DEAN != null) pic_DEAN.Visible = isVisible;
            if (pic_ACCRED != null) pic_ACCRED.Visible = isVisible;
            if (pic_LEC2 != null) pic_LEC2.Visible = isVisible;
            if (pic_LEC3 != null) pic_LEC3.Visible = isVisible;
            if (pic_OCTA != null) pic_OCTA.Visible = isVisible;
        }

        private void SetBuildingB_Visibility(bool isVisible)
        {
            if (pic_LAB1 != null) pic_LAB1.Visible = isVisible;
            if (pic_LAB2 != null) pic_LAB2.Visible = isVisible;
            if (pic_LAB3 != null) pic_LAB3.Visible = isVisible;
        }

        private void Room_Click(object sender, EventArgs e)
        {
            PictureBox clickedSpot = (PictureBox)sender;
            string roomName = clickedSpot.Tag?.ToString();

            if (!string.IsNullOrEmpty(roomName))
            {
                using (RoomScheduleForm popup = new RoomScheduleForm(roomName))
                {
                    popup.ShowDialog();
                }
            }
        }

        private void Room_MouseEnter(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            p.BackColor = System.Drawing.Color.FromArgb(100, 255, 255, 0); // Highlight Yellow
        }

        private void Room_MouseLeave(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            p.BackColor = System.Drawing.Color.Transparent;
        }

        #endregion

        #region 4. Schedule Grid Logic

        private void SetupGrid()
        {
            dgvStudentSchedule.Columns.Clear();
            dgvStudentSchedule.Columns.Add("Time", "TIME");
            string[] days = { "MONDAY", "TUESDAY", "WEDNESDAY", "THURSDAY", "FRIDAY", "SATURDAY", "SUNDAY" };
            foreach (var d in days) dgvStudentSchedule.Columns.Add(d, d);

            // Grid Styling
            dgvStudentSchedule.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStudentSchedule.Columns["Time"].FillWeight = 60;
            dgvStudentSchedule.ScrollBars = ScrollBars.None;
            dgvStudentSchedule.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 7.5F, FontStyle.Regular);
            dgvStudentSchedule.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Bold);
            dgvStudentSchedule.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvStudentSchedule.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvStudentSchedule.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            // Dynamic Height Calculation
            int availableHeight = dgvStudentSchedule.Height - dgvStudentSchedule.ColumnHeadersHeight;
            int exactRowHeight = availableHeight / 11;

            dgvStudentSchedule.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvStudentSchedule.RowTemplate.Height = exactRowHeight;
            dgvStudentSchedule.AllowUserToAddRows = false;
            dgvStudentSchedule.RowHeadersVisible = false;

            foreach (DataGridViewColumn col in dgvStudentSchedule.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            // Generate Rows (7:00 AM - 6:00 PM)
            for (int i = 7; i < 18; i++)
            {
                string timeLabel = ToSimple12Hour($"{i}:00 - {i + 1}:00");
                int rowIndex = dgvStudentSchedule.Rows.Add(timeLabel, "", "", "", "", "", "", "");
                dgvStudentSchedule.Rows[rowIndex].Height = exactRowHeight;
            }
        }

        private void LoadMySchedule()
        {
            System.Drawing.Color defaultColor = System.Drawing.Color.White;

            // Check if data exists
            if (DataManager.MasterSchedule.Count == 0)
            {
                dgvStudentSchedule.Visible = false;
                Control lbl = this.Controls.Find("lblNoSchedule", true).FirstOrDefault();
                if (lbl != null) lbl.Visible = true;
                return;
            }
            else
            {
                dgvStudentSchedule.Visible = true;
                Control lbl = this.Controls.Find("lblNoSchedule", true).FirstOrDefault();
                if (lbl != null) lbl.Visible = false;
            }

            // Clear Grid Content
            foreach (DataGridViewRow row in dgvStudentSchedule.Rows)
                for (int c = 1; c < 8; c++)
                {
                    row.Cells[c].Value = "";
                    row.Cells[c].Style.BackColor = defaultColor;
                }

            // Filter Data
            var myClasses = DataManager.MasterSchedule
                .Where(x => x.Section == currentUser.StudentSection)
                .ToList();

            // Populate Grid
            foreach (var item in myClasses)
            {
                int startHour = int.Parse(item.Time.Split(':')[0]);
                int rowIndex = startHour - 7;

                int colIndex = 0;
                switch (item.Day) { case "Mon": colIndex = 1; break; case "Tue": colIndex = 2; break; case "Wed": colIndex = 3; break; case "Thu": colIndex = 4; break; case "Fri": colIndex = 5; break; case "Sat": colIndex = 6; break; case "Sun": colIndex = 7; break; }

                if (rowIndex >= 0 && rowIndex < dgvStudentSchedule.Rows.Count)
                {
                    dgvStudentSchedule.Rows[rowIndex].Cells[colIndex].Value = $"{item.Subject}\n{item.Teacher}\n{item.Room}";

                    if (item.Subject.Contains("(Lab)"))
                        dgvStudentSchedule.Rows[rowIndex].Cells[colIndex].Style.BackColor = System.Drawing.Color.LightSalmon;
                    else
                        dgvStudentSchedule.Rows[rowIndex].Cells[colIndex].Style.BackColor = GetSubjectColor(item.Subject);
                }
            }
            dgvStudentSchedule.ClearSelection();
        }

        private void dgvStudentSchedule_Resize(object sender, EventArgs e)
        {
            if (dgvStudentSchedule.Rows.Count > 0)
            {
                int availableHeight = dgvStudentSchedule.Height - dgvStudentSchedule.ColumnHeadersHeight;
                int exactRowHeight = availableHeight / 11;
                foreach (DataGridViewRow row in dgvStudentSchedule.Rows) row.Height = exactRowHeight;
            }
        }

        #endregion

        #region 5. Account Settings (Improved)

        // 1. INITIALIZATION: Lock everything down
        private void InitializeSettingsUI()
        {
            // Lock fields
            txtEditName.ReadOnly = true;
            txtEditPass.ReadOnly = true;
            txtEditConfirm.ReadOnly = true;

            // Gray background to indicate disabled state
            txtEditName.BackColor = System.Drawing.SystemColors.Control;
            txtEditPass.BackColor = System.Drawing.SystemColors.Control;
            txtEditConfirm.BackColor = System.Drawing.SystemColors.Control;

            // Button States
            btnEdit.Visible = true;
            btnSaveChanges.Visible = false; // Hide Save until they click Edit
            btnEdit.Text = "Edit Info";

            // Security
            txtEditPass.UseSystemPasswordChar = true;
            txtEditConfirm.UseSystemPasswordChar = true;
        }

        // 2. THE "EDIT" BUTTON TOGGLE
        private void btnEdit_Click(object sender, EventArgs e)
        {
            bool isEditing = !txtEditName.ReadOnly; // Check current state

            if (isEditing)
            {
                // CANCEL EDITING (Revert)
                InitializeSettingsUI();

                // Restore original values (So they don't lose the old data visually)
                txtEditName.Text = currentUser.FullName;
                txtEditPass.Text = currentUser.Password;
                txtEditConfirm.Text = currentUser.Password;
            }
            else
            {
                // START EDITING (Unlock)
                txtEditName.ReadOnly = false;
                txtEditPass.ReadOnly = false;
                txtEditConfirm.ReadOnly = false;

                // White background for editing
                txtEditName.BackColor = System.Drawing.Color.White;
                txtEditPass.BackColor = System.Drawing.Color.White;
                txtEditConfirm.BackColor = System.Drawing.Color.White;

                btnEdit.Text = "Cancel";
                btnSaveChanges.Visible = true;
            }
        }

        // 3. THE "SHOW PASSWORD" CHECKBOX
        private void chkShowPass_CheckedChanged(object sender, EventArgs e)
        {
            // If Checked -> Show Text (UseSystemPasswordChar = false)
            // If Unchecked -> Show Dots (UseSystemPasswordChar = true)
            txtEditPass.UseSystemPasswordChar = !chkShowPass.Checked;
            txtEditConfirm.UseSystemPasswordChar = !chkShowPass.Checked;
        }

        // 4. SAVE LOGIC (With Validation)
        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEditName.Text))
            {
                MessageBox.Show("Name cannot be empty.");
                return;
            }

            if (txtEditPass.Text != txtEditConfirm.Text)
            {
                MessageBox.Show("Passwords do not match!");
                return;
            }

            // Update Database/Memory
            var userInDb = DataManager.Users.FirstOrDefault(u => u.Username == currentUser.Username);
            if (userInDb != null)
            {
                userInDb.FullName = txtEditName.Text;
                userInDb.Password = txtEditPass.Text;

                // Update Local Current User
                currentUser.FullName = txtEditName.Text;
                currentUser.Password = txtEditPass.Text;

                // Update Sidebar Label
                Control lbl = this.Controls.Find("lblStudentName", true).FirstOrDefault();
                if (lbl != null) lbl.Text = currentUser.FullName;

                MessageBox.Show("Account updated successfully!");

                // Lock fields again after saving
                InitializeSettingsUI();
            }
        }

        #endregion

        #region 6. PDF Export Logic

        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "PDF File|*.pdf";
            save.FileName = "Official_Class_Schedule.pdf";

            if (save.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    PdfWriter writer = new PdfWriter(save.FileName);
                    PdfDocument pdf = new PdfDocument(writer);
                    pdf.SetDefaultPageSize(PageSize.A4.Rotate());

                    Document document = new Document(pdf);
                    document.SetMargins(15, 20, 10, 20);

                    // Add Content using Helper Methods
                    document.Add(GeneratePdfHeader());
                    document.Add(GenerateStudentInfoTable());
                    document.Add(GenerateScheduleTable());
                    document.Add(GeneratePdfFooter());

                    document.Close();
                    MessageBox.Show("Official Schedule Generated Successfully!");
                }
                catch (IOException) { MessageBox.Show("Error: Please close the PDF file before generating a new one."); }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            }
        }

        // Helper: Generates the official university header
        private Paragraph GeneratePdfHeader()
        {
            return new Paragraph()
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(9)
                .SetMultipliedLeading(1.0f)
                .Add("Republic of the Philippines\n")
                .Add(new Text("Laguna State Polytechnic University\n").SetFontSize(11).SetBold())
                .Add("Province of Laguna\n")
                .Add("College of Computer Studies\n\n")
                .Add(new Text("CLASS SCHEDULE\n").SetFontSize(13).SetBold())
                .Add("First Semester, Academic Year 2025-2026");
        }

        // Helper: Generates the student details block
        private Table GenerateStudentInfoTable()
        {
            string section = currentUser.StudentSection;
            string program = section.Contains("-") ? section.Split('-')[0] : section;
            string yearLevel = section.Length > 5 ? section.Substring(5, 1) : "N/A";

            Table infoTable = new Table(UnitValue.CreatePercentArray(new float[] { 1, 1, 1 }));
            infoTable.SetWidth(UnitValue.CreatePercentValue(100));
            infoTable.SetBorder(iText.Layout.Borders.Border.NO_BORDER);
            infoTable.SetMarginTop(5);
            infoTable.SetMarginBottom(5);

            infoTable.AddCell(new Cell().Add(new Paragraph($"Program: {program}").SetBold()).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetFontSize(10));
            infoTable.AddCell(new Cell().Add(new Paragraph($"Year: {yearLevel}nd Year").SetBold()).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(10));
            infoTable.AddCell(new Cell().Add(new Paragraph($"Section: {section}").SetBold()).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT).SetFontSize(10));

            return infoTable;
        }

        // Helper: Generates the main schedule grid with colors
        private Table GenerateScheduleTable()
        {
            float[] colWidths = { 1.2f, 2, 2, 2, 2, 2, 2, 2 };
            Table table = new Table(UnitValue.CreatePercentArray(colWidths));
            table.SetWidth(UnitValue.CreatePercentValue(100));

            // Headers
            string[] headers = { "TIME", "MONDAY", "TUESDAY", "WEDNESDAY", "THURSDAY", "FRIDAY", "SATURDAY", "SUNDAY" };
            foreach (string h in headers)
            {
                table.AddCell(new Cell().Add(new Paragraph(h).SetBold().SetFontSize(7))
                    .SetBackgroundColor(ColorConstants.WHITE)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .SetPadding(0).SetHeight(15));
            }

            // Rows
            foreach (DataGridViewRow row in dgvStudentSchedule.Rows)
            {
                string niceTime = ToSimple12Hour(row.Cells[0].Value?.ToString() ?? "");
                table.AddCell(new Cell().Add(new Paragraph(niceTime).SetFontSize(7).SetBold())
                    .SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetPadding(0).SetHeight(25));

                for (int i = 1; i < 7; i++)
                {
                    string content = row.Cells[i].Value?.ToString() ?? "";
                    Cell dataCell = new Cell().Add(new Paragraph(content).SetFontSize(7).SetMultipliedLeading(0.9f));
                    dataCell.SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetPadding(1).SetHeight(25);

                    System.Drawing.Color winColor = row.Cells[i].Style.BackColor;
                    if (winColor != System.Drawing.Color.White && winColor.Name != "0")
                    {
                        DeviceRgb pdfColor = new DeviceRgb(winColor.R, winColor.G, winColor.B);
                        dataCell.SetBackgroundColor(pdfColor);
                    }
                    table.AddCell(dataCell);
                }
                table.AddCell(new Cell().SetHeight(25)); // Sunday
            }
            return table;
        }

        // Helper: Generates signatures footer
        private Table GeneratePdfFooter()
        {
            Table footerTable = new Table(UnitValue.CreatePercentArray(new float[] { 1, 1 }));
            footerTable.SetWidth(UnitValue.CreatePercentValue(100));
            footerTable.SetBorder(iText.Layout.Borders.Border.NO_BORDER);
            footerTable.SetMarginTop(10);

            Cell left = new Cell().Add(new Paragraph("Generated by: Auto-Scheduler System v1.0").SetFontSize(8)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
            Cell right = new Cell().Add(new Paragraph("Approved by: ______________________\nCollege Dean").SetFontSize(8)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

            footerTable.AddCell(left);
            footerTable.AddCell(right);
            return footerTable;
        }

        #endregion

        #region 7. Formatting Helpers

        private string ToSimple12Hour(string timeRange)
        {
            try
            {
                var parts = timeRange.Split('-');
                string start = DateTime.Parse(parts[0].Trim()).ToString("h:mm");
                string end = DateTime.Parse(parts[1].Trim()).ToString("h:mm");
                return $"{start} - {end}";
            }
            catch { return timeRange; }
        }

        private System.Drawing.Color GetSubjectColor(string subjectName)
        {
            string baseName = subjectName.Replace(" (Lec)", "").Replace(" (Lab)", "").Trim();
            int seed = baseName.GetHashCode();
            Random r = new Random(seed);
            return System.Drawing.Color.FromArgb(r.Next(160, 255), r.Next(160, 255), r.Next(160, 255));
        }

        #endregion
    }
}