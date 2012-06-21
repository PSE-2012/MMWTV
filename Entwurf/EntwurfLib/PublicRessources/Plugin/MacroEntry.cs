namespace Oqat.PublicRessources.Plugin
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    /// <summary>
    /// This class represents a particular plugin and its memento wich enables a way to store such
    /// processing informatin within a <see cref="PF_Macro"/> or <see cref="PM_Metric"/> object.
    /// See <see cref="MacroEntryMetric"/> and <see cref="MacroEntryFilter"/> for usecases.
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

