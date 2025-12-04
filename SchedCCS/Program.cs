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
            ApplicationConfiguration.Initialize();

            // START THE DATABASE
            DataManager.Initialize();

            // Launch Login Screen
            Application.Run(new LoginForm());
        }
    }
}