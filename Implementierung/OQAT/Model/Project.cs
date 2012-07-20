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
    using System.Collections.ObjectModel;

	/// <summary>
	/// This class is the Model for a user's project in OQAT.
	/// </summary>
    public class Project : IMemorizable
	{
		/// <summary>
		/// Name of the project.
		/// </summary>
        public string name
		{
			get;
			private set;
		}
        private int _unusedId = 0;
        private object idLock;
        public int unusedId {
            get
            {
                lock(idLock) {
                return ++_unusedId;
                }
            }
        }
        private Dictionary<int, SmartNode> smartIndex;

		/// <summary>
		/// Description of the project.
		/// </summary>
        public string description
		{
			get;
			private set;
		}

		/// <summary>
		/// Path to the project folder.
		/// </summary>
        public string path_Project
		{
			get;
			private set;
		}

        /// <see cref="SmartNode"/>
        /// <summary>
        /// The tree structure of videos belonging to the project.
        /// </summary>
        ObservableCollection<SmartNode> smartTree;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param> Name of the project.
        /// <param name="path"></param> Path to the project folder.
        /// <param name="vidList"></param> List of videos belonging to the project.
        /// <param name="description"></param> Description of the project.
		public Project(string name, string path, string description, List<Video> vidList = null)
		{
            this.name = name;
            this.path_Project = path;
            this.description = description;
            this.smartTree = new ObservableCollection<SmartNode>();
            if ((vidList != null) & (vidList.Count > 0))
            {
                foreach (Video vid in vidList)
                {
                    addNode(vid, 0);
                }
            }
		}
        public void addNode(Video vid, int idFather) {
            SmartNode newNode = new SmartNode(vid, unusedId, idFather);
            smartIndex.Add(newNode.id, newNode);
            smartTree.Add(newNode);
        }

        public void rmNode(int id, bool force)
        {
            SmartNode toRmNode;
            smartIndex.TryGetValue(id, out toRmNode);
            if (toRmNode != null)
            {
                SmartNode child;
                smartIndex.TryGetValue(id, out child);
                SmartNode father;
                smartIndex.TryGetValue(child.idFather, out father);
                if ((toRmNode.smartTree.Count > 0) & !force)
                {
                    foreach (SmartNode i in child.smartTree)
                    {
                        i.idFather = father.id;
                    }
                    father.smartTree.Concat(child.smartTree);

                }
                father.smartTree.Remove(child);
                smartIndex.Remove(id);
            }
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

