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
using System.Xml;
using System.IO;
using System.Threading;


namespace Oqat.ViewModel.Macro
{
    public partial class MacroMetricControl : UserControl
    {

        public PM_MacroMetric macro
        {
            get;
            set;
        }

       

        public MacroMetricControl(PM_MacroMetric macro)
        {
            InitializeComponent();
            local("VM_Macro_" + Thread.CurrentThread.CurrentCulture + ".xml");
            this.macro = macro;
            this.DataContext = this.macro;

            this.macroTable.ItemsSource = this.macro.macroQueue;

            macroTable.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(macroTable_MouseLeftButtonDown);
            macroTable.Drop += new DragEventHandler(macroTable_Drop);
            local("VM_Macro_" + Thread.CurrentThread.CurrentCulture + ".xml");
        }

        //Drag'N'Drop start#
        private int oldIndex = -1;
        private delegate Point GetPositionDelegate(IInputElement element);

        /// <summary>
        /// Method to get current index using the mouse on macroQueue 
        /// </summary>
        /// <param name="getPosition">Position of mouse</param>
        /// <returns>index of macroQueue item</returns>
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

        /// <summary>
        /// Method to get a macroQueue item with index
        /// </summary>
        /// <param name="index">index of macroQueue item</param>
        /// <returns>Element the index has pointed</returns>
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

        /// <summary>
        /// Drop method for macroQueue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">index to drop into</param>
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

        /// <summary>
        /// Method to get item to Drag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Mouseposition</param>
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

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            deleteSelected();
        }

        /// <summary>
        /// All selected items in macroQueue will be deleted.
        /// </summary>
        private void deleteSelected()
        {
            // Selecting multiple entries with ctrl doesn't work anymore ever since drag and drop was implemented. Shift requires double click
            while (macroTable.SelectedIndex != -1)
            {
                int index = macroTable.SelectedIndex;
                this.macro.macroQueue.RemoveAt(index);
            }
        }
        #region eyeCancer
        /// <summary>
        /// method to read local xml file and put the language in the vm.
        /// </summary>
        private void local(String s)
        {
            try
            {
                String sFilename = Directory.GetCurrentDirectory() + "/" + s;
                XmlTextReader reader = new XmlTextReader(sFilename);
                reader.Read();
                reader.Read();
                String[] t = new String[8];
                String[] t2 = new String[8];
                for (int i = 0; i < 8; i++)
                {
                    reader.Read();
                    reader.Read();
                    t[i] = reader.Name;
                    reader.MoveToNextAttribute();
                    t2[i] = reader.Value;
                }
                hd5.Header = t2[7];
                hd2.Header = t2[1];
               
                tb1.Text = t2[4];
                deletebutton.Content = t2[5];


            }
            catch (IndexOutOfRangeException e) { }
            catch (FileNotFoundException e) { }
            catch (XmlException e) { }
        }
        #endregion
    }
}
