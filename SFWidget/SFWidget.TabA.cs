using System;
using Xwt;

namespace SFEditor
{
    public partial class SFWidget
    {
        private void InitalizeA()
        {
            check_defchar.Toggled += (sender, e) => entry_defchar.Sensitive = check_defchar.Active;
            entry_defchar.KeyPressed += Entry_defchar_KeyPressed;
        }

        protected void Entry_defchar_KeyPressed (object sender, KeyEventArgs e)
        {
            Console.WriteLine(e.NativeKeyCode);

            if (!entry_defchar.HasFocus)
                return;
            
            entry_defchar.Text = ((char)e.NativeKeyCode).ToString();
            e.Handled = true;
        }

        private void ReloadA()
        {
            check_bold.Active = _core.Bold;
            check_italic.Active = _core.Italic;
            check_kerning.Active = _core.Kerning;
            entry_size.Text = _core.Size.ToString();
            entry_spacing.Text = _core.Spacing.ToString();

            check_defchar.Active = _core.HasDefChar;
            entry_defchar.Sensitive = _core.HasDefChar;
			entry_defchar.Text = ((int)(_core.DefaultCharacter) == 0) ? "" : _core.DefaultCharacter.ToString();
        }

        private bool SaveA()
        {
            float tmpSize, tmpSpacing;

            if (check_defchar.Active && entry_defchar.Text == "")
            {
                skip = true;
                notebook1.CurrentTabIndex = prevtab;

                MessageDialog.ShowError("Default character cannot be empty.");
                return false;
            }

            try
            {
                tmpSize = float.Parse(entry_size.Text);
            }
            catch
            {
                skip = true;
                notebook1.CurrentTabIndex = prevtab;

                MessageDialog.ShowError("Size must be a number.");
                return false;
            }

            try
            {
                tmpSpacing = float.Parse(entry_spacing.Text);
            }
            catch
            {
                skip = true;
                notebook1.CurrentTabIndex = prevtab;

                MessageDialog.ShowError("Spacing must be a number.");
                return false;
            }

            _core.Bold = check_bold.Active;
            _core.Italic = check_italic.Active;
            _core.Kerning = check_kerning.Active;

            _core.Size = tmpSize;
            _core.Spacing = tmpSpacing;

            _core.HasDefChar = check_defchar.Active;
            if(_core.HasDefChar)
                _core.DefaultCharacter = entry_defchar.Text[0];

            return true;
        }
    }
}

