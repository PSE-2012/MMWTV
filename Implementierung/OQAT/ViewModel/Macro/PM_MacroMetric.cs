//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.ViewModel.Macro
{
	using Oqat.PublicRessources.Plugin;
	using Plugins;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    using System.Drawing;

    /// <summary>
    /// This class is a implementation of IMetricOqat, <see cref="IMetricOqat"/> for further informations.
    /// Besides this class inherits from the abstract class <see cref="Macro"/> wich in turn
    /// only implements IMacro, see <see cref="IMacro"/> for further informations.
    /// </summary>
	public class PM_MacroMetric : Macro, IMetricOqat
    {

        public AnalysisInfo analyse(Bitmap frameRef, Bitmap frameProc)
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

