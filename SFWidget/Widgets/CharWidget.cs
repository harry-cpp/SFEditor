using System;
using Xwt;

namespace SFEditor
{
    internal partial class CharWidget : Widget
    {
        public event EventHandler<EventArgs> OnChanged;

        public bool GetCurrentChar(out char c)
        {
            if (entry2.Text == "Error")
            {
                c = new Char();
                return false;
            }
            else
            {
                c = (char)int.Parse(entry1.Text);
                return true;
            }
        }

        public CharWidget(char c)
        {
            Build();

            entry1.Text = ((int)c).ToString();
            entry2.Text = c.ToString();

            entry1.Changed += Entry1_Changed;
            entry2.KeyPressed += Entry2_KeyPressed;
        }

        protected void Entry1_Changed (object sender, EventArgs e)
        {
            if (!entry1.HasFocus)
                return;

            try
            {
                int num = int.Parse(entry1.Text);

                if(num < 32)
                    entry2.Text = "Error";
                else
                    entry2.Text = ((char)num).ToString();
            }
            catch {
                entry2.Text = "Error";
            }

            OnChange(new EventArgs());
        }

        protected void Entry2_KeyPressed (object sender, KeyEventArgs e)
        {
            if (!entry2.HasFocus)
                return;

            char c = (char)e.NativeKeyCode;
            entry2.Text = c.ToString();
            entry1.Text = ((int)c).ToString();
            e.Handled = true;

            OnChange(new EventArgs());
        }

        protected virtual void OnChange(EventArgs e)
        {
            EventHandler<EventArgs> handler = OnChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}

