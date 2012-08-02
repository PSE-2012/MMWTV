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

		public VM_ProjectExplorer(Project project, Panel parent)
		{
            InitializeComponent();

            // projectExplorer
            this.project = project;
            smartTreeExplorer.DataContext = project.smartTree;

            // fileExplorer
            fileExp.ViewMode = Microsoft.WindowsAPICodePack.Controls.ExplorerBrowserViewMode.List;
            this.Loaded += new RoutedEventHandler(ExpBrows_Loaded);
            parent.Children.Add(this);
		}
        void ExpBrows_Loaded(object sender, RoutedEventArgs e)
        {
            fileExp.ExplorerBrowserControl.Navigate((ShellObject)KnownFolders.Desktop);
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


        }



	}
}

