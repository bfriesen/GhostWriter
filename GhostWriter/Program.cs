namespace GhostWriter
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
			string filePath = null;

            if (args != null && args.Length == 1 && File.Exists(args[0]))
            {
                filePath = args[0];
            }
			
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm(filePath));
        }
    }
}