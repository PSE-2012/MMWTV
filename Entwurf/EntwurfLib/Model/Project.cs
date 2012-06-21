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

	/// <summary>
	/// This class is the Model for a user's project in OQAT.
	/// </summary>
    public class Project : IMemorizable
	{
		/// <summary>
		/// Name of the project.
		/// </summary>
        private string name
		{
			get;
			set;
		}

		/// <summary>
		/// Description of the project.
		/// </summary>
        private string description
		{
			get;
			set;
		}

		/// <summary>
		/// Path to the project folder.
		/// </summary>
        private string path_Project
		{
			get;
			set;
		}

        /// <see cref="SmartNode"/>
        /// <summary>
        /// The tree structure of videos belonging to the project.
        /// </summary>
        private SmartNode smartTree;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param> Name of the project.
        /// <param name="path"></param> Path to the project folder.
        /// <param name="vidList"></param> List of videos belonging to the project.
        /// <param name="description"></param> Description of the project.
		public Project(string name, string path, List<Video> vidList, string description)
		{
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param> Path to the project folder.
		public Project(string path)
		{
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param> Name of the project.
        /// <param name="path"></param> Path to the project folder.
        /// <param name="description"></param> Description of the project.
		public Project(string name, string path, string description)
		{
		}

        /// <summary>
        /// Loads a saved project from the hard disk drive.
        /// </summary>
        /// <returns></returns>
		public virtual Memento getMemento()
		{
			throw new System.NotImplementedException();
		}

        /// <summary>
        /// Saves the project to the hard disk drive so it can be loaded again during
        /// another execution of OQAT.
        /// </summary>
        /// <param name="memento"></param>
		public virtual void setMemento(Memento memento)
		{
			throw new System.NotImplementedException();
		}

	}
}

