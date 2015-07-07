using System;
using Xwt;

namespace SFEditor
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            bool restart = true;

            while(restart)
            {
                Settings.Init();
                Application.Initialize(Settings.GetToolkit());

                MainWindow window = new MainWindow();
                window.Show();

                if (args != null && args.Length > 0)
                {
                    var projectFilePath = string.Join(" ", args);
                    window.Open(projectFilePath);
                }

                Application.Run();
                restart = false;
                window.Dispose();
            }
        }
    }
}

