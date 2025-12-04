using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SchedCCS
{
    public partial class RoomScheduleForm : Form
    {
        // [Encapsulation] Use readonly to ensure this value is immutable after initialization.
        private readonly string _targetRoom;

        #region 1. Initialization

        public RoomScheduleForm(string roomName)
        {
            InitializeComponent();
            _targetRoom = roomName;

            // Set window title
            lblRoomName.Text = $"Schedule: {_targetRoom}";

            // Initialize UI and Data
            SetupGrid();
            LoadRoomSchedule();

            // Subscribe to events
            this.Resize += new EventHandler(RoomScheduleForm_Resize);
        }

        #endregion

        #region 2. UI Setup & Styling

        // Configures the DataGridView columns, rows, and visual styles.
        private void SetupGrid()
        {
            // Reset grid state
            dgvRoomSchedule.Columns.Clear();
            dgvRoomSchedule.Rows.Clear();

            // Define columns
            dgvRoomSchedule.Columns.Add("Time", "TIME");
            string[] days = { "MONDAY", "TUESDAY", "WEDNESDAY", "THURSDAY", "FRIDAY", "SATURDAY", "SUNDAY" };
            foreach (var d in days) dgvRoomSchedule.Columns.Add(d, d);

            // Apply visual styling
            dgvRoomSchedule.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRoomSchedule.Columns["Time"].FillWeight = 60;
            dgvRoomSchedule.ScrollBars = ScrollBars.None;

            dgvRoomSchedule.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 7.5F, FontStyle.Regular);
            dgvRoomSchedule.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Bold);
            dgvRoomSchedule.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRoomSchedule.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRoomSchedule.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            // Calculate dynamic row height to fill the screen
            int availableHeight = dgvRoomSchedule.Height - dgvRoomSchedule.ColumnHeadersHeight;
            int exactRowHeight = availableHeight / 11; // 11 Rows (7am - 6pm)

            // Prevent rows from becoming too small on small screens
            if (exactRowHeight < 20) exactRowHeight = 20;

            // Lock row height
            dgvRoomSchedule.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvRoomSchedule.RowTemplate.Height = exactRowHeight;

            // Disable user interaction for structure
            dgvRoomSchedule.AllowUserToAddRows = false;
            dgvRoomSchedule.RowHeadersVisible = false;

            // Disable column sorting
            foreach (DataGridViewColumn col in dgvRoomSchedule.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            // Populate Time Rows
            for (int i = 7; i < 18; i++)
            {
                string timeLabel = ToSimple12Hour($"{i}:00 - {i + 1}:00");
                int rowIndex = dgvRoomSchedule.Rows.Add(timeLabel, "", "", "", "", "", "", "");
                dgvRoomSchedule.Rows[rowIndex].Height = exactRowHeight;
            }
        }

        #endregion

        #region 3. Data Logic

        // Retrieves data from the DataManager and populates the grid cells.
        private void LoadRoomSchedule()
        {
            // [Abstraction] Accessing global data without exposing internal list implementation
            var roomClasses = DataManager.MasterSchedule
                .Where(x => x.Room == _targetRoom).ToList();

            foreach (var item in roomClasses)
            {
                // Parse time to find correct row index
                int startHour = int.Parse(item.Time.Split(':')[0]);
                int rowIndex = startHour - 7;

                // Map day string to column index
                int colIndex = 0;
                switch (item.Day)
                {
                    case "Mon": colIndex = 1; break;
                    case "Tue": colIndex = 2; break;
                    case "Wed": colIndex = 3; break;
                    case "Thu": colIndex = 4; break;
                    case "Fri": colIndex = 5; break;
                    case "Sat": colIndex = 6; break;
                    case "Sun": colIndex = 7; break;
                }

                // Populate cell if indices are valid
                if (rowIndex >= 0 && rowIndex < dgvRoomSchedule.Rows.Count)
                {
                    dgvRoomSchedule.Rows[rowIndex].Cells[colIndex].Value =
                        $"{item.Subject}\n{item.Section}\n{item.Teacher}";

                    // Apply Color Coding
                    if (item.Subject.Contains("(Lab)"))
                        dgvRoomSchedule.Rows[rowIndex].Cells[colIndex].Style.BackColor = Color.LightSalmon;
                    else
                        dgvRoomSchedule.Rows[rowIndex].Cells[colIndex].Style.BackColor = GetSubjectColor(item.Subject);
                }
            }
            dgvRoomSchedule.ClearSelection();
        }

        #endregion

        #region 4. Helper Methods

        // Converts 24-hour time string to 12-hour format.
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

        // Generates a consistent color based on the subject name hash.
        private Color GetSubjectColor(string subjectName)
        {
            string baseName = subjectName.Replace(" (Lec)", "").Replace(" (Lab)", "").Trim();
            int seed = baseName.GetHashCode();
            Random r = new Random(seed);

            // Use high values (160-255) for pastel colors
            int red = r.Next(160, 255);
            int green = r.Next(160, 255);
            int blue = r.Next(160, 255);
            return Color.FromArgb(red, green, blue);
        }

        #endregion

        #region 5. Event Handlers

        private void RoomScheduleForm_Resize(object sender, EventArgs e)
        {
            // Check if window is minimized to prevent calculation errors
            if (this.WindowState == FormWindowState.Minimized) return;

            SetupGrid();
            LoadRoomSchedule();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}