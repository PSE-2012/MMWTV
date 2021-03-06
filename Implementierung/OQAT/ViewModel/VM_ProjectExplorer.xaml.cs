﻿namespace Oqat.ViewModel
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
    using System.Threading;

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
                using (XmlTextReader reader = new XmlTextReader(sFilename))
                {
                    reader.Read();
                    reader.Read();
                    int count = 10;
                    String[] t = new String[count];
                    String[] t2 = new String[count];
                    for (int i = 0; i < count; i++)
                    {
                        reader.Read();
                        reader.Read();
                        t[i] = reader.Name;
                        reader.MoveToNextAttribute();
                        t2[i] = reader.Value;
                        if (t2[i] == "")
                        {
                            throw new XmlException("datei nicht lang genug");
                        }
                    }
                    lb1.Content = t2[0];
                    lb2.Content = t2[1];
                    btnExport.Content = t2[2];
                    this.Resources["proc"] = t2[3];
                    this.Resources["ref"] = t2[4];
                    this.Resources["ana"] = t2[5];
                    this.Resources["exp"] = t2[6];
                    importconsiserror = t2[7];
                    notfound1 = t2[8];
                    notfound2 = t2[9];
                }
            }
            catch (IndexOutOfRangeException) { }
            catch (FileNotFoundException) { }
            catch (XmlException) { }
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
            this.Resources["proc"] = "Video auswählen";
            this.Resources["ref"] = "als Referenz auswählen";
            this.Resources["ana"] = "Analyse anzeigen";
            this.Resources["exp"] = "Analyse exportieren";
            local("VM_ProjectExplorer_" + Thread.CurrentThread.CurrentCulture + ".xml");
            PluginManager.macroProcessingFinished += this.onMacroProcessingFinished;

            // projectExplorer
            this.project = project;
            smartTreeExplorer.DataContext = project.smartTree;
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

        private void smartTreeExplorer_PreviewDrop(object sender, DragEventArgs e)
        {
            e.Handled = true;

            //select item to drop in if there is one
            var dropInItem = (getNearestFather<TreeViewItem>((DependencyObject)e.OriginalSource));
            SmartNode dropInNode = null;
            if (dropInItem != null)
                dropInNode = dropInItem.DataContext as SmartNode;

       
            if (e.Data is DataObject && ((DataObject)e.Data).ContainsFileDropList())
            {

                    // ask if user wants to open many windows...
                    StringCollection fileList  = ((DataObject)e.Data).GetFileDropList();
                    importVideos(fileList);
            }
            else if (e.Data is DataObject && ((DataObject)e.Data).GetDataPresent("SmartNode"))
            {
                SmartNode smartNode = ((DataObject)e.Data).GetData("SmartNode") as SmartNode;
               
                if ((smartNode != null) && (smartNode != dropInNode))
                {
                    project.rmNode(smartNode.id, false);

                    if(dropInNode != null)
                    project.addNode(smartNode.video, dropInNode.id);
                    else
                        project.addNode(smartNode.video,-1);//add at topLevel
                }
            }
        }

        /// <summary>
        /// Imports the videofiles into the smarttree by opening a vidImportOptionsDialog.
        /// </summary>
        /// <param name="fileList">the filenames of videos to import.</param>
        string importconsiserror = "Mindestens eine Videodatei ist kaputt";
        public void importVideos(StringCollection fileList)
        {
           
                VM_VidImportOptionsDialog vidImp = new VM_VidImportOptionsDialog(fileList);
                vidImp.Owner = Window.GetWindow(this);
                Nullable<bool> result = vidImp.ShowDialog();

                if ((result != null) & (bool)result)
                {
                    foreach (var vid in vidImp.videoList)
                    {
                        if (vid.handler.consistent)
                        {
                            project.addNode(vid, -1);
                        }
                        else
                        {
                            MessageBox.Show(importconsiserror);
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
        private void smartTreeExplorer_PreviewDragEnter(object sender, DragEventArgs e)
        {
                e.Handled = true;

                TreeViewItem smartItem = getNearestFather<TreeViewItem>(e.OriginalSource as DependencyObject);
                if (smartItem != null)
                    smartItem.IsExpanded = true;
                

        }

        private void miLoadAna_Click(object sender, RoutedEventArgs e)
        {
            //select the TreeViewItem that whose contextmenu was opened
            TextBlock tblock = ((TextBlock)((ContextMenu)((MenuItem)sender).Parent).PlacementTarget);
            getNearestFather<TreeViewItem>(tblock).IsSelected = true;

            SmartNode selNode = (SmartNode)smartTreeExplorer.SelectedItem;
            loadVideo(selNode);
        }
        private void miLoadRef_Click(object sender, RoutedEventArgs e)
        {
            //select the TreeViewItem that whose contextmenu was opened
            TextBlock tblock = ((TextBlock)((ContextMenu)((MenuItem)sender).Parent).PlacementTarget);
            getNearestFather<TreeViewItem>(tblock).IsSelected = true;

            SmartNode selNode = (SmartNode)smartTreeExplorer.SelectedItem;
            loadVideo(selNode, true);
        }
        private void miLoadProc_Click(object sender, RoutedEventArgs e)
        {
            //select the TreeViewItem that whose contextmenu was opened
            TextBlock tblock = ((TextBlock)((ContextMenu)((MenuItem)sender).Parent).PlacementTarget);
            getNearestFather<TreeViewItem>(tblock).IsSelected = true;

            SmartNode selNode = (SmartNode)smartTreeExplorer.SelectedItem;
            loadVideo(selNode);
        }


        private void onMacroProcessingFinished(object sender, VideoEventArgs e)
        {
            project.addNode(e.video, e.id);
            //dispVideoEventArgs = e;
            //actProjectAdd();
        }
        //VideoEventArgs dispVideoEventArgs;
        //private void actProjectAdd()
        //{
        //    if (!this.smartTreeExplorer.Dispatcher.CheckAccess())
        //    {
        //        this.smartTreeExplorer.Dispatcher.Invoke(
        //                new System.Windows.Forms.MethodInvoker(actProjectAdd));
        //        return;
        //    }

        //        project.addNode(dispVideoEventArgs.video, dispVideoEventArgs.id);
        //}

        private void treeitem_MouseDoubleClicked(object sender, RoutedEventArgs e)
        {
            if (this.smartTreeExplorer.SelectedItem == ((SmartNode)((TreeViewItem)e.Source).Header))
            {
                SmartNode selNode = (SmartNode)smartTreeExplorer.SelectedItem;
                loadVideo(selNode, false);
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
                using (StreamWriter myFile = new StreamWriter(p))
                {
                    myFile.Write(s);
                }
            }
    
        }

        private void smartTreeExplorer_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            e.Handled = true;
            // remove active element, if there was one
            readOnlyPropViewPanel.Children.Clear();
            
            var newSelSmartNode = (SmartNode)e.NewValue;
            if (newSelSmartNode != null)
            {
                if (videoCheck(newSelSmartNode))
                    readOnlyPropViewPanel.Children.Add(newSelSmartNode.video.handler.readOnlyInfoView);

                if (newSelSmartNode.video != null && newSelSmartNode.video.isAnalysis == true)
                {
                    btnExport.Visibility = Visibility.Visible;
                }
                else
                {
                    btnExport.Visibility = Visibility.Hidden;
                }
            }
        }

        private void smartTreeExplorer_PreviewDragLeave(object sender, DragEventArgs e)
        {
           // e.Handled = true;
        }

        private bool isMouseDown = false;
        private bool isDragging = false;
        private Point lastLeftBtnDownPos;
        private void smartTreeExplorer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //e.Handled = true;

            lastLeftBtnDownPos = e.GetPosition(null);
            isMouseDown = true;
        }

        private void smartTreeExplorer_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
           // e.Handled = true;
            isDragging = false;
            isMouseDown = false;
        }

        private void smartTreeExplorer_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point currMousePos = e.GetPosition(null);
            Vector diff = lastLeftBtnDownPos - currMousePos;

            if (isMouseDown && !isDragging &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                isDragging = true;
                TreeView smartTree = sender as TreeView;
                TreeViewItem treeItem = getNearestFather<TreeViewItem>(e.OriginalSource as DependencyObject);
                if (treeItem != null)
                {
                    SmartNode smartNode = treeItem.DataContext as SmartNode;
                    if (smartNode != null)
                    {
                        DataObject dragData = new DataObject("SmartNode", smartNode);
                        DragDrop.DoDragDrop(treeItem, dragData, DragDropEffects.Move);
                        isDragging = false;
                        isMouseDown = false;
                    }
                }
            }
        }






        private void loadVideo(SmartNode node, bool isRef = false)
        {
            if(videoCheck(node))
            {
                PluginManager.pluginManager.raiseEvent(PublicRessources.Plugin.EventType.videoLoad,
                    new VideoEventArgs(node.video, node.id, isRef));
            }
        }

        string notfound1="Die Videodatei konnte nicht gefunden werden, soll das Video aus dem Projekt entfernt werden?";
        string notfound2 = "Datei nicht gefunden";
        private bool videoCheck(SmartNode node)
        {
            //check if videofile still exists
            if (!File.Exists(node.video.vidPath))
            {
                MessageBoxResult result = MessageBox.Show(notfound1,notfound2 , MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    project.rmNode(node.id, false);
                }
                return false;
            }
            else
            {
                return true;
            }
        }




    }

    public class NegatingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, 
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
