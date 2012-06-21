//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.ViewModel.Macro
{
	using Oqat.PublicRessources.Plugin;
	using Plugins;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    /// <summary>
    /// This class implements the IMacro interface, see <see cref="IMacro"/> for further information
    /// </summary>
	public abstract class Macro : IMacro
	{


        public void createNewMemento(List<MacroEntry> macroEntrys, string mementoName)
        {
            throw new NotImplementedException();
        }

        public List<MacroEntry> getPluginMementoList()
        {
            throw new NotImplementedException();
        }


        public void fetchPlugins()
        {
            throw new NotImplementedException();
        }

        public void onPluginEntrySelected(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}

