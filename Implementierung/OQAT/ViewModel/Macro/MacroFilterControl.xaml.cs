using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Oqat.PublicRessources.Plugin;
using Oqat.PublicRessources.Model;
using System.Windows.Controls.Primitives;
using System.Data;
using AC.AvalonControlsLibrary.Controls;
using System.Collections.ObjectModel;


namespace Oqat.ViewModel.Macro
{
    public partial class MacroFilterControl : UserControl
    {
        private PF_MacroFilter _macro;

        public PF_MacroFilter macro
        {
            get
            {
                return this._macro;
            }

            set
            {
                _macro = value;
            }
        }

        private VM_Macro _vmmacro;

        public VM_Macro vmmacro
        {
            get
            {
                return this._vmmacro;
            }
            set
            {
                _vmmacro = value;
            }
        }

        // The user may drag the mouse over the Macro Table itself instead of using a scrollbar, in which case ScrollViewer2 needs to be synchronised.
        // Also when a table entry gets deleted, ScrollViewer1 is automatically reset and ScrollViewer2 requires resynchronisation.
        public void scroll(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer2.ScrollToVerticalOffset(e.VerticalOffset);
        }

        public void scroll2(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer1.ScrollToVerticalOffset(e.VerticalOffset);
        }

        public MacroFilterControl(PF_MacroFilter macro, VM_Macro vmmacro)
        {
            this.macro = macro;
            this.vmmacro = vmmacro;
            InitializeComponent();
            DataTable macroEntryTable = macro.macroQueue;
            GridView gv = new GridView();
            foreach (DataColumn c in this.macro.macroQueue.Columns)
            {
                if (c.ColumnName != "Macro Entry")
                {
                    GridViewColumn gvColumn = new GridViewColumn();
                    if (c.ColumnName == "Start" || c.ColumnName == "Stop")
                    {
                        gvColumn.Width = 50;
                    }
                    else
                    {
                        gvColumn.Width = 150;
                    }
                    // TODO: disable column resizing!?
                    gvColumn.DisplayMemberBinding = new Binding(c.ColumnName);
                    gvColumn.Header = c.ColumnName;
                    gv.Columns.Add(gvColumn);
                }
            }
            macroTable.View = gv;
            macroTable.DataContext = this.macro.macroQueue;
            Binding bind = new Binding();
            macroTable.SetBinding(ListView.ItemsSourceProperty, bind);
            GridView gvs = new GridView();
            GridViewColumn gvsColumn = new GridViewColumn();
            gvsColumn.Header = "Frames Relative";
            gvs.Columns.Add(gvsColumn);
            rangeSliders.View = gvs;
            rangeSliders.DataContext = this.macro.rsl;
            Binding bind2 = new Binding();
            rangeSliders.SetBinding(ListView.ItemsSourceProperty, bind2);
            this.DataContext = this.macro;
            macroTable.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(macroTable_MouseLeftButtonDown);
            macroTable.Drop += new DragEventHandler(macroTable_Drop);
        }

        //Drag'N'Drop start#
        private int oldIndex = -1;
        private delegate Point GetPositionDelegate(IInputElement element);

        private int GetCurrentIndex(GetPositionDelegate getPosition)
        {
            int index = -1;
            int i = -1;
            while (i < this.macroTable.Items.Count && index == -1)
            {
                ++i;
                ListViewItem item = GetListViewItem(i);
                if (this.IsMouseOverTarget(item, getPosition))
                {
                    index = i;
                }
            }
            return index;
        }

