using System;
using Xwt;

namespace SFEditor
{
    internal partial class NumericEntry : Widget
    {
        HBox hbox1;
        TextEntry entry1;
        ImageView image1;

        private void Build(string tooltipText)
        {
            hbox1 = new HBox();

            entry1 = new TextEntry();
            hbox1.PackStart(entry1, true);

            image1 = new ImageView();
            image1.Image = StockIcons.Warning;
            image1.TooltipText = tooltipText;
            hbox1.PackStart(image1);

            this.Content = hbox1;
        }
    }
}

