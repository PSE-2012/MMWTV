//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.PublicRessources.Plugin
{
    using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    using System.Drawing;
    using Oqat.PublicRessources.Model;

	public interface IVideoHandler  : IPlugin
	{
		IVideoInfo vidInfo { get;set; }


		Bitmap getFrame(int frameNm);

		Bitmap[] getFrames(int frameNm, int offset);

		void writeFrame(int frameNum, Bitmap frame);

		void writeFrames(int frameNum, Bitmap[] frames);

	}
}

