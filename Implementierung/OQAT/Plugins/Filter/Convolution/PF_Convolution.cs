//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
namespace PF_Convolution
{
	using Oqat.PublicRessources.Plugin;
    using Oqat.PublicRessources.Model;
    using System.Drawing;
	using Plugins;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class PF_Convolution : IFilterOqat
	{

        public Bitmap process(Bitmap frame)
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

        public Memento getMemento()
        {
            throw new NotImplementedException();
        }

        public void setMemento(Memento memento)
        {
            throw new NotImplementedException();
        }
    }
}

