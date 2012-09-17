using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;

using Oqat.PublicRessources.Model;
using Oqat.PublicRessources.Plugin;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Oqat.Model
{
    /// <see cref="Project"/>
    /// <summary>
    /// This class is the Model for the tree structure where each video in a project 
    /// is represented by a node containing the path to it.
    /// </summary>
    [Serializable()]
    class SmartNode
    {


        private ObservableCollection<SmartNode> _smartTree;

        /// <summary>
        /// get and sets the Smarttree for the VM_Projectexplorer
        /// </summary>
        public ObservableCollection<SmartNode> smartTree
        {
            get {
                if (_smartTree == null)
                    smartTree = new ObservableCollection<SmartNode>();
                return _smartTree;
            }
            private set {
                _smartTree = value;
            }
        }

        public string name
        {
            get
            {
               return Path.GetFileNameWithoutExtension(video.vidPath);
            }
        }
        public int idFather;
        public readonly int id;
        /// <summary>
        /// The Video object represented by the SmartNode.
        /// </summary>
        public Video video
        {
            get;
            private set;
        }

        public string icon
        {
            get
            {
                if(video == null) return "";
                if (video.isAnalysis) return "/OQAT;component/vidanalysed-icon.png";
                //else if (video.processedBy != null) return "/OQAT;component/vidfiltered-icon.png";
                else return "/OQAT;component/vid-icon.png";
            }
        }

        /// <summary>
        /// Creates a new SmartNode from a given path to a video file.
        /// </summary>
        public SmartNode(IVideo vid, int id, int idFather, ObservableCollection<SmartNode> smartTree = null)
        {
            this.video =(Video) vid;
            this.id = id;
            this.idFather = idFather;
            if (smartTree != null)
                this.smartTree = smartTree;
        }

        /// <summary>
        /// return the name
        /// </summary>

        public override string ToString()
        {
            return name;
        }
        /// <summary>
        /// returns true if obj = this
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is SmartNode) || obj == null)
                return false;

            SmartNode node = obj as SmartNode;

            if (!this.name.Equals(node.name) || this.idFather!=node.idFather || this.id!=node.id
                || this.smartTree.Count != node.smartTree.Count||!this.video.Equals(node.video))
                return false;

            for (int i = 0; i < this.smartTree.Count; i++)
            {
                if (this.smartTree[i] != node.smartTree[i])
                    return false;
            }


            return true;
        }

    }
}
