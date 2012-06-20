//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    using Oqat.PublicRessources.Model;
    using System.IO;

	public class Caretaker
	{
		private Caretaker caretaker
		{
			get;
			set;
		}

		private Dictionary<string, List<Memento>> mementoTable
		{
			get;
			set;
		}

		public static Caretaker getCaretaker()
		{
			throw new System.NotImplementedException();
		}

		public virtual void removeMemento(string origin, string nameMemento)
		{
			throw new System.NotImplementedException();
		}

		public virtual void toDisk()
		{
			throw new System.NotImplementedException();
		}

		private Caretaker()
		{
		}

		public virtual void fromDisk(string mementoDir)
		{
			throw new System.NotImplementedException();
		}

		public virtual Memento[] getMementos(string origin)
		{
			throw new System.NotImplementedException();
		}

		public virtual Memento getMemento(string origin, string mementoname)
		{
			throw new System.NotImplementedException();
		}

		public virtual void addMemento(string origin, Memento memento)
		{
			throw new System.NotImplementedException();
		}

	}
}

