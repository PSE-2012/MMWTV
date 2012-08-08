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
        public VM_Oqat()
        {
            InitializeComponent();

            // PluginManager is initializet by OqatApp
            PluginManager.OqatNewProjectCreatedHandler += onNewProjectCreated;
            PluginManager.OqatToggleView += onToggleView;
            


            //  initMenu();
            vM_Welcome = new VM_Welcome();
            this.welcomePanel.Children.Add(vM_Welcome);

            //  initPluginLists
            this.vM_PluginsList = new VM_PluginsList();
            this.pluginListsPanel.Children.Add(this.vM_PluginsList);

            //  initPresentation
            this.vM_presentation = new VM_Presentation();
            this.presentationPanel.Children.Add(this.vM_presentation);
            
            // initMacro();

            
            //startup viewType is WelcomeView (visibilities set in xaml)
        }



#region VM fields


        private VM_Welcome _vM_Welcome;
        private VM_Welcome vM_Welcome
        {
            get;
            set;
        }
        private VM_PluginsList _vM_PluginLists;
        private VM_PluginsList vM_PluginsList
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


        private ViewType currentView
        {
            get;
            set;
        }


#endregion




        public void onNewProjectCreated(object sender, ProjectEventArgs e)
        {
            //e.project.addNode(new Video(false, "C:\\Users\\Public\\Videos\\Sample Videos\\firstVideo.avi", null), -1);
            //e.project.addNode(new Video(false, "C:\\Users\\Public\\Videos\\Sample Videos\\secondVideo.avi", null), -1);
            //e.project.addNode(new Video(false, "C:\\Users\\Public\\Videos\\Sample Videos\\childOfFirst.avi", null), 0);
            //e.project.addNode(new Video(false, "C:\\Users\\Public\\Videos\\Sample Videos\\childOfSecond.avi", null), 1);

            this.vM_ProjectExplorer = new VM_ProjectExplorer(e.project);
            this.projectExplorerPanel.Children.Add(vM_ProjectExplorer);

            PluginManager.pluginManager.raiseEvent(PublicRessources.Plugin.EventType.toggleView, new ViewTypeEventArgs(ViewType.FilterView));
        }

        
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
            if (e.viewType != currentView)
            {
                switch (e.viewType)
                {
                    case ViewType.WelcomeView:
                        welcomePanel.Visibility = Visibility.Visible;
                        runningAppPanel.Visibility = Visibility.Collapsed;
                        break;
                    case ViewType.MetricView:
                    case ViewType.FilterView:
                    case ViewType.AnalyzeView:
                        welcomePanel.Visibility = Visibility.Collapsed;
                        runningAppPanel.Visibility = Visibility.Visible;
                        break;
                }
            }
        }


        #region styles


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

        #endregion



        private void miErrorConsole_Click(object sender, RoutedEventArgs e)
        {
            OqatApp.errorConsole.Show();
        }

        private void vm_Oqat_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void miInfo_Click(object sender, RoutedEventArgs e)
        {
            WindowOqatInfo wi = new WindowOqatInfo();
            wi.ShowDialog();
        }


    }

}

