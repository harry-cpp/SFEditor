using System;
using Xwt;

namespace SFEditor
{
    internal partial class NumericEntry
    {
        public string Text
        {
            get 
            {
                return entry1.Text;
            }
            set
            {
                entry1.Text = value;
                Refresh();
            }
        }

        public NumericEntry(string text, string tooltipText)
        {
            Build(tooltipText);

            this.entry1.Text = text;
            Refresh();

            entry1.Changed += Entry1_Changed;
        }

        protected void Entry1_Changed (object sender, EventArgs e)
        {
            Refresh();
        }

        private void Refresh()
        {
            try
            {
                float.Parse(entry1.Text);
                image1.Visible = false;
            }
            catch
            {
                image1.Visible = true;
            }
        }
    }
}

