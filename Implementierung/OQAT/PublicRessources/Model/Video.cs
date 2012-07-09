namespace Oqat.PublicRessources.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    using System.IO;
    using Oqat.PublicRessources.Plugin;
    using Oqat.ViewModel;


	/// <summary>
	/// This class is the Model for a Video object containing relevant information about a Video file.
	/// </summary>
    public class Video : IMemorizable
	{
        private IVideoInfo _vidInfo = null;
        private bool _isAnalysis = false;
        private string _path;
        private float[][] _metricValues;

        private List<MacroEntry> _processedBy;

        /// <summary>
        /// Creates a new instance of Video with empty parameters.
        /// </summary>
        public Video()
        {

        }

        /// <summary>
        /// Creates a new instance of Video.
        /// </summary>
        /// <param name="isAnalysis">true if the video is the result of an analysis.</param>
        /// <param name="vidPath">filepath to the video</param>
        /// <param name="vidInfo">videoInfo containing relevant information about the video. 
        /// This can be created by the VideoHandler using user input to its propertiesView.</param>
        /// <remarks>This contructor defaults processedBy to an empty list.</remarks>
        public Video(bool isAnalysis, string vidPath, IVideoInfo vidInfo)
            : this(isAnalysis, vidPath, vidInfo, new List<MacroEntry>())
        {
        }

        /// <summary>
        /// Creates a new instance of Video.
        /// </summary>
        /// <param name="isAnalysis">true if the video is the result of an analysis.</param>
        /// <param name="vidPath">filepath to the video</param>
        /// <param name="vidInfo">videoInfo containing relevant information about the video. 
        /// This can be created by the VideoHandler using user input to its propertiesView.</param>
        /// <param name="processedBy">a list of MacroEntries describing through which operations this video was created.</param>
        public Video(bool isAnalysis, string vidPath, IVideoInfo vidInfo, List<MacroEntry> processedBy)
        {
            this.isAnalysis = isAnalysis;
            this.vidPath = vidPath;
            this.vidInfo = vidInfo;
            this.processedBy = processedBy;

            if (videoObjectCreated != null)
                videoObjectCreated(this, new VideoEventArgs(this));
        }







        /// <summary>
        /// Returns the VideoInfo object containing additional format-specific information about the video.
        /// </summary>
		public IVideoInfo vidInfo
		{
			get
            {
                return _vidInfo;
            }
            private set
            {
                _vidInfo = value;
            }
		}

		/// <summary>
		/// Equals true if the video was created as a result of an analysis, e.g. to show
        /// the difference between two videos.
		/// </summary>
        public bool isAnalysis
		{
            get
            {
                return _isAnalysis;
            }
            private set
            {
                _isAnalysis = value;
            }
		}

        /// <summary>
        /// Path to the video file represented by this Video object.
        /// </summary>
		public string vidPath
		{
            get
            {
                return _path;
            }
            private set
            {
                _path = value;
            }
		}

        /// <summary>
        /// Gets or sets values calculated by the analyzation metric that created this video.
        /// Only applies if "isAnalysis" is true.
        /// The two dimensional array holds data series (first dimension) with values for each frame (second dimension)
        /// </summary>
		public float[][] frameMetricValue
		{
            get
            {
                return _metricValues;
            }
            set
            {
                _metricValues = value;
            }
		}

        /// <summary>
        /// Returns a list of filters (or a metric) the video has been processed by.
        /// If this is a reference video an empty list is returned.
        /// </summary>
		public List<MacroEntry> processedBy
		{
            get
            {
                return this._processedBy;
            }
            set
            {
                _processedBy = value;
            }
		}

        /// <summary>
        /// Extra resources relevant to the video, e.g. motion vectors.
        /// </summary>
		public Dictionary<PresentationPluginType, List<string>> extraResources
		{
            //TODO: extraResources
			get;
			set;
		}



        

        /// <summary>
        /// An event that indicates the creation of a new Video object.
        /// </summary>
        public delegate void videoObjectCreatedEventHandler(object sender, VideoEventArgs e);
		public event videoObjectCreatedEventHandler videoObjectCreated;





        /// <summary>
        /// Returns a video handler that may be used for getting or writing frames of the video.
        /// e.g. a YuvVideoHandnler to process the video file if it is of yuv-format.
        /// </summary>
        /// <returns>a video handler to acess the video frames.</returns>
		public virtual IVideoHandler getVideoHandler()
		{
            PluginManager pm = PluginManager.pluginManager;

            string handlerPluginName = Path.GetExtension(this.vidPath).ToLower() + "VideoHandler";

            IVideoHandler handler = pm.getPlugin < IVideoHandler>(handlerPluginName);

            handler = handler.createVideoHandlerInstance();
            handler.setVideo(this.vidPath, this.vidInfo);

            return handler;
		}


        /// <summary>
        /// Returns a memento that contains the current state of this video object
        /// in order to save this and restore it later.
        /// </summary>
        /// <returns>a memento that contains the current state of this video object</returns>
		public virtual Memento getMemento()
		{
            //TODO: implement video memento
			throw new System.NotImplementedException();
		}

        /// <summary>
        /// Sets the state of this video object to the one saved in the given memento.
        /// </summary>
        /// <param name="memento">a memento of a video instance</param>
		public virtual void setMemento(Memento memento)
		{
            //TODO: implement video memento
			throw new System.NotImplementedException();
		}




        
	}
}

