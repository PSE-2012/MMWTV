namespace Oqat.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    using Oqat.PublicRessources.Plugin;
    using Oqat.PublicRessources.Model;
    using System.Windows.Controls;

    /// <summary>
    /// This class is responsible for displaying userimportedvideo and visualising analysis results.
    /// </summary>
	public class VM_Presentation
	{
        /// <summary>
        /// According to the current view type the Presetaion will show or hide some features.
        /// </summary>
		private ViewType currentViewType
		{
			get;
			set;
		}


        /// <summary>
        /// A video player plugin of the <see cref="PresentationPluginType"/> Player.
        /// This player is displayed in the MetricView <see cref="ViewType"/> only.
        /// </summary>
		private IPresentation currentPlayerProc
		{
			get;
			set;
		}
        /// <summary>
        /// A video player plugin of the <see cref="PresentationPluginType"/> Player.
        /// This player is displayed in the MetricView, FilterView, AnalysisView <see cref="ViewType"/>
        /// and contains the reference Video (if currentView == MetricView) or the only Video (if currentView == FilterView).
        /// </summary>
        private IPresentation currentPlayerRef
        {
            get;
            set;
        }

        /// <summary>
        /// CurrentDiagramm is of the PresentationPluginType diagram and is visible only in the AnalysisView.
        /// </summary>
		private IPresentation currentDiagram
		{
			get;
			set;
		}

        /// <summary>
        /// ThirdPary plugins needed for visualisation of extra ressources (<see cref="Video"/>).
        /// Such plugins can be visible (user has to choose) in alle ViewTypes where the VM_Presentation is
        /// active, i.e. all except the WelcomeView.
        /// </summary>
		private List<IPresentation> currentCustoms
		{
			get;
			set;
		}

        /// <summary>
        /// Video, presentation is currently focused on. (before it is passed to a player)
        /// </summary>
        private Video currentVideoProc
        {
            set;
            get;
        }

        /// <summary>
        /// reference Video, presentation is currently focused on. (before it is passed to a player)
        /// </summary>
        private Video currentVideoRef
        {
            set;
            get;
        }



        /// <summary>
        /// This methode will be called if a videoLoad event is raised, i.e. 
        /// if user selects a video from the SmartTree, the <see cref="VM_Macro"/>
        /// wants to pass a filter preview or filter- / metricresults
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void onVideoLoad(object sender, VideoEventArgs e)
		{
			throw new System.NotImplementedException();
		}

        /// <summary>
        /// This method will be called if the view was toggled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void onToggleView(object sender, ViewTypeEventArgs e)
		{
			throw new System.NotImplementedException();
		}

        /// <summary>
        /// Corresponding event is called from whithin VM_Presentation ( a
        /// droppanel or something simmilar) if user chooses some extra resources
        /// to visualise.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onExtraResourceSelected(object sender, EventArgs e) { }

        /// <summary>
        /// Will be called if the view was toggled. This methode
        /// does things like detaching a video from the player and diagram.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onFlushPresentationPlugins(object sender, EventArgs e) { }

		public VM_Presentation(Panel parent)
		{
		}

        /// <summary>
        /// Can be used to remove presentation plugins (e.g. PresentationType == Custom) from the VM_Presentation
        /// view.
        /// </summary>
		private void resetPanel()
		{
			throw new System.NotImplementedException();
		}

		private void showExtraResourceList()
		{
			throw new System.NotImplementedException();
		}

	}
}

