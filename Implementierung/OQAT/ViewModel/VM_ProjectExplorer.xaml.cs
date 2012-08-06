namespace Oqat.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

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


    /// <summary>
    /// This class is mainly responsible to sync the SmartTree(GUI) with the SmartNodes (Model) and
    /// raising the right events if user wants to delete/play a video.
    /// </summary>
	public partial class VM_ProjectExplorer : UserControl
	{


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
        private void onVideoClick(object sender, VideoEventArgs e) { }

        private void onSmartNodeSelect(object sender, RoutedPropertyChangedEventArgs<object> e) 
        {
            SmartNode selected = (SmartNode)e.NewValue;

            // TODO: Event to load video on proper userinput
            PluginManager.pluginManager.raiseEvent(PublicRessources.Plugin.EventType.videoLoad,
                new VideoEventArgs(selected.video));
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

	}
}

