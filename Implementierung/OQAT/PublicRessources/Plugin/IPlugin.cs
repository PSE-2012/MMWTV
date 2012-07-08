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
        /// Displays a view with GUI components of the plugin in the given parent container.
        /// </summary>
        /// <param name="parent">panel where the plugin may place own gui components</param>
		void setParentControl(Panel parent);

        /// <summary>
        /// If a plugin wants to listen for some events of <see cref="EventType"/> it has to
        /// return the event handler and the corresponding types in this method.
        /// </summary>
        /// <returns></returns>
		Dictionary<EventType,List<Delegate>> getEventHandlers();


	}
}

