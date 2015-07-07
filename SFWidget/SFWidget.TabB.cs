using System;
using System.Collections.Generic;
using Xwt;
using Xwt.Drawing;

namespace SFEditor
{
    /*[Serializable]
    class DragData
    {
        public string Data;

        public DragData(string data)
        {
            this.Data = data;
        }
    }*/

    public partial class SFWidget
    {
        Menu listMenu;
        MenuItem editMenuItem, addMenuItem, removeMenuItem, upMenuItem, downMenuItem;
        ListStore listStore;

        DataField<string> startCol = new DataField<string>();
        DataField<string> endCol = new DataField<string>();

        private void InitalizeB()
        {
            listMenu = new Menu();

            editMenuItem = new MenuItem("Edit");

            editMenuItem.Clicked += Button_edit_Clicked;
            listMenu.Items.Add(editMenuItem);

            addMenuItem = new MenuItem("Add");
            addMenuItem.Clicked += Button_plus_Clicked;
            listMenu.Items.Add(addMenuItem);

            removeMenuItem = new MenuItem("Remove");
            removeMenuItem.Clicked += Button_minus_Clicked;
            listMenu.Items.Add(removeMenuItem);

            upMenuItem = new MenuItem("Move Up");
            upMenuItem.Clicked += Button_up_Clicked;
            listMenu.Items.Add(upMenuItem);

            downMenuItem = new MenuItem("Move Down");
            downMenuItem.Clicked += Button_down_Clicked;
            listMenu.Items.Add(downMenuItem);

			listStore = new ListStore(startCol, endCol);
			listView1.DataSource = listStore;
			listView1.Columns.Add("Start           ", startCol).CanResize = true;
			listView1.Columns.Add("End", endCol).CanResize = true;
            listView1.SelectionMode = SelectionMode.Single;

            listView1.ButtonPressed += ListView1_ButtonPressed;
            listView1.ButtonReleased += ListView1_ButtonReleased;
            listView1.SelectionChanged += ListView1_SelectionChanged;
            listView1.RowActivated += ListView1_RowActivated;
            button_plus.Clicked += Button_plus_Clicked;
            button_minus.Clicked += Button_minus_Clicked;
            button_edit.Clicked += Button_edit_Clicked;
            button_up.Clicked += Button_up_Clicked;
            button_down.Clicked += Button_down_Clicked;

            /*listView1.SetDragSource (TransferDataType.FromType(typeof(DragData)));
            listView1.SetDragDropTarget (DragDropAction.All ,TransferDataType.FromType(typeof(DragData)));
            listView1.DragStarted += ListView1_DragStarted;
            listView1.DragOver += ListView1_DragOver;*/
        }

        /*void ListView1_DragStarted (object sender, DragStartedEventArgs args)
        {
            var dragOper = args.DragOperation;
            var d = new DragData("yrdy");
            dragOper.Data.AddValue(d);

            var img = Image.FromResource("Resources.dragrec.png");
            dragOper.SetDragImage(img, 0, 0);

            dragOper.AllowedActions = DragDropAction.All;
        }

        void ListView1_DragOver (object sender, DragOverEventArgs e)
        {
            try
            {
                var o = (DragData)e.Data.GetValue(TransferDataType.FromType(typeof(DragData)));
                e.AllowedAction = e.Action;
            }
            catch
            {
            }
        }*/

        void Button_up_Clicked (object sender, EventArgs e)
        {
            MoveRow(listView1.SelectedRow, listView1.SelectedRow - 1);
            listView1.SelectRow(listView1.SelectedRow - 1);
        }

        void Button_down_Clicked (object sender, EventArgs e)
        {
            MoveRow(listView1.SelectedRow, listView1.SelectedRow + 1);
            listView1.SelectRow(listView1.SelectedRow + 1);
        }

        private void MoveRow(int rowa, int rowb)
        {
            string start = listStore.GetValue(rowa, startCol);
            string end = listStore.GetValue(rowa, endCol);

            listStore.SetValue(rowa, startCol, listStore.GetValue(rowb, startCol));
            listStore.SetValue(rowa, endCol, listStore.GetValue(rowb, endCol));

            listStore.SetValue(rowb, startCol, start);
            listStore.SetValue(rowb, endCol, end);
        }

        private void EditRow(int row)
        {
            int s = GetListValue(listStore.GetValue(row, startCol));
            int e = GetListValue(listStore.GetValue(row, endCol));

            var dialog = new CharDialog(this.ParentWindow.Icon, (char)s, (char)e);
            dialog.TransientFor = this.ParentWindow;

            if (dialog.Run() == Command.Ok)
            {
                char start, end;

                dialog.char1.GetCurrentChar(out start);
                dialog.char2.GetCurrentChar(out end);
                SetListValues(row, start, end);
            }

            dialog.Dispose();
        }

