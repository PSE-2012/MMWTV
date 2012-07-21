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
namespace Oqat.ViewModel
{
    /// <summary>
    /// This class initializes all viewmodels and controls the layout of Oqat.
    /// </summary>
    public partial class VM_Oqat : Window
    {
        private VM_Welcome vM_Welcome
        {
            get;
            set;
        }

        private VM_PluginLists vM_PluginList
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
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Initializes the ProjectExplorer(SmartTree and FileExplorer).
        /// </summary>
        private void initProjectExplorer()
        {
        }

        public VM_Oqat()
        {
            // PluginManager is initializet by OqatApp
            initMenu();
            initWelcome();
            initProjectExplorer();
            initPluginLists();
            initPresentation();
            initMacro();
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
            throw new System.NotImplementedException();
        }

    }
}
