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
        private VM_Macro _vmmacro;

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

        public MacroFilterControl(PF_MacroFilter macro)
        {
            InitializeComponent();

            this.macro = macro;
            
            DataTable macroEntryTable = macro.macroQueue;
            GridView gv = new GridView();
            foreach (DataColumn c in this.macro.macroQueue.Columns)
            {
                if (c.ColumnName != "Macro Entry")
                {
                    GridViewColumn gvColumn = new GridViewColumn();
                    gvColumn.DisplayMemberBinding = new Binding(c.ColumnName);
                    gvColumn.Header = c.ColumnName;
                    gv.Columns.Add(gvColumn);
                }
            }
            macroTable.View = gv;
            macroTable.DataContext = this.macro.macroQueue;
            Binding bind = new Binding();
            macroTable.SetBinding(ListView.ItemsSourceProperty, bind);
            updateSliders();
            this.DataContext = this.macro;
            macroTable.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(macroTable_MouseLeftButtonDown);
            macroTable.Drop += new DragEventHandler(macroTable_Drop);
        }



        #region drag'n'drop

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
                        macroTable.IsEnabled = false;
                        RangeSlider sliderOld = this.macro.rsl[oldIndex];
                        DataRow oldRow = this.macro.macroQueue.Rows[oldIndex];
                        double startOld = (double) oldRow["Start"];
                        double stopOld = (double) oldRow["Stop"];
                        string filterOld = (string) oldRow["Filter"];
                        string propertiesOld = (string) oldRow["Properties"];
                        MacroEntryFilter macroentryOld = (MacroEntryFilter) oldRow["Macro Entry"];
                        DataRow tempRow = this.macro.macroQueue.Rows[index];
                        double startTemp = (double)tempRow["Start"];
                        double stopTemp = (double)tempRow["Stop"];
                        string filterTemp = (string)tempRow["Filter"];
                        string propertiesTemp = (string)tempRow["Properties"];
                        MacroEntryFilter macroentryTemp = (MacroEntryFilter)tempRow["Macro Entry"];
                        RangeSlider sliderTemp = this.macro.rsl[index];
                        this.macro.macroQueue.Rows[index]["Start"] = startOld;
                        this.macro.macroQueue.Rows[index]["Stop"] = stopOld;
                        this.macro.macroQueue.Rows[index]["Filter"] = filterOld;
                        this.macro.macroQueue.Rows[index]["Properties"] = propertiesOld;
                        this.macro.macroQueue.Rows[index]["Macro Entry"] = macroentryOld;
                        this.macro.rsl[index] = sliderOld;
                        if (index < oldIndex)
                        {
                            for (int i = index + 1; i <= oldIndex; i++)
                            {
                                oldRow = this.macro.macroQueue.Rows[i];
                                sliderOld = this.macro.rsl[i];
                                startOld = (double)oldRow["Start"];
                                stopOld = (double)oldRow["Stop"];
                                filterOld = (string)oldRow["Filter"];
                                propertiesOld = (string)oldRow["Properties"];
                                macroentryOld = (MacroEntryFilter)oldRow["Macro Entry"];
                                this.macro.macroQueue.Rows[i]["Start"] = startTemp;
                                this.macro.macroQueue.Rows[i]["Stop"] = stopTemp;
                                this.macro.macroQueue.Rows[i]["Filter"] = filterTemp;
                                this.macro.macroQueue.Rows[i]["Properties"] = propertiesTemp;
                                this.macro.macroQueue.Rows[i]["Macro Entry"] = macroentryTemp;
                                this.macro.rsl[i] = sliderTemp;
                                startTemp = startOld;
                                stopTemp = stopOld;
                                filterTemp = filterOld;
                                propertiesTemp = propertiesOld;
                                macroentryTemp = macroentryOld;
                                sliderTemp = sliderOld;
                            }
                            List<RangeSelectionChangedEventHandler> tempList = new List<RangeSelectionChangedEventHandler>();
                            for (int j = index; j <= oldIndex; j++)
                            {
                                foreach (RangeSelectionChangedEventHandler ev in this.vmmacro.delList)
                                {
                                    this.macro.rsl[j].RangeSelectionChanged -= ev;
                                }
                                addDelegate(this.macro.rsl[j], j, tempList);
                            }
                            for (int j = index; j <= oldIndex; j++)
                            {
                                this.vmmacro.delList[j] = null;
                                this.vmmacro.delList[j] += tempList[j - index];
                            }
                            updateSliders();
                        }
                        else
                        {
                            for (int i = index - 1; i >= oldIndex; i--)
                            {
                                oldRow = this.macro.macroQueue.Rows[i];
                                sliderOld = this.macro.rsl[i];
                                startOld = (double)oldRow["Start"];
                                stopOld = (double)oldRow["Stop"];
                                filterOld = (string)oldRow["Filter"];
                                propertiesOld = (string)oldRow["Properties"];
                                macroentryOld = (MacroEntryFilter)oldRow["Macro Entry"];
                                this.macro.macroQueue.Rows[i]["Start"] = startTemp;
                                this.macro.macroQueue.Rows[i]["Stop"] = stopTemp;
                                this.macro.macroQueue.Rows[i]["Filter"] = filterTemp;
                                this.macro.macroQueue.Rows[i]["Properties"] = propertiesTemp;
                                this.macro.macroQueue.Rows[i]["Macro Entry"] = macroentryTemp;
                                this.macro.rsl[i] = sliderTemp;
                                startTemp = startOld;
                                stopTemp = stopOld;
                                filterTemp = filterOld;
                                propertiesTemp = propertiesOld;
                                macroentryTemp = macroentryOld;
                                sliderTemp = sliderOld;
                            }
                            List<RangeSelectionChangedEventHandler> tempList = new List<RangeSelectionChangedEventHandler>();
                            for (int j = oldIndex; j <= index; j++)
                            {
                                foreach (RangeSelectionChangedEventHandler ev in this.vmmacro.delList)
                                {
                                    this.macro.rsl[j].RangeSelectionChanged -= ev;
                                }
                                addDelegate(this.macro.rsl[j], j, tempList);
                            }
                            for (int j = oldIndex; j <= index; j++)
                            {
                                this.vmmacro.delList[j] = null;
                                this.vmmacro.delList[j] += tempList[index - j];
                            }
                            updateSliders();
                        }
                        macroTable.IsEnabled = true;
                    }
                }
            }
            if (oldIndex < 0)
            {
                //TODO: external drag and drop
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
                        // The item was dropped into a new location, so make it the new selected item.
                        this.macroTable.SelectedItem = selectedRow;
                    }

                    PluginManager.pluginManager.raiseEvent(EventType.macroEntrySelected,
                        new MementoEventArgs(selectedRow["Properties"].ToString(), selectedRow["Filter"].ToString()));
                }
            }
        }

        #endregion



        private void Save_Click(object sender, RoutedEventArgs e)
        {
            //this.macro.saveMacro("PF_MacroFilter", this.tbMacroName.Text);
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

        public void addDelegate(RangeSlider rs, int j, List<RangeSelectionChangedEventHandler> delList)
        {
            RangeSelectionChangedEventHandler del;
            del = delegate(object sender2, RangeSelectionChangedEventArgs e2)
            {
                MacroEntryFilter mfeTemp = new MacroEntryFilter();
                mfeTemp = (MacroEntryFilter)this.macro.macroQueue.Rows[j]["Macro Entry"];
                mfeTemp.startFrameRelative = ((double)e2.NewRangeStart / 500) * 100;
                mfeTemp.endFrameRelative = ((double)e2.NewRangeStop / 500) * 100;
                if (mfeTemp.startFrameRelative > 100) mfeTemp.startFrameRelative = 100; // slider values go out of range for some reason -> bugfix
                if (mfeTemp.endFrameRelative < 0) mfeTemp.endFrameRelative = 0; // slider values go out of range for some reason -> bugfix
                this.macro.macroQueue.Rows[j]["Macro Entry"] = mfeTemp;
                this.macro.macroQueue.Rows[j]["Start"] = mfeTemp.startFrameRelative;
                this.macro.macroQueue.Rows[j]["Stop"] = mfeTemp.endFrameRelative;
            };
            rs.RangeSelectionChanged += del;
            delList.Add(del);
        }
        
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            // Selecting multiple entries with ctrl doesn't work anymore ever since drag and drop was implemented. Shift requires double click
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
                    addDelegate(this.macro.rsl[i], i, tempList);
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
