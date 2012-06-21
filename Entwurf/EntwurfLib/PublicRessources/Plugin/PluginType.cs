namespace Oqat.PublicRessources.Plugin
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    /// <summary>
    /// This enum contains all types of plugins known to the <see cref="PluginManager"/>.
    /// A Plugin has to be of some of this types to be compatible to oqat.
    /// </summary>
	public enum PluginType : int
	{
		Filter,
		Metric,
		System,
		Presentation,
		VideoHandler,
		Macro,
	}
}
