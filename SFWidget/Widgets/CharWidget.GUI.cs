using System;
using Xwt;

namespace SFEditor
{
    internal partial class CharWidget
    {
        Table table1;
        TextEntry entry1, entry2;
        Label label1, label2;

        private void Build()
        {
            table1 = new Table();

            label1 = new Label("Number: ");
            table1.Add(label1, 0, 0);

            entry1 = new TextEntry();
            table1.Add(entry1, 1, 0);

            label2 = new Label("Char: ");
            table1.Add(label2, 0, 1);

            entry2 = new TextEntry();
            table1.Add(entry2, 1, 1);

            this.Content = table1;
        }
    }
}

