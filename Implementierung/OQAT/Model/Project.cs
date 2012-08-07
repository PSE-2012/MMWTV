namespace Oqat.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;


    using System.IO;
    using Oqat.PublicRessources.Plugin;
    using Oqat.PublicRessources.Model;
    using Oqat.ViewModel;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;

	/// <summary>
	/// This class is the Model for a user's project in OQAT.
	/// </summary>
    [Serializable()]
    public class Project
	{
		/// <summary>
		/// Name of the project.
		/// </summary>
        public string name
		{
			get;
			private set;
		}

        public int unusedId
        {
            get
            {
                return _unusedId;
            }
        }
        /// <summary>
        /// Each SmartNode has an unambiguos id
        /// by which it can be identified.
        /// </summary>
        private int _unusedId = 0;
        private object idLock;
        private int getUnusedId()
        {
            lock (idLock)
            {
                return _unusedId++;
            }
        }
        /// <summary>
        /// In order to reduce the searching overhead
        /// for a given SmartNode, each SmartNode
        /// is stored (the reference) within the
        /// smartIndex where the key is an unambiguos id.
        /// </summary>
        private Dictionary<int, SmartNode> smartIndex;

		/// <summary>
		/// Project description, made by the user.
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
        internal ObservableCollection<SmartNode> smartTree
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructs a new project.
        /// </summary>
        /// <param name="name"></param> Project name.
        /// <param name="path"></param> Path to the project folder.
        /// <param name="vidList"></param> List of videos belonging to the project.
        /// <param name="description"></param> Description of the project.
		internal Project(string name, string path, string description, List<Video> vidList = null)
		{
            this.idLock = new Object();
            this.name = name;
            this.path_Project = path;
            this.description = description;
            this.smartTree = new ObservableCollection<SmartNode>();
            this.smartIndex = new Dictionary<int, SmartNode>();

            if ((vidList != null) && (vidList.Count > 0))
            {
                foreach (Video vid in vidList)
                {
                    addNode(vid, 0);
                }
            }
            saveProject();
		}

        internal void saveProject()
        {
            Caretaker.caretaker.writeMemento(new Memento(this.name, this, this.path_Project));
        }


        /// <summary>
        /// Adds a new SmartNode to the SmartTree.
        /// </summary>
        /// <param name="vid">Video object to put into SmartNode. If
        /// given video is not referenced, a ArgumentExcetion will
        /// be raised.</param>
        /// <param name="idFather">Father id of the new SmartNode.
        /// If father id is smaller 0, the SmartNode will
        /// be added on Top of the SmartTree.If father id is
        /// greater 0 but does not exist in the current SmartTree structure
        /// a ArgumentException will be raised.</param>
        /// 
        /// 
        internal void addNode(Video vid, int idFather) {
            // Check arguments for validity.
            if (vid == null)
                throw new ArgumentException("Given Video object is null.");
            else if ((idFather >= 0) && !smartIndex.ContainsKey(idFather))
                throw new ArgumentException("Given father id is unknown.");

            // Process valid arguments.
            SmartNode newChild = new SmartNode(vid, getUnusedId(), idFather);
            smartIndex.Add(newChild.id, newChild);
            SmartNode father;
            if (idFather >= 0)
            {
                smartIndex.TryGetValue(idFather, out father);
                    father.smartTree.Add(newChild);
            }
            else
            {
                smartTree.Add(newChild);
            }

            saveProject();
        }

        /// <summary>
        /// Remove SmartNode with
        /// the given id from the SmartTree.
        /// </summary>
        /// <param name="id">Id of the SmartNode to remove.</param>
        /// <param name="force">If force is true, all children will be removed, else
        /// children will be passed to next father node.</param>
        internal void rmNode(int id, bool force)
        {

            if (!smartIndex.ContainsKey(id))
                throw new ArgumentException("Given id is unknown.");

            SmartNode toRmNode;
            smartIndex.TryGetValue(id, out toRmNode);

            if ((toRmNode.idFather >= 0) && !smartIndex.ContainsKey(toRmNode.idFather))
                throw new ArgumentException("Given father id is unknown.");

            SmartNode father;
            smartIndex.TryGetValue(toRmNode.idFather, out father);

            if (father != null)
                father.smartTree.Remove(toRmNode);
            else
                this.smartTree.Remove(toRmNode);

            smartIndex.Remove(id);

            if ((toRmNode.smartTree.Count > 0) & !force)
             {
                int idFathersFather = (father != null) ? father.id : -1;
                 foreach (SmartNode i in toRmNode.smartTree)
                    {
                        i.idFather = idFathersFather;
                    }
                 var fatherTree = (father == null) ? smartTree : father.smartTree;
                 foreach (SmartNode n in toRmNode.smartTree)
                 {
                     fatherTree.Add(n);
                 }

             }
            saveProject();
        }
	}


}