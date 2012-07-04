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
        /// <remarks>
        /// It will be set during construction of a new PluginManager instance
        /// and will be set to "Plugins" (relative to the codebase of the assembly 
        /// PluginManager class is in, i.e. "C:\Oqat\Plugins" if oqat.exe is in "C:\Oqat").
        /// </remarks>
        private readonly string PLUGIN_PATH;

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
        /// Constructor
        /// </summary>
		private PluginManager()
		{
            PLUGIN_PATH = Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(PluginManager)).Location) + "\\Plugins";

            watcher = new FileSystemWatcher(PLUGIN_PATH);
            watcher.Created += new FileSystemEventHandler(onPluginFolderChanged);
            watcher.Deleted += new FileSystemEventHandler(onPluginFolderChanged);
            watcher.EnableRaisingEvents = true;
		}

        /// <summary>
        /// Will be called if a file was either created or deleted.
        /// According to the type of changes (deletion, creation) new
        /// Plugins(if valid) will be loaded or existing deleted.
        /// </summary>
        /// <param name="source">The caller</param>
        /// <param name="e"></param>
        /// <remarks>
        /// If a Plugin is currently in use, the assembly cannot be unloaded till
        /// dispose() was called on that particular plugin.
        /// </remarks>
        private void onPluginFolderChanged(object source, FileSystemEventArgs e)
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

