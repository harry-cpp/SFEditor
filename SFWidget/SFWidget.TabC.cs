using Xwt;

namespace SFEditor
{
    public partial class SFWidget
    {
        private void InitalizeC()
        {
            
        }

        private void ReloadC()
        {
            ReloadC(_core.GetText());
        }

        private void ReloadC(string text)
		{
            textEditor1.Document.Text = text;

            radioButton1.Sensitive = false;
            radioButton2.Sensitive = false;
            combo_font.Sensitive = false;
            entry_font.Sensitive = false;
            button_font.Sensitive = false;
        }

        private bool SaveC()
		{
            if (!_core.LoadFromText(textEditor1.Document.Text))
            {
                skip = true;
                notebook1.CurrentTabIndex = prevtab;

                MessageDialog.ShowError("Your xml is broken. Please fix it.");

                return false;
            }
            return true;
        }
    }
}

