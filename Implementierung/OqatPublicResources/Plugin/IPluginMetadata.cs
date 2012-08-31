using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oqat.PublicRessources.Plugin
{
    /// <summary>
    /// Each Plugin has to export this metadata in
    /// order to be loaded correctly.
    /// Sample export:
    ///     [ExportMetadata("namePlugin", "FilterPlugin")]
    ///     [ExportMetadata("type", PluginType.Filter)]
    ///     [ExportMetadata("threadSafe", false)]
    /// </summary>
        public interface IPluginMetadata
        {
            /// <summary>
            /// Please note that this name has to be unambiguous, if another
            /// plugin with the same name will be found Oqat has to stop execution
            /// and wait untill the user has moved the conflicting plugin from
            /// the plugin folder.
            /// </summary>
            string namePlugin { get; }

            /// <summary>
            /// Please note that this type has to be implemented (i.e. type = Filter -> plugin has to 
            /// implement the IFilterOqat interface). If this type is not implemented, the PluginManager
            /// will ignore this plugin.
            /// </summary>
            PluginType type { get; }

            /// <summary>
            /// Indicates whether a given plugin is threadsafe and is set to "true" if it is. If it is set
            /// to false, the pluginManager will enforce that no instance of a as not threadsafe marked plugin
            /// is used by more than one thread.
            /// </summary>
            /// <remarks>
            /// Note that <see cref="createPluginInstance"/> has to be implemented in order for this feateure to work properly.
            /// If a plugin is not marked as threadsafe but "createPluginInstance" does not return a working copy
            /// the corresponding plugin will be set on the blackList.
            /// 
            /// </remarks>
            bool threadSafe
            {
                get;
            }
        }
}