        private ListViewItem GetListViewItem(int index)
        {
            if (macroTable.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                return null;

            return macroTable.ItemContainerGenerator.ContainerFromIndex(index) as ListViewItem;
        }

        private bool IsMouseOverTarget(Visual target, GetPositionDelegate getPosition)
        {
            if (target != null)
            {
                Rect bounds = VisualTreeHelper.GetDescendantBounds(target);
                Point mousePos = getPosition((IInputElement)target);
                return bounds.Contains(mousePos);
            }
            else
            {
                return false;
            }

        }

        private void macroTable_Drop(object sender, DragEventArgs e)
        {
            if (oldIndex >= 0)
            {
                int index = this.GetCurrentIndex(e.GetPosition);

                if (index >= 0)
                {
                    if (index != oldIndex)
                    {
                        // TODO neues object bei index einfügen und altes bei oldindex löschen
                        this.macro.macroQueue.Rows.RemoveAt(index);
                    }
                }
            }
            if (oldIndex < 0)
            {
                //todo: extern drag and drop
            }
        }

        private void macroTable_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            oldIndex = this.GetCurrentIndex(e.GetPosition);

            if (oldIndex >= 0)
            {
                macroTable.SelectedIndex = oldIndex;
                DataRow selectedRow = this.macro.macroQueue.Rows[oldIndex];
                

                if (selectedRow != null)
                {
                    DragDropEffects allowedEffects = DragDropEffects.Move;

                    if (DragDrop.DoDragDrop(this.macroTable, selectedRow, allowedEffects) != DragDropEffects.None)
                    {
                        // The item was dropped into a new location,
                        // so make it the new selected item.

                        this.macroTable.SelectedItem = selectedRow;
                    }

                    PluginManager.pluginManager.raiseEvent(EventType.macroEntrySelected,
                        new MementoEventArgs(selectedRow["Memento Name"].ToString(), selectedRow["Plugin Name"].ToString()));
                }
            }
        }

        //Drag'N'Drop end#


        private void Save_Click(object sender, RoutedEventArgs e)
        {
            MacroSaveDialog msd = new MacroSaveDialog();
            msd.vmmacro = vmmacro;
            msd.Visibility = System.Windows.Visibility.Visible;
        }

        public void updateSliders()
        {
            GridView gvs = new GridView(); // since we can't figure out a way to set the DisplayMemberBinding of the GridViewColumn
            GridViewColumn gvsColumn = new GridViewColumn(); // in a way that the slider is visible, we rebuild the GridViewColumn after we add/delete an entry
            gvsColumn.Header = "Frames Relative";
            gvs.Columns.Add(gvsColumn);
            rangeSliders.View = gvs;
            rangeSliders.DataContext = this.macro.rsl;
            Binding bind = new Binding();
            rangeSliders.SetBinding(ListView.ItemsSourceProperty, bind);
        }
        
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Selecting multiple entries with drag and drop doesn't work anymore ever since drag and drop was implemented
            while (macroTable.SelectedIndex != -1)
            {
                int index = macroTable.SelectedIndex;
                this.macro.rsl.RemoveAt(index);
                List<RangeSelectionChangedEventHandler> tempList = new List<RangeSelectionChangedEventHandler>();
                for (int i = index; i < this.macro.rsl.Count; i++)
                {
                    foreach (RangeSelectionChangedEventHandler ev in this.vmmacro.delList)
                    {
                        this.macro.rsl[i].RangeSelectionChanged -= ev;
                    }
                    // this.macro.rsl[i].RangeSelectionChanged -= this.vmmacro.delList[i]; I have no idea why this doesn't work
                    RangeSelectionChangedEventHandler del;
                    int j = i;
                    del = delegate(object sender2, RangeSelectionChangedEventArgs e2)
                    {
                        MacroEntryFilter mfeTemp = new MacroEntryFilter();
                        mfeTemp = (MacroEntryFilter)this.macro.macroQueue.Rows[j]["Macro Entry"];
                        mfeTemp.startFrameRelative = ((double)e2.NewRangeStart / 500) * 100;
                        mfeTemp.endFrameRelative = ((double)e2.NewRangeStop / 500) * 100;
                        if (mfeTemp.startFrameRelative > 100) mfeTemp.startFrameRelative = 100; // bugfix rangeslider
                        if (mfeTemp.endFrameRelative < 0) mfeTemp.endFrameRelative = 0; // bugfix rangeslider
                        this.macro.macroQueue.Rows[j]["Macro Entry"] = mfeTemp;
                        this.macro.macroQueue.Rows[j]["Start"] = mfeTemp.startFrameRelative;
                        this.macro.macroQueue.Rows[j]["Stop"] = mfeTemp.endFrameRelative;
                    };
                    this.macro.rsl[i].RangeSelectionChanged += del;
                    tempList.Add(del);
                }

                for (int i = index; i < this.macro.rsl.Count; i++)
                {
                    this.vmmacro.delList[i] = null;
                    this.vmmacro.delList[i] += tempList[i - index];
                }
                updateSliders();
                this.macro.macroQueue.Rows.RemoveAt(index);
            }           
        }
    }
}
