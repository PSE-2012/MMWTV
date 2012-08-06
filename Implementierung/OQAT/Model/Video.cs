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
    using System.Runtime.Serialization;

	/// <summary>
	/// This class is the Model for a Video object containing relevant information about a Video file.
	/// </summary>
    [Serializable()]
    public class Video : IVideo, ISerializable
	{
        private IVideoInfo _vidInfo = null;
        private bool _isAnalysis = false;
        private string _path = "";
        private float[][] _metricValues;
        private Dictionary<PresentationPluginType, List<string>> _extraResources;

        private List<MacroEntry> _processedBy;


        /// <summary>
        /// Creates a new instance of Video.
        /// </summary>
        /// <param name="isAnalysis">true if the video is the result of an analysis.</param>
        /// <param name="vidPath">filepath to the video</param>
        /// This can be created by the VideoHandler using user input to its propertiesView.</param>
        /// <param name="processedBy">a list of MacroEntries describing through which operations this video was created.</param>
        /// <remarks>Note that optional parameters are used (for vidInfo and processedBy). If you do not
        /// need them just dont mention them in the invocation, else see <see cref="http://msdn.microsoft.com/de-de/library/dd264739.aspx"/>
        public Video(bool isAnalysis, string vidPath, IVideoInfo vidInfo = null,List<MacroEntry> processedBy = null)
        {
            this.isAnalysis = isAnalysis;
            this.vidPath = vidPath;
            
            this.vidInfo =vidInfo;
            this.processedBy = (processedBy != null) ? processedBy: new List<MacroEntry>();


            // dont see the need for this event right now...
            // vidImport dialog constructs this object and the video can be extracted from it
            // TODO: Maybe this should not be done in the video class but in a class creating the videos.
            //if (videoObjectCreated != null)
            //    videoObjectCreated(this, new VideoEventArgs(this));
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
            set
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
            set
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
            get
            {
                return _extraResources;
            }
            set
            {
                _extraResources = value;
            }
		}



        

        /// <summary>
        /// An event that indicates the creation of a new Video object.
        /// </summary>
        // TODO: Maybe this should not be done in the video class but in a class creating the videos.
        public delegate void videoObjectCreatedEventHandler(object sender, VideoEventArgs e);
		public event videoObjectCreatedEventHandler videoObjectCreated;


        private IVideoHandler _handler;
        public IVideoHandler handler
        {
            get
            {
                return (_handler != null) ? _handler : getVideoHandler();
            }
        }


        /// <summary>
        /// Returns a video handler that may be used for getting or writing frames of the video.
        /// e.g. a YuvVideoHandnler to process the video file if it is of yuv-format.
        /// </summary>
        /// <returns>a video handler to acess the video frames.</returns>
		private IVideoHandler getVideoHandler()
		{

            PluginManager pm = PluginManager.pluginManager;

            string handlerPluginName = Path.GetExtension(this.vidPath).ToLower().TrimStart(new char[] { '.' }) + "VideoHandler";

            _handler = pm.getPlugin<IVideoHandler>(handlerPluginName);
            if (_handler == null)
                throw new Exception("Cant process given video file format.");
            //TODO: catch this exception. oqat shouldn't crash on a unknown file!

                //PluginManager.pluginManager.raiseEvent(EventType.panic, 
                //    new ErrorEventArgs(new Exception("No such ")));

            _handler = handler.createVideoHandlerInstance();
            if (this.vidInfo != null)
            {
                _handler.setVideo(this.vidPath, this.vidInfo);
            }

            return _handler;
		}


        /// <summary>
        /// Returns a memento that contains the current state of this video object
        /// in order to save this and restore it later.
        /// </summary>
        /// <returns>a memento that contains the current state of this video object</returns>
		public virtual Memento getMemento()
		{
            FileInfo inf = new FileInfo(this._path);

            return new Memento(inf.Name, this, inf.FullName + ".mem");
		}

        /// <summary>
        /// Sets the state of this video object to the one saved in the given memento.
        /// </summary>
        /// <exception cref="ArgumentException">Throws ArgumentException if the memento is not a memento of a video object.</exception>
        /// <param name="memento">a memento of a video instance</param>
		public virtual void setMemento(Memento memento)
		{
            if( !(memento.state is Video) )
                throw new ArgumentException("Invalid Memento to restore Video.\n"+memento.mementoPath+" is not a Video object memento.");
            
            Video refv =(Video) memento.state;
            this._path = refv._path;
            this._extraResources = refv._extraResources;
            this._isAnalysis = refv._isAnalysis;
            this._metricValues = refv._metricValues;
            this._processedBy = refv._processedBy;
            this._vidInfo = refv._vidInfo;
		}

        public Video(SerializationInfo info, StreamingContext ctxt) :
            this((bool)info.GetValue("isAnalysis", typeof(bool)), 
            (string)info.GetValue("vidPath", typeof(string)),
            (IVideoInfo)info.GetValue("vidInfo", typeof(IVideoInfo)), 
            (List<MacroEntry>)info.GetValue("processedBy", typeof(List<MacroEntry>)))
        {
            this.extraResources = 
                (Dictionary<PresentationPluginType, List<string>>)info.GetValue("extraResources", 
                typeof(Dictionary<PresentationPluginType, List<string>>));
        }


        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("isAnalysis", this.isAnalysis);
            info.AddValue("vidPath", this.vidPath);
            info.AddValue("vidInfo", this.vidInfo);
            info.AddValue("processedBy", this.processedBy);
            info.AddValue("extraResources", this.extraResources);
            info.AddValue("frameMetricValue", this.frameMetricValue);

        }
    }
}

