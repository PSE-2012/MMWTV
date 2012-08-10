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
using System.Collections.Specialized;
using System.ComponentModel;

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
            int index = this.GetCurrentIndex(e.GetPosition);
            if (oldIndex >= 0)
            {
                
                if (index >= 0)
                {
                    if (index != oldIndex)
                    {
                        macroTable.IsEnabled = false;
                        RangeSlider sliderOld = this.macro.rsl[oldIndex];

                        //move Entry to drop position
                        MacroEntryFilter movedEntry = this.macro.macroQueue[oldIndex];
                        macro.macroQueue.RemoveAt(oldIndex);
                        macro.macroQueue.Insert(index, movedEntry);
                        
                        RangeSlider sliderTemp = this.macro.rsl[index];
                        
                        this.macro.rsl[index] = sliderOld;
                        if (index < oldIndex)
                        {
                            for (int i = index + 1; i <= oldIndex; i++)
                            {
                                MacroEntryFilter oldRow = this.macro.macroQueue[i];
                                sliderOld = this.macro.rsl[i];
                                this.macro.rsl[i] = sliderTemp;
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
                                this.macro.rsl[i] = sliderTemp;
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
                //move Entry to drop position, from last (added with VM_Macro onEntrySelect())
                MacroEntryFilter movedEntry = macro.macroQueue[macro.macroQueue.Count - 1];
                macro.macroQueue.RemoveAt(macro.macroQueue.Count - 1);
                macro.macroQueue.Insert(index, movedEntry);
            }
        }

        private void macroTable_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            oldIndex = this.GetCurrentIndex(e.GetPosition);
            if (oldIndex >= 0)
            {
                macroTable.SelectedIndex = oldIndex;
                MacroEntryFilter selectedRow = this.macro.macroQueue[oldIndex];
                if (selectedRow != null)
                {
                    DragDropEffects allowedEffects = DragDropEffects.Move;

                    if (DragDrop.DoDragDrop(this.macroTable, selectedRow, allowedEffects) != DragDropEffects.None)
                    {
                        // The item was dropped into a new location, so make it the new selected item.
                        this.macroTable.SelectedItem = selectedRow;
                    }

                    /**PluginManager.pluginManager.raiseEvent(EventType.macroEntrySelected,
                        new MementoEventArgs(selectedRow["Properties"].ToString(), selectedRow["Filter"].ToString()));**/
                }
            }
        }

        #endregion



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
                MacroEntryFilter mfeTemp = macro.macroQueue[j];
                mfeTemp.startFrameRelative = ((double)e2.NewRangeStart / 500) * 100;
                mfeTemp.endFrameRelative = ((double)e2.NewRangeStop / 500) * 100;
                
                if (mfeTemp.startFrameRelative > 100) mfeTemp.startFrameRelative = 0; // slider values go out of range for some reason -> bugfix
                if (mfeTemp.endFrameRelative < 0) mfeTemp.endFrameRelative = 100; // slider values go out of range for some reason -> bugfix

                // TODO: BUGFIX

                this.macro.macroQueue[j] = mfeTemp;
                this.macro.macroQueue[j].startFrameRelative = mfeTemp.startFrameRelative;
                this.macro.macroQueue[j].endFrameRelative = mfeTemp.endFrameRelative;
            };
            rs.RangeSelectionChanged += del;
            delList.Add(del);
        }
        
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            deleteSelected();
        }

        private void deleteSelected()
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
                this.macro.macroQueue.RemoveAt(index);
            }
        }
    }
}
