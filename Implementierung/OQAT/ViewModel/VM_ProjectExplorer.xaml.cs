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
    using Microsoft.WindowsAPICodePack.Controls.WindowsPresentationFoundation;
    using System.Windows.Data;
    using Microsoft.WindowsAPICodePack.Shell;
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
                    VM_VidImportOptionsDialog vidImp = new VM_VidImportOptionsDialog(fileList);
                    vidImp.Owner = Window.GetWindow(this);
                   Nullable<bool> result = vidImp.ShowDialog();

                    if ((result!=null) & (bool)result)
                    {
                        foreach (var vid in vidImp.videoList)
                        {
                            project.addNode(vid,
                                (smartTreeExplorer.SelectedItem!= null)?
                                ((SmartNode)smartTreeExplorer.SelectedItem).id: -1);
                        }
                        
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
        private TreeViewItem getNearestFather(UIElement element)
        {
            // Walk up the element tree to the nearest tree view item.
            TreeViewItem container = element as TreeViewItem;
            while ((container == null) && (element != null))
            {
                element = VisualTreeHelper.GetParent(element) as UIElement;
                container = element as TreeViewItem;
            }
            return container;
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

                TreeViewItem smartItem = getNearestFather(e.OriginalSource as UIElement);
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
            getNearestFather(tblock).IsSelected = true;

            SmartNode selNode = (SmartNode)smartTreeExplorer.SelectedItem;
            PluginManager.pluginManager.raiseEvent(PublicRessources.Plugin.EventType.videoLoad, 
                new VideoEventArgs(selNode.video));
        }
        private void miLoadRef_Click(object sender, RoutedEventArgs e)
        {
            //select the TreeViewItem that whose contextmenu was opened
            TextBlock tblock = ((TextBlock)((ContextMenu)((MenuItem)sender).Parent).PlacementTarget);
            getNearestFather(tblock).IsSelected = true;

            SmartNode selNode = (SmartNode)smartTreeExplorer.SelectedItem;
            PluginManager.pluginManager.raiseEvent(PublicRessources.Plugin.EventType.videoLoad,
                new VideoEventArgs(selNode.video, true));
        }
        private void miLoadProc_Click(object sender, RoutedEventArgs e)
        {
            //select the TreeViewItem that whose contextmenu was opened
            TextBlock tblock = ((TextBlock)((ContextMenu)((MenuItem)sender).Parent).PlacementTarget);
            getNearestFather(tblock).IsSelected = true;

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
    }
}
