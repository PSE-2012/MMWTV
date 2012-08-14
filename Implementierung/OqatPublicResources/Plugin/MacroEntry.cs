namespace Oqat.PublicRessources.Plugin
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
    using System.Runtime.Serialization;

    /// <summary>
    /// This class represents a particular plugin and its memento wich enables a way to store such
    /// processing informatin within a PF_Macro or PM_Metric object.
    /// See MacroEntryMetric and MacroEntryFilter for usecases.
    /// </summary>
    [Serializable()]
	public abstract class MacroEntry
	{
        protected string _pluginName;
        protected string _mementoName;

        public string pluginName
        {
            get
            {
                return this._pluginName;
            }
            set
            {
                this._pluginName = value;
            }
        }

        public string mementoName
        {
            get
            {
                return this._mementoName;
            }
            set
            {
                this._mementoName = value;
            }
        }

        
    }
}

