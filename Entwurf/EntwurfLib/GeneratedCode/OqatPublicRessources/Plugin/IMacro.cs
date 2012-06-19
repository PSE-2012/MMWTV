//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace OqatPublicRessources.Plugin
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public interface IMacro  : IPlugin
	{
        public delegate void macroEntryClickedHandler(object sender, EventArgs e);
		event macroEntryClickedHandler MacroEntryClicked;

		void createNewMemento(List<MacroEntry> macroEntrys, string mementoName);

		List<MacroEntry> getPluginMementoList();

	}
}

