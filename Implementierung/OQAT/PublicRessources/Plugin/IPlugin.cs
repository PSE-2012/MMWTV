namespace Oqat.PublicRessources.Plugin
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    using System.Windows.Controls;

    /// <summary>
    /// Every plugin has to implement this interface to be compatible with <see cref="PluginManager"/>.
    /// </summary>
	public interface IPlugin  : IMemorizable
	{

        /// <summary>
        /// A panel where the plugin may place own gui components
        /// </summary>
        /// <param name="parent"></param>
		void setParentControl(Panel parent);

        /// <summary>
        /// If a plugin wants to listen for some events of <see cref="EventType"/> it has to
        /// return the event handler and the corresponding types in this method.
        /// </summary>
        /// <returns></returns>
		Dictionary<EventType,List<Delegate>> getEventHandlers();


	}
}

