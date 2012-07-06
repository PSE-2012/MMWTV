using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oqat.PublicRessources.Plugin;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Oqat.ViewModel
{
    class PluginSandbox
    {
        [Import(typeof(IPlugin))]
        internal Lazy<IPlugin, IPluginMetadata> tmpPlugin
        {
            get;
            private set;
        }

        
        CompositionContainer tmpContainer;
        internal PluginSandbox(string file) {

            AssemblyCatalog tmpCatalog = new AssemblyCatalog(file);
            tmpContainer = new CompositionContainer(tmpCatalog);
            tmpContainer.ComposeParts(this);
        }
    }
}
