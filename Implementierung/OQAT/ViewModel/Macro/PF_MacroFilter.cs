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
    using System.Drawing;
    using System.Threading;
    using System.Diagnostics;

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
        public string namePlugin 
        {
            get
            {
                return "PF_MacroFilter";
            }
        }

        public PluginType type
        {
            get { return PluginType.IFilterOqat; }
        }

        internal List<RangeSlider> rsl;
        internal ObservableCollection<MacroEntryFilter> macroQueue
        {
            get;
            set;
        }

        public PF_MacroFilter()
        {
            macroQueue = new ObservableCollection<MacroEntryFilter>();
            rsl = new List<RangeSlider>();
            this.macroQueue.CollectionChanged += macroQueue_collectionChanged;
            
            macroControl = new MacroFilterControl(this);
        }



        public void addFilter(MementoEventArgs e)
        {
            long startValue = 0;
            long stopValue = 100;
            long startValueSlider = 0;
            long stopValueSlider = 500;
            MacroEntryFilter mEntryFilter = new MacroEntryFilter(e.pluginKey, e.mementoName, stopValue, startValue);
            
            RangeSlider rs = new AC.AvalonControlsLibrary.Controls.RangeSlider();
            rs.RangeStart = startValueSlider;
            rs.RangeStop = stopValueSlider;
            rs.RangeStartSelected = startValueSlider;
            rs.RangeStopSelected = stopValueSlider;
            rs.MinRange = 1L;
            rs.Width = 270; // TODO: changing width at runtime
            rs.Height = 17.29; // this height fits the height of the data rows in the macro table
            
            this.macroQueue.Add(mEntryFilter);
            int j = this.macroQueue.Count - 1;
            ((MacroFilterControl)macroControl).addDelegate(rs, j, delList);
            this.rsl.Add(rs);
            ((MacroFilterControl)macroControl).updateSliders();
        }

        /// <summary>
        /// Method to update macroQueue if slider changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        }

        private IVideoHandler refHand;
        private int totalFrames;
        private int i = 0;
        private Bitmap resultFrame;
        private IFilterOqat currentPlugin;
        private Memento currentMemento;
        private MacroEntryFilter currentMacroEntry;
        private bool isMacro;
        private double maxprogress;
        private double progress;
        private Progressbar progressbar;

        /// <summary>
        /// Method to split macro in it's Macroentrys
        /// </summary>
        /// <param name="memento"></param>
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
                    mementoProcess(currentMementoEntry);
                }
            }
        }

        /// <summary>
        /// Method to assign settings to plugin and write temporary video
        /// </summary>
        /// <param name="memento">settings of used plugin</param>
        private void mementoProcess(Memento memento)
        {
                if ((currentMacroEntry.startFrameRelative / 100) * totalFrames <= i && i <= (currentMacroEntry.endFrameRelative / 100) * totalFrames)
                {
                    currentPlugin.setMemento(memento);
                    System.Drawing.Bitmap tempmap = currentPlugin.process(resultFrame);
                    resultFrame = tempmap;
                }
        }

        /// <summary>
        /// Method to initialize Data for process
        /// </summary>
        /// <param name="vidRef">video to process</param>
        /// <param name="vidResult">new video after process</param>
        public void init(Video vidRef, Video vidResult)
        {
            refHand = vidRef.handler;
            refHand.setReadContext(vidRef.vidPath, vidRef.vidInfo);
            refHand.setWriteContext(vidResult.vidPath, vidRef.vidInfo);
            totalFrames = vidRef.vidInfo.frameCount;
            i = 0;
            maxprogress = totalFrames * macroQueue.Count;
        }

        private void ProgressBarThread()
        {
            progressbar = new Progressbar();
            progressbar.progressBar1.Value = 0;
            progressbar.percent.Text = 0.ToString() + "%";
            progressbar.Show();
            double displayedProgress = 0;
            Stopwatch stopwatch = Stopwatch.StartNew();
            int wait = 500;
            while (i < totalFrames)
            {
                if (stopwatch.ElapsedMilliseconds >= wait)
                {
                    if (progress != displayedProgress)
                    {
                        progressbar.progressBar1.Value = progress;
                        progressbar.percent.Text = progress.ToString() + "%";
                        displayedProgress = progress;
                    }
                }
                stopwatch.Reset();
            }
            progressbar.Close();
        }

        /// <summary>
        /// Assign frames to macroEncode/mementoProcess and write the result to disk
        /// </summary>
        /// <param name="vidRef">video to process</param>
        /// <param name="vidResult">new video after process</param>
        public void process(Video vidRef, Video vidResult)
        {
            Thread thread = new Thread(new ThreadStart(ProgressBarThread));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            foreach (MacroEntryFilter c in macroQueue)
            {
                currentPlugin = (IFilterOqat)PluginManager.pluginManager.getPlugin<IPlugin>((String)c.pluginName);
                currentMemento = PluginManager.pluginManager.getMemento((String)c.pluginName, (String)c.mementoName);
                currentMacroEntry = (MacroEntryFilter)c;
                isMacro = false;
                // decide if a macro is used
                if (currentPlugin is IMacro)
                {
                    isMacro = true;
                }
                // set reader to begin of the yuv
                refHand.positionReader = 0;
                while (i < totalFrames)
                {
                    resultFrame = refHand.getFrame();
                    if (isMacro == true)
                    {
                        macroEncode(currentMemento);
                    }
                    else
                    {
                        mementoProcess(currentMemento);
                    }
                    Bitmap[] tmp= new Bitmap[1];
                    tmp[0] = resultFrame;
                    refHand.writeFrames(i, tmp);
                    i++;
                    progress += (1 / maxprogress) * 100;
                }
            i = 0;
            isMacro = false;
            refHand.setReadContext(vidResult.vidPath, vidResult.vidInfo);
            }
            // reset after finished work
            resultFrame = null;
            currentPlugin = null;
            currentMemento = null;
            refHand = null;
            thread.Abort();
            progressbar = null;
            // add to ProjectExplorer
            PluginManager.pluginManager.raiseEvent(PublicRessources.Plugin.EventType.macroProcessingFinished, new VideoEventArgs(vidResult));
        }

        public System.Drawing.Bitmap process(System.Drawing.Bitmap frame) 
        {
            //TODO: Do we ever use a macro like a filter without checking if it is macro?
            throw new NotImplementedException();
        }


        public override Memento getMemento()
        {
            return new Memento(this.namePlugin, this.macroQueue.ToArray());
        }

        public override void setMemento(Memento memento)
        {
            this.macroQueue.Clear();
            foreach(MacroEntryFilter f in ((MacroEntryFilter[])memento.state))
            {
                this.macroQueue.Add(f);
            }
        }
    }
}

