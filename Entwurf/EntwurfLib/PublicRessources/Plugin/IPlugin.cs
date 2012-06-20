//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.PublicRessources.Plugin
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


		void setParentControll(Panel parent);

		Dictionary<EventType,List<Delegate>> getEventHandlers();


	}
}

