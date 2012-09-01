using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel.Composition;
using Oqat.PublicRessources.Plugin;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;

namespace Metric
{
    [ExportMetadata("namePlugin", namePlugin)]
    [ExportMetadata("type", type)]
    [Export(typeof(IPlugin))]
    public class MetricSample : IMetricOqat
    {

        public const string namePlugin = "MetricSample";

        public const PluginType type = PluginType.IMetric;



        public AnalysisInfo analyse(Bitmap frameRef, Bitmap frameProc)
        {
            MessageBox.Show("MetricSample process(..) called.");
            return new AnalysisInfo(new Bitmap(10, 10), new float[2]);
        }

        public Dictionary<EventType, List<Delegate>> getEventHandlers()
        {
            throw new NotImplementedException();
        }

        public void setParentControl(Panel parent)
        {
            throw new NotImplementedException();
        }

        public Oqat.PublicRessources.Model.Memento getMemento()
        {
            throw new NotImplementedException();
        }

        public void setMemento(Oqat.PublicRessources.Model.Memento memento)
        {
            throw new NotImplementedException();
        }
    }
    
}