        private int GetListValue(string text)
        {
            string[] split = text.Split('(', ')');
            return int.Parse(split[split.Length - 2]);
        }

        private string CharToString(char c)
        {
            if ((int)c < 32)
                return "SPECIAL";

            return c.ToString();
        }

        private void SetListValues(int row, char start, char end)
        {
            listStore.SetValue(row, startCol, "\"" + CharToString(start) + "\"" + " (" + (int)start + ")");
            listStore.SetValue(row, endCol, "\"" + CharToString(end) + "\"" + " (" + (int)end + ")");
        }

        void ListView1_ButtonPressed (object sender, ButtonEventArgs e)
        {
            if (Toolkit.CurrentEngine.Type != ToolkitType.Wpf)
            {
                int columnHeight = (int)listView1.GetCellBounds(0, listView1.Columns[0].Views[0], false).Height;

                int row = listView1.GetRowAtPosition(e.X, e.Y - columnHeight);
                if (row == -1)
                    listView1.UnselectAll();
            }
        }

        void ListView1_ButtonReleased (object sender, ButtonEventArgs e)
        {
            if (e.Button == PointerButton.Right)
            {
                editMenuItem.Visible = button_edit.Sensitive;
                addMenuItem.Visible = button_plus.Sensitive;
                removeMenuItem.Visible = button_minus.Sensitive;
                upMenuItem.Visible = button_up.Sensitive;
                downMenuItem.Visible = button_down.Sensitive;

                listMenu.Popup();
            }
        }

        private void RefreshButtons()
        {
            button_minus.Sensitive = listView1.SelectedRows.Length > 0 ? true : false;
            button_edit.Sensitive = button_minus.Sensitive;
            button_up.Sensitive = button_minus.Sensitive && listView1.SelectedRow != 0;
            button_down.Sensitive = button_minus.Sensitive && listView1.SelectedRow != listStore.RowCount - 1;
        }

        protected void ListView1_SelectionChanged (object sender, EventArgs e)
        {
            if (Toolkit.CurrentEngine.Type == ToolkitType.Wpf)
                skip = true;
            RefreshButtons();
        }

        protected void ListView1_RowActivated (object sender, ListViewRowEventArgs e)
        {
            EditRow(e.RowIndex);
        }

        void Button_edit_Clicked (object sender, EventArgs e)
        {
            EditRow(listView1.SelectedRow);
        }

        protected void Button_plus_Clicked (object sender, EventArgs args)
        {
            var dialog = new CharDialog(this.ParentWindow.Icon, (char)32, (char)126);
            dialog.TransientFor = this.ParentWindow;

            if (dialog.Run() == Command.Ok)
            {
                char start, end;

                dialog.char1.GetCurrentChar(out start);
                dialog.char2.GetCurrentChar(out end);
                SetListValues(listStore.AddRow(), start, end);
                RefreshButtons();
            }

            dialog.Dispose();
        }

        protected void Button_minus_Clicked (object sender, EventArgs e)
        {
            if (listView1.SelectedRows.Length == 0)
                return;

            listStore.RemoveRow(listView1.SelectedRow);
        }

        private bool ReloadB()
		{
			listStore.Clear();

            foreach (CharacterRegion reg in _core.CharacterRegions)
                SetListValues(listStore.AddRow(), (char)reg.Start, (char)reg.End);

            RefreshButtons();

            return true;
        }

        private bool SaveB()
        {
            var chars = new List<CharacterRegion>();

            for (int i = 0; i < listStore.RowCount; i++)
            {
                int start = GetListValue(listStore.GetValue(i, startCol));
                int end = GetListValue(listStore.GetValue(i, endCol));

                chars.Add(new CharacterRegion(start, end));
            }

            if (!CharRegionsUtil.CheckConflict(chars))
            {
                int tmptab = notebook1.CurrentTabIndex;

                skip = true;
                notebook1.CurrentTabIndex = 1;

                var c = MessageDialog.AskQuestion("Character Region Conflict Detected", "How would you like to resolve it?", new[]
                    {
                        new Command(Command.Yes.Id, "Auto-Resolve"),
                        new Command(Command.Cancel.Id, "Manually Resolve"),
                        new Command(Command.No.Id, "Don't Resolve")
                    });

                skip = false;
                if (c.ToString() == Command.Yes.ToString())
                    chars = CharRegionsUtil.ResolveConflicts(chars);
                else if (c.ToString() == Command.No.ToString())
                {
                    skip = true;
                    notebook1.CurrentTabIndex = tmptab;
                    Reload();
                }
                else
                    return false;
            }

            _core.CharacterRegions = chars;
            return true;
        }
    }
}

