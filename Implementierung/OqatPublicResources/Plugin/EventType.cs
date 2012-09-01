//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.PublicRessources.Plugin
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    /// <summary>
    /// This enum defines all public event types known to oqat components.
    /// </summary>
	public enum EventType : int
	{
        /// <summary>
        /// Will be raised if a video (VM_ProjectExplorer, VM_Presentation) was selected
        /// to load into a player.
        /// </summary>
		videoLoad,
        /// <summary>
        /// Will be raised if a plugin has detected the changes in his current memento.
        /// </summary>
		saveMacroCreated,

        /// <summary>
        /// Will be raised if a new video object was created ( VM_VideoImportDialog).
        /// </summary>
		vidObjectCreated,

        /// <summary>
        /// Will be raised if some minor errors were discovered and Oqat was capable of recovery or
        /// if some optional information is available.
        /// </summary>
        info,
        /// <summary>
        /// Will be raised if some Error were Discovered and Oqat needs user Input to recover.
        /// </summary>
        panic,
        /// <summary>
        /// Will be raised if some Error were Discovered and Oqat cannot recover, usually this means
        /// oqat has to be restarted in order to gain full functionallity.
        /// </summary>
        failure,

        /// <summary>
        /// Will be raised if PluginManager refreshed(added or deleted some entries) pluginTable.
        /// </summary>
        pluginTableChanged,

        /// <summary>
        /// Will be raised if a new project was created (usually by <see cref="VM_OqatWelcome"/>).
        /// </summary>
        newProjectCreated,

        /// <summary>
        /// Will be raised if the viewType is changed in order to adapt the presentation.
        /// </summary>
        toggleView,


        /// <summary>
        /// Will be raised if a macroentry (filter/metric) is selected in the macroView.
        /// </summary>
        macroEntrySelected,

        /// <summary>
        /// Will be raised when a filter/metric is selected to be added to the current macro.
        /// </summary>
        macroEntryAdd,

        /// <summary>
        /// Will be raised after the macro finished processing/analyzing a video.
        /// </summary>
        macroProcessingFinished,


	}


}
