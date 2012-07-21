using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;

using Oqat.PublicRessources.Model;
using Oqat.PublicRessources.Plugin;
using System.Collections.Generic;

namespace Oqat.Model
{
    /// <see cref="Project"/>
    /// <summary>
    /// This class is the Model for the tree structure where each video in a project 
    /// is represented by a node containing the path to it.
    /// </summary>
    class SmartNode
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
        /// <param name="vidPath"></param>
        public SmartNode(Video vid, int id, int idFather)
        {
            this.smartTree = new ObservableCollection<SmartNode>();
            this.video = vid;
            this.id = id;
            this.idFather = idFather;
        }


        /// <summary>
        /// Creates a new SmartNode from a given path to a video file.
        /// </summary>
        /// <param name="vidPath"></param>
        public SmartNode(bool isAnalysis, string vidPath, IVideoInfo vidInfo,int id, int idFather, List<MacroEntry> processedBy = null)
        {
            this.id = id;
            this.idFather = idFather;
            video = new Video(isAnalysis, vidPath, vidInfo, processedBy);
        }

        public override string ToString()
        {
            return name;
        }
    }
}
