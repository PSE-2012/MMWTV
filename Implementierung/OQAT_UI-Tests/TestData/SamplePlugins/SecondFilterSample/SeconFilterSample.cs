using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel.Composition;
using Oqat.PublicRessources.Plugin;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;

namespace Filter
{
    [ExportMetadata("namePlugin", namePlugin)]
    [ExportMetadata("type", type)]
    [Export(typeof(IPlugin))]
    public class SeconFilterSample : IFilterOqat
    {
        public Bitmap process(Bitmap frame)
        {
            MessageBox.Show("SecondFilterSample process(..) called.");
            return new Bitmap(10, 10);
        }

        public const string namePlugin = "SecondFilterSample";

        public const PluginType type = PluginType.IFilterOqat;


        public void setParentControl(Panel parent)
        {
            throw new NotImplementedException();
        }

        public Dictionary<EventType, List<Delegate>> getEventHandlers()
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
