using Oqat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

using Oqat.PublicRessources.Model;
using Oqat.PublicRessources.Plugin;
using System.Diagnostics;


namespace Oqat.ViewModel
{
    /// <summary>
    /// This class is responsible for Dependency Injection trough the MEF Framework.
    /// It is responsible for getting all compatible plugins out of the plugin folder.
    /// Besides methods for plugin management, other (Oqat intern, i.e. not plugins) components
    /// can register for and raise events for a specified <see cref="EventType"/>.
    /// Pluginsmanager is a singleton.
    /// </summary>
    /// <remarks>
    /// a Plugin needs to export the following MEF stuff to be importable by PluginManager:
    /// [ExportMetadata("namePlugin", namePlugin)]
    /// [ExportMetadata("type", type)]
    /// [Export(typeof(IPlugin))]
    /// </remarks>
    public class PluginManager
    {
        private static Object key = new Object();
        private static PluginManager plMan;

        /// <summary>
        /// The one and only PluginManager instance in a running OQAT application.
        /// </summary>
        internal static PluginManager pluginManager
        {
            get
            {
                lock (key)
                {
                    if (plMan == null)
                        plMan = new PluginManager();
                    return plMan;
                }
            }
        }


        #region Initialization

        /// <summary>
        /// Constructor
        /// </summary>
        private PluginManager()
        {
            PLUGIN_PATH = Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(PluginManager)).Location) + "\\Plugins";
            if (!System.IO.Directory.Exists(PLUGIN_PATH))
            {
                System.IO.Directory.CreateDirectory(PLUGIN_PATH);
                raiseEvent(EventType.info, new ErrorEventArgs(new Exception("Cant find Pluginfolder, create new one.")));
            }
            blackList = new SortedDictionary<string, List<ErrorEventArgs>>();

            consistencyCheck();

