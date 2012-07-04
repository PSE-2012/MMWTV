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
        /// Plugins on this list will not be returned by getPluginNames(), getPlugin(), etc.
        /// 
        /// The blackList dictionary can be used to check if a given plugin is valid (blackList does not contain the
        /// name of it) or if invalid why (value of key (pluginName) is a ErrorEventArgs List containing all
        /// violations).
        /// </summary>
        /// <remarks>
        /// Usually a plugin will be set on this list if some conventinons have been violated,
        /// i.e. ambiguous name, not implemented pluginType (<see cref="IPluginMetadata"/>).
        /// </remarks>
        private Dictionary<string, List<ErrorEventArgs>> blackList;


        [Import(typeof(IPlugin))]
        Lazy<IPlugin,IPluginMetadata> tmpPlugin { get; set; }

        
        /// <summary>
        /// Checks the PLUGIN_PATH folder for consistancy, i.e. violations of oqat plugin conventions
        /// and adds name and crime  of illegal plugins to the blackList.
        /// </summary>
        private void consistencyCheck()
        {
            SortedDictionary<string, int> tmpDictionary = new SortedDictionary<string, int>();
            blackList.Clear();
            foreach (string file in Directory.GetFiles(PLUGIN_PATH))
            {
                if (Path.GetExtension(file).Equals(".dll")) 
                {
                    AssemblyCatalog tmpCatalog;
                    CompositionContainer tmpContainer;
                    List<ErrorEventArgs> tmpList = new List<ErrorEventArgs>();
                    try
                    {
                        tmpCatalog = new AssemblyCatalog(file);
                        tmpContainer = new CompositionContainer(tmpCatalog);
                        tmpContainer.ComposeParts(this.tmpPlugin);

                        tmpDictionary.Add(tmpPlugin.Metadata.namePlugin, tmpPlugin.GetHashCode() );
                        if (!checkIfPluginTypeIsValid(tmpPlugin.Value, Type.GetType(tmpPlugin.Metadata.type.ToString())))
                        {
                            tmpList.Add(new ErrorEventArgs(new Exception("The type" +
                            " specified in the attribute pluginType(" + tmpPlugin.Metadata.type + ") is not implemented." +
                             "Oqat will ignore this assembly:" + file + ". To be able to use " + tmpPlugin.Metadata.namePlugin
                            + "Plugin it has to implement the" + tmpPlugin.Metadata.type + " interface")));

                            if (blackList.ContainsKey(tmpPlugin.Metadata.namePlugin))
                            blackList.TryGetValue(tmpPlugin.Metadata.namePlugin, out tmpList);
                            else
                            blackList.Add(tmpPlugin.Metadata.namePlugin, tmpList);
                                
                        }

                        tmpContainer.Dispose();
                        tmpCatalog.Dispose();
                    }
                    catch (Exception exc)
                    {
                        raiseEvent(EventType.info, new ErrorEventArgs(exc));
                    }
                    int tmpHashCode;
                    foreach (string entry in tmpDictionary.Keys)
                    {
                        tmpDictionary.TryGetValue(entry, out tmpHashCode);
                        if (tmpPlugin.Metadata.namePlugin.Equals(entry)
                            & (tmpPlugin.GetHashCode() != tmpHashCode)) {

                                tmpList.Add(new ErrorEventArgs(new Exception("Plugin name conflict, Oqat will"
                                + " ignore this assembly: " + file + "To be able to use " + tmpPlugin.Metadata.namePlugin
                                + "Plugin it has to export an unambiguous name.")));

                                if (blackList.ContainsKey(entry))
                                    blackList.TryGetValue(entry, out tmpList);
                                else
                                    blackList.Add(entry, tmpList);
                            }
                    }
                }
            }
        }

        /// <summary>
        /// Checks if given plugin is compatible with the generic
        /// type T.
        /// </summary>
        /// <param name="plType">Type to check plugin against.</typeparam>
        /// <param name="plugin">Plugin to check type from.</param>
        /// <returns></returns>
        private bool checkIfPluginTypeIsValid(IPlugin plugin, Type plType)
        {
            object myTestCast = null;
            try
            {
                myTestCast = Convert.ChangeType(plugin, plType);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Here happens the matching of exports provided by a Catalog with import attributes provided by
        /// this class.
        /// </summary>
        private CompositionContainer pluginContainer;

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
            blackList = new Dictionary<string, List<ErrorEventArgs>>();
            try
            {
                pluginContainer = new CompositionContainer(new DirectoryCatalog(PLUGIN_PATH));
            }
            catch (Exception exc)
            {
                /// cannot recover if someone messed within the pluginFolder
                raiseEvent(EventType.failure, new ErrorEventArgs(exc));
            }
            pluginContainer.ComposeParts(this.pluginTable);
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

