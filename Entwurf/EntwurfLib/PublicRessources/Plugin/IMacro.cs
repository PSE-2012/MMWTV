namespace Oqat.PublicRessources.Plugin
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public interface IMacro
	{

        void createNewMemento(List<MacroEntry> macroEntrys, string mementoName);

		List<MacroEntry> getPluginMementoList();

        void fetchPlugins();
        void onPluginEntrySelected(object sender, EventArgs e);
	}
}

