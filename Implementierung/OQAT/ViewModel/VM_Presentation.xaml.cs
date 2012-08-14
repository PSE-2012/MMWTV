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
using Oqat.ViewModel.Macro;

using System.Xml;
using System.IO;

namespace Oqat.ViewModel
{
    /// <summary>
    /// Interaktionslogik für VM_Presentation.xaml
    /// </summary>
    public partial class VM_Presentation : UserControl
    {
        List<IPresentation> _custom;
        IPresentation _playerProc;
        IPresentation _playerRef;
        IPresentation _diagram;
        internal VM_Macro vm_macro;

        ViewType vtype;

        IVideo videoProc;
        IVideo videoRef;

        String msgBox1= "Bitte wählen Sie zunächst Videos.";
        String msgBox2 = "Macro Ausführung nicht möglich";
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
                bttProcessMacro.Content = t2[0];
                msgBox1 = t2[1];
                msgBox2 = t2[2];


            }
            catch (IndexOutOfRangeException e) { }
            catch (FileNotFoundException e) { }
            catch (XmlException e) { }
        }


        public VM_Presentation()
        {
            InitializeComponent();
            local("VM_Presentation_default.xml");
            PluginManager.OqatToggleView += this.onToggleView;
            PluginManager.videoLoad += this.onVideoLoad;

            //init macro
            vm_macro = new VM_Macro();


            this._custom = new List<IPresentation>();


            this.gridPlayer1.Children.Add(playerProc.propertyView);
            this.gridPlayer2.Children.Add(playerRef.propertyView);
            this.otherPanel.Children.Add(diagram.propertyView);
            this.gridMacro.Children.Add(vm_macro.propertiesView);
        }



        /// <summary>
        /// A video player plugin of the <see cref="PresentationPluginType"/> Player.
        /// This player is displayed in the MetricView <see cref="ViewType"/> only.
        /// </summary>
        private IPresentation playerProc
        {
            get
            {
                if (_playerProc == null)
                {
                    _playerProc = PluginManager.pluginManager.getPlugin<IPresentation>("PP_Player");

                    if (_playerProc == null)
                    {
                        //no Player is available from PluginManager
                        throw new ApplicationException("Es wurde kein Player-Plugin gefunden.");
                    }
                }
                return _playerProc;
            }
        }

        /// <summary>
        /// A video player plugin of the <see cref="PresentationPluginType"/> Player.
        /// This player is displayed in the MetricView, FilterView, AnalysisView <see cref="ViewType"/>
        /// and contains the reference Video (if currentView == MetricView) or the only Video (if currentView == FilterView).
        /// </summary>
        private IPresentation playerRef
        {
            get
            {
                if (_playerRef == null)
                {
                    _playerRef =(IPresentation) PluginManager.pluginManager.getPlugin<IPresentation>("PP_Player").Clone();

                    if (_playerRef == null)
                    {
                        //no Player is available from PluginManager
                        throw new ApplicationException("Es wurde kein Player-Plugin gefunden.");
                    }
                }
                return _playerRef;
            }
        }

        /// <summary>
        /// CurrentDiagramm is of the PresentationPluginType diagram and is visible only in the AnalysisView.
        /// </summary>
        /// <exception cref="Dll"
        private IPresentation diagram
        {
            get
            {
                if (_diagram == null)
                {
                    _diagram = PluginManager.pluginManager.getPlugin<IPresentation>("PP_Diagram");

                    if (_diagram == null)
                    {
                        //no Player is available from PluginManager
                        throw new ApplicationException("Es wurde kein Diagramm-Plugin gefunden.");
                    }
                }
                return _diagram;
            }
        }



        /// <summary>
        /// ThirdPary plugins needed for visualisation of extra ressources (<see cref="Video"/>).
        /// Such plugins can be visible (user has to choose) in alle ViewTypes where the VM_Presentation is
        /// active, i.e. all except the WelcomeView.
        /// </summary>









        #region OQAT Events

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

                this.videoRef =(IVideo) e.video;
                this.playerRef.setVideo(videoRef);
            }
            else
            {
                this.videoProc = (IVideo)e.video;
                this.playerProc.setVideo(videoProc);
            }

            if (this.vtype == ViewType.AnalyzeView)
            {
                this.diagram.setVideo(e.video);
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
            switch (vtype)
            {
                case ViewType.FilterView:
                    this.gridPlayer1.Visibility = System.Windows.Visibility.Visible;
                    this.gridPlayer2.Visibility = System.Windows.Visibility.Collapsed;
                    this.otherPanel.Visibility = System.Windows.Visibility.Collapsed;
                    this.gridMacro.Visibility = System.Windows.Visibility.Visible;
                    break;
                case ViewType.MetricView:
                    this.gridPlayer1.Visibility = System.Windows.Visibility.Visible;
                    this.gridPlayer2.Visibility = System.Windows.Visibility.Visible;
                    this.otherPanel.Visibility = System.Windows.Visibility.Collapsed;
                    this.gridMacro.Visibility = System.Windows.Visibility.Visible;
                    break;
                case ViewType.AnalyzeView:
                    this.gridPlayer1.Visibility = System.Windows.Visibility.Visible;
                    this.gridPlayer2.Visibility = System.Windows.Visibility.Visible;
                    this.otherPanel.Visibility = System.Windows.Visibility.Visible;
                    this.gridMacro.Visibility = System.Windows.Visibility.Collapsed;
                    break;
            }
		}
        

        /// <summary>
        /// Will be called if the view was toggled. This methode
        /// does things like detaching a video from the player and diagram.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onFlushPresentationPlugins(object sender, EventArgs e) 
        {
            try
            {
                this.playerProc.flush();
                this.playerRef.flush();
                this.diagram.flush();

                foreach (IPresentation p in _custom)
                {
                    p.flush();
                }
            }
            catch (NullReferenceException ex)
            {
                //ignore broken plugins dealing with null references
                PluginManager.pluginManager.raiseEvent(EventType.failure, new System.IO.ErrorEventArgs(ex));
            }
        }

        #endregion


        #region extraResources

        private void showExtraResourceList()
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
        private void onExtraResourceSelected(object sender, EventArgs e)
        {

        }

        #endregion



        private void bttProcessMacro_Click(object sender, RoutedEventArgs e)
        {
            if(this.videoProc == null || 
                (vtype == ViewType.MetricView && videoRef == null))
            {
                MessageBox.Show(msgBox1, msgBox2);
                return;
            }
            
            //TODO: seems like the naming proc vs. ref was understood the other way round in macro
            vm_macro.vidProc = (Oqat.Model.Video) this.videoRef;
            vm_macro.vidRef = (Oqat.Model.Video) this.videoProc;

            vm_macro.startProcess();
        }

    }
}
