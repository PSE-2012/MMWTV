//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace PP_Presentation
{
	using Oqat.PublicRessources.Plugin;
	using Plugins;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class PP_Diagram : IPresentation
	{


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

        public void onFlushPresentationPlugins(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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

        public void setParentControl(System.Windows.Controls.Panel parent)
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

        private void createDataSeries(float[] series) { }


        public void loadVideo(object sender, Oqat.PublicRessources.Model.VideoEventArgs vid)
        {
            throw new NotImplementedException();
        }
    }
}

