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
using System.Data;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;


namespace Oqat.ViewModel.Macro
{
    public partial class MacroMetricControl : UserControl
    {
        private PM_MacroMetric _macro;

        public PM_MacroMetric macro
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

        public MacroMetricControl(PM_MacroMetric macro, VM_Macro vmmacro)
        {
            this.macro = macro;
            this.vmmacro = vmmacro;
            InitializeComponent();

            macroTable.DataContext = this.macro.macroQueue;
            Binding bind = new Binding();
            macroTable.SetBinding(ListView.ItemsSourceProperty, bind);
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
            int index = this.GetCurrentIndex(e.GetPosition);
            //intern Drop
            if (oldIndex >= 0)
            {
                if (index >= 0)
                {
                    if (index != oldIndex)
                    {
                        //move Entry to drop position
                        MacroEntryMetric movedEntry = (MacroEntryMetric)this.macro.macroQueue[oldIndex];
                        macro.macroQueue.RemoveAt(oldIndex);
                        macro.macroQueue.Insert(index, movedEntry);
                    }
                }
            }
            //extern Drop
            if (oldIndex < 0)
            {
                //move Entry to drop position, from last (added with VM_Macro onEntrySelect())
                MacroEntryMetric movedEntry = (MacroEntryMetric)macro.macroQueue[macro.macroQueue.Count - 1];
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
                MacroEntryMetric selectedRow = (MacroEntryMetric)this.macro.macroQueue[oldIndex];
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
        //Drag'N'Drop end#

        public void scroll(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer1.ScrollToVerticalOffset(e.VerticalOffset);
        }
    }
}
