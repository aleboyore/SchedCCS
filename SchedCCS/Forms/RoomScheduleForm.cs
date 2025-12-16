using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SchedCCS
{
    public partial class RoomScheduleForm : Form
    {
        #region Fields

        // Immutable field for the room being viewed
        private readonly string _targetRoom;

        #endregion

        #region 1. Initialization

        public RoomScheduleForm(string roomName)
        {
            InitializeComponent();
            _targetRoom = roomName;

            // --- DOUBLE BUFFERING (Anti-Flicker) ---
            SetDoubleBuffered(this);

            // Buffer the header panel to stop image flickering
            if (this.Controls.ContainsKey("panel1"))
                SetDoubleBuffered(this.Controls["panel1"]);

            // Buffer the Grid for smooth scrolling
            if (this.Controls.ContainsKey("dgvRoomSchedule"))
                SetDoubleBuffered(this.Controls["dgvRoomSchedule"]);
            // ---------------------------------------

            // 1. Setup Labels
            lblRoomName.Text = $"ROOM: {_targetRoom.ToUpper()}";
            lblSemesterYear.Text = "1st Semester, A.Y. 2025-2026";

            // 2. Transparency Fix (Parenting labels to the header image)
            Control headerParent = this.Controls["panel1"];
            if (headerParent != null)
            {
                lblRoomName.Parent = headerParent;
                lblSemesterYear.Parent = headerParent;
            }
            else
            {
                lblRoomName.Parent = this;
                lblSemesterYear.Parent = this;
            }

            // 3. Grid & Data Setup
            SetupGrid();
            LoadRoomSchedule();

            this.Resize += new EventHandler(RoomScheduleForm_Resize);
        }

        #endregion

        #region 2. Grid Configuration

        private void SetupGrid()
        {
            // Clear previous structure
            dgvRoomSchedule.Columns.Clear();
            dgvRoomSchedule.Rows.Clear();

            // Define Columns
            dgvRoomSchedule.Columns.Add("Time", "TIME");
            string[] days = { "MONDAY", "TUESDAY", "WEDNESDAY", "THURSDAY", "FRIDAY", "SATURDAY", "SUNDAY" };
            foreach (var d in days) dgvRoomSchedule.Columns.Add(d, d);

            // Styling Configuration
            dgvRoomSchedule.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRoomSchedule.Columns["Time"].FillWeight = 60;
            dgvRoomSchedule.ScrollBars = ScrollBars.None;

            dgvRoomSchedule.DefaultCellStyle.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular);
            dgvRoomSchedule.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            dgvRoomSchedule.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRoomSchedule.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRoomSchedule.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dgvRoomSchedule.AllowUserToAddRows = false;
            dgvRoomSchedule.RowHeadersVisible = false;

            // Dynamic Layout Calculation
            int availableHeight = dgvRoomSchedule.Height - dgvRoomSchedule.ColumnHeadersHeight;
            int exactRowHeight = availableHeight / 11;
            if (exactRowHeight < 20) exactRowHeight = 20;

            dgvRoomSchedule.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvRoomSchedule.RowTemplate.Height = exactRowHeight;

            // Lock sorting
            foreach (DataGridViewColumn col in dgvRoomSchedule.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;

            // Generate Time Slots (7am - 6pm)
            for (int i = 7; i < 18; i++)
            {
                string timeLabel = ToSimple12Hour($"{i}:00 - {i + 1}:00");
                int rowIndex = dgvRoomSchedule.Rows.Add(timeLabel, "", "", "", "", "", "", "");
                dgvRoomSchedule.Rows[rowIndex].Height = exactRowHeight;
            }

            dgvRoomSchedule.BackgroundColor = Color.White;
        }

        #endregion

        #region 3. Data Loading Logic

        private void LoadRoomSchedule()
        {
            // Robust Search: Ignore casing and spaces
            var roomClasses = DataManager.MasterSchedule
                .Where(s => s.Room.Trim().ToUpper() == _targetRoom.Trim().ToUpper())
                .ToList();

            foreach (var item in roomClasses)
            {
                int startHour = int.Parse(item.Time.Split(':')[0]);
                int rowIndex = startHour - 7;
                int colIndex = GetDayColumnIndex(item.Day);

                if (rowIndex >= 0 && rowIndex < dgvRoomSchedule.Rows.Count && colIndex > 0)
                {
                    var cell = dgvRoomSchedule.Rows[rowIndex].Cells[colIndex];
                    cell.Value = $"{item.Subject}\n{item.Section}\n{item.Teacher}";

                    // Color Coding Logic
                    if (item.Subject.Contains("(Lab)"))
                        cell.Style.BackColor = Color.LightSalmon;
                    else
                        cell.Style.BackColor = GetSubjectColor(item.Subject);
                }
            }
            dgvRoomSchedule.ClearSelection();
        }

        #endregion

        #region 4. Window Events

        private void RoomScheduleForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized) return;
            SetupGrid();
            LoadRoomSchedule();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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

        #region 5. Helpers

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

        private Color GetSubjectColor(string subjectName)
        {
            string baseName = subjectName.Replace(" (Lec)", "").Replace(" (Lab)", "").Trim();
            int seed = baseName.GetHashCode();
            Random r = new Random(seed);
            return Color.FromArgb(r.Next(160, 255), r.Next(160, 255), r.Next(160, 255));
        }

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