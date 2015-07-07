using System;
using Xwt;

using SFEditor;

namespace SFEditor.Windows
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Settings.Init(Platform.Windows, ToolkitType.Wpf, new[] { ToolkitType.Gtk, ToolkitType.Wpf });
            Application.Initialize(Settings.GetToolkit());

            MainWindow window = new MainWindow(args);
            window.Show();

            Application.Run();
        }
    }
}
