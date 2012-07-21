namespace Oqat.ViewModel.Macro
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Oqat.ViewModel;
    using Oqat.PublicRessources.Model;
    using Oqat.Model;

    /// <summary>
    /// This components provides the user a way to coordinate choosen filters or metrics (from the <see cref="PluginLists"/>)
    /// and invoke the according plugins (<see cref="PluginManager"/>) on the currently choosen Video/s.
    /// </summary>
	public class VM_Macro
	{
        /// <summary>
        /// Currently active view.
        /// VM_Macro is displayed if viewType == ( Filter || Metric)
        /// </summary>
		private ViewType viewType
		{
			get;
			set;
		}

        /// <summary>
        /// Temporary macro object (can be <see cref="PF_MacroFilter"/> or <see cref="PM_MacroMetric"/>).
        /// </summary>
		private Macro macro
		{
			get;
			set;
		}

        /// <summary>
        /// Currently selected reference video
        /// </summary>
		private Video vidRef
		{
			get;
			set;
		}

        /// <summary>
        /// Currently selected processed video.
        /// </summary>
		private Video vidProc
		{
			get;
			set;
		}

        /// <summary>
        /// This is were the results (filter or metric process) are placed in.
        /// </summary>
		private Video vidResult
		{
			get;
			set;
		}

		public VM_Macro()
		{
		}

        /// <summary>
        /// Raised if user changes the macroQueue ( filters or metrics currently selected for execution).
        /// </summary>
        /// <param name="sender">Caller, should be the <see cref="VM_PluginLists"/></param>
        /// <param name="e">Has to contain information about wich filter or metric were selected or changed.</param>
        private void onPreviewLoad(object sender, EventArgs e) { }

		private void onToggleView(object sender, ViewTypeEventArgs e)
		{
			throw new System.NotImplementedException();
		}

        /// <summary>
        /// Raised if user clicks on the process button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Should contain informations about the result video i.e. name, format.</param>
		private void onStartProcess(object sender, EventArgs e)
		{
			throw new System.NotImplementedException();
		}

		private void onEntrySelect(object sender, MementoEventArgs e)
		{
			throw new System.NotImplementedException();
		}

        /// <summary>
        /// Raised if user wishes to save current macroQueue for later use.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">name the new macro should have.</param>
		private void onMacroSave(object sender, EventArgs e)
		{
			throw new System.NotImplementedException();
		}

	}
}

