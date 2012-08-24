using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using Oqat.PublicRessources.Plugin;
using Oqat.PublicRessources.Model;
using Oqat.ViewModel.Macro;
using System.Threading;
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
        int idProc;
        IVideo videoRef;
        int idRef;

        String msgBox1= "Bitte wählen Sie zunächst Videos.";
        String msgBox2 = "Macro Ausführung nicht möglich";


        #region eyeCancer
        /// <summary>
        /// method to read local xml file and put the language in the vm.
        /// </summary>
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
        #endregion

        /// <summary>
        /// constructor
        /// </summary>
        public VM_Presentation()
        {
            InitializeComponent();
            local("VM_Presentation_" + Thread.CurrentThread.CurrentCulture + ".xml");
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
            if (e.video.isAnalysis)
            {
                this.onToggleView(this, new ViewTypeEventArgs(ViewType.AnalyzeView));

                this.videoProc = (IVideo)e.video;
                this.playerProc.setVideo(videoProc);
                this.diagram.setVideo(videoProc);
            }
            else if (e.isRefVid)
            {
                if (isCompatibleVideo(e.video, this.videoProc))
                {
                    this.onToggleView(this, new ViewTypeEventArgs(ViewType.MetricView));

                    this.videoRef = (IVideo)e.video;
                    this.idRef = e.id;
                    this.playerRef.setVideo(videoRef);
                }
            }
            else
            {
                if(vtype != ViewType.MetricView || isCompatibleVideo(e.video, this.videoRef))
                {
                    this.videoProc = (IVideo)e.video;
                    this.idProc = e.id;
                    this.playerProc.setVideo(videoProc);
                }
            }
		}

        /// <summary>
        /// check if video is compatible with other, already loaded video. 
        /// (e.g. has same dimensions as the one to be compared to)
        /// </summary>
        private bool isCompatibleVideo(IVideo vid1, IVideo vid2)
        {
            if (vid1 != null && vid2 != null)
            {
                if(vid1 == vid2) 
                {
                    //video should only be loaded once
                    MessageBox.Show("Das Video wurde bereits geladen und kann nicht mehrfach hinzugefügt werden.", "Video bereits geladen");
                    return false;
                }

                if (vid1.vidInfo.height != vid2.vidInfo.height
                    || vid1.vidInfo.width != vid2.vidInfo.width
                    || vid1.vidInfo.frameCount != vid2.vidInfo.frameCount)
                {
                    MessageBox.Show("Das Video muss gleiche Framezahl und Abmessungen haben, um mit dem vorhanden Video verglichen zu werden.", "Video nicht kompatibel");
                    return false;
                }
            }
            return true;
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
                    this.gridPlayer2.Visibility = System.Windows.Visibility.Collapsed;
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


        /// <summary>
        /// event to signalice the macro to process or analyse the current que
        /// </summary>
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
            vm_macro.idProc = this.idRef;

            vm_macro.vidRef = (Oqat.Model.Video) this.videoProc;
            vm_macro.idRef = this.idProc;

            vm_macro.startProcess();
        }

        private void gridPlayer_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("SmartNode") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
            else
            {
                e.Effects = DragDropEffects.Link;
            }

            System.Windows.Media.BrushConverter bc = new System.Windows.Media.BrushConverter();
            ((Grid)sender).Background = (System.Windows.Media.Brush)bc.ConvertFrom("#cccccc");
        }

        private void gridPlayer_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("SmartNode"))
            {
                Oqat.Model.Video vid = (e.Data.GetData("SmartNode") as Oqat.Model.SmartNode).video;
                switch (((Grid)sender).Name)
                {
                    case "gridPlayer1":
                        this.videoProc = (IVideo)vid;
                        this.playerProc.setVideo(vid);
                        break;
                    case "gridPlayer2":
                        this.videoRef = (IVideo)vid;
                        this.playerRef.setVideo(vid);
                        break;
                    case "otherPanel":
                        this.diagram.setVideo(vid);
                        break;
                }
            }
        }

        private void gridPlayer_DragLeave(object sender, DragEventArgs e)
        {
            System.Windows.Media.BrushConverter bc = new System.Windows.Media.BrushConverter();
            ((Grid)sender).Background = (System.Windows.Media.Brush)bc.ConvertFrom("#eeeeee");
        }

    }
}
