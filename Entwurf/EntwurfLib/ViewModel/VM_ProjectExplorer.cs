//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    using Oqat.Model;
    using Oqat.PublicRessources.Model;
    using System.Windows.Controls;

	public class VM_ProjectExplorer
	{
		private  TabControl projectExplorer
		{
			get;
			set;
		}

		private TreeView smartTree
		{
			get;
			set;
		}

		private TreeView fileExplorer
		{
			get;
			set;
		}

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

        private void onVideoClick(object sender, VideoEventArgs e);

	}
}

