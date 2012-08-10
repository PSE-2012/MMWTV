//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.ViewModel.Macro
{
    using Oqat.PublicRessources.Plugin;
    using Oqat.PublicRessources.Model;
    using Oqat.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;
    using System.Windows.Controls;
    using AC.AvalonControlsLibrary.Controls;
    using System.ComponentModel.Composition;

    
    [ExportMetadata("namePlugin", "PF_MacroFilter")]
    [ExportMetadata("type", PluginType.IFilterOqat)]
    [Export(typeof(IPlugin))]

    /// <summary>
    /// This class is a implementation of IFilterOqat, <see cref="IFilterOqat"/> for further informations.
    /// Besides this class inherits from the abstract class <see cref="Macro"/> which in turn
    /// only implements IMacro, see <see cref="IMacro"/> for further informations.
    /// </summary>
    public class PF_MacroFilter : Macro, IFilterOqat
    {
        internal List<RangeSlider> rsl;
        public MacroFilterControl macroControl;

        public UserControl propertyView
        {
            get
            {
                return macroControl;
            }
        }

        public PF_MacroFilter()
        {
            macroQueue = new DataTable();
            macroQueue.Columns.Add("Start", typeof(Double));
            macroQueue.Columns.Add("Stop", typeof(Double));
            macroQueue.Columns.Add("Filter", typeof(String));
            macroQueue.Columns.Add("Properties", typeof(String));
            macroQueue.Columns.Add("Macro Entry", typeof(MacroEntryFilter));
            rsl = new List<RangeSlider>();
        }

        public System.Drawing.Bitmap process(System.Drawing.Bitmap frame) // TODO: What should this method actually be used for?
        {
            throw new NotImplementedException();
        }

        private IVideoHandler refHand;
        private IVideoHandler resultHand;
        private int BUFFERSIZE; // should be tested with different buffer sizes later
        private int totalFrames;
        private int i;
        private System.Drawing.Bitmap[] resultFrames;
        private IFilterOqat currentPlugin;
        private Memento currentMemento;
        private MacroEntryFilter currentMacroEntry;

        private void macroEncode(Memento memento)
        {
            currentPlugin = null; // to avoid loading the same plugin twice
            IFilterOqat currentPluginEntry;
            Memento currentMementoEntry;
            List<MacroEntry> macroEntrys = (List<MacroEntry>)memento.state;
            foreach (MacroEntry currentEntry in macroEntrys)
            {
                currentPluginEntry = (IFilterOqat)PluginManager.pluginManager.getPlugin<IPlugin>(currentEntry.pluginName);
                currentMementoEntry = PluginManager.pluginManager.getMemento(currentEntry.pluginName, currentEntry.mementoName);
                if (currentPluginEntry is IMacro)
                {
                    macroEncode(currentMementoEntry);
                }
                else
                {
                    MacroEntryFilter currentFilterEntry = (MacroEntryFilter)currentEntry;
                    // here error handling in case the plugin doesn't implement IFilterOqat
                    int arraycount = resultFrames.Count();
                    for (int j = 0; j < arraycount; j++)
                    {
                        // TODO: Handling of an entry that is another macrofilter - what to do with the slider values?
                        if ((currentFilterEntry.startFrameRelative / 100) * totalFrames <= (i+j) && (i+j) <= (currentFilterEntry.endFrameRelative / 100) * totalFrames)
                        {
                            currentPluginEntry.setMemento(currentMementoEntry);
                            System.Drawing.Bitmap tempmap = currentPluginEntry.process(resultFrames[j]);
                            resultFrames[j] = tempmap;
                        }
                    }
                }
            }
        }

        private void mementoProcess(Memento memento)
        {
            int arraycount = resultFrames.Count();
            for (int j = 0; j < arraycount; j++) // iterate over all frames to be processed
            {
                if ((currentMacroEntry.startFrameRelative / 100) * totalFrames <= (i + j) && (i + j) <= (currentMacroEntry.endFrameRelative / 100) * totalFrames)
                {
                    currentPlugin.setMemento(memento);
                    System.Drawing.Bitmap tempmap = currentPlugin.process(resultFrames[j]);
                    resultFrames[j] = tempmap;
                }
            }
        }

        public void init(Video vidRef, Video vidResult)
        {
            refHand = vidRef.handler;
            resultHand = vidResult.handler;
            BUFFERSIZE = 255;
            totalFrames = vidRef.vidInfo.frameCount;
            i = 0;
        }

        public void process(Video vidRef, Video vidResult)
        {
            while (i < totalFrames)
            {
                if ((i + BUFFERSIZE - totalFrames) > 0)
                {
                    resultFrames = refHand.getFrames(i, totalFrames - i);
                }
                else
                {
                    resultFrames = refHand.getFrames(i, BUFFERSIZE); // initialize the first BUFFERSIZE frames to be processed
                }
                foreach (DataRow c in macroQueue.Rows)
                {
                    // here maybe error handling in case the plugin doesn't implement IFilterOqat, although plugin lists has probably checked that already
                    currentPlugin = (IFilterOqat)PluginManager.pluginManager.getPlugin<IPlugin>((String)c["Filter"]);
                    currentMemento = PluginManager.pluginManager.getMemento((String)c["Filter"], (String)c["Properties"]);
                    currentMacroEntry = (MacroEntryFilter)c["Macro Entry"];
                    if (currentPlugin is IMacro)
                    {
                        macroEncode(currentMemento);
                    }
                    else
                    {
                        mementoProcess(currentMemento);
                    }
                }
                resultHand.writeFrames(i, resultFrames); // write the processed frames to disk
                i += BUFFERSIZE;
            }
            resultFrames = null;
            currentPlugin = null;
            currentMemento = null;
            refHand = null;
            resultHand = null;
            PluginManager.pluginManager.raiseEvent(PublicRessources.Plugin.EventType.macroProcessingFinished, new VideoEventArgs(vidResult));
        }

        public string namePlugin
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public PluginType type
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Dictionary<EventType, List<Delegate>> getEventHandlers()
        {
            throw new NotImplementedException();
        }

        public PublicRessources.Model.Memento getMemento()
        {
            throw new NotImplementedException();
        }

        public List<MacroEntry> getPluginMementoList()
        {
            macroEntryList = new List<MacroEntry>();
            foreach (DataRow c in macroQueue.Rows)
            {
                MacroEntryFilter newEntry = (MacroEntryFilter)c["Macro Entry"];
                macroEntryList.Add(newEntry);
            }
            return macroEntryList;
        }

        public void setMemento(PublicRessources.Model.Memento memento)
        {
            throw new NotImplementedException();
        }

    }

}

