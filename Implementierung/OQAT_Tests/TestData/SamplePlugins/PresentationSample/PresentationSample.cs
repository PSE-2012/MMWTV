using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel.Composition;
using Oqat.PublicRessources.Plugin;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;

namespace Presentation
{
    [ExportMetadata("namePlugin", namePlugin)]
    [ExportMetadata("type", type)]
    [Export(typeof(IPlugin))]
    public class PresentationSample : IPresentation
    {
       

        public const string namePlugin = "FilterSample";

        public const PluginType type = PluginType.IFilterOqat;



        public void loadVideo(object sender, Oqat.PublicRessources.Model.VideoEventArgs vid)
        {
            MessageBox.Show("Presentation process(..) called.");
        }

        public void onFlushPresentationPlugins(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public PresentationPluginType presentationType
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

        public void unloadVideo()
        {
            throw new NotImplementedException();
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
