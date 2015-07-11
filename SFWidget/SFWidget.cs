using System;
using Xwt;
using System.IO;

namespace SFEditor
{
    public partial class SFWidget : Widget
    {
        Core _core;

        FileDialogFilter _ttfFileFilter;
        FileDialogFilter _anyFilesFilter;

        int prevtab = -1;
        bool skip = false;

        public SFWidget()
        {
            this.Build();

            _core = new Core();

            _ttfFileFilter = new FileDialogFilter("True Type Fonts (*.ttf)", "*.ttf");
            _anyFilesFilter = new FileDialogFilter("All Files (*.*)", "*.*");

            combo_font.SelectionChanged += Combo_font_SelectionChanged;
            radioButton1.ActiveChanged += RadioButtonChanged;
            radioButton2.ActiveChanged += RadioButtonChanged;
            button_font.Clicked += Browse_Clicked;
            notebook1.CurrentTabChanged += Notebook1_CurrentTabChanged;

            InitalizeA();
            InitalizeB();
            InitalizeC();
            InitalizeD();

            Reload();
            prevtab = notebook1.CurrentTabIndex;
        }

        public string FileName
        {
            get
            {
                return _core.FileName;
            }
        }

        private void SaveFont()
        {
            _core.LocalFont = radioButton2.Active;
            _core.Font = radioButton1.Active ? combo_font.SelectedText : entry_font.Text;
        }

        void Combo_font_SelectionChanged (object sender, EventArgs e)
        {
            combo_font.Font = Xwt.Drawing.Font.FromName(combo_font.SelectedText).WithSize(combo_font.Font.Size);
            SaveFont();
        }

        void Notebook1_CurrentTabChanged (object sender, EventArgs e)
        {
            if (!skip)
                SaveReload();
            else
                skip = false;
        }

        bool skipfont = false;

        void RadioButtonChanged (object sender, EventArgs e)
        {
            combo_font.Sensitive = radioButton1.Active;
            entry_font.Sensitive = radioButton2.Active;
            button_font.Sensitive = radioButton2.Active;

            if (skipfont)
                return;

            SaveFont();
        }

        void Browse_Clicked (object sender, EventArgs e)
        {
            var ofdialog = new OpenFileDialog();

            if (!string.IsNullOrEmpty(FileName))
                ofdialog.CurrentFolder = Path.GetDirectoryName(FileName);

            ofdialog.Multiselect = false;
            ofdialog.Filters.Add(_ttfFileFilter);
            ofdialog.Filters.Add(_anyFilesFilter);

            var result = ofdialog.Run (this.ParentWindow);
            if (result)
            {
                entry_font.Text = !string.IsNullOrEmpty(FileName) ? 
                    PathHelper.GetRelativePath(Path.GetDirectoryName(FileName), ofdialog.FileName) : ofdialog.FileName;
                SaveFont();
            }

            ofdialog.Dispose ();
        }

        public bool ProjectDirty()
        {
            //TODO Implemet maybe?
            return false;
        }

        public void New()
        {
            _core.SetDefaults();
            _core.FileName = null;

            Reload();
        }

        public void Open(string fileName)
        {
            if (_core.Open(fileName))
            {
                Reload();
                prevtab = notebook1.CurrentTabIndex;
            }
            else
            {
                prevtab = 2;
                skip = true;
                notebook1.CurrentTabIndex = 2;

                ReloadC(System.IO.File.ReadAllText(fileName));
            }
        }

        public SaveError Save()
        {
            if (!Save(notebook1.CurrentTabIndex))
                return SaveError.Nothing;

            return _core.Save();
        }

        public SaveError Save(string fileName)
        {
            _core.FileName = fileName;
            return Save();
        }

        private bool Save(int tab)
        {
            if (tab == 0)
                return SaveA();
            else if (tab == 1)
                return SaveB();
            else if (tab == 2)
                return SaveC();
            
            return true;
        }

        public void SaveReload()
        {
            if (!Save(prevtab))
                return;

            Reload();
            prevtab = notebook1.CurrentTabIndex;
        }

        public void Reload()
        {
            skipfont = true;

            if (notebook1.CurrentTabIndex != 2)
            {
                if (_core.LocalFont)
                {
                    radioButton2.Active = true;
                    entry_font.Text = _core.Font;
                }
                else
                {
                    radioButton1.Active = true;
                    combo_font.SelectedText = _core.Font;
                }

                radioButton1.Sensitive = true;
                radioButton2.Sensitive = true;

                combo_font.Sensitive = radioButton1.Active;
                entry_font.Sensitive = radioButton2.Active;
                button_font.Sensitive = radioButton2.Active;
            }

            if (notebook1.CurrentTabIndex == 0)
                ReloadA();
            else if (notebook1.CurrentTabIndex == 1)
                ReloadB();
            else if (notebook1.CurrentTabIndex == 2)
                ReloadC();
            else if (notebook1.CurrentTabIndex == 3)
                ReloadD();
            
            prevtab = -1;
            skipfont = false;
        }
    }
}

