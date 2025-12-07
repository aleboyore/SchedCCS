using System;
using System.Windows.Forms;

namespace SchedCCS
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Initialize Windows Forms application settings
            ApplicationConfiguration.Initialize();

            // Initialize the in-memory database and seed data
            DataManager.Initialize();

            // Launch the application starting with the Login Form
            Application.Run(new LoginForm());
        }
    }
}