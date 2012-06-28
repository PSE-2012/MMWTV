//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace PM_PSNR
{
	using Oqat.PublicRessources.Plugin;
	using Plugins;
	using Plugins.Metric;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class PM_PSNR :  IMetricOqat
	{


        public AnalysisInfo analyse(System.Drawing.Bitmap frameRef, System.Drawing.Bitmap frameProc)
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
    }
}

