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
    public abstract class Macro : IMacro
    {
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
        /// New Macro Memento
        /// </summary>
        /// <param name="macroEntrys">List of Plugins and their settings</param>
        /// <param name="mementoName">Name the Macro Memento has</param>
        public void createNewMemento(List<MacroEntry> macroEntrys, string mementoName)
        {
            Memento newMacroMemento = new Memento(mementoName, macroEntrys);
            // SaveMementoEventArgs memArgs = new SaveMementoEventArgs();
            // memArgs.pluginKey = "macro";
            // memArgs.mementoName = mementoName;
            // memArgs.memento = newMacroMemento;
            // PluginManager.pluginManager.raiseEvent(EventType.newMementoCreated, memArgs);
            //TODO: memento saving method in pluginmanager
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
        /// As the properties view of a macroplugin does not contained properties but plugins hidden by a particular
        /// macro this delegate is responsible for opening the properties view of the plugin the user clicks on (in
        /// the properties view of a macro).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void onPluginEntrySelected(object sender, EventArgs e)
        {
            //TODO: show properties view of the plugin the user clicks on (in
            // the properties view of a macro).
            throw new NotImplementedException();
        }

        public Dictionary<EventType, List<Delegate>> getEventHandlers()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a Macro Memento
        /// </summary>
        /// <returns>A Macro Memento</returns>
        public Memento getMemento()
        {
            throw new NotImplementedException();
            //return this.macroMemento;
        }

        /// <summary>
        /// Set a Macro Memento
        /// </summary>
        /// <param name="memento">The Memento that should be set as Macro Memento</param>
        public void setMemento(Memento memento)
        {
            throw new NotImplementedException();
            //this.macroMemento = memento;
        }
    }
}



