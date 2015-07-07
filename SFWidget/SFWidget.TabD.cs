using Xwt;

namespace SFEditor
{
    public partial class SFWidget
    {
        bool donce = false;

        private void InitalizeD()
        {
            button_preview.Clicked += Button_preview_Clicked;
        }

        void Button_preview_Clicked (object sender, System.EventArgs e)
        {
            if (radioButton2.Active)
            {
                MessageDialog.ShowError("Sorry, Preview is only suported for system fonts :(");
                return;
            }

            web1.LoadHtml(GenerateHTML(), "");
        }

        private string GenerateHTML()
        {
            string html = "<html><head><style>body {";

            html += "font-family: " + _core.Font + ";";

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

