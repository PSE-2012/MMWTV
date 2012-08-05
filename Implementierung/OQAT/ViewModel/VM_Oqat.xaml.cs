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

using Oqat.ViewModel.Macro;
using Oqat.Model;
using System.Runtime.InteropServices;
using System.Windows.Interop;
namespace Oqat.ViewModel
{
    /// <summary>
    /// This class initializes all viewmodels and controls the layout of Oqat.
    /// </summary>
    public partial class VM_Oqat : Window
    {
        private VM_Welcome _vM_Welcome;
        private VM_Welcome vM_Welcome
        {
            get;
            set;
        }
        private VM_PluginLists _vM_PluginLists;
        private VM_PluginLists vM_PluginList
        {
            get;
            set;
        }
        private VM_ProjectExplorer _vM_ProjectExplorer;
        private VM_ProjectExplorer vM_ProjectExplorer
        {
            get;
            set;
        }

        private VM_Presentation _vM_Presentation;
        private VM_Presentation vM_presentation
        {
            get;
            set;
        }

        private VM_Macro _vM_Macro;
        private VM_Macro vM_Macro
        {
            get;
            set;
        }

        private PluginManager pluginManager
        {
            get;
            set;
        }

        private ViewType currentView
        {
            get;
            set;
        }


        /// <summary>
        /// Initializes the pluginList (Filter and MetricList).
        /// </summary>
        private void initPluginLists()
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Initializes the Welcome view.
        /// </summary>
        private void initWelcome()
        {
            vM_Welcome = new VM_Welcome();
            this.welcomePanel.Children.Add(vM_Welcome);
        }

        /// <summary>
        /// Initializes the ProjectExplorer(SmartTree and FileExplorer).
        /// </summary>
        private void initProjectExplorer()
        {

            //var tmpPr = new Project("testProject", "C:\\Users\\Public\\Videos\\Sample Videos\\someProject.proj", "description");

            //this.vM_ProjectExplorer = new VM_ProjectExplorer(tmpPr, projectExplorerPanel);
       
        }

        public void onNewProjectCreated(object sender, ProjectEventArgs e)
        {
            //e.project.addNode(new Video(false, "C:\\Users\\Public\\Videos\\Sample Videos\\firstVideo.avi", null), -1);
            //e.project.addNode(new Video(false, "C:\\Users\\Public\\Videos\\Sample Videos\\secondVideo.avi", null), -1);
            //e.project.addNode(new Video(false, "C:\\Users\\Public\\Videos\\Sample Videos\\childOfFirst.avi", null), 0);
            //e.project.addNode(new Video(false, "C:\\Users\\Public\\Videos\\Sample Videos\\childOfSecond.avi", null), 1);

            this.vM_ProjectExplorer = new VM_ProjectExplorer(e.project);

            PluginManager.pluginManager.raiseEvent(PublicRessources.Plugin.EventType.toggleView, new ViewTypeEventArgs(ViewType.FilterView));
        }

        public VM_Oqat()
        {
            InitializeComponent();
            PluginManager.pluginManager.OqatNewProjectCreatedHandler += onNewProjectCreated;
            PluginManager.pluginManager.OqatToggleView += onToggleView;
                // PluginManager is initializet by OqatApp
                //  initMenu();
             initWelcome();
            //  initPluginLists();
            //  initPresentation();
            // initMacro();
        }

        /// <summary>
        /// Initializes the main menu.
        /// </summary>
        private void initMenu()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Initializes the presentation viewmodel.
        /// </summary>
        private void initPresentation()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Initializes the macro viewmodel.
        /// </summary>
        private void initMacro() { }


        /// <summary>
        /// From here all the project relevant initialization methods are called.
        /// This delegate will be called if a existing project was open or
        /// if a new one was created.
        /// </summary>
        /// <param name="sender">Reference to the caller.</param>
        /// <param name="e">Created project.</param>
        private void onBuildProjectView(object sender, ProjectEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Can be used to check if all viewmodels have disconnected or connected
        /// acoording to the current view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onToggleView(object sender, ViewTypeEventArgs e)
        {
            if(e.viewType!=currentView)
            switch (e.viewType)
            {
                case ViewType.WelcomeView:
                    welcomePanel.Children.Add(vM_Welcome);
                    projectExplorerPanel.Children.Remove(vM_ProjectExplorer);
                  //  pluginListsPanel.Children.Remove(vM_PluginList);
                    break;
                case ViewType.MetricView:
                    welcomePanel.Children.Remove(vM_Welcome);
                    
                    projectExplorerPanel.Children.Add(vM_ProjectExplorer);                    
                    break;
                case ViewType.FilterView:
                    welcomePanel.Children.Remove(vM_Welcome);

                    projectExplorerPanel.Children.Add(vM_ProjectExplorer);
                    break;
                case ViewType.AnalyzeView:
                    welcomePanel.Children.Remove(vM_Welcome);
                    projectExplorerPanel.Children.Add(vM_ProjectExplorer);
                    break;
            }
        }



            [StructLayout(LayoutKind.Sequential)]
            private struct MARGINS
            {
                public int cxLeftWidth;      // width of left border that retains its size
                public int cxRightWidth;     // width of right border that retains its size
                public int cyTopHeight;      // height of top border that retains its size
                public int cyBottomHeight;   // height of bottom border that retains its size
            };

            [DllImport("DwmApi.dll")]
            private static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS pMarInset);

            private void Window_Loaded(object sender, RoutedEventArgs e)
            {
                WindowInteropHelper windowInteropHelper = new WindowInteropHelper(this);
                IntPtr myHwnd = windowInteropHelper.Handle;
                HwndSource mainWindowSrc = System.Windows.Interop.HwndSource.FromHwnd(myHwnd);

                mainWindowSrc.CompositionTarget.BackgroundColor = Color.FromArgb(0, 0, 0, 0);

                MARGINS margins = new MARGINS()
                {
                    cxLeftWidth = -1,
                    cxRightWidth = -1,
                    cyBottomHeight = -1,
                    cyTopHeight = -1
                };

                DwmExtendFrameIntoClientArea(myHwnd, ref margins);
            }
        


    }
}
