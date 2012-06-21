//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.PublicRessources.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    /// <summary>
    /// This class can be used to pass Mementos through events, i.e. when the user wants to save
    /// the settings of a plugin.
    /// </summary>
	public class MementoEventArgs : EventArgs
	{
        /// <summary>
        /// Name of the plugin the Memento belongs to.
        /// </summary>
		private string pluginKey
		{
			get;
			set;
		}

        /// <summary>
        /// Name of the Memento.
        /// </summary>
		private string mementoName
		{
			get;
			set;
		}

	}
}

