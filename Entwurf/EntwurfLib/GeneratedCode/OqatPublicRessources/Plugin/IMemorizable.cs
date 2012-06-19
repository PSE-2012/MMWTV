//------------------------------------------------------------------------------
// corrected
// rtur
//------------------------------------------------------------------------------
namespace OqatPublicRessources.Plugin
{
	using Plugins;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    using OqatPublicRessources.Model;
	public interface IMemorizable 
	{
		Memento getMemento();

		void setMemento(Memento memento);

	}
}

