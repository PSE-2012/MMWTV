using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;


namespace Oqat.PublicRessources.Plugin
{
    public interface IMacroEntry : INotifyPropertyChanged
    {


        string pluginName { get;}

        PluginType type { get; }

        string mementoName { get; }

        List<IMacroEntry> macroEntries { get;}


        int frameCount { get; }

        long endFrameAbs { get; }

        long startFrameAbs { get; }
    }
}
