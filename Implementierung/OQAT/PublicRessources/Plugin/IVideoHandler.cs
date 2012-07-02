namespace Oqat.PublicRessources.Plugin
{
    using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    using System.Drawing;
    using Oqat.PublicRessources.Model;


    /// <summary>
    /// Every VideoHandler has to implement this interface.
    /// A VideoHandler is used for importing and exporting videos from and to hard drive.
    /// Third parties can extent the video format support by writing a VideoHandler for
    /// a specific video format.
    /// </summary>
	public interface IVideoHandler  : IPlugin
	{
        /// <summary>
        /// A format specific VideoInfo object, see <see cref="IVideoInfo"/> for further information.
        /// </summary>
		IVideoInfo vidInfo { get; }

        /// <summary>
        /// Returns a frame from the currently loaded Video.
        /// </summary>
        /// <param name="frameNm">The frame to return.</param>
        /// <returns>Frame from currently loaded Video.</returns>
		Bitmap getFrame(int frameNm);

        /// <summary>
        /// Nearly the same as getFrame(int frameNm) except that
        /// offset number of frames will be returned.
        /// </summary>
        /// <param name="frameNm">The frame to return.</param>
        /// <param name="offset"Number of frames to return.></param>
        /// <returns>Array of frames from the currently loaded Video.</returns>
		Bitmap[] getFrames(int frameNm, int offset);

        /// <summary>
        /// Writes a frame to the currently loaded Video.
        /// </summary>
        /// <param name="frameNum">The "slot" the given frame will be written in.</param>
        /// <param name="frame">Frame to write.</param>
		void writeFrame(int frameNum, Bitmap frame);

        /// <summary>
        /// Nearly the same as writeFrame(int frameNum, Bitman frame) except that
        /// a array of frames will be written.
        /// </summary>
        /// <param name="frameNum">Slot to place the first frame in.</param>
        /// <param name="frames">Array of frames to place into the currently loaded video.</param>
		void writeFrames(int frameNum, Bitmap[] frames);

	}
}

