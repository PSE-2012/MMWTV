//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
namespace Oqat.ViewModel
{
	using Plugins.Metric;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    using Oqat.PublicRessources.Plugin;
    using Oqat.PublicRessources.Model;
    using System.Windows.Controls;
	public class VM_Presentation
	{
		private ViewType currentViewType
		{
			get;
			set;
		}


		private IPresentation currentPlayerProc
		{
			get;
			set;
		}

        private IPresentation currentPlayerRef
        {
            get;
            set;
        }

		private IPresentation currentDiagram
		{
			get;
			set;
		}

		private List<IPresentation> currentCustoms
		{
			get;
			set;
		}

        private Video currentVideo
        {
            set;
            get;
        }

        private delegate void onToggleView(object sender, ViewTypeEventArgs e);
        private delegate void onVideoLoad(object sender, VideoEventArgs e);


		private void onVideoLoad(object sender, VideoEventArgs e)
		{
			throw new System.NotImplementedException();
		}

		private void onToggleView(object sender, ViewTypeEventArgs e)
		{
			throw new System.NotImplementedException();
		}

        private void onExtraRessourceSelected(object sender, EventArgs e) { }

        private void onFlushPresentationPlugins(object sender, EventArgs e) { }

		public VM_Presentation(Panel parent)
		{
		}

		private void resetPanel()
		{
			throw new System.NotImplementedException();
		}

		private void showExtraRessourceList()
		{
			throw new System.NotImplementedException();
		}

	}
}

