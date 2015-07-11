using System;
using Xwt;

namespace SFEditor
{
    public partial class MainWindowBase: Window
    {
        FileDialogFilter _spritefontFileFilter;
        FileDialogFilter _anyFilesFilter;

        public MainWindowBase(string[] args)
        {
            this.Build();
            ReloadTitle();

            _spritefontFileFilter = new FileDialogFilter("Spritefont Files (*.spritefont)", "*.spritefont");
            _anyFilesFilter = new FileDialogFilter("All Files (*.*)", "*.*");

            MessageDialog.RootWindow = this;

            this.Closed += OnWindowClosed;

            if (args != null && args.Length > 0)
            {
                sfwidget1.Open(string.Join(" ", args));
                ReloadTitle();
            }
        }

        public virtual void ReloadTitle()
        {
            string title = "Spritefont Editor";

            if (!string.IsNullOrEmpty(sfwidget1.FileName))
                title += " - " + System.IO.Path.GetFileName(sfwidget1.FileName);

            this.Title = title;
        }

        protected void ToolKitClicked (object sender, EventArgs e)
        {
            RadioButtonMenuItem mi = (RadioButtonMenuItem)sender;

            if (!mi.Checked)
            {
                mi.Checked = true;
                return;
            }

            Command c = MessageDialog.AskQuestion("Switching toolkits may cause the app to not start, the app will close in order to switch to the new toolkit, are you sure you want to continue?", 
                "In case the app doesnt start please delete the file found in: " + Settings.SettingsFile, new[] { Command.Yes, Command.No, Command.Cancel }); 

            if (c == Command.Yes)
            {
                Settings.SetToolkit((ToolkitType)mi.Tag);
                Settings.Save();
                Application.Exit();
            }
            else
                mi.Checked = false;
        }

        public void SaveAs()
        {
            var sfdialog = new SaveFileDialog();
            sfdialog.Filters.Add(_spritefontFileFilter);
            sfdialog.Filters.Add(_anyFilesFilter);

            var result = sfdialog.Run(this);
            if (result)
            {
                var err = sfwidget1.Save((sfdialog.FileName.ToLower().EndsWith(".spritefont")) ? sfdialog.FileName : sfdialog.FileName + ".spritefont");

                if (err != SaveError.Nothing)
                    MessageDialog.ShowError("Unknown Error has Occured");
            }

            sfwidget1.Dispose();
            ReloadTitle();
        }

        public void NewClicked (object sender, EventArgs e)
        {
            sfwidget1.New();
            ReloadTitle();
        }

        public void OpenClicked (object sender, EventArgs e)
        {
            var ofdialog = new OpenFileDialog();
            ofdialog.Multiselect = false;
            ofdialog.Filters.Add(_spritefontFileFilter);
            ofdialog.Filters.Add(_anyFilesFilter);

            var result = ofdialog.Run (this);
            if (result)
                sfwidget1.Open(ofdialog.FileName);

            ofdialog.Dispose ();
            ReloadTitle();
        }

        public void SaveClicked (object sender, EventArgs e)
        {
            var err = sfwidget1.Save();

            if (err == SaveError.NoFileNameSpecified)
                SaveAs();
            else if (err != SaveError.Nothing)
                MessageDialog.ShowError("Unknown Error has Occured");
            ReloadTitle();
        }

        public void OnWindowClosed(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

