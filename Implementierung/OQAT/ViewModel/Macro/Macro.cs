//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.ViewModel.Macro
{
	using Oqat.PublicRessources.Plugin;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
    using System.Windows.Controls;
    using Oqat.PublicRessources.Model;
    using Oqat.Model;
    using System.Data;
    using AC.AvalonControlsLibrary.Controls;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// This class implements the IMacro interface, see <see cref="IMacro"/> for further information
    /// </summary>
    public abstract class Macro : IMacro, IPlugin
    {
        string namePlugin
        {
            get
            {
                return "Macro";
            }
        }

        /// <summary>
        /// The Macro Queue, with Pluginnames and Mementonames
        /// </summary>
        //internal DataTable macroQueue;
        //internal ObservableCollection<MacroEntry> macroQueue;
        internal List<MacroEntry> macroEntryList;


        internal Macro()
        {
        }

        public UserControl macroControl;

        public UserControl propertyView
        {
            get
            {
                return macroControl;
            }
        }



        /// <summary>
        /// Returns names of plugins and mementos a macroplugin hides.
        /// </summary>
        /// <returns>List of Macro Entrys</returns>
        public List<MacroEntry> getPluginMementoList()
        {
            return macroEntryList;
        }


        /// <summary>
        /// Get a Macro Memento
        /// </summary>
        /// <returns>A Macro Memento</returns>
        public Memento getMemento()
        {
            return new Memento(this.namePlugin, this.getPluginMementoList());
        }

        /// <summary>
        /// Set a Macro Memento
        /// </summary>
        /// <param name="memento">The Memento that should be set as Macro Memento</param>
        public void setMemento(Memento memento)
        {
            this.macroEntryList = (List<MacroEntry>)memento.state;
        }


    }
}



