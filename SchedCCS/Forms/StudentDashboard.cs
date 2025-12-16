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

            // --- DOUBLE BUFFERING (Anti-Flicker) ---
            SetDoubleBuffered(pnlContent);      // Main container
            SetDoubleBuffered(pnlViewHome);     // Map View
            SetDoubleBuffered(pnlViewSchedule); // Grid View
            SetDoubleBuffered(pnlViewSettings); // Settings View

            // Buffer Building Panels (if they exist in designer)
            if (this.Controls.Find("pnlBuildingA", true).Length > 0)
                SetDoubleBuffered(pnlBuildingA);

            if (this.Controls.Find("pnlBuildingB", true).Length > 0)
                SetDoubleBuffered(pnlBuildingB);
            // ---------------------------------------

            // 1. Initialize UI components
            InitializeDashboardUI();
            InitializeMapHotspots();
            InitializeSettingsUI();

            // 2. Load Data
            try
            {
                SetupGrid();
                LoadMySchedule();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading schedule: " + ex.Message);
            }
        }

        private void InitializeDashboardUI()
        {
            try
            {
                Control lbl = this.Controls.Find("lblStudentName", true).FirstOrDefault();
                if (lbl != null) lbl.Text = currentUser.FullName;

                UpdatePageTitle("Dashboard Home");
                ShowView(pnlViewHome); // Default View
            }
            catch { }
        }

        #endregion

        #region 2. Navigation & View Logic

        private void ShowView(Panel panelToShow)
        {
            // Suspend layout to reduce lag during switch
            this.SuspendLayout();

            pnlViewSchedule.Visible = false;
            pnlViewSettings.Visible = false;
            if (pnlViewHome != null) pnlViewHome.Visible = false;

            panelToShow.Visible = true;
            panelToShow.BringToFront();

            this.ResumeLayout();
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
            InitializeSettingsUI(); // Revert to clean state
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Log out?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit the application?", "Exit", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Application.Exit();
            }
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

        private void UpdatePageTitle(string titleText)
        {
            Control title = this.Controls.Find("lblPageTitle", true).FirstOrDefault();
            if (title != null) title.Text = titleText;
        }

        #endregion

        #region 3. Map Interaction Logic

        private void InitializeMapHotspots()
        {
            // Building A
            SetupMapHotspot(pic_LEC9); SetupMapHotspot(pic_LEC10); SetupMapHotspot(pic_FACULTY);
            SetupMapHotspot(pic_LEC11); SetupMapHotspot(pic_LEC12); SetupMapHotspot(pic_LEC4);
            SetupMapHotspot(pic_LEC5); SetupMapHotspot(pic_LEC6); SetupMapHotspot(pic_LEC7);
            SetupMapHotspot(pic_LEC8); SetupMapHotspot(pic_DEAN); SetupMapHotspot(pic_ACCRED);
            SetupMapHotspot(pic_LEC2); SetupMapHotspot(pic_LEC3); SetupMapHotspot(pic_OCTA);

            // Building B
            SetupMapHotspot(pic_LAB1); SetupMapHotspot(pic_LAB2); SetupMapHotspot(pic_LAB3);
            SetupMapHotspot(pic_LAB4); SetupMapHotspot(pic_UnK); SetupMapHotspot(pic_LEC1);
            SetupMapHotspot(pic_LAB5); SetupMapHotspot(pic_LAB6);

            if (cmbBuilding.Items.Count > 0) cmbBuilding.SelectedIndex = 0;
        }

        private void SetupMapHotspot(PictureBox hotspot)
        {
            if (hotspot == null) return;

            hotspot.Visible = true;
            hotspot.BringToFront();
            hotspot.BackColor = System.Drawing.Color.Transparent;

            string roomName = hotspot.Tag?.ToString() ?? "Unknown Room";
            toolTip1.SetToolTip(hotspot, roomName);

            // Prevent duplicate event subscription
            hotspot.Click -= Room_Click;
            hotspot.Click += Room_Click;

            hotspot.MouseEnter -= Room_MouseEnter;
            hotspot.MouseEnter += Room_MouseEnter;

            hotspot.MouseLeave -= Room_MouseLeave;
            hotspot.MouseLeave += Room_MouseLeave;
        }

        private void cmbBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBuilding.SelectedItem == null) return;

            string selected = cmbBuilding.SelectedItem.ToString();

            // Suspend layout to prevent flicker when switching building images
            pnlViewHome.SuspendLayout();

            if (selected == "Building A")
            {
                pnlBuildingA.Visible = true;
                pnlBuildingB.Visible = false;
                pnlBuildingA.BringToFront();
            }
            else if (selected == "Building B")
            {
                pnlBuildingB.Visible = true;
                pnlBuildingA.Visible = false;
                pnlBuildingB.BringToFront();
            }

            // Ensure floating controls stay on top
            cmbBuilding.BringToFront();
            lblSelectBuilding.BringToFront();

            pnlViewHome.ResumeLayout();
        }

        private void Room_Click(object sender, EventArgs e)
        {
            PictureBox clickedSpot = (PictureBox)sender;
            string rawTag = clickedSpot.Tag?.ToString();

            if (string.IsNullOrEmpty(rawTag)) return;

            string upperTag = rawTag.ToUpper();

            // Special Room Handling (Offices/Storage)
            if (upperTag.Contains("FACULTY") || upperTag.Contains("DEAN") ||
                upperTag.Contains("ACCRED") || upperTag.Contains("OCTA") ||
                upperTag.Contains("UNKNOWN") || upperTag.Contains("STORAGE") ||
                upperTag.Contains("OFFICE"))
            {
                string msg = "";
                if (upperTag.Contains("FACULTY")) msg = "Faculty Room\n(Teachers Only)";
                else if (upperTag.Contains("DEAN")) msg = "College Dean's Office\n(Official Business Only)";
                else if (upperTag.Contains("OCTA")) msg = "OCTA Research Office";
                else msg = $"{rawTag}\n(Restricted Area)";

                MessageBox.Show(msg, "Room Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Normalize Tag to Database Code
            string dbRoomName = rawTag;
            if (dbRoomName.Contains("Laboratory Room"))
                dbRoomName = dbRoomName.Replace("Laboratory Room", "LAB").Trim();
            else if (dbRoomName.Contains("Lecture Room"))
                dbRoomName = dbRoomName.Replace("Lecture Room", "LEC").Trim();

            // Open Schedule Popup
            using (RoomScheduleForm popup = new RoomScheduleForm(dbRoomName))
            {
                popup.ShowDialog();
            }
        }

        private void Room_MouseEnter(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            // Highlight color (Semi-transparent Yellow)
            p.BackColor = System.Drawing.Color.FromArgb(100, 255, 255, 0);
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

            dgvStudentSchedule.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStudentSchedule.Columns["Time"].FillWeight = 60;
            dgvStudentSchedule.ScrollBars = ScrollBars.None;
            dgvStudentSchedule.DefaultCellStyle.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular);
            dgvStudentSchedule.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            dgvStudentSchedule.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvStudentSchedule.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvStudentSchedule.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            int availableHeight = dgvStudentSchedule.Height - dgvStudentSchedule.ColumnHeadersHeight;
            int exactRowHeight = availableHeight / 11;

            dgvStudentSchedule.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvStudentSchedule.RowTemplate.Height = exactRowHeight;
            dgvStudentSchedule.AllowUserToAddRows = false;
            dgvStudentSchedule.RowHeadersVisible = false;

            foreach (DataGridViewColumn col in dgvStudentSchedule.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;

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

            // Reset Grid
            foreach (DataGridViewRow row in dgvStudentSchedule.Rows)
                for (int c = 1; c < 8; c++)
                {
                    row.Cells[c].Value = "";
                    row.Cells[c].Style.BackColor = defaultColor;
                }

            // Populate Data
            var myClasses = DataManager.MasterSchedule
                .Where(x => x.Section == currentUser.StudentSection)
                .ToList();

            foreach (var item in myClasses)
            {
                int startHour = int.Parse(item.Time.Split(':')[0]);
                int rowIndex = startHour - 7;
                int colIndex = GetDayColumnIndex(item.Day);

                if (rowIndex >= 0 && rowIndex < dgvStudentSchedule.Rows.Count && colIndex > 0)
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

        #region 5. Account Settings Logic

        private void InitializeSettingsUI()
        {
            // 1. Lock fields
            txtEditName.ReadOnly = true;
            txtEditPass.ReadOnly = true;
            txtEditConfirm.ReadOnly = true;

            // 2. Visuals (Gray background)
            txtEditName.BackColor = System.Drawing.SystemColors.Control;
            txtEditPass.BackColor = System.Drawing.SystemColors.Control;
            txtEditConfirm.BackColor = System.Drawing.SystemColors.Control;

            // 3. Load Data
            txtEditName.Text = currentUser.FullName;

            // 4. Force "Hidden" text state
            txtEditPass.PasswordChar = '\0';
            txtEditConfirm.PasswordChar = '\0';

            txtEditPass.Text = "(Hidden)";
            txtEditConfirm.Text = "(Hidden)";

            txtEditPass.ForeColor = System.Drawing.Color.Gray;
            txtEditConfirm.ForeColor = System.Drawing.Color.Gray;

            // 5. Button States
            btnEdit.Visible = true;
            btnSaveChanges.Visible = false;
            btnEdit.Text = "Edit Info";

            // 6. Hide Toggle
            chkShowPass.Visible = false;
            chkShowPass.Checked = false;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            bool isEditing = !txtEditName.ReadOnly;

            if (isEditing)
            {
                // === CANCEL EDITING (Revert to Clean State) ===
                InitializeSettingsUI();
                txtEditName.Text = currentUser.FullName;
            }
            else
            {
                // === START EDITING (Unlock & Reveal) ===
                txtEditName.ReadOnly = false;
                txtEditPass.ReadOnly = false;
                txtEditConfirm.ReadOnly = false;

                // Visuals
                txtEditName.BackColor = System.Drawing.Color.White;
                txtEditPass.BackColor = System.Drawing.Color.White;
                txtEditConfirm.BackColor = System.Drawing.Color.White;

                // Reset Text Color
                txtEditPass.ForeColor = System.Drawing.Color.Black;
                txtEditConfirm.ForeColor = System.Drawing.Color.Black;

                // Disable the System dots so we have full control
                txtEditPass.UseSystemPasswordChar = false;
                txtEditConfirm.UseSystemPasswordChar = false;

                // Force the Asterisk (*) mask immediately
                txtEditPass.PasswordChar = '*';
                txtEditConfirm.PasswordChar = '*';

                txtEditPass.Clear();
                txtEditConfirm.Clear();

                // Show the checkbox
                chkShowPass.Visible = true;

                btnEdit.Text = "Cancel";
                btnSaveChanges.Visible = true;
            }
        }

        private void chkShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPass.Checked)
            {
                // SHOW PASSWORD: Set the mask to "null" (nothing)
                txtEditPass.PasswordChar = '\0';
                txtEditConfirm.PasswordChar = '\0';
            }
            else
            {
                // HIDE PASSWORD: Set the mask back to '*'
                txtEditPass.PasswordChar = '*';
                txtEditConfirm.PasswordChar = '*';
            }
        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEditName.Text))
            {
                MessageBox.Show("Name cannot be empty.");
                return;
            }

            var userInDb = DataManager.Users.FirstOrDefault(u => u.Username == currentUser.Username);
            if (userInDb != null)
            {
                userInDb.FullName = txtEditName.Text;
                currentUser.FullName = txtEditName.Text;

                // Only update password if user typed something
                if (!string.IsNullOrEmpty(txtEditPass.Text))
                {
                    if (txtEditPass.Text != txtEditConfirm.Text)
                    {
                        MessageBox.Show("Passwords do not match!");
                        return;
                    }

                    string newHash = SecurityHelper.HashPassword(txtEditPass.Text);
                    userInDb.Password = newHash;
                    currentUser.Password = newHash;
                }

                Control lbl = this.Controls.Find("lblStudentName", true).FirstOrDefault();
                if (lbl != null) lbl.Text = currentUser.FullName;

                MessageBox.Show("Account updated successfully!");
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
                    pdf.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4.Rotate());

                    Document document = new Document(pdf);
                    document.SetMargins(15, 20, 10, 20);

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

        private Table GenerateScheduleTable()
        {
            float[] colWidths = { 1.2f, 2, 2, 2, 2, 2, 2, 2 };
            Table table = new Table(UnitValue.CreatePercentArray(colWidths));
            table.SetWidth(UnitValue.CreatePercentValue(100));

            string[] headers = { "TIME", "MONDAY", "TUESDAY", "WEDNESDAY", "THURSDAY", "FRIDAY", "SATURDAY", "SUNDAY" };
            foreach (string h in headers)
            {
                table.AddCell(new Cell().Add(new Paragraph(h).SetBold().SetFontSize(7))
                    .SetBackgroundColor(ColorConstants.WHITE)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .SetPadding(0).SetHeight(15));
            }

            foreach (DataGridViewRow row in dgvStudentSchedule.Rows)
            {
                string niceTime = ToSimple12Hour(row.Cells[0].Value?.ToString() ?? "");
                table.AddCell(new Cell().Add(new Paragraph(niceTime).SetFontSize(7).SetBold())
                    .SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetPadding(0).SetHeight(25));

                for (int i = 1; i < 8; i++)
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
            }
            return table;
        }

        private Table GeneratePdfFooter()
        {
            Table footerTable = new Table(UnitValue.CreatePercentArray(new float[] { 2, 1 }));
            footerTable.SetWidth(UnitValue.CreatePercentValue(100));
            footerTable.SetBorder(iText.Layout.Borders.Border.NO_BORDER);
            footerTable.SetMarginTop(25);

            // SYSTEM NAME
            string systemName = "Generated by: Let's Sched Started v1.0\n(College of Computer Studies)";

            Cell left = new Cell().Add(new Paragraph(systemName).SetFontSize(8).SetItalic())
                .SetBorder(iText.Layout.Borders.Border.NO_BORDER);

            // APPROVAL
            Cell right = new Cell().Add(new Paragraph("Approved by: ______________________\nCollege Dean")
                .SetFontSize(10).SetBold())
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetBorder(iText.Layout.Borders.Border.NO_BORDER);

            footerTable.AddCell(left);
            footerTable.AddCell(right);
            return footerTable;
        }

        #endregion

        #region 7. Helpers

        private int GetDayColumnIndex(string day)
        {
            switch (day)
            {
                case "Mon": return 1;
                case "Tue": return 2;
                case "Wed": return 3;
                case "Thu": return 4;
                case "Fri": return 5;
                case "Sat": return 6;
                case "Sun": return 7;
                default: return 0;
            }
        }

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

        private void dgvStudentSchedule_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}