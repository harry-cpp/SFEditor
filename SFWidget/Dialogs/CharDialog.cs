using System;
using Xwt;
using Xwt.Drawing;

namespace SFEditor
{
    internal partial class CharDialog: Dialog
    {
        public CharDialog(Image icon, char start, char end)
        {
            Build(icon, start, end);

            char1.OnChanged += Char_OnChanged;
            char2.OnChanged += Char_OnChanged;

            ReloadText();
        }

        protected void Char_OnChanged (object sender, EventArgs e)
        {
            ReloadText();
        }

        private void ReloadText()
        {
            char start, end;

            if (char1.GetCurrentChar(out start) && char2.GetCurrentChar(out end))
            {
                if ((int)start < (int)end)
                {
                    string chars = "";

                    for (int i = Math.Max((int)start, 32); i < Math.Min((int)end + 1, 300); i++)
                        chars += ((char)i).ToString();

                    if (end > 300)
                        chars += "...";

                    rich1.LoadText(chars, Xwt.Formats.TextFormat.Plain);
                    buttonOk.Sensitive = true;
                    return;
                }
                else
                    rich1.LoadText("ERROR: Start character needs to be smaller than the end character", Xwt.Formats.TextFormat.Plain);
            }
            else
                rich1.LoadText("ERROR: Bad characters", Xwt.Formats.TextFormat.Plain);

            buttonOk.Sensitive = false;
        }
    }
}

