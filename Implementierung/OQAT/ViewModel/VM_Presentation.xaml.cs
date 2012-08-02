using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Oqat.PublicRessources.Plugin;
using Oqat.PublicRessources.Model;


namespace Oqat.ViewModel
{
    /// <summary>
    /// Interaktionslogik für VM_Presentation.xaml
    /// </summary>
    public partial class VM_Presentation : UserControl
    {
                /// <summary>
        /// A video player plugin of the <see cref="PresentationPluginType"/> Player.
        /// This player is displayed in the MetricView <see cref="ViewType"/> only.
        /// </summary>
        IPresentation playerProc;

        /// <summary>
        /// A video player plugin of the <see cref="PresentationPluginType"/> Player.
        /// This player is displayed in the MetricView, FilterView, AnalysisView <see cref="ViewType"/>
        /// and contains the reference Video (if currentView == MetricView) or the only Video (if currentView == FilterView).
        /// </summary>
        IPresentation playerRef;

        /// <summary>
        /// CurrentDiagramm is of the PresentationPluginType diagram and is visible only in the AnalysisView.
        /// </summary>
        IPresentation diagramm;

        /// <summary>
        /// ThirdPary plugins needed for visualisation of extra ressources (<see cref="Video"/>).
        /// Such plugins can be visible (user has to choose) in alle ViewTypes where the VM_Presentation is
        /// active, i.e. all except the WelcomeView.
        /// </summary>
        List<IPresentation> custom;



        /// <summary>
        /// According to the current view type the Presetaion will show or hide some features.
        /// </summary>
        ViewType vtype;

        Video videoProc;
        Video videoRef;



        public VM_Presentation()
        {
            InitializeComponent();


            //get presentationPlugins from pluginmanager

            //initializing presentationPlugins

            this.playerProc = PluginManager.pluginManager.getPlugin<IPresentation>("VideoPlayer");
            this.playerRef =(IPresentation) this.playerProc.Clone();

            
            
            
            //player.loadVideo(this, vidargs);
            //player.setParentControl(this.gridPlayer);

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
            if (e.isRefVid)
            {
                this.onToggleView(this, new ViewTypeEventArgs(ViewType.MetricView));

                this.videoRef =(Video) e.video;
                this.playerRef.loadVideo(this, e);
            }
            else
            {
                this.videoProc = (Video)e.video;
                this.playerProc.loadVideo(this, e);
            }

            if (this.vtype == ViewType.AnalyzeView)
            {
                this.diagramm.loadVideo(this, e);
            }
		}



        /// <summary>
        /// This method will be called if the view was toggled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void onToggleView(object sender, ViewTypeEventArgs e)
		{
            if (this.vtype == e.viewType)
                return;

            this.vtype = e.viewType;
            this.resetPanel();
            switch (vtype)
            {
                case ViewType.FilterView:
                    this.playerProc.setParentControl(this.mainGrid);
                    break;
                case ViewType.MetricView:
                    this.playerProc.setParentControl(this.mainGrid);
                    this.playerRef.setParentControl(this.mainGrid);
                    break;
                case ViewType.AnalyzeView:
                    this.playerProc.setParentControl(this.mainGrid);
                    this.playerRef.setParentControl(this.mainGrid);
                    this.diagramm.setParentControl(this.mainGrid);
                    break;
            }
		}

        /// <summary>
        /// Corresponding event is called from whithin VM_Presentation ( a
        /// droppanel or something simmilar) if user chooses some extra resources
        /// to visualise.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onExtraResourceSelected(object sender, EventArgs e) 
        { 
        
        }

        /// <summary>
        /// Will be called if the view was toggled. This methode
        /// does things like detaching a video from the player and diagram.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onFlushPresentationPlugins(object sender, EventArgs e) { }

		public VM_Presentation(Panel parent)
		{
            parent.Children.Add(this);
		}

        /// <summary>
        /// Can be used to remove presentation plugins (e.g. PresentationType == Custom) from the VM_Presentation
        /// view.
        /// </summary>
		private void resetPanel()
		{
            this.mainGrid.Children.Clear();
		}

		private void showExtraResourceList()
		{
			throw new System.NotImplementedException();
		}
    }
}
