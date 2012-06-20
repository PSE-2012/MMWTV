//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    using Oqat.ViewModel.Macro;
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

		

		private void initPluginLists()
		{
			throw new System.NotImplementedException();
		}

		private void initWelcome()
		{
			throw new System.NotImplementedException();
		}

		private void initProjectExplorer()
		{
			throw new System.NotImplementedException();
		}

		public VM_Oqat()
		{
		}

		private void initMenu()
		{
			throw new System.NotImplementedException();
		}

		private void initPresentation()
		{
			throw new System.NotImplementedException();
		}

        private void initMacro() {}

		private void onBuildProjectView(object sender, EventArgs e)
		{
			throw new System.NotImplementedException();
		}

		private void onToggleView(object sender, ViewTypeEventArgs e)
		{
			throw new System.NotImplementedException();
		}

	}
}

