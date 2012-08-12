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
        string namePlugin
        {
            get
            {
                return "PM_MacroMetric";
            }
        }
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

        private int BUFFERSIZE;
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
        private Thread thread = null;
        private Video[] vidRes;
        internal delegate void threadAbortHandler(object sender, EventArgs e);
        internal static event threadAbortHandler threadAbort;

        private void onThreadAbort(object sender, EventArgs e)
        {
            // notify OQAT about finished analysis
            if (vidRes != null)
            {
                foreach (Video v in vidRes)
                {
                    PluginManager.pluginManager.raiseEvent(PublicRessources.Plugin.EventType.macroProcessingFinished,
                        new VideoEventArgs(v));
                }
            }
            // vidRes = null;
            thread.Abort();
            thread = null;
            threadAbort = null;
        }

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
            BUFFERSIZE = 255;
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
            thread = new Thread(new ThreadStart(WorkerThread));
            threadAbort += new threadAbortHandler(onThreadAbort);
            thread.Start();
        }

        private void WorkerThread()
        {
            // Warning: the method, implemented this way, does not support having another macrometric inside the list of metrics
            for (int m = 0; m < macroQueue.Count; m++)
            {
                MacroEntryMetric c = (MacroEntryMetric)macroQueue[m];
                currentPlugin = (IMetricOqat)PluginManager.pluginManager.getPlugin<IPlugin>((String)c.mementoName);
                currentMemento = PluginManager.pluginManager.getMemento((String)c.pluginName, (String)c.mementoName);
                vidRes[m].frameMetricValue = new float[totalFrames][];
                while (i < totalFrames)
                {
                    if ((i + BUFFERSIZE - totalFrames) > 0)
                    {
                        refFrames = refHand.getFrames(i, totalFrames - i);
                        procFrames = procHand.getFrames(i, totalFrames - i);
                        resultFrames = new System.Drawing.Bitmap[totalFrames - i];
                    }
                    else
                    {
                        refFrames = refHand.getFrames(i, BUFFERSIZE);
                        procFrames = procHand.getFrames(i, BUFFERSIZE);
                        resultFrames = new System.Drawing.Bitmap[BUFFERSIZE];
                    }
                    int arraycount = refFrames.Count();
                    for (int j = 0; j < arraycount; j++) // iterate over all frames to be analysed
                    {
                        currentPlugin.setMemento(currentMemento);
                        analyseInfo = currentPlugin.analyse(refFrames[j], procFrames[j]); // result of the current analysis
                        resultFrames[j] = analyseInfo.frame; // write the result frames to buffer
                        vidRes[m].frameMetricValue[i + j] = analyseInfo.values; // sets frameMetricValue for the result video of the current metric
                    }
                    resHand[m].writeFrames(i, resultFrames); // write the result frames to disk
                    i += BUFFERSIZE;
                }
                i = 0;
            }
            currentPlugin = null;
            currentMemento = null;
            refFrames = null;
            procFrames = null;
            refHand = null;
            resHand = null;
            procHand = null;
            refFrames = null;
            procFrames = null;
            analyseInfo = null;
            resultFrames = null;
            onThreadAbort(this, new EventArgs());
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

