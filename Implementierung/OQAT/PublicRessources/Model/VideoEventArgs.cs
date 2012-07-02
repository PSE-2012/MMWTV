//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
namespace Oqat.PublicRessources.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// This class can be used to pass videos through events, i.e. loading a video.
	/// </summary>
    public class VideoEventArgs : EventArgs
	{
        private Video _video;
        private bool _isRefVid;

        /// <summary>
        /// Constructs a new instance of VideoEventArgs with the given video.
        /// The property isRefVideo defaults to false.
        /// </summary>
        /// <param name="video">the video object for these eventArgs</param>
        public VideoEventArgs(Video video)
        {
            this.video = video;
            this.isRefVid = false;
        }
        /// <summary>
        /// Constructs a new instance of VideoEventArgs with the given video and status.
        /// </summary>
        /// <param name="video">the video object for these eventArgs</param>
        /// <param name="isRef">status wether this video is passed as a reference video</param>
        public VideoEventArgs(Video video, bool isRef)
        {
            this.video = video;
            this.isRefVid = isRef;
        }


        /// <see cref="Video"/>
        /// <summary>
        /// The video associated with this event.
        /// </summary>
		public virtual Video video
		{
            get
            {
                return _video;
            }
            private set
            {
                _video = value;
            }
		}

        /// <summary>
        /// Returns true if it's a reference video. Default is false;
        /// </summary>
		public virtual bool isRefVid
		{
            get
            {
                return _isRefVid;
            }
            private set
            {
                _isRefVid = value;
            }
		}
	}
}

