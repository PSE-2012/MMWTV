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
        }
}
