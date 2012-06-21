namespace Oqat.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    using Oqat.Model;
    using Oqat.PublicRessources.Model;
    using System.Windows.Controls;

    /// <summary>
    /// This class is mainly responsible to sync the SmartTree(GUI) with the SmartNodes (Model) and
    /// raising the right events if user wants to delete/play a video.
    /// </summary>
	public class VM_ProjectExplorer
	{
		private  TabControl projectExplorer
		{
			get;
			set;
		}

        /// <summary>
        /// All videos (and analysis results) are listed in here.
        /// The contents are bound to the SmartNode of the current project
        /// </summary>
		private TreeView smartTree
		{
			get;
			set;
		}

        /// <summary>
        /// This component is a ordinary file explorer and can be used to import Videos.
        /// </summary>
		private TreeView fileExplorer
		{
			get;
			set;
		}

        /// <summary>
        /// Reference to the currently active project.
        /// </summary>
		private Project project
		{
			get;
			set;
		}


		private PluginManager pluginManager
		{
			get;
			set;
		}

		public VM_ProjectExplorer(Project project, Panel parent)
		{
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

