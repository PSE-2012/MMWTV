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

    /// <summary>
    /// This class is a implementation of IMetricOqat, <see cref="IMetricOqat"/> for further informations.
    /// Besides this class inherits from the abstract class <see cref="Macro"/> wich in turn
    /// only implements IMacro, see <see cref="IMacro"/> for further informations.
    /// </summary>
	public class PM_MacroMetric : Macro, IMetricOqat
    {
        public MacroMetricControl macroControl;

        public UserControl propertyView
        {
            get
            {
                return macroControl;
            }
        }

        public AnalysisInfo analyse(Bitmap frameRef, Bitmap frameProc)
        {
            throw new NotImplementedException();
        }

        public PM_MacroMetric()
        {
            macroQueue = new DataTable();
            macroQueue.Columns.Add("Metric Name", typeof(String));
            macroQueue.Columns.Add("Memento Name", typeof(String));
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

        public void init(Video vidRef, Video vidProc, Video[] vidResult)
        {
            IVideoHandler refHand = vidRef.handler;
            IVideoHandler procHand = vidProc.handler;
            IVideoHandler[] resHand = new IVideoHandler[vidResult.Length];
            for (int k = 0; k < vidResult.Length; k++)
            {
                resHand[k] = vidResult[k].handler;
            }
            BUFFERSIZE = 255;
            totalFrames = vidRef.vidInfo.frameCount; // TODO: do not allow the analyse if vidRef and vidProc have got a different frame count
            i = 0;
        }

        public void analyse(Video vidRef, Video vidProc, Video[] vidResult)
        {
            // Warning: the method, implemented this way, does not support having another macrometric inside the list of metrics
            for (int m = 0; m < macroQueue.Rows.Count; m++)
            {
                DataRow c = macroQueue.Rows[m];
                currentPlugin = (IMetricOqat)PluginManager.pluginManager.getPlugin<IPlugin>((String)c["Metric Name"]);
                currentMemento = PluginManager.pluginManager.getMemento((String)c["Metric Name"], (String)c["Memento Name"]);
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
                    refFrames = refHand.getFrames(i, BUFFERSIZE);
                    procFrames = procHand.getFrames(i, BUFFERSIZE);
                    int arraycount = refFrames.Count();
                    for (int j = 0; j < arraycount; j++) // iterate over all frames to be analysed
                    {
                        currentPlugin.setMemento(currentMemento);
                        analyseInfo = currentPlugin.analyse(refFrames[j], procFrames[j]); // result of the current analysis
                        resultFrames[j] = analyseInfo.frame; // write the result frames to buffer
                        vidResult[m].frameMetricValue[i] = analyseInfo.values; // sets frameMetricValue for the result video of the current metric
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
            // TODO: event analyse finished
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

        public void setMemento(PublicRessources.Model.Memento memento)
        {
            throw new NotImplementedException();
        }
    }
}

