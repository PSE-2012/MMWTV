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
        /// <see cref="Video"/>
        /// <summary>
        /// 
        /// </summary>
		public virtual Video video
		{
			get;
			set;
		}

        /// <summary>
        /// Returns true if it's a reference video.
        /// </summary>
		public virtual bool isRefVid
		{
			get;
			set;
		}

	}
}

