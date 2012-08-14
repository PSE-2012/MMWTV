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
        private IVideoHandler[] resHand;
        private IMetricOqat currentPlugin;
        private Memento currentMemento;
        private AnalysisInfo analyseInfo;
        private System.Drawing.Bitmap[] refFrames;
        private System.Drawing.Bitmap[] procFrames;
        private System.Drawing.Bitmap[] resultFrames;
        //private Thread thread = null;
        private Video[] vidRes;
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
        public void init(Video vidRef, Video vidProc, Video[] vidResult)
        {
            refHand = vidRef.handler;
            procHand = vidProc.handler;
            vidRes = vidResult;
            resHand = new IVideoHandler[vidRes.Length];
            for (int k = 0; k < vidRes.Length; k++)
            {
                resHand[k] = vidRes[k].handler;
            }
            //TODO: do not allow the analyse if vidRef and vidProc have got a different frame count
            totalFrames = vidRef.vidInfo.frameCount;
            i = 0;
        }

        /// <summary>
        /// Method to assign settings and bitmaps to Metric Plugins
        /// and write new video to disk
        /// </summary>
        /// <param name="vidRef">first video</param>
        /// <param name="vidProc">second video</param>
        /// <param name="vidResult">video results</param>
        public void analyse(Video vidRef, Video vidProc, Video[] vidResult)
        {
        ////    thread = new Thread(new ThreadStart(WorkerThread));
        ////    threadAbort += new threadAbortHandler(onThreadAbort);
        ////    thread.Start();
        ////}

        ////private void WorkerThread()
        ////{
        //    // Warning: the method, implemented this way, does not support having another macrometric inside the list of metrics
        //    for (int m = 0; m < macroQueue.Count; m++)
        //    {
        //        resultFrames = new System.Drawing.Bitmap[vidRef.vidInfo.frameCount];
        //        refFrames = new System.Drawing.Bitmap[vidRef.vidInfo.frameCount];
        //        procFrames = new System.Drawing.Bitmap[vidProc.vidInfo.frameCount];

        //        // Define current entry of macroQueue
        //        MacroEntryMetric macroEntryMetric = (MacroEntryMetric)macroQueue[m];
        //        currentPlugin = (IMetricOqat)PluginManager.pluginManager.getPlugin<IPlugin>((String)macroEntryMetric.mementoName);
        //        currentMemento = PluginManager.pluginManager.getMemento((String)macroEntryMetric.pluginName, (String)macroEntryMetric.mementoName);

        //        vidRes[m].frameMetricValue = new float[totalFrames][];

        //        while (i < totalFrames)
        //        {
        //            refFrames[i] = refHand.getFrame(i);
        //            procFrames[i] = procHand.getFrame(i);

        //            if (currentPlugin is IMacro)
        //            {
        //                macroEncode(currentMemento);
        //            }
        //            else
        //            {
        //                mementoAnalyse(currentMemento);
        //            }
                     

        //            vidRes[m].frameMetricValue[i] = analyseInfo.values; // sets frameMetricValue for the result video of the current metric
        //            resHand[m].writeFrame(i, resultFrames[i]); // write the result frames to disk
        //            i++;
        //        }
        //        i = 0;
        //    }
        //    currentPlugin = null;
        //    currentMemento = null;
        //    refFrames = null;
        //    procFrames = null;
        //    refHand = null;
        //    resHand = null;
        //    procHand = null;
        //    refFrames = null;
        //    procFrames = null;
        //    analyseInfo = null;
        //    resultFrames = null;
        //    //onThreadAbort(this, new EventArgs());

        //    foreach (Video v in vidResult)
        //    {
        //        PluginManager.pluginManager.raiseEvent(PublicRessources.Plugin.EventType.macroProcessingFinished,
        //            new VideoEventArgs(v));
        //    }
        }

        private void mementoAnalyse(Memento memento)
        {
            currentPlugin.setMemento(memento);
            analyseInfo = currentPlugin.analyse(refFrames[i], procFrames[i]); // result of the current analysis
            resultFrames[i] = analyseInfo.frame; // write the result frames
        }

        //TODO macro aufsplitten
        private void macroEncode(Memento MacroMemento)
        {
            currentPlugin = null; // to avoid loading the same plugin twice
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
            }
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

