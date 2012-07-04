namespace Oqat.ViewModel
{
	using Oqat.Model;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    using Oqat.PublicRessources.Model;
    using Oqat.PublicRessources.Plugin;
    using System.IO;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;

    /// <summary>
    /// This class is responsible for Dependency Injection trough the MEF Framework.
    /// It is responsible for getting all compatible plugins out of the plugin folder.
    /// Besides methods for plugin management, other (Oqat intern, i.e. not plugins) components
    /// can register for and raise events for a specified <see cref="EventType"/>.
    /// Pluginsmanager is a singleton.
    /// </summary>
	public class PluginManager
	{
        /// <summary>
        /// The one and only PluginManager instance in a running Oqat instance.
        /// </summary>
        internal static PluginManager pluginManager
        {
            get
            {
                if (pluginManager == null)
                    return new PluginManager();
                else
                    return pluginManager;
            }
        }

        /// <summary>
        /// In this Dictionary PluginManager holds references to all known plugins.
        /// All plugins of some <see cref="PluginType"/> or a particular
        /// plugin can be found by providing a PluginType or additionally a
        /// PluginName ( usefull for <see cref="VideoHandler"/> since we have
        /// to determine (at runtime) which videoCodec has to be used).
        /// </summary>
        [ImportMany(typeof(IPlugin), AllowRecomposition = true, RequiredCreationPolicy = CreationPolicy.Shared)]
        private IEnumerable<Lazy<IPlugin, IPluginMetadata>> pluginTable { get; set; }


        /// <summary>
        /// FileSystemWatcher to monitor changes within the pluginPath folder.
        /// </summary>
        private FileSystemWatcher watcher;

        /// <summary>
        /// This is the path where PluginManager will search for plugins.
        /// </summary>
		private string pluginPath
		{
			get;
			set;
		}

        /// <summary>
        /// Used to store registered eventhandler.
        /// </summary>
		private Dictionary<EventType, Delegate > handlerTable
		{
			get;
			set;
		}

        /// <summary>
        /// All Oqat intern components can raise a event with this method,
        /// registered eventhandler should check if the raised event
        /// is valid.
        /// </summary>
        /// <param name="eType"> The type of event to raise.</param>
        public virtual void raiseEvent(EventType eType, EventArgs e)
        {
        }

        /// <summary>
        /// This is (and should remain so) the only point to work with MEF.
        /// This Method fills the <see cref="pluginTable"/> .
        /// loadPluginTable will be called in the PluginManager constructor
        /// and if the plugin folder was changed ( a new plugin was dropped in it).
        /// </summary>
		public virtual void loadPluginTable()
		{
			throw new System.NotImplementedException();
		}

        /// <summary>
        /// Nothing happens here yet. (implementation details)
        /// </summary>
		private PluginManager()
		{
		}

        /// <summary>
        /// This method allow to get a particular memento.
        /// </summary>
        /// <param name="namePlugin">The name of the plugin the memento belongs to. </param>
        /// <param name="nameMemento">The name of the memento you are looking for.</param>
        /// <returns></returns>
		public virtual Memento getMemento(string namePlugin, string nameMemento)
		{
			throw new System.NotImplementedException();
		}

		public virtual IPlugin getPlugin(string namePlugin)
		{
			throw new System.NotImplementedException();
		}

        /// <summary>
        /// Returns names of all Plugins of a given PluginType, usefull
        /// when you want to display them in a list.
        /// </summary>
        /// <param name="type">The PluginType you want a list for.</param>
        /// <returns></returns>
		public virtual List<string> getPluginNames(PluginType type)
		{
			throw new System.NotImplementedException();
		}

        /// <summary>
        /// Provides a way to register a given delgate for a 
        /// event of a given EventType.
        /// </summary>
        /// <param name="eType">The EventType to listen.</param>
        /// <param name="handler">The handler to call if a event is raised.</param>
		public virtual void addEventHandler(EventType eType, Delegate handler)
		{
			throw new System.NotImplementedException();
		}


        /// <summary>
        /// Provides a way to unregister a delegate from an Event.
        /// </summary>
        /// <param name="eType">The EventType the given handler is registered.</param>
        /// <param name="handler">The handler to unregister.</param>
		public virtual void removeEventHandler(EventType eType, Delegate handler)
		{
			throw new System.NotImplementedException();
		}

        /// <summary>
        /// Return all known mementos of a plugin.
        /// </summary>
        /// <param name="namePlugin"></param>
        /// <returns></returns>
		public virtual List<String> getMementoNames(string namePlugin)
		{
			throw new System.NotImplementedException();
		}

	}
}

