using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;

using Oqat.PublicRessources.Model;
namespace Oqat.Model
{
    /// <see cref="Project"/>
    /// <summary>
    /// This class is the Model for the tree structure where each video in a project 
    /// is represented by a node containing the path to it.
    /// </summary>
    class SmartNode : Collection<SmartNode>
    {
        /// <summary>
        /// The Video object represented by the SmartNode.
        /// </summary>
        private Video video
        {
            get;
            set;
        }

        /// <summary>
        /// Creates an empty SmartNode.
        /// </summary>
        public SmartNode()
        {
        }
        /// <summary>
        /// Creates a new SmartNode from a given path to a video file.
        /// </summary>
        /// <param name="vidPath"></param>
        public SmartNode(string vidPath)
        {
        }
    }
}
