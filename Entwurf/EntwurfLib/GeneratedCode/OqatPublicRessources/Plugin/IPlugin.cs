//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace OqatPublicRessources.Plugin
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    using System.Windows.Controls;

	public interface IPlugin  : IMemorizable
	{
		string namePlugin { get;set; }

		PluginType type { get;set; }

        delegate void eventHandler(object sender, EventArgs e);

		void setParentControll(Panel parent);

		Dictionary<EventType,List<eventHandler>> getEventHandlers();


	}
}

