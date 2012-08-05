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
		public string pluginKey
		{
			get;
			private set;
		}

        /// <summary>
        /// Name of the Memento.
        /// </summary>
		public string mementoName
		{
			get;
			private set;
		}

        public MementoEventArgs(string mementoName, string pluginKey)
        {
            this.mementoName = mementoName;
            this.pluginKey = pluginKey;
        }

	}
}