            try
            {
                pluginCatalog = new DirectoryCatalog(PLUGIN_PATH);
                catalog = new AggregateCatalog(
                    pluginCatalog,
                    new AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly()));
            }
            catch (Exception exc)
            {
                /// cannot recover if someone messed within the pluginFolder
                raiseEvent(EventType.failure, new ErrorEventArgs(exc));
            }

            try
            {
                pluginContainer = new CompositionContainer(catalog);
                pluginContainer.ComposeParts(this);

                watcher = new FileSystemWatcher(PLUGIN_PATH);
                watcher.Created += new FileSystemEventHandler(onPluginFolderChanged);
                watcher.Deleted += new FileSystemEventHandler(onPluginFolderChanged);
                watcher.EnableRaisingEvents = true;
            }
            catch (Exception exc)
            {
                raiseEvent(EventType.failure, new ErrorEventArgs(exc));
            }
        }



        /// <summary>
        /// Here happens the matching of exports provided by a Catalog with import attributes provided by
        /// this class.
        /// </summary>
        private CompositionContainer pluginContainer;

        /// <summary>
        /// All composable parts found by MEF will be placed here.
        /// </summary>
        private DirectoryCatalog pluginCatalog;
        private AggregateCatalog catalog;

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
        public readonly string PLUGIN_PATH;

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
        private SortedDictionary<string, List<ErrorEventArgs>> blackList;



        /// <summary>
        /// Checks the PLUGIN_PATH folder for consistancy, i.e. violations of oqat plugin conventions
        /// and adds name and crime of illegal plugins to the blackList.
        /// </summary>
        private bool consistencyCheck()
        {
            bool errorsOcured;
            lock (key)
            {
                /// if no errors occured the plugin List must not be refreshed 
                /// completly, only the new one
                /// which name will be passed by the pluginTableChanged event.
                errorsOcured = false;
                SortedDictionary<string, int> tmpDictionary = new SortedDictionary<string, int>();
                blackList.Clear();
                string[] files;
                try
                {
                    files = Directory.GetFiles(PLUGIN_PATH);
                }
                catch (Exception exc)
                {
                    raiseEvent(EventType.failure, new ErrorEventArgs(exc));
                    files = null;
                    errorsOcured = true;
                }

                if (files != null)
                {
                    foreach (string file in files)
                    {
                        if (Path.GetExtension(file).Equals(".dll"))
                        {
                            Lazy<IPlugin, IPluginMetadata> tmpPlugin;
                            try
                            {
                                tmpPlugin = (new PluginSandbox(file)).tmpPlugin;
                            }
                            catch (Exception exc)
                            {
                                errorsOcured = true;

                                /// Provide a delete dialogue ?
                                /// Maybe set on a ignoreList ( not the Blacklist)
                                raiseEvent(EventType.info, new ErrorEventArgs(new Exception("Uncompatible Plugin" +
                               " was found in the Pluginfolder: " + file, exc)));
                                continue;
                            }

                            List<ErrorEventArgs> tmpList = new List<ErrorEventArgs>();

                            try
                            {
                                tmpDictionary.Add(tmpPlugin.Metadata.namePlugin, tmpPlugin.GetHashCode());

                                if (!checkIfPluginTypeIsValid(tmpPlugin.Value, tmpPlugin.Metadata.type))
                                {
                                    tmpList.Add(new ErrorEventArgs(new Exception("The type" +
                                    " specified in the attribute pluginType(" + tmpPlugin.Metadata.type + ") is not implemented." +
                                     "Oqat will ignore this assembly:" + file + ". To be able to use " + tmpPlugin.Metadata.namePlugin
                                    + "Plugin it has to implement the" + tmpPlugin.Metadata.type + " interface")));

                                    if (blackList.ContainsKey(tmpPlugin.Metadata.namePlugin))
                                        blackList.TryGetValue(tmpPlugin.Metadata.namePlugin, out tmpList);
                                    else
                                        blackList.Add(tmpPlugin.Metadata.namePlugin, tmpList);
                                    errorsOcured = true;
                                }
                            }
                            catch (Exception exc)
                            {
                                raiseEvent(EventType.info, new ErrorEventArgs(exc));
                                errorsOcured = true;
                            }

                            int tmpHashCode;
                            foreach (string entry in tmpDictionary.Keys)
                            {
                                tmpDictionary.TryGetValue(entry, out tmpHashCode);
                                if (tmpPlugin.Metadata.namePlugin.Equals(entry)
                                    & (tmpPlugin.GetHashCode() != tmpHashCode))
                                {

                                    tmpList.Add(new ErrorEventArgs(new Exception("Plugin name conflict, Oqat will"
                                    + " ignore this assembly: " + file + "To be able to use " + tmpPlugin.Metadata.namePlugin
                                    + "Plugin it has to export an unambiguous name.")));

                                    if (blackList.ContainsKey(entry))
                                        blackList.TryGetValue(entry, out tmpList);
                                    else
                                        blackList.Add(entry, tmpList);

                                    errorsOcured = true;
                                }
                            }
                        }
                    }
                }
            }
            List<ErrorEventArgs> errorList = new List<ErrorEventArgs>();
            foreach (string entry in blackList.Keys)
            {
                blackList.TryGetValue(entry, out errorList);
                foreach (ErrorEventArgs e in errorList)
                {
                    raiseEvent(EventType.info, e);
                }
            }
            return errorsOcured;
        }

        /// <summary>
        /// Checks if given plugin is compatible with the generic
        /// type T.
        /// </summary>
        /// <param name="plType">Type to check plugin against.</typeparam>
        /// <param name="plugin">Plugin to check type from.</param>
        /// <returns></returns>
        private bool checkIfPluginTypeIsValid(IPlugin plugin, PluginType plType)
        {
            bool retVal;
            switch (plType)
            {

                case PluginType.IFilterOqat:
                    retVal = castTest<IFilterOqat>(plugin);
                    break;
                case PluginType.IMacro:
                    retVal = castTest<IMacro>(plugin);
                    break;
                case PluginType.IMetricOqat:
                    retVal = castTest<IMetricOqat>(plugin);
                    break;
                case PluginType.IPresentation:
                    retVal = castTest<IPresentation>(plugin);
                    break;
                case PluginType.IVideoHandler:
                    retVal = castTest<IVideoHandler>(plugin);
                    break;
                default:
                    retVal = false;
                    break;
            }
            return retVal;
        }
        private bool castTest<T>(IPlugin plugin) where T : IPlugin
        {
            try
            {
                var tmp = (T)plugin;
            }
            catch { return false; }
            return true;
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
        /// the corresponding appdomain is unloaded.
        /// </remarks>
        private void onPluginFolderChanged(object source, FileSystemEventArgs e)
        {
            if (((File.GetAttributes(e.FullPath) & FileAttributes.Directory) != FileAttributes.Directory)
                & ((e.ChangeType == System.IO.WatcherChangeTypes.Created)
                    | (e.ChangeType == System.IO.WatcherChangeTypes.Deleted)))
            {
                var tmpSlot = consistencyCheck();
                pluginCatalog.Refresh();
                EntryEventArgs pluginTableChanges;
                if (!tmpSlot) // i.e. no errors occured
                {
                    var tmpPlugin = new PluginSandbox(e.FullPath);
                    pluginTableChanges = new EntryEventArgs(tmpPlugin.tmpPlugin.Metadata.namePlugin);
                }
                else
                {
                    pluginTableChanges = new EntryEventArgs("");
                }

                raiseEvent(EventType.pluginTableChanged, pluginTableChanges);
            }
        }

        #endregion


        #region getPlugins

        /// <summary>
        /// Will return a plugin of type T, if it is loaded and
        /// not blacklisted.
        /// </summary>
        /// <typeparam name="T">The type of plugin to return</typeparam>
        /// <param name="namePlugin">The name of plugin to return</param>
        /// <returns>A instance of namePlugin</returns>
        public virtual T getPlugin<T>(string namePlugin)
        {
            foreach (Lazy<IPlugin, IPluginMetadata> i in pluginTable)
            {
                if (i.Metadata.namePlugin.Equals(namePlugin)
                    && !blackList.ContainsKey(i.Metadata.namePlugin))
                {
                    if (i.Value is T)
                    {
                        if (i.Metadata.threadSafe)
                        {
                            return (T)i.Value;
                        }
                        else
                        {
                            var copy = i.Value.createExtraPluginInstance();

                            Debug.Assert(copy != i.Value, i.Metadata.namePlugin + 
                                " hasnt implemented the IPlugin member \"createExtraPluginInstance\" " + 
                                "correctly, this member returns the same plugin " + 
                                "despite the fact that it is marked as not thredSafe");

                            return (T)copy;
                        }
                    }
                }
            }
            return default(T);
        }

        /// <summary>
        /// Returns names of all Plugins(not blacklisted) of a given PluginType, usefull
        /// when you want to display them in a list.
        /// </summary>
        /// <param name="type">The PluginType you want a list for.</param>
        /// <returns></returns>
        public virtual List<string> getPluginNames(PluginType type)
        {
            return new List<string>(from i in pluginTable
                                    where (i.Metadata.type == type) & !blackList.ContainsKey(i.Metadata.namePlugin)
                                    select i.Metadata.namePlugin);
        }

        #endregion


        #region PluginMementos

        /// <summary>
        /// One or more Mementos sorted by the corresponding plugin.
        /// </summary>
        private SortedDictionary<string, List<Memento>> _memTable;
        private SortedDictionary<string, List<Memento>> memTable
        {
            get
            {
                if (_memTable == null)
                    initMemTable();
                return _memTable;
            }
            set
            {
                _memTable = value;
            }
        }


        /// <summary>
        /// This method allow to get a particular memento.
        /// </summary>
        /// <param name="namePlugin">The name of the plugin the memento belongs to. </param>
        /// <param name="nameMemento">The name of the memento you are looking for.</param>
        /// <returns>Memento instance according to arguments or null if no such memento was found.</returns>
        public virtual Memento getMemento(string namePlugin, string nameMemento)
        {
            List<Memento> memObjects;
            getMementoList(namePlugin, out memObjects);
            return (from i in memObjects
                    where i.name.Equals(nameMemento)
                    select i).FirstOrDefault();
        }

        private void initMemTable()
        {
            bool saveRequired = false;
            memTable = new SortedDictionary<string, List<Memento>>();
            foreach (var pl in pluginTable)
            {
                Memento tmp;
                try
                {
                    tmp = Caretaker.caretaker.getMemento(getMementoSavePath(pl.Metadata.namePlugin));
                }
                catch (Exception)
                {
                    tmp = null;
                }
                    if (tmp == null) // no memento exists
                    {
                        var memToAdd = pl.Value.getMemento();
                        var memListToAdd = new List<Memento>();
                        memListToAdd.Add(memToAdd);
                        memTable.Add(pl.Metadata.namePlugin, memListToAdd);
                        saveRequired = true;
                    }
                    else
                    {
                        var readMemento = (List<Memento>)tmp.state;
                        memTable.Add(pl.Metadata.namePlugin, readMemento);
                    }
         
            }
            if (saveRequired)
                save();
        }

        /// <summary>
        /// Return a list of all known mementos of a plugin.
        /// </summary>
        /// <param name="namePlugin">name of the Plugin to return mementos for</param>
        /// <returns>List of mementos if all went well. Empty List if no mementos are assigned to the
        /// Plugin with the given name. Null if Plugin with the name namePlugin was found.</returns>
        private void getMementoList(string namePlugin, out List<Memento> mementoObjects)
        {
            if (blackList.ContainsKey(namePlugin) || !memTable.ContainsKey(namePlugin))
            {
                mementoObjects = null;    // should never come to this, since noone can have plugin names (getPluginNames)
                raiseEvent(EventType.info, new ErrorEventArgs(new Exception("Plugin: " + namePlugin + " is blacklisted.")));
                return;                 // of blacklisted plugins
            }


            memTable.TryGetValue(namePlugin, out mementoObjects);

        }

        private string getMementoSavePath(string namePlugin)
        {
            return PLUGIN_PATH + "\\" + namePlugin + ".mem";
        }

        private void save()
        {
            string supposedPath;
            List<Memento> memObjects;
            foreach(var pl in pluginTable)
            {
                supposedPath = getMementoSavePath(pl.Metadata.namePlugin);
                getMementoList(pl.Metadata.namePlugin, out memObjects);
                Caretaker.caretaker.writeMemento(new Memento(pl.Metadata.namePlugin, memObjects, supposedPath));
            }
        }

        /// <summary>
        /// Return a list with memento names of a given plugin.
        /// </summary>
        /// <param name="namePlugin">Name of the plugin to get memento names from</param>
        /// <returns>List of known memento names. Please note that the list will be empty if no mementos could be found or 
        /// null if no corresponding plugin was found.</returns>
        internal virtual List<String> getMementoNames(string namePlugin)
        {
            List<string> nameList = new List<string>();
            List<Memento> tmpMemList;
            getMementoList(namePlugin, out tmpMemList);
            if (tmpMemList != null)
                foreach (Memento i in tmpMemList)
                {
                    nameList.Add(i.name);
                }
            else
                nameList = null;

            return nameList;
        }


        /// <summary>
        /// Writes the given Memento to disk, overwriting if a memento with the same name exists.
        /// </summary>
        /// <param name="namePlugin"></param>
        /// <param name="mem"></param>
        /// <exception cref="ArgumentException">thrown if no plugin with the given name is found.</exception>
        internal virtual void addMemento(string namePlugin, Memento mem)
        {
            List<string> memNames = getMementoNames(namePlugin);
            if (memNames == null)
                throw new ArgumentException("Plugin: " + namePlugin + " is unknown.");

            List<Memento> memObjects;
            getMementoList(namePlugin, out memObjects);
            if (memNames.Contains(mem.name))
            {
                /// change existing memento

                foreach (var i in memObjects)
                {
                    if (i.name.Equals(mem.name))
                    {
                        memObjects.Remove(i);
                        if (mem.state != null)
                        {
                            memObjects.Add(mem);
                        }
                        break;
                    }
                }
            }
            else
            {
                memObjects.Add(mem);
            }
            save();
        }

        #endregion


        #region events

        internal delegate void OqatErrorHandler(object sender, ErrorEventArgs e);
        internal static event OqatErrorHandler OqatInfo;
        internal static event OqatErrorHandler OqatPanic;
        internal static event OqatErrorHandler OqatFailure;
        internal delegate void notificationHandler(object sender, EntryEventArgs e);
        internal static event notificationHandler OqatPluginTableChanged;

        internal delegate void videoLoadHandler(object sender, VideoEventArgs e);
        internal static event videoLoadHandler videoLoad;
        internal delegate void macroEntrySelectedHandler(object sender, MementoEventArgs e);
        internal static event macroEntrySelectedHandler macroEntrySelected;
        internal delegate void newMementoCreatedHandler(object sender, MementoEventArgs e);
        internal static event newMementoCreatedHandler newMementoCreated;
        internal delegate void newProjectCreatedHandler(object sender, ProjectEventArgs e);
        internal static event newProjectCreatedHandler OqatNewProjectCreatedHandler;
        internal delegate void toggleViewHandler(object sender, ViewTypeEventArgs e);
        internal static event toggleViewHandler OqatToggleView;
        internal delegate void macroEntryAddHandler(object sender, MementoEventArgs e);
        internal static event macroEntryAddHandler macroEntryAdd;
        internal delegate void macroProcessingFinishedHandler(object sender, VideoEventArgs e);
        internal static event macroProcessingFinishedHandler macroProcessingFinished;


        /// <summary>
        /// All Oqat intern components can raise a event with this method,
        /// registered eventhandler should check if the raised event
        /// is valid.
        /// </summary>
        /// <param name="eType"> The type of event to raise.</param>
        internal virtual void raiseEvent(EventType eType, EventArgs e)
        {
            switch (eType)
            {
                case EventType.info:
                    try
                    {
                        if (OqatInfo != null)
                            OqatInfo(this, (ErrorEventArgs)e);
                    }
                    catch (Exception exc)
                    {
                        raiseEvent(EventType.info, new ErrorEventArgs(exc));
                    }
                    break;
                case EventType.toggleView:
                    try
                    {
                        if (OqatToggleView != null)
                            OqatToggleView(this, (ViewTypeEventArgs)e);
                    }
                    catch (Exception exc)
                    {
                        raiseEvent(EventType.info, new ErrorEventArgs(exc));
                    }
                    break;
                case EventType.newProjectCreated:
                    try
                    {

                        OqatNewProjectCreatedHandler(this, (ProjectEventArgs)e);
                    }
                    catch (Exception exc)
                    {
                        raiseEvent(EventType.info, new ErrorEventArgs(exc));
                    }
                    break;
                case EventType.newMementoCreated:
                    try
                    {
                        if (newMementoCreated != null)
                            newMementoCreated(this, (MementoEventArgs)e);
                    }
                    catch (Exception exc)
                    {
                        raiseEvent(EventType.info, new ErrorEventArgs(exc));
                    }
                    break;
                case EventType.macroEntrySelected:
                    try
                    {
                        if (macroEntrySelected != null)
                            macroEntrySelected(this, (MementoEventArgs)e);
                    }
                    catch (Exception exc)
                    {
                        raiseEvent(EventType.info, new ErrorEventArgs(exc));
                    }
                    break;
                case EventType.panic:
                    try
                    {
                        if (OqatPanic != null)
                            OqatPanic(this, (ErrorEventArgs)e);
                    }
                    catch (Exception exc)
                    {
                        raiseEvent(EventType.info, new ErrorEventArgs(exc));
                    }
                    break;
                case EventType.failure:
                    try
                    {
                        if (OqatFailure != null)
                            OqatFailure(this, (ErrorEventArgs)e);
                    }
                    catch (Exception exc)
                    {
                        raiseEvent(EventType.info, new ErrorEventArgs(exc));
                    }
                    break;
                case EventType.pluginTableChanged:
                    try
                    {
                        if (OqatPluginTableChanged != null)
                            OqatPluginTableChanged(this, (EntryEventArgs)e);
                    }
                    catch (Exception exc)
                    {
                        raiseEvent(EventType.info, new ErrorEventArgs(exc));
                    }
                    break;
                case EventType.videoLoad:
                    try
                    {
                        if (videoLoad != null)
                            videoLoad(this, (VideoEventArgs)e);
                    }
                    catch (Exception exc)
                    {
                        raiseEvent(EventType.info, new ErrorEventArgs(exc));
                    }
                    break;
                case EventType.macroEntryAdd:
                    try
                    {
                        if (macroEntryAdd != null)
                            macroEntryAdd(this, (MementoEventArgs)e);
                    }
                    catch (Exception exc)
                    {
                        raiseEvent(EventType.info, new ErrorEventArgs(exc));
                    }
                    break;
                case EventType.macroProcessingFinished:
                    try
                    {
                        if (macroProcessingFinished != null)
                            macroProcessingFinished(this, (VideoEventArgs)e);
                    }
                    catch (Exception exc)
                    {
                        raiseEvent(EventType.info, new ErrorEventArgs(exc));
                    }
                    break;

            }
        }

        #endregion


    }
}

