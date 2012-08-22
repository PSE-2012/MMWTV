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
    using System.Windows.Controls;
    using System.Data;
    using System.Drawing;
    using System.Collections.ObjectModel;
    using System.ComponentModel.Composition;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Threading;
    using System.Drawing;

    [ExportMetadata("namePlugin", "PM_MacroMetric")]
    [ExportMetadata("type", PluginType.IMetricOqat)]
    [Export(typeof(IPlugin))]

    /// <summary>
    /// This class is a implementation of IMetricOqat, <see cref="IMetricOqat"/> for further informations.
    /// Besides this class inherits from the abstract class <see cref="Macro"/> wich in turn
    /// only implements IMacro, see <see cref="IMacro"/> for further informations.
    /// </summary>
	public class PM_MacroMetric : Macro, IMetricOqat
    {
        public string namePlugin
        {
            get
            {
                return "PM_MacroMetric";
            }
        }
        public PluginType type {
            get {
                return PluginType.IMetricOqat;
            }}
        /// <summary>
        /// Metric macroQueue
        /// </summary>
        internal ObservableCollection<MacroEntryMetric> macroQueue;

        public AnalysisInfo analyse(Bitmap frameRef, Bitmap frameProc)
        {
            //TODO: Do we ever use a macro like a metric without checking if it is macro?
            throw new NotImplementedException();
        }

        public PM_MacroMetric()
        {
            macroQueue = new ObservableCollection<MacroEntryMetric>();
            macroControl = new MacroMetricControl(this);
        }

        private int totalFrames;
        private int i;
        private IVideoHandler refHand;
        private IVideoHandler procHand;
        private IMetricOqat currentPlugin;
        private Memento currentMemento;
        private AnalysisInfo analyseInfo;
        private Bitmap refFrame;
        private Bitmap procFrame;
        private Bitmap resultFrame;
        //private Thread thread = null;
        private Video[] vidRes;
        private int nextMetric;
        //internal delegate void threadAbortHandler(object sender, EventArgs e);
        //internal static event threadAbortHandler threadAbort;

        //private void onThreadAbort(object sender, EventArgs e)
        //{
        //    // notify OQAT about finished analysis
        //    if (vidRes != null)
        //    {
        //        foreach (Video v in vidRes)
        //        {
        //            PluginManager.pluginManager.raiseEvent(PublicRessources.Plugin.EventType.macroProcessingFinished,
        //                new VideoEventArgs(v));
        //        }
        //    }
        //    // vidRes = null;
        //    thread.Abort();
        //    thread = null;
        //    threadAbort = null;
        //}

        /// <summary>
        /// Method to initialize Data for analyze
        /// </summary>
        /// <param name="vidRef">first video</param>
        /// <param name="vidProc">secend video</param>
        /// <param name="vidResult">video results</param>
        //public void init(Video vidRef, Video vidProc, Video[] vidResult)
        //{
        //    refHand = vidRef.handler;
        //    procHand = vidProc.handler;
        //    vidRes = vidResult;
        //    refHand.setReadContext(vidRef.vidPath, vidRef.vidInfo);
        //    procHand.setReadContext(vidProc.vidPath, vidProc.vidInfo);
        //    //TODO: do not allow the analyse if vidRef and vidProc have got a different frame count
        //    totalFrames = vidRef.vidInfo.frameCount;
        //    i = 0;
        //    nextMetric = 0;
        //}

        /// <summary>
        /// Method to assign settings and bitmaps to Metric Plugins
        /// and write new video to disk
        /// </summary>
        /// <param name="vidRef">first video</param>
        /// <param name="vidProc">second video</param>
        /// <param name="vidResult">video results</param> 

        public void analyse(Video vidRef, Video vidProc, Video[] vidResult)
        {
            //init Data
            refHand = vidRef.handler;
            procHand = vidProc.handler;
            vidRes = vidResult;
            refHand.setReadContext(vidRef.vidPath, vidRef.vidInfo);
            procHand.setReadContext(vidProc.vidPath, vidProc.vidInfo);
            //TODO: do not allow the analyse if vidRef and vidProc have got a different frame count
            totalFrames = vidRef.vidInfo.frameCount;
            nextMetric = 0;

            //give work for each entry
            foreach(MacroEntryMetric metric in macroQueue)
            {
                // Define current entry of macroQueue
                MacroEntryMetric macroEntryMetric = (MacroEntryMetric)macroQueue[nextMetric];
                currentPlugin = (IMetricOqat)PluginManager.pluginManager.getPlugin<IPlugin>((String)macroEntryMetric.mementoName);
                currentMemento = PluginManager.pluginManager.getMemento((String)macroEntryMetric.pluginName, (String)macroEntryMetric.mementoName);
                //check if macro
                if (currentPlugin is IMacro)
                {
                    macroEncode(currentMemento);
                }
                else
                {
                    
                    videoAnalyse(currentPlugin, currentMemento, vidRef.vidInfo);
                }
                nextMetric++;
            }
            //Reset all
            currentPlugin = null;
            currentMemento = null;
            refHand = null;
            procHand = null;
            analyseInfo = null;
            refFrame = null;
            procFrame = null;
            resultFrame = null;
            //onThreadAbort(this, new EventArgs());
        }

        private void videoAnalyse(IMetricOqat metric, Memento memento, IVideoInfo vidInfo)
        {
            //init first frame, 
            int i = 0;
            refHand.setWriteContext(vidRes[nextMetric].vidPath, vidInfo);
            vidRes[nextMetric].frameMetricValue = new float[totalFrames][];
            refHand.positionReader = 0;
            procHand.positionReader = 0;
            metric.setMemento(memento);

            //analyse video
            while (i < totalFrames)
            {
                //get frames to analyse
                refFrame = refHand.getFrame();
                procFrame = procHand.getFrame();

                analyseInfo = currentPlugin.analyse(refFrame, procFrame); // result of the current analysis

                vidRes[nextMetric].frameMetricValue[i] = analyseInfo.values; // sets frameMetricValue for the result video of the current metric
                Bitmap[] tmp = new Bitmap[1];
                tmp[0] = analyseInfo.frame;
                refHand.writeFrames(i, tmp);// write the result frames
                i++;
            }
            //tell PluginManager work is done
            //TODO macroAnalysedFinished (one video)
            PluginManager.pluginManager.raiseEvent(PublicRessources.Plugin.EventType.macroProcessingFinished,
                    new VideoEventArgs(vidRes[nextMetric]));
        }
        //TODO macro aufsplitten
        private void macroEncode(Memento MacroMemento)
        {
            /**currentPlugin = null; // to avoid loading the same plugin twice
            IMetricOqat currentPluginEntry;
            Memento currentMementoEntry;
            List<MacroEntryMetric> macroEntrys = (List<MacroEntryMetric>)MacroMemento.state;
            foreach (MacroEntryMetric currentEntry in macroEntrys)
            {
                currentPluginEntry = (IMetricOqat)PluginManager.pluginManager.getPlugin<IPlugin>(currentEntry.pluginName);
                currentMementoEntry = PluginManager.pluginManager.getMemento(currentEntry.pluginName, currentEntry.mementoName);
                if (currentPluginEntry is IMacro)
                {
                    macroEncode(currentMementoEntry);
                }
                else
                {
                    MacroEntryMetric currentFilterEntry = (MacroEntryMetric)currentEntry;
                    mementoAnalyse(currentMementoEntry);
                }
            } **/

        //public void analyse(Video vidRef, Video vidProc, Video[] vidResult)
        //{
        //    //    thread = new Thread(new ThreadStart(WorkerThread));
        //    //    threadAbort += new threadAbortHandler(onThreadAbort);
        //    //    thread.Start();
        //    //}

        //    //private void WorkerThread()
        //    //{
            
        //    for (int m = 0; m < macroQueue.Count; m++)
        //    {
        //        refHand.setWriteContext(vidRes[m].vidPath, vidRef.vidInfo);
        //        // Define current entry of macroQueue
        //        MacroEntryMetric macroEntryMetric = (MacroEntryMetric)macroQueue[m];
        //        currentPlugin = (IMetricOqat)PluginManager.pluginManager.getPlugin<IPlugin>((String)macroEntryMetric.mementoName);
        //        currentMemento = PluginManager.pluginManager.getMemento((String)macroEntryMetric.pluginName, (String)macroEntryMetric.mementoName);
        //        vidRes[m].frameMetricValue = new float[totalFrames][];
        //        refHand.positionReader = 0;
        //        procHand.positionReader = 0;

        //        if (currentPlugin is IMacro)
        //        {
        //            macroEncode(currentMemento);
        //        }
        //        else
        //        {
        //            mementoAnalyse(currentMemento);
        //        }
        //        while (i < totalFrames)
        //        {
        //            refFrame = refHand.getFrame();
        //            procFrame = procHand.getFrame();

                    
        //            vidRes[m].frameMetricValue[i] = analyseInfo.values; // sets frameMetricValue for the result video of the current metric
        //            Bitmap[] tmp = new Bitmap[1];
        //            tmp[0] = resultFrame;
        //            refHand.writeFrames(i, tmp);
        //            i++;
        //        }
        //        i = 0;
        //    }
        //    currentPlugin = null;
        //    currentMemento = null;
        //    refHand = null;
        //    procHand = null;
        //    analyseInfo = null;
        //    refFrame = null;
        //    procFrame = null;
        //    resultFrame = null;
        //    //onThreadAbort(this, new EventArgs());

        //    foreach (Video v in vidResult)
        //    {
        //        PluginManager.pluginManager.raiseEvent(PublicRessources.Plugin.EventType.macroProcessingFinished,
        //            new VideoEventArgs(v));
        //    }
        //}

        //private void mementoAnalyse(Memento memento)
        //{
        //    currentPlugin.setMemento(memento);
        //    analyseInfo = currentPlugin.analyse(refFrame, procFrame); // result of the current analysis
        //    resultFrame = analyseInfo.frame; // write the result frames
        //}

        
        }

        public override Memento getMemento()
        {
            return new Memento(this.namePlugin, this.macroQueue.ToArray());
        }

        public override void setMemento(Memento memento)
        {
            this.macroQueue.Clear();
            foreach (MacroEntryMetric f in ((MacroEntryMetric[])memento.state))
            {
                this.macroQueue.Add(f);
            }
        }
    }
}

