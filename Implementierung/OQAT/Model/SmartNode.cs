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

        public ObservableCollection<SmartNode> smartTree
        {
            get;
            private set;
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
        public SmartNode(Video vid, int id, int idFather)
        {
            this.smartTree = new ObservableCollection<SmartNode>();
            this.video = vid;
            this.id = id;
            this.idFather = idFather;
        }

        public override string ToString()
        {
            return name;
        }

        public SmartNode(SerializationInfo info, StreamingContext ctxt) :
            this((Video)info.GetValue("video", typeof(Video)), 
            (int)info.GetValue("id", typeof(int)), 
            (int)info.GetValue("idFather", typeof(int)))
        {
            this.smartTree = 
                (ObservableCollection<SmartNode>)info.GetValue("smartTree", typeof(ObservableCollection<SmartNode>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("smartTree", this.smartTree);
            info.AddValue("idFather", this.idFather);
            info.AddValue("id", this.id);
            info.AddValue("video", this.video);
        }
    }
}
