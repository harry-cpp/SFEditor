using System;
using Xwt;

using SFEditor;

namespace SFEditor.GNOME
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Settings.Init(Platform.Linux, ToolkitType.Gtk3, new[] { ToolkitType.Gtk3 });
            Application.Initialize(ToolkitType.Gtk3);

            var window = new HeaderBarWindow(args);
            window.Show();

            Application.Run();
        }
    }
}

