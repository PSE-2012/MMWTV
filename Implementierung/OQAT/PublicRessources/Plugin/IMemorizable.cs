//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.PublicRessources.Plugin
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    using Oqat.PublicRessources.Model;

    /// <summary>
    /// If a plugin has properties it wants to be saved it has to
    /// implement this interface so the Pluginmanager 
    /// can collect and pass them to the Caretaker.
    /// </summary>
	public interface IMemorizable 
	{
        /// <summary>
        /// Getter for properties to save.
        /// </summary>
        /// <returns>Properties to save</returns>
		Memento getMemento();

        /// <summary>
        /// Setter for properties (recoverd from disk by the Caretaker, passed by the PluginManager)
        /// </summary>
        /// <param name="memento">Properties to set.</param>
		void setMemento(Memento memento);

	}
}

