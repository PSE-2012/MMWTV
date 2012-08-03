namespace Oqat.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    /// <summary>
    /// This enum containis different Views, according to which components can decide which
    /// content they want to show.
    /// </summary>
	public enum ViewType : int
	{
        /// <summary>
        /// The view displayed if no Project is open.
        /// Contains:
        /// <see cref="VM_Welcome"/>
        /// <see cref="VM_Menu"/>
        /// </summary>
		WelcomeView,

        /// <summary>
        /// The view displayed if a project is open and the FilterList tab
        /// from the <see cref="VM_PluginLists"/> Tabcontrol is open.
        /// Contains:
        /// <see cref="VM_ProjectExplorer"/>
        /// <see cref="VM_Menu"/>
        /// <see cref="VM_Macro"/>
        /// <see cref="VM_PluginLists"/>
        /// <see cref="VM_Presentation_org"/>
        /// </summary>
		FilterView,

        /// <summary>
        /// The view displayed if a project is open and the MetricList tab
        /// from the <see cref="VM_PluginLists"/> Tabcontrol is open.
        /// Contains:
        /// <see cref="VM_ProjectExplorer"/>
        /// <see cref="VM_Menu"/>
        /// <see cref="VM_Macro"/>
        /// <see cref="VM_PluginLists"/>
        /// <see cref="VM_Presentation_org"/>
        /// 
        /// Despite this looks exactly like the FilterView ViewType, the particular
        /// components (especially VM_Macro and Presentation) behave very differently
        /// than in FilterView.
        /// </summary>
		MetricView,

        /// <summary>
        /// The view is used to display analysis results, either after processing a metric or
        /// selcting a analyse result file from the SmartTree.
        /// Contains:
        /// <see cref="VM_ProjectExplorer"/>
        /// <see cref="VM_Menu"/>
        /// <see cref="VM_Presentation_org"/>
        /// </summary>
		AnalyzeView,
	}
}
