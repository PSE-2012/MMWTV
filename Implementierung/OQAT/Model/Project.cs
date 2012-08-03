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

        /// <summary>
        /// Each SmartNode has an unambiguos id
        /// by which it can be identified.
        /// </summary>
        public int unusedId {
            get
            {
                lock(idLock) {
                return _unusedId++;
                }
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
		}

        /// <summary>
        /// Constructs a new project according to 
        /// the state object withing the given memento.
        /// </summary>
        /// <remarks>The state object within the given memento has to 
        /// be of the type <see cref="ProjectProperties"/> or a
        /// ArgumentException will be thrown.</remarks>
        /// <param name="mem">Memento to extract new settings from.</param>
        internal Project(Memento mem)
        {
          
            try
            {
               var prProperties = (ProjectProperties)mem.state;
               if (prProperties.complete)
                   this.setMemento(mem);
               else
                   throw new ArgumentException("Given memento does not contain all requered properties.");
            }
            catch (InvalidCastException exc)
            {
                throw new ArgumentException("State object within the given memento: " + mem.name + " is not of the type"
                    + " ProejctProperties.", exc);
            }

 

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
            SmartNode newChild = new SmartNode(vid, unusedId, idFather);
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
                 foreach (SmartNode i in toRmNode.smartTree)
                    {
                        i.idFather = father.id;
                    }
                 if (father == null)
                     smartTree.Concat(toRmNode.smartTree);
                 else
                     father.smartTree.Concat(toRmNode.smartTree);

             }
        }
        /// <summary>
        /// Return a memento for the current project.
        /// </summary>
        /// <returns></returns>
		public virtual Memento getMemento()
		{
            return new Memento(this.name, 
                new ProjectProperties(name, description,path_Project, smartIndex, smartTree, _unusedId), 
                this.path_Project + this.name + "_Project.oqat");
		}

        /// <summary>
        /// Sets the current properties to those within
        /// the given memento state object.
        /// </summary>
        /// <param name="memento">Memento to get the new properties from.</param>
		public virtual void setMemento(Memento mem)
		{
            ProjectProperties prProperties;
            try
            {
                prProperties = (ProjectProperties)mem.state;
            }
            catch (InvalidCastException exc)
            {
                throw new ArgumentException("State object within the given memento: " + mem.name + " is not of the type"
                    + " ProejctProperties.", exc);
            }

            name = String.IsNullOrEmpty(prProperties.name) ? name : prProperties.name;
            description = String.IsNullOrEmpty(prProperties.description) ? description : prProperties.description;

            // ProjectProperties checked path for validity at construction time.
            path_Project = String.IsNullOrEmpty(prProperties.path_Project) ? path_Project : prProperties.description;

            if (prProperties.newTrees)
            {
                _unusedId = prProperties._unusedId;
                smartIndex.Clear();
                smartIndex.Concat(prProperties.smartIndex);

                smartTree.Clear();
                smartTree.Concat(prProperties.smartTree);
            }
            

		}

	}


    /// <summary>
    /// This class can be stored within a <see cref="Memento"/> and
    /// be retrieved by a Project to restore or change settings respectively
    /// smartNodes. All values within this class are either null or valid.
    /// </summary>
    internal class ProjectProperties
    {
        /// <summary>
        /// Indicates whether all properties requered
        /// to consruct a new Project instance are valid.
        /// </summary>
        internal bool complete = false;

        /// <summary>
        /// Project name.
        /// </summary>
        internal string name
        {
            get;
            private set;
        }

        /// <summary>
        /// Project description.
        /// </summary>
        internal string description
        {
            get;
            private set;
        }

        /// <summary>
        /// This bool indicates whether smartIndex, smartTree and
        /// _unusedId are valid.
        /// </summary>
        internal bool newTrees = false;


        /// <summary>
        /// The smallest unused id, were unused
        /// means that there is no element within
        /// the smartIndex or the smartTree with
        /// the same id.
        /// </summary>
        internal int _unusedId
        {
            get;
            private set;
        }

        /// <summary>
        /// Contains same smartNodes as 
        /// smartTree but arranges them 
        /// by an id.
        /// </summary>
        internal Dictionary<int, SmartNode> smartIndex
        {
            get;
            private set;
        }

        /// <summary>
        /// Contains smae smartNodes as
        /// smartIndex but arranges them
        /// as a tree.
        /// </summary>
        internal ObservableCollection<SmartNode> smartTree
        {
            get;
            private set;
        }


        /// <summary>
        /// Project path. It
        /// is made sure that this path
        /// is valid.
        /// </summary>
        internal string path_Project
        {
            get;
            private set;
        }


        internal ProjectProperties(string name, string description, string path_Project,
            Dictionary<int, SmartNode> smartIndex, ObservableCollection<SmartNode> smartTree, int _unusedId) 
        {
            this.name = String.IsNullOrWhiteSpace(name) ? null : name;
            this.description = String.IsNullOrWhiteSpace(description) ? null : description;

            try
            {
                new FileInfo(path_Project);
                this.path_Project = path_Project;
            }
            catch (Exception exc)
            {
                throw new ArgumentException("Given project path" + path_Project + " is invalid.", exc);
            }

            newTrees = smartConsistency(smartIndex, smartTree, _unusedId);
            if (newTrees)
            {
                this._unusedId = _unusedId;
                this.smartTree = smartTree;
                this.smartIndex = smartIndex;
            }

            complete = newTrees && String.IsNullOrWhiteSpace(name) && 
                        String.IsNullOrWhiteSpace(description) && 
                            String.IsNullOrWhiteSpace(path_Project);

        }

        /// <summary>
        /// Checks given parameters for consistency. 
        /// </summary>
        /// <param name="smartIndex"></param>
        /// <param name="smartTree"></param>
        /// <param name="_unusedId"></param>
        /// <returns>Returns false if
        /// incosistencies were found.</returns>
        private bool smartConsistency(Dictionary<int, SmartNode> smartIndex, ObservableCollection<SmartNode> smartTree, int _unusedId) {
           
            bool consistent = true;
            if ((smartTree != null) && (smartIndex != null))
            {


                if (smartIndex.ContainsKey(_unusedId))
                    consistent = false;

                int smartTreeCount = 0;

                if (consistent)
                    foreach (SmartNode i in smartTree)
                    {

                        if ((i.id == _unusedId) 
                            || !smartIndex.ContainsKey(i.id)
                            || i.idFather >= 0)
                        {
                            consistent = false;
                            break;
                        }

                        smartTreeCount++;
                        foreach (SmartNode k in i.smartTree)
                        {
                            if ((k.id == _unusedId) 
                                || !smartIndex.ContainsKey(k.id)
                                || k.idFather != i.idFather)
                            {
                                consistent = false;
                                break;
                            }

                            smartTreeCount++;
                        }
                    }
                if (smartIndex.Count != smartTreeCount)
                    consistent = false;
            }
            else
            {
                consistent = false;
            }
            
            return consistent;
        }

    }
}

