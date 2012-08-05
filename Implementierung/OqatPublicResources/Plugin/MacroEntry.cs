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
    [Serializable]
	public abstract class MacroEntry
	{
		private string pluginName
		{
			get;
			set;
		}

		private string mementoName
		{
			get;
			set;
		}

        //public MacroEntry(SerializationInfo info, StreamingContext context)
        //{
        //    this.pluginName = (string)info.GetValue("pluginName", typeof(string));
        //    this.mementoName = (string)info.GetValue("mementoName", typeof(string));
        //}

        //public void GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    info.AddValue("pluginName", this.pluginName);
        //    info.AddValue("mementoName", this.mementoName);
        //}
    }
}

