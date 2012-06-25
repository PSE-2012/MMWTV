//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.PublicRessources.Model
{

	using Plugins;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

using System.IO;
using Oqat.PublicRessources.Plugin;
	/// <summary>
	/// This class is the Model for a Video object containing relevant information about a Video file.
	/// </summary>
    public class Video : IMemorizable
	{
		private IVideoInfo vidInfo
		{
			get;
			set;
		}

		/// <summary>
		/// Equals true if the video was created as a result of an analysis, e.g. to show
        /// the difference between two videos.
		/// </summary>
        private bool isAnalysis
		{
			get;
			set;
		}

        /// <summary>
        /// Path to the video file represented by the Video object.
        /// </summary>
		private string vidPath
		{
			get;
			set;
		}

        /// <summary>
        /// Relevant for Analysis videos.
        /// </summary>
		public float[][] frameMetricValue
		{
			get;
			set;
		}

        /// <summary>
        /// Returns a list of filters the video has been processed by through OQAT (if any).
        /// </summary>
		private List<MacroEntry> processedBy
		{
			get;
			set;
		}

        /// <summary>
        /// Extra resources relevant to the video, e.g. motion vectors.
        /// P.S. in English it's spelled as "resource", with one 's' ;-)
        /// </summary>
		private Dictionary<PresentationPluginType, List<string>> extraRessources
		{
			get;
			set;
		}

        /// <see cref="VideoEventArgs"/>
        /// <summary>
        /// This method is executed on invokation of an event indicating the creation
        /// of a new Video object.
        /// </summary>
        /// <param name="sender"></param> Sender of the event.
        /// <param name="e"></param> Video to be passed through the event.
        private delegate void videoObjectCreatedEventHandler(Object sender, VideoEventArgs e);
        /// <summary>
        /// An event that indicates the creation of a new Video object.
        /// </summary>
		private event videoObjectCreatedEventHandler videoObjectCreated;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isAnalysis"></param> Equals true if the video is the result of an analysis.
        /// <param name="vidPath"></param> Path to the video.
        /// <param name="vidInfo"></param> An object containing relevant information about the video, i.e. codec name.
		public Video(bool isAnalysis,  string vidPath, IVideoInfo vidInfo)
		{
		}

        /// <summary>
        /// Returns the video handler that may be used for processing the video, 
        /// e.g. a YUV video handler for extracting or overwriting frames.
        /// </summary>
        /// <returns></returns>
		public virtual IVideoHandler getVideoHandler()
		{
			throw new System.NotImplementedException();
		}

        /// <summary>
        /// Can be used to load a saved previous state of a Video object.
        /// </summary>
        /// <returns></returns>
		public virtual Memento getMemento()
		{
			throw new System.NotImplementedException();
		}

        /// <summary>
        /// Can be used to save the current state of a Video object.
        /// </summary>
        /// <param name="memento"></param>
		public virtual void setMemento(Memento memento)
		{
			throw new System.NotImplementedException();
		}

	}
}

