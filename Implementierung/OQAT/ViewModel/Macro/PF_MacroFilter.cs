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
    using System.Collections.ObjectModel;
    using System.ComponentModel.Composition;
    using System.Collections.Specialized;
    using System.ComponentModel;

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
        string namePlugin 
        {
            get
            {
                return "PF_MacroFilter";
            }
        }

        internal List<RangeSlider> rsl;
        internal ObservableCollection<MacroEntryFilter> macroQueue;

        public PF_MacroFilter()
        {
            macroQueue = new ObservableCollection<MacroEntryFilter>();
            rsl = new List<RangeSlider>();
            this.macroQueue.CollectionChanged += macroQueue_collectionChanged;
            
            macroControl = new MacroFilterControl(this);
        }

        private void macroQueue_collectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e != null)
            {
                if (e.OldItems != null)
                    foreach (INotifyPropertyChanged item in e.OldItems)
                        item.PropertyChanged -= item_PropertyChanged;
                if (e.NewItems != null)
                    foreach (INotifyPropertyChanged item in e.NewItems)
                        item.PropertyChanged += item_PropertyChanged;
            }
        }

        private void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var reset = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            // this.macroQueue.OnCollectionChanged(reset);
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
            List<MacroEntryFilter> macroEntrys = (List<MacroEntryFilter>)memento.state;
            foreach (MacroEntryFilter currentEntry in macroEntrys)
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
                        if ((currentFilterEntry.endFrameRelative / 100) * totalFrames <= (i+j) && (i+j) <= (currentFilterEntry.startFrameRelative / 100) * totalFrames)
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

                    //DEBUG Pixelvergleich
                    IPlugin usedPlugin = currentPlugin;
                    var refVideoPixel = resultFrames[j].GetPixel(5,5);
                    var resulVideoPixel = tempmap.GetPixel(5,5);
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
            BUFFERSIZE = 127;
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
                foreach (MacroEntryFilter c in macroQueue)
                {
                    // here maybe error handling in case the plugin doesn't implement IFilterOqat, although plugin lists has probably checked that already
                    currentPlugin = (IFilterOqat)PluginManager.pluginManager.getPlugin<IPlugin>((String)c.pluginName);
                    currentMemento = PluginManager.pluginManager.getMemento((String)c.pluginName, (String)c.mementoName);
                    currentMacroEntry = (MacroEntryFilter)c;
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


        public System.Drawing.Bitmap process(System.Drawing.Bitmap frame) // TODO: What should this method actually be used for?
        {
            throw new NotImplementedException();
        }



        public List<MacroEntry> getPluginMementoList()
        {
            macroEntryList = new List<MacroEntry>();
            foreach (MacroEntry c in macroQueue)
            {
                MacroEntry newEntry = (MacroEntry)c;
                macroEntryList.Add(newEntry);
            }
            return macroEntryList;
        }

    }
}

