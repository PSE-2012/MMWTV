using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oqat.PublicRessources.Plugin;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Oqat.ViewModel
{
    class PluginSandbox : IDisposable
    {
        [Import(typeof(IPlugin))]
        internal Lazy<IPlugin, IPluginMetadata> tmpPlugin
        {
            get;
            private set;
        }

        
        CompositionContainer tmpContainer;
        internal PluginSandbox(string file) {

            using (AssemblyCatalog tmpCatalog = new AssemblyCatalog(file))
            {
                tmpContainer = new CompositionContainer(tmpCatalog);
                tmpContainer.ComposeParts(this);
            }
        }

        // Dispose() calls Dispose(true)
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    // NOTE: Leave out the finalizer altogether if this class doesn't 
    // own unmanaged resources itself, but leave the other methods
    // exactly as they are. 
    ~PluginSandbox() 
    {
        // Finalizer calls Dispose(false)
        Dispose(false);
    }
    // The bulk of the clean-up code is implemented in Dispose(bool)
    protected virtual void Dispose(bool disposing)
    {
        if (disposing) 
        {
            if (tmpContainer != null)
                tmpContainer.Dispose();
        }
    }

    }
}
