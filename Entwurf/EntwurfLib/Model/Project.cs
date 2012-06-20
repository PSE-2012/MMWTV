//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.Model
{
	using OqatPublicRessources;
	using Plugins;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;


    using System.IO;
    using Oqat.PublicRessources.Plugin;
    using Oqat.PublicRessources.Model;

	public class Project : IMemorizable
	{
		private string name
		{
			get;
			set;
		}

		private string description
		{
			get;
			set;
		}

		private Path path_Project
		{
			get;
			set;
		}

		public virtual SmartNode smartTree
		{
			get;
			set;
		}

		public Project(string name, Path path, List<Video> vidList, string description)
		{
		}

		public Project(Path path)
		{
		}

		public Project(string name, Path path, string description)
		{
		}

		public virtual Memento getMemento()
		{
			throw new System.NotImplementedException();
		}

		public virtual void setMemento(Memento memento)
		{
			throw new System.NotImplementedException();
		}

	}
}

