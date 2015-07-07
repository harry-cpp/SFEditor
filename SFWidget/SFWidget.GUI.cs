using System.Collections.Generic;
using Xwt;
using XwtPlus.TextEditor;
using Xwt.Drawing;

namespace SFEditor
{
    public partial class SFWidget
    {
        Label label1, label2, label3, label4, label5, label6, label7, label8, label9;
        Table table1, table2;
        TextEntry entry_font, entry_defchar, entry_text;
        Button button_font, button_plus, button_minus, button_edit, button_up, button_down, button_preview;
        ListView listView1;
        HBox hbox1, hbox2, hbox3, hbox4, hbox5;
        VBox vbox1, vbox2, vbox3;
        RadioButton radioButton1, radioButton2;
        Notebook notebook1;
        ComboBox combo_font;
        CheckBox check_bold, check_italic, check_kerning, check_defchar;
        ScrollView scrollView2;
        TextEditor textEditor1;
        NumericEntry entry_size, entry_spacing;
        WebView web1;
        ColorPicker color_font, color_back;

        private void Build()
        {
            if(Toolkit.CurrentEngine.Type == ToolkitType.Wpf)
                this.BackgroundColor = (new Button()).BackgroundColor;

            vbox1 = new VBox();

            label1 = new Label("Select Font:");
            label1.MarginTop = 4;
            label1.MarginRight = 4;
            label1.MarginLeft = 4;
            vbox1.PackStart(label1);

            table1 = new Table();
            table1.MarginRight = 4;
            table1.MarginLeft = 4;

            radioButton1 = new RadioButton("From System: ");
            table1.Add(radioButton1, 0, 0);

            combo_font = new ComboBox();

            List<string> fonts = new List<string>();
            foreach (System.Drawing.FontFamily font in System.Drawing.FontFamily.Families)
                fonts.Add(font.Name);
            fonts.Sort();
            foreach (string font in fonts)
                combo_font.Items.Add(font);

            if(combo_font.Items.Contains("Arial"))
                combo_font.SelectedText = "Arial";
            else if(combo_font.Items.Count > 0)
                combo_font.SelectedIndex = 0;
            combo_font.Font = Xwt.Drawing.Font.FromName(combo_font.SelectedText);

            table1.Add(combo_font, 1, 0, 1, 1, true);

            radioButton2 = new RadioButton();
            radioButton2.Label = "From File: ";
            radioButton2.Sensitive = true;
            radioButton2.Group = radioButton1.Group;
            table1.Add(radioButton2, 0, 1);

            hbox1 = new HBox();

            entry_font = new TextEntry();
            entry_font.Sensitive = false;
            hbox1.PackStart(entry_font, true);

            button_font = new Button("Browse");
            button_font.Sensitive = false;
            hbox1.PackStart(button_font);

            table1.Add(hbox1, 1, 1);

            vbox1.PackStart(table1);

            notebook1 = new Notebook();
            notebook1.ExpandHorizontal = true;
            notebook1.ExpandVertical = true;

            table2 = new Table();
            table2.Margin = 4;

            label4 = new Label("Style:");
            table2.Add(label4, 0, 0);

            hbox2 = new HBox();

            check_bold = new CheckBox("Bold ");
            check_bold.BackgroundColor = Color.FromBytes(0, 0, 0, 0);
            hbox2.PackStart(check_bold);

            check_italic = new CheckBox("Italic ");
            check_italic.BackgroundColor = Color.FromBytes(0, 0, 0, 0);
            hbox2.PackStart(check_italic);

            check_kerning = new CheckBox("Kerning ");
            check_kerning.BackgroundColor = Color.FromBytes(0, 0, 0, 0);
            hbox2.PackStart(check_kerning);

            table2.Add(hbox2, 0, 1, 1, 1);

            label2 = new Label("Size:");
            table2.Add(label2, 0, 2);

            entry_size = new NumericEntry("0", "WARNING: Size needs to be a number");
            table2.Add(entry_size, 0, 3, 1, 1, true);

            label3 = new Label("Spacing:");
            table2.Add(label3, 0, 4);

            entry_spacing = new NumericEntry("0", "WARNING: Spacing needs to be a number");
            table2.Add(entry_spacing, 0, 5, 1, 1, true);

            check_defchar = new CheckBox("Default Character:");
            check_defchar.BackgroundColor = Color.FromBytes(0, 0, 0, 0);
            table2.Add(check_defchar, 0, 6);

            entry_defchar = new TextEntry();
            entry_defchar.Sensitive = false;
            entry_defchar.TextAlignment = Alignment.Center;
            table2.Add(entry_defchar, 0, 7, 1, 1, true);

            notebook1.Add(table2, "Global");

            hbox3 = new HBox();

            listView1 = new ListView();
            hbox3.PackStart(listView1, true);

            vbox2 = new VBox();
            vbox2.MarginRight = 5;
            vbox2.MarginTop = 5;

            label8 = new Label(" Main:");
            vbox2.PackStart(label8);

            button_plus = new Button("Add");
            vbox2.PackStart(button_plus);

            button_minus = new Button("Remove");
            button_minus.Sensitive = false;
            vbox2.PackStart(button_minus);

            button_edit = new Button("Edit");
            button_edit.Sensitive = false;
            vbox2.PackStart(button_edit);

            vbox2.PackStart(new HSeparator());

            label9 = new Label(" Move:");
            vbox2.PackStart(label9);

            button_up  = new Button("Up");
            button_up.Sensitive = false;
            vbox2.PackStart(button_up);

            button_down  = new Button("Down");
            button_down.Sensitive = false;
            vbox2.PackStart(button_down);

            hbox3.PackStart(vbox2);

            notebook1.Add(hbox3, "Characters");

            var pa = new VBox();

            textEditor1 = new TextEditor();
            textEditor1.Document.MimeType = "application/xml";

            pa.PackStart(textEditor1, true);
            notebook1.Add(pa, "Xml");

            vbox3 = new VBox();

            hbox4 = new HBox();
            hbox4.Margin = 5;

            label5 = new Label("Font Color: ");
            hbox4.PackStart(label5);

            color_font = new ColorPicker();
            color_font.Color = Color.FromBytes(0, 0, 0);
            color_font.SupportsAlpha = false;
            hbox4.PackStart(color_font);

            label6 = new Label("Background Color: ");
            hbox4.PackStart(label6);

            color_back = new ColorPicker();
            color_back.Color = Color.FromBytes(224, 224, 209);
            color_back.SupportsAlpha = false;
            hbox4.PackStart(color_back);

            vbox3.PackStart(hbox4);

            hbox5 = new HBox();
            hbox5.MarginLeft = 5;
            hbox5.MarginRight = 5;

            label7 = new Label("Text: ");
            hbox5.PackStart(label7);

            entry_text = new TextEntry();
            entry_text.Text = "The quick brown fox jumps over the lazy dog";
            hbox5.PackStart(entry_text, true);

            button_preview = new Button("Preview");
            hbox5.PackStart(button_preview);

			vbox3.PackStart(hbox5);

            web1 = new WebView();

            scrollView2 = new ScrollView();
            scrollView2.HorizontalScrollPolicy = ScrollPolicy.Automatic;
            scrollView2.VerticalScrollPolicy = ScrollPolicy.Automatic;

            if (Toolkit.CurrentEngine.Type != ToolkitType.Gtk)
                scrollView2.Content = web1;

            vbox3.PackStart(scrollView2, true);

            notebook1.Add(vbox3, "Preview");

            vbox1.PackStart(notebook1, true);

            this.Content = vbox1;
        }
    }
}

