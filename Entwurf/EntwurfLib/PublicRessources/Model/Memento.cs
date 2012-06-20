//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.PublicRessources.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
    using System.IO;

	public class Memento
	{
		public virtual string name
		{
			get;
			set;
		}

		public virtual object state
		{
			get;
			set;
		}

		public virtual Path mementoPath
		{
			get;
			set;
		}


		public Memento(string nameMemento, object state)
		{
		}

		public Memento(string nameMemento, object state, Path mementoPath)
		{
		}

	}
}

