namespace Oqat.PublicRessources.Plugin
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    /// <summary>
    /// This enum contains all types of plugins known to the PluginManager.
    /// A Plugin has to be of some of this types to be compatible to oqat.
    /// </summary>
	public enum PluginType : int
	{
		IFilterOqat,
		IMetric,
		System,
		IPresentation,
		IVideoHandler,
		IMacro,
        IPlugin,
	}
}
