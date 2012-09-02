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
using System.Threading;
using Oqat.ViewModel.MacroPlugin;
using Oqat.Model;
using Oqat.PublicRessources.Model;
using System.Runtime.InteropServices;
using System.Windows.Interop;

using System.IO;
using System.Xml;

namespace Oqat.ViewModel
{
    /// <summary>
    /// This class initializes all viewmodels and controls the layout of Oqat.
    /// </summary>
    public partial class VM_Oqat : Window
    {

        /// <summary>
        /// Sets the Language Content and reads it from an XML File.
        /// </summary>
        private void local(String s)
        {
            try
            {
                String sFilename = Directory.GetCurrentDirectory() + "/" + s;
                XmlTextReader reader = new XmlTextReader(sFilename);
                reader.Read();
                reader.Read();
                String[] t = new String[8];
                String[] t2 = new String[8];
                for (int i = 0; i < 8; i++)
                {
                    reader.Read();
                    reader.Read();
                    t[i] = reader.Name;
                    reader.MoveToNextAttribute();
                    t2[i] = reader.Value;
                    if (t2[i] == "")
                    {
                        throw new XmlException("datei nicht lang genug");
                    }
                }
                mn1.Header= t2[0];
                mn2.Header = t2[1];
                miInfo.Header= t2[2];
                tabFilter.Header = t2[3];
                tabMetric.Header= t2[4];
                miVidImport.Header = t2[5];
                miNewProject.Header = t2[6];
                miOpenProject.Header = t2[7];
              

            }
            catch (IndexOutOfRangeException e) { }
            catch (FileNotFoundException e) { }
            catch (XmlException e) { }
        }
        /// <summary>
        /// constructor of the PRogramm. initialises all components.
        /// </summary>
        public VM_Oqat()
        {
            InitializeComponent();
           
            local("VM_Oqat_" + Thread.CurrentThread.CurrentCulture+".xml");
            // PluginManager is initializet by OqatApp
            PluginManager.OqatNewProjectCreatedHandler += onNewProjectCreated;
            PluginManager.OqatToggleView += onToggleView;
            


            //  initMenu();
            vM_Welcome = new VM_Welcome();
            this.welcomePanel.Children.Add(vM_Welcome);

            //  initPluginLists
            vM_FilterList = new VM_PluginsList(Oqat.PublicRessources.Plugin.PluginType.IFilterOqat);
            this.tabFilter.Content = vM_FilterList;
           // vM_FilterList.macroLoaded += onMacroFilterLoad;

            vM_MetricList = new VM_PluginsList(Oqat.PublicRessources.Plugin.PluginType.IMetricOqat);
            this.tabMetric.Content = vM_MetricList;
           // vM_MetricList.macroLoaded += onMacroMetricLoad;

            //  initPresentation
            this.vM_presentation = new VM_Presentation();
            this.presentationPanel.Children.Add(this.vM_presentation);
            
            // initMacro();

            
            //startup viewType is WelcomeView (visibilities set in xaml)
        }



        #region VM fields


        private VM_Welcome vM_Welcome
        {
            get;
            set;
        }

        private VM_PluginsList vM_FilterList
        {
            get;
            set;
        }
        private VM_PluginsList vM_MetricList
        {
            get;
            set;
        }
        private VM_ProjectExplorer vM_ProjectExplorer
        {
            get;
            set;
        }

        private VM_Presentation vM_presentation
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


        #region obsolete
        //private void onMacroFilterLoad(object sender, MementoEventArgs e)
        //{
        //    Memento mem = PluginManager.pluginManager.getMemento(e.pluginKey, e.mementoName);
        //    if (mem != null)
        //    {
        //        vM_presentation.macro.macroFilter.setMemento(mem);
        //    }
        //}
        //private void onMacroMetricLoad(object sender, MementoEventArgs e)
        //{
        //    Memento mem = PluginManager.pluginManager.getMemento(e.pluginKey, e.mementoName);
        //    if (mem != null)
        //    {
        //        vM_presentation.macro.macroMetric.setMemento(mem);
        //    }
        //}
        #endregion

        public void onNewProjectCreated(object sender, ProjectEventArgs e)
        {
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
                        this.tabMetric.IsSelected = true;
                        welcomePanel.Visibility = Visibility.Collapsed;
                        runningAppPanel.Visibility = Visibility.Visible;
                        break;
                    case ViewType.FilterView:
                        this.tabFilter.IsSelected = true;
                        welcomePanel.Visibility = Visibility.Collapsed;
                        runningAppPanel.Visibility = Visibility.Visible;
                        break;
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
            this.vM_presentation.flush();
            Application.Current.Shutdown();
        }

        private void miInfo_Click(object sender, RoutedEventArgs e)
        {
            WindowOqatInfo wi = new WindowOqatInfo();
            wi.ShowDialog();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewType newv = ViewType.FilterView;
            if (tabMetric.IsSelected) newv = ViewType.MetricView;

            if (newv != currentView)
            {
                PluginManager.pluginManager.raiseEvent(PublicRessources.Plugin.EventType.toggleView,
                    new ViewTypeEventArgs(newv));
            }
        }

        private void miVidImport_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Yuv-Videos (.yuv)|*.yuv|All files (*.*)|*.*"; // Filter files by extension 
            dlg.Multiselect = true;

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                System.Collections.Specialized.StringCollection sc = new System.Collections.Specialized.StringCollection();
                sc.AddRange(dlg.FileNames);
                // Open document 
                this.vM_ProjectExplorer.importVideos(sc);
            }
        }

        private void miNewProject_Click(object sender, RoutedEventArgs e)
        {
            this.vM_Welcome.createProject();
        }

        private void miOpenProject_Click(object sender, RoutedEventArgs e)
        {
            this.vM_Welcome.openProject();
        }


    }

}

