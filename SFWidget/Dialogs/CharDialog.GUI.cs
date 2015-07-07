using System;
using Xwt;
using Xwt.Drawing;

namespace SFEditor
{
    internal partial class CharDialog
    {
        public CharWidget char1, char2;

        VBox vbox1, vbox2, vbox3;
        HBox hbox1;
        DialogButton buttonOk, buttonCancel;
        ScrollView scroll1;
        RichTextView rich1;
        Label label1, label2, label3;

        private void Build(Image icon, char start, char end)
        {
            if (icon != null)
                this.Icon = icon;

            this.Title = "Character Region Selection Dialog";
            this.Padding = 0;
            this.Width = 400;
            this.Height = 300;

            vbox1 = new VBox();

            if(Toolkit.CurrentEngine.Type == ToolkitType.Wpf)
                vbox1.BackgroundColor = (new Button()).BackgroundColor;

            hbox1 = new HBox();
            hbox1.MarginTop = 6;
            hbox1.MarginRight = 6;
            hbox1.MarginLeft = 6;
            hbox1.Spacing = 8;

            vbox2 = new VBox();

            label1 = new Label("Start Character:");
            vbox2.PackStart(label1);

            char1 = new CharWidget(start);
            vbox2.PackStart(char1);

            hbox1.PackStart(vbox2, true);

            hbox1.PackStart(new VSeparator());

            vbox3 = new VBox();

            label2 = new Label("End Character:");
            vbox3.PackStart(label2);

            char2 = new CharWidget(end);
            vbox3.PackStart(char2, true);

            hbox1.PackStart(vbox3, true);

            vbox1.PackStart(hbox1);

            label3 = new Label("Preview of characters to include (Max 300):");
            label3.MarginLeft = 4;
            vbox1.PackStart(label3);

            rich1 = new RichTextView();

            scroll1 = new ScrollView();
            scroll1.Content = rich1;

            vbox1.PackStart(scroll1, true);

            this.Content = vbox1;
            rich1.SetFocus();

            buttonOk = new DialogButton(Command.Ok);
            this.Buttons.Add(buttonOk);

            buttonCancel = new DialogButton(Command.Cancel);
            this.Buttons.Add(buttonCancel);
        }
    }
}

