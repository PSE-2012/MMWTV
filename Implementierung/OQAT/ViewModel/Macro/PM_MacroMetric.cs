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
        public PluginType type
        {
            get
            {
                return PluginType.IMetricOqat;
            }
        }
        /// <summary>
        /// Metric macroQueue
        /// </summary>
        internal ObservableCollection<MacroEntryMetric> macroQueue;

        public AnalysisInfo analyse(Bitmap frameRef, Bitmap frameProc)
        {
            //TODO: Do we ever use a macro like a metric without checking if it is macro? metricVorschau
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
        private List<Video> vidRes;
        private int nextMetric;


        /// <summary>
        /// Method to assign settings and bitmaps to Metric Plugins
        /// and write new video to disk
        /// </summary>
        /// <param name="vidRef">first video</param>
        /// <param name="vidProc">second video</param>
        /// <param name="vidResult">video results</param> 

        public void analyse(Video vidRef, Video vidProc, int idProc, List<Video> vidResult)
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
            foreach (MacroEntryMetric metric in macroQueue)
            {
                // Define current entry of macroQueue
                MacroEntryMetric macroEntryMetric = metric;
                //MacroEntryMetric macroEntryMetric = (MacroEntryMetric)macroQueue[nextMetric];
                currentPlugin = (IMetricOqat)PluginManager.pluginManager.getPlugin<IPlugin>((String)macroEntryMetric.pluginName);
                currentMemento = PluginManager.pluginManager.getMemento((String)macroEntryMetric.pluginName, (String)macroEntryMetric.mementoName);
                //check if macro
                if (currentPlugin is IMacro)
                {
                    macroEncode(currentMemento, idProc);
                }
                else
                {
                    videoAnalyse(currentPlugin, currentMemento, idProc);
                }
                //nextMetric++;
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

        private void videoAnalyse(IMetricOqat metric, Memento memento, int idProc)
        {
            //init first frame, 
            int i = 0;
            refHand.setWriteContext(vidRes[nextMetric].vidPath, vidRes[nextMetric].vidInfo);
            vidRes[nextMetric].frameMetricValue = new float[totalFrames][];
            refHand.positionReader = 0;
            procHand.positionReader = 0;
            if (currentPlugin.propertyView != null)
            {
                currPluginRef = metric;
                currMemRef = memento;
                setProcessingMementoHelper();
            }

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
            PluginManager.pluginManager.raiseEvent(PublicRessources.Plugin.EventType.macroProcessingFinished,
                    new VideoEventArgs(vidRes[nextMetric], idProc));
            nextMetric++;
        }

        //split macro
        private void macroEncode(Memento MacroMemento, int idProc)
        {
            foreach (MacroEntryMetric entry in (List<MacroEntryMetric>)MacroMemento.state)
            {
                currentPlugin = (IMetricOqat)PluginManager.pluginManager.getPlugin<IPlugin>((String)entry.pluginName);
                currentMemento = PluginManager.pluginManager.getMemento((String)entry.pluginName, (String)entry.mementoName);
                if (currentPlugin is IMacro)
                {
                    macroEncode(currentMemento, idProc);
                }
                else
                {
                    videoAnalyse(currentPlugin, currentMemento, idProc);
                    refHand.positionReader = 0;
                    procHand.positionReader = 0;
                    //nextMetric++;
                }
            }
        }

        public override Memento getMemento()
        {
            return new Memento(this.namePlugin, this.macroQueue.ToList());
        }

        public override void setMemento(Memento memento)
        {
            this.macroQueue.Clear();
            foreach (MacroEntryMetric f in ((List<MacroEntryMetric>)memento.state))
            {
                this.macroQueue.Add(f);
            }
        }
    }
}

