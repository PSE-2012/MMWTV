using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oqat.PublicRessources.Plugin;

namespace Oqat.PublicRessources.Model
{
    public interface IVideo : IMemorizable
    {


        /// <summary>
        /// Returns the VideoInfo object containing additional format-specific information about the video.
        /// </summary>
		IVideoInfo vidInfo
		{
			get;
		}

		/// <summary>
		/// Equals true if the video was created as a result of an analysis, e.g. to show
        /// the difference between two videos.
		/// </summary>
        bool isAnalysis
		{
            get;
		}

        /// <summary>
        /// Path to the video file represented by this Video object.
        /// </summary>
		string vidPath
		{
            get;
		}

        /// <summary>
        /// Gets or sets values calculated by the analyzation metric that created this video.
        /// Only applies if "isAnalysis" is true.
        /// The two dimensional array holds data series (first dimension) with values for each frame (second dimension)
        /// </summary>
		float[][] frameMetricValue
		{
            get;
		}

        /// <summary>
        /// Returns a list of filters (or a metric) the video has been processed by.
        /// If this is a reference video an empty list is returned.
        /// </summary>
		List<MacroEntry> processedBy
		{
            get;
		}

        /// <summary>
        /// Extra resources relevant to the video, e.g. motion vectors.
        /// </summary>
		Dictionary<PresentationPluginType, List<string>> extraResources
		{
           get;
		}

        /// <summary>
        /// Returns a video handler that may be used for getting or writing frames of the video.
        /// e.g. a YuvVideoHandnler to process the video file if it is of yuv-format.
        /// </summary>
        /// <returns>a video handler to acess the video frames.</returns>
		IVideoHandler getVideoHandler();


    }
}
