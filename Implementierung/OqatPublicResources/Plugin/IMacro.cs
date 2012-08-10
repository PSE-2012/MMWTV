namespace Oqat.PublicRessources.Plugin
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;


    /// <summary>
    /// Every macro has to implement this interface.
    /// Methods within this interface are used for actual processing (i.e. invoking plugins on a video).
    /// </summary>
	public interface IMacro : IPlugin
	{
        /// <summary>
        /// Returns names of plugins and mementos a macroplugin hides.
        /// </summary>
        /// <returns></returns>
		List<MacroEntry> getPluginMementoList();
	}
}

