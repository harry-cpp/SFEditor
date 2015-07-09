using System;
using Xwt;

using SFEditor;

namespace SFEditor.Linux
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Settings.Init(Platform.Linux, ToolkitType.Gtk3, new[] { ToolkitType.Gtk, ToolkitType.Gtk3 });
            Application.Initialize(Settings.GetToolkit());

            var window = new MainWindow(args);
            window.Show();

            Application.Run();
        }
    }
}
