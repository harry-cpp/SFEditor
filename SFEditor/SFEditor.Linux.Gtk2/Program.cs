using System;
using Xwt;

namespace SFEditor
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Settings.Init(Platform.Linux, ToolkitType.Gtk, new[] { ToolkitType.Gtk });
            Application.Initialize(ToolkitType.Gtk);

            var window = new MainWindowBase(args);
            window.Show();

            Application.Run();
        }
    }
}
