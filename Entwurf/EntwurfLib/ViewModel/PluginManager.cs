//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.ViewModel
{
	using Oqat.Model;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    using Oqat.PublicRessources.Model;
    using Oqat.PublicRessources.Plugin;
    using System.IO;

	public class PluginManager
	{
		private PluginManager pluginManager
		{
			get;
			set;
		}

		private Dictionary<PluginType,Dictionary<string, IPlugin>> pluginTable
		{
			get;
			set;
		}

		private Caretaker caretaker
		{
			get;
			set;
		}

		private string pluginPath
		{
			get;
			set;
		}


		private Dictionary<EventType, Delegate > handlerTable
		{
			get;
			set;
		}

        public virtual void raiseEvent(EventType eType)
        {
        }

		public virtual void loadPluginTable()
		{
			throw new System.NotImplementedException();
		}

		public virtual void getPluginManager()
		{
			throw new System.NotImplementedException();
		}

		private PluginManager()
		{
		}

		public virtual Memento getMemento(string namePlugin, string nameMemento)
		{
			throw new System.NotImplementedException();
		}

		public virtual IPlugin getPlugin(string namePlugin)
		{
			throw new System.NotImplementedException();
		}

		public virtual List<string> getPluginNames(PluginType type)
		{
			throw new System.NotImplementedException();
		}

		public virtual void addEventHandler(EventType eType, Delegate handler)
		{
			throw new System.NotImplementedException();
		}


		public virtual void rmEventHandler(EventType eType, Delegate handler)
		{
			throw new System.NotImplementedException();
		}

		public virtual List<String> getMementoNames(string namePlugin)
		{
			throw new System.NotImplementedException();
		}

	}
}

