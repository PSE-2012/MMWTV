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
    class SmartNode : ISerializable
    {

        private List<SmartNode> _smartTreeBackup;
        private List<SmartNode> smartTreeBackup
        {
            get
            {
                if (_smartTree == null)
                    throw new ArgumentNullException("SmartTree uninitialized, cant backup.");
                if (_smartTreeBackup == null)
                {
                    _smartTreeBackup = new List<SmartNode>();
                    foreach (var i in smartTree)
                    {
                        _smartTreeBackup.Add(i);
                    }
                }
                return _smartTreeBackup;
            }
        }

        private ObservableCollection<SmartNode> _smartTree;
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

        /// <summary>
        /// Creates a new SmartNode from a given path to a video file.
        /// </summary>
        public SmartNode(Video vid, int id, int idFather, ObservableCollection<SmartNode> smartTree = null)
        {
            this.video = vid;
            this.id = id;
            this.idFather = idFather;
            if (smartTree != null)
                this.smartTree = smartTree;
        }

        public override string ToString()
        {
            return name;
        }

        private ObservableCollection<SmartNode> restoreIndex(List<SmartNode> smartTreeBackup)
        {
            ObservableCollection<SmartNode> restoredIndex = new ObservableCollection<SmartNode>();
            foreach (var i in smartTreeBackup)
            {
                restoredIndex.Add(i);
            }
            return restoredIndex;
        }

        public SmartNode(SerializationInfo info, StreamingContext ctxt) :
            this((Video)info.GetValue("video", typeof(Video)), 
            (int)info.GetValue("id", typeof(int)), 
            (int)info.GetValue("idFather", typeof(int)))
        {
            this.smartTree = restoreIndex(
                (List<SmartNode>)info.GetValue("smartTreeBackup", typeof(List<SmartNode>)));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("smartTreeBackup", this.smartTreeBackup);
            info.AddValue("idFather", this.idFather);
            info.AddValue("id", this.id);
            info.AddValue("video", this.video);
        }
    }
}
