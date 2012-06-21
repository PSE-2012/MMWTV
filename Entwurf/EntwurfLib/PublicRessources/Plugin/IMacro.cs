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
	public interface IMacro
	{

        void createNewMemento(List<MacroEntry> macroEntrys, string mementoName);

        /// <summary>
        /// Returns names of plugins and mementos a macroplugin hides.
        /// </summary>
        /// <returns></returns>
		List<MacroEntry> getPluginMementoList();

        /// <summary>
        /// Loads all plugins from the <see cref="PluginManager"/> contained in the macro entry list.
        /// </summary>
        void fetchPlugins();

        /// <summary>
        /// As the properties view of a macroplugin does not contained properties but plugins hidden by a particular
        /// macro this delegate is responsible for opening the properties view of the plugin the user clicks on (in
        /// the properties view of a macro).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void onPluginEntrySelected(object sender, EventArgs e);
	}
}

