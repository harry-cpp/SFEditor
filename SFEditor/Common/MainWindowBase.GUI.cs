using System;
using Xwt;

namespace SFEditor
{
    public partial class MainWindowBase
	{
        public SFWidget sfwidget1;
        public Menu menu1;
        public MenuItem menuitem_new, menuitem_open, menuitem_save, menuitem_saveas, menuitem_exit, menuitem_toolkit;

        private void Build()
        {
            this.Title = "Spritefont Editor";
            this.Width = 600;
            this.Height = 480;
            this.Icon = Xwt.Drawing.Image.FromResource("Resources.icon.png");

            menu1 = new Menu ();

            MenuItem fileMenuItem = new MenuItem ("File");
            Menu fileMenu = new Menu ();

            menuitem_new = new MenuItem ("New");
            fileMenu.Items.Add (menuitem_new);

            menuitem_open = new MenuItem ("Open");
            fileMenu.Items.Add (menuitem_open);

            fileMenu.Items.Add(new SeparatorMenuItem());

            menuitem_save = new MenuItem ("Save");
            fileMenu.Items.Add (menuitem_save);

            menuitem_saveas = new MenuItem ("Save As");
            fileMenu.Items.Add (menuitem_saveas);

            fileMenu.Items.Add(new SeparatorMenuItem());

            menuitem_exit = new MenuItem ("Exit");
            fileMenu.Items.Add (menuitem_exit);

            fileMenuItem.SubMenu = fileMenu;
            menu1.Items.Add (fileMenuItem);

            if (Settings.SuportedPlatformToolkits.Length > 1)
            {
                menuitem_toolkit = new MenuItem("Toolkit");
                var toolKitMenu = new Menu();

                foreach (var t in Settings.SuportedPlatformToolkits)
                {
                    var m = new RadioButtonMenuItem(t.ToString());
                    m.Checked = (t == Settings.GetToolkit());
                    m.Clicked += ToolKitClicked;
                    m.Tag = t;
                    toolKitMenu.Items.Add(m);
                }

                menuitem_toolkit.SubMenu = toolKitMenu;
                menu1.Items.Add(menuitem_toolkit);
            }

            this.MainMenu = menu1;

            sfwidget1 = new SFWidget();
            this.Content = sfwidget1;
            this.Padding = 0;

            this.menuitem_new.Clicked += NewClicked;
            this.menuitem_open.Clicked += OpenClicked;
            this.menuitem_save.Clicked += SaveClicked;
            this.menuitem_saveas.Clicked += (sender, e) => SaveAs();
            this.menuitem_exit.Clicked += (sender, e) => Application.Exit();
        }
	}
}

