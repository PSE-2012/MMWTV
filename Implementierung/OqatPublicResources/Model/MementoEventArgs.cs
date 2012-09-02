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

        public string previousMementoName
        {
            get;
            private set;
        }

        public Memento memento;

        public delegate Memento getMemento_Delegate();
        public getMemento_Delegate getMemDel;

        public MementoEventArgs(string mementoName, string pluginKey, string previousMementoname = "", getMemento_Delegate getMemDel = null, Memento memento = null)
        {
            this.mementoName = mementoName;
            this.pluginKey = pluginKey;
            this.getMemDel = getMemDel;
            this.memento = memento;
            this.previousMementoName = previousMementoname;
        }

	}
}

