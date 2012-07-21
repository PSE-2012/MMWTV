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
            this.project = project;
            myTreeView.DataContext = project.smartTree;
            parent.Children.Add(this);
            //////
            this.project.addNode(new Video(false, "someOther", null), -1);
            this.project.addNode(new Video(false, "andOneMore", null), 0);
            this.project.addNode(new Video(false, "someOther", null), -1);
            this.project.addNode(new Video(false, "andOneMore", null), 1);
            this.project.addNode(new Video(false, "someOther", null), -1);
            this.project.addNode(new Video(false, "andOneMore", null), 2);



		}

        /// <summary>
        /// If the user clicks a entry out of the SmartTree the <see cref="IVideoInfo"/> object contents (size, history..)
        /// of the corresponding Video (smartNode) are displayed in the lower part of the smartTree.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onVideoClick(object sender, VideoEventArgs e) { }

	}
}

