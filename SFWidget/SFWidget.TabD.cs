using Xwt;
using System.Drawing.Text;
using System;
using System.IO;

namespace SFEditor
{
    public partial class SFWidget
    {
        string font, fontloc;
        bool donce = false;

        private void InitalizeD()
        {
            button_preview.Clicked += Button_preview_Clicked;
        }

        void Button_preview_Clicked (object sender, System.EventArgs e)
        {
            if (radioButton2.Active)
            {
                try
                {
                    fontloc = string.IsNullOrEmpty(FileName) ?
                        entry_font.Text : Path.GetFullPath((new Uri(Path.Combine(Path.GetDirectoryName(FileName), entry_font.Text))).LocalPath);

                    PrivateFontCollection fontCol = new PrivateFontCollection();
                    fontCol.AddFontFile(fontloc);
                    font = fontCol.Families[0].Name;
                }
                catch
                {
                    MessageDialog.ShowError("Could not load the font :(");
                    return;
                }
            }
            else
                font = _core.Font;

            web1.LoadHtml(GenerateHTML(), "");
        }

        private string GenerateHTML()
        {
            string html = "<html><head><style>body {";

            html += "font-family: " + font + ";";

            if (radioButton2.Active)
                html += "src: url(" + fontloc + ");";

            if (_core.Bold)
                html += "font-weight: bold;";
            
            if (_core.Italic)
                html += "font-style: italic;";
            
            html += _core.Kerning ? "font-kerning: normal;" : "font-kerning: none;";

            html += "font-size: " + _core.Size + ";";

            html += "letter-spacing: " + _core.Spacing + ";";

            html += "background-color: " + color_back.Color.ToHexString().Substring(0, 7) + ";";

            html += "color: " + color_font.Color.ToHexString().Substring(0, 7) + ";";

            html += "}</style></head><body>";

            html += entry_text.Text;

            html += "</body></html>";

            return html;
        }

        private void ReloadD()
        {
            if (Toolkit.CurrentEngine.Type == ToolkitType.Gtk && !donce)
            {
                scrollView2.Content = web1;
                donce = true;
            }
        }
    }
}

