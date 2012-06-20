//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.Model
{
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

		private string path_Project
		{
			get;
			set;
		}

        private SmartNode smartTree;

		public Project(string name, string path, List<Video> vidList, string description)
		{
		}

		public Project(string path)
		{
		}

		public Project(string name, string path, string description)
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

