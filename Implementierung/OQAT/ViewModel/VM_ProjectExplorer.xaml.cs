namespace Oqat.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
    using Microsoft.Win32;
    using Oqat.Model;
    using Oqat.PublicRessources.Model;
    using System.Windows.Controls;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using System.Windows;
    using System.Windows.Data;
    using System.IO;
    using System.Windows.Media;
    using System.Collections.Specialized;
    using System.Xml;

    /// <summary>
    /// This class is mainly responsible to sync the SmartTree(GUI) with the SmartNodes (Model) and
    /// raising the right events if user wants to delete/play a video.
    /// </summary>
	public partial class VM_ProjectExplorer : UserControl
	{

        private void local(String s)
        {
            try
            {
                String sFilename = Directory.GetCurrentDirectory() + "/" + s;
                XmlTextReader reader = new XmlTextReader(sFilename);
                reader.Read();
                reader.Read();
                String[] t = new String[3];
                String[] t2 = new String[3];
                for (int i = 0; i < 3; i++)
                {
                    reader.Read();
                    reader.Read();
                    t[i] = reader.Name;
                    reader.MoveToNextAttribute();
                    t2[i] = reader.Value;
                }
                lb1.Content = t2[0];
                lb2.Content = t2[1];
                btnExport.Content = t2[2];


            }
            catch (IndexOutOfRangeException e) { }
            catch (FileNotFoundException e) { }
            catch (XmlException e) { }
        }
        /// <summary>
        /// Reference to the currently active project.
        /// </summary>
		private Project project
		{
			get;
			set;
		}

        public VM_ProjectExplorer(Project project)
        {
            InitializeComponent();
            local("VM_ProjectExplorer_default.xml");
            PluginManager.macroProcessingFinished += this.onMacroProcessingFinished;

            // projectExplorer
            this.project = project;
            smartTreeExplorer.DataContext = project.smartTree;
        }


        /// <summary>
        /// If the user clicks a entry out of the SmartTree the <see cref="IVideoInfo"/> object contents (size, history..)
        /// of the corresponding Video (smartNode) are displayed in the lower part of the smartTree.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onVideoClick(object sender, VideoEventArgs e) 
        {

        }

        private void smartTreeExplorer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                e.Handled = true;
                SmartNode selNode = (SmartNode)smartTreeExplorer.SelectedItem;
                project.rmNode(selNode.id, false);
            }

        }

        private void smartTreeExplorer_Drop(object sender, DragEventArgs e)
        {
            e.Handled = true;
            if (e.Data is DataObject && ((DataObject)e.Data).ContainsFileDropList())
            {

                    // ask if user wants to open many windows...
                    StringCollection fileList  = ((DataObject)e.Data).GetFileDropList();
                    importVideos(fileList);
            }
            else if (e.Data is DataObject && ((DataObject)e.Data).GetDataPresent("SmartNode"))
            {
                SmartNode smartNode = ((DataObject)e.Data).GetData("SmartNode") as SmartNode;
               
                if (smartNode != null)
                {
                    project.rmNode(smartNode.id, false);
                    project.addNode(smartNode.video, 
                                (smartTreeExplorer.SelectedItem != null) ?
                                ((SmartNode)smartTreeExplorer.SelectedItem).id : -1);
                }
            }
        }

        /// <summary>
        /// Imports the videofiles into the smarttree by opening a vidImportOptionsDialog.
        /// </summary>
        /// <param name="fileList">the filenames of videos to import.</param>
        public void importVideos(StringCollection fileList)
        {
            VM_VidImportOptionsDialog vidImp = new VM_VidImportOptionsDialog(fileList);
            vidImp.Owner = Window.GetWindow(this);
            Nullable<bool> result = vidImp.ShowDialog();

            if ((result != null) & (bool)result)
            {
                foreach (var vid in vidImp.videoList)
                {
                    project.addNode(vid,
                        (smartTreeExplorer.SelectedItem != null) ?
                        ((SmartNode)smartTreeExplorer.SelectedItem).id : -1);
                }

            }
        }


        /// <summary>
        /// If a bubbling event occured it may be not on the element we
        /// need, therefore this method walks along the tree until
        /// a sought element (smartTree item) is found and returns it.
        /// </summary>
        /// <param name="element">Elemnt the event occured on.</param>
        /// <returns>The nearest father element of the given UIElement</returns>
        private T getNearestFather<T>(DependencyObject current)            
             where T : DependencyObject
        {
            // Walk up the element tree to the nearest tree view item.
            do
            {
                if( current is T )
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
                 }

        /// <summary>
        /// This method will be invoked if the user drags a object (i.e. file)
        /// over a smartTree element. If this happens the according element will be selected
        /// and expanded.
        /// </summary>
        /// <param name="sender">The Element on wich the drag event occured.</param>
        /// <param name="e">DragEventArgs</param>
        private void smartTreeExplorer_DragEnter(object sender, DragEventArgs e)
        {
                e.Handled = true;

                TreeViewItem smartItem = getNearestFather<TreeViewItem>(e.OriginalSource as DependencyObject);
                if (smartItem != null)
                {
                    smartItem.IsSelected = true;
                    smartItem.IsExpanded = true;
                }

        }

        private void miLoadAna_Click(object sender, RoutedEventArgs e)
        {
            //select the TreeViewItem that whose contextmenu was opened
            TextBlock tblock = ((TextBlock)((ContextMenu)((MenuItem)sender).Parent).PlacementTarget);
            getNearestFather<TreeViewItem>(tblock).IsSelected = true;

            SmartNode selNode = (SmartNode)smartTreeExplorer.SelectedItem;
            PluginManager.pluginManager.raiseEvent(PublicRessources.Plugin.EventType.videoLoad, 
                new VideoEventArgs(selNode.video));
        }
        private void miLoadRef_Click(object sender, RoutedEventArgs e)
        {
            //select the TreeViewItem that whose contextmenu was opened
            TextBlock tblock = ((TextBlock)((ContextMenu)((MenuItem)sender).Parent).PlacementTarget);
            getNearestFather<TreeViewItem>(tblock).IsSelected = true;

            SmartNode selNode = (SmartNode)smartTreeExplorer.SelectedItem;
            PluginManager.pluginManager.raiseEvent(PublicRessources.Plugin.EventType.videoLoad,
                new VideoEventArgs(selNode.video, true));
        }
        private void miLoadProc_Click(object sender, RoutedEventArgs e)
        {
            //select the TreeViewItem that whose contextmenu was opened
            TextBlock tblock = ((TextBlock)((ContextMenu)((MenuItem)sender).Parent).PlacementTarget);
            getNearestFather<TreeViewItem>(tblock).IsSelected = true;

            SmartNode selNode = (SmartNode)smartTreeExplorer.SelectedItem;
            PluginManager.pluginManager.raiseEvent(PublicRessources.Plugin.EventType.videoLoad,
                new VideoEventArgs(selNode.video, false));
        }


        private void onMacroProcessingFinished(object sender, VideoEventArgs e)
        {
            //TODO: find correct parentid
            project.addNode(e.video, -1);
        }

        private void treeitem_MouseDoubleClicked(object sender, RoutedEventArgs e)
        {
            if (this.smartTreeExplorer.SelectedItem == ((SmartNode)((TreeViewItem)e.Source).Header))
            {
                SmartNode selNode = (SmartNode)smartTreeExplorer.SelectedItem;
                PluginManager.pluginManager.raiseEvent(PublicRessources.Plugin.EventType.videoLoad,
                    new VideoEventArgs(selNode.video));
                if (selNode.video.isAnalysis == true)
                {
                    btnExport.Visibility = Visibility.Visible;
                }
                else
                {
                    btnExport.Visibility = Visibility.Hidden;
                }
            }


        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.DefaultExt = ".csv"; 
            dlg.Filter = "Comma Seperated Values (.csv)|*.csv"; 
            Nullable<bool> result = dlg.ShowDialog();

            SmartNode selNode = (SmartNode)smartTreeExplorer.SelectedItem;
            Video vid = selNode.video;

            if (result == true)
            {
                String s = "";


                for (int j = 0; j < vid.frameMetricValue.Length; j++)
                {
                    for (int i = 0; i < vid.frameMetricValue[i].Length; i++)
                    {
                        s = s + vid.frameMetricValue[j][i] + " ";
                    }
                    s = s + System.Environment.NewLine;
                }
                String p = dlg.FileName;
                StreamWriter myFile = new StreamWriter(p);
                myFile.Write(s);
                myFile.Close();
            }
    
        }

        private void smartTreeExplorer_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            e.Handled = true;

            // remove active element, if there was one
            readOnlyPropViewPanel.Children.Clear();
            
            var newSelSmartNode = (SmartNode)e.NewValue;
            if (newSelSmartNode != null)
                readOnlyPropViewPanel.Children.Add(newSelSmartNode.video.handler.readOnlyInfoView);

        }

        private void smartTreeExplorer_DragLeave(object sender, DragEventArgs e)
        {
            e.Handled = true;

            TreeViewItem smartItem = getNearestFather<TreeViewItem>(e.OriginalSource as DependencyObject);
            if (smartItem != null)
            {
                smartItem.IsSelected = false;
            }

        }

        private Point lastLeftBtnDownPos;
        private void smartTreeExplorer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            lastLeftBtnDownPos = e.GetPosition(null);
        }

        private void smartTreeExplorer_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point currMousePos = e.GetPosition(null);
            Vector diff = lastLeftBtnDownPos - currMousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                TreeView smartTree = sender as TreeView;
                TreeViewItem treeItem = getNearestFather<TreeViewItem>(e.OriginalSource as DependencyObject);
                if (treeItem != null)
                {
                    SmartNode smartNode = smartTree.ItemContainerGenerator.ItemFromContainer(treeItem) as SmartNode;
                    if (smartNode != null)
                    {
                        DataObject dragData = new DataObject("SmartNode", smartNode);
                        DragDrop.DoDragDrop(treeItem, dragData, DragDropEffects.Move);
                    }
                }
            }
        }
    }
}
