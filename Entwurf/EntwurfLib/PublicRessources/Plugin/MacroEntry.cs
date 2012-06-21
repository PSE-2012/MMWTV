namespace Oqat.PublicRessources.Plugin
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    /// <summary>
    /// This class represents a particular plugin and its memento wich enables a way to store such
    /// processing informatin within a PF_Macro or PM_Metric object.
    /// See MacroEntryMetric and MacroEntryFilter for usecases.
    /// </summary>
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

	}
}

