using System.Diagnostics;
using Gtk;

using UI = Gtk.Builder.ObjectAttribute;

namespace SFEditor
{
    public class HeaderBarWindow : MainWindowBase
    {
        bool UseHeaderBar = Global.MajorVersion == 3 && Global.MinorVersion >= 9;

        HeaderBar headerbar1;

        [UI] Button new_button;
        [UI] Button open_button;
        [UI] Button save_button;
        [UI] Menu menu2;

        public HeaderBarWindow(string[] program_args) : base(program_args)
        {
            Process proc = new Process ();
            proc.StartInfo.FileName = "/bin/bash";
            proc.StartInfo.Arguments = "-c \"echo $XDG_CURRENT_DESKTOP\"";
            proc.StartInfo.UseShellExecute = false; 
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start ();

            while (!proc.StandardOutput.EndOfStream) {
                string line = proc.StandardOutput.ReadLine ();
                UseHeaderBar &= (line == "GNOME");
            }

            if (!UseHeaderBar)
                return;

            this.MainMenu = new Xwt.Menu();
            Xwt.GtkBackend.GtkEngine ge = new Xwt.GtkBackend.GtkEngine();
            var gtk_window = (Window)ge.GetNativeParentWindow(this.Content);

            Builder builder = new Builder(null, "HeaderBar.glade", null);
            headerbar1 = new HeaderBar(builder.GetObject("headerbar").Handle);
            builder.Autoconnect(this);

            gtk_window.Titlebar = headerbar1;
            headerbar1.ShowCloseButton = true;
            headerbar1.ShowAll();

            var saveas_menuitem = new MenuItem("Save As");
            saveas_menuitem.ButtonPressEvent += (o, args) => SaveAs(); 
            menu2.Add(saveas_menuitem);
            menu2.ShowAll();

            new_button.Clicked += NewClicked;
            open_button.Clicked += OpenClicked;
            save_button.Clicked += SaveClicked;

            ReloadTitle();
        }

        public override void ReloadTitle()
        {
            if (!UseHeaderBar)
            {
                base.ReloadTitle();
                return;
            }

            if (headerbar1 == null)
                return;

            if (string.IsNullOrEmpty(sfwidget1.FileName))
            {
                this.Title = "Spritefont Editor";
                headerbar1.Subtitle = "";
            }
            else
            {
                this.Title = System.IO.Path.GetFileName(sfwidget1.FileName);
                headerbar1.Subtitle = System.IO.Path.GetDirectoryName(sfwidget1.FileName);
            }
        }
    }
}

