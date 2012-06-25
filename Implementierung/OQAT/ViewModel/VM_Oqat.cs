namespace Oqat.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    using Oqat.ViewModel.Macro;

    /// <summary>
    /// This class initializes all viewmodels and controls the layout of Oqat.
    /// </summary>
	public class VM_Oqat
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
			throw new System.NotImplementedException();
		}

		public VM_Oqat()
		{
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
        private void initMacro() {}


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

