//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace PP_Player
{
	using Oqat.PublicRessources.Model;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    /// <summary>
    ///  All supported Yuv formats.
    /// </summary>
    /// <remarks>reference: http://www.fourcc.org/yuv.php#IYUV </remarks>
    public enum YuvFormat
    {
        YUV444,
        YUV422_UYVY,
        YUV411_Y41P,
        YUV420_IYUV,
    }

    /// <summary>
    ///  Holds information about a yuv video file that can not be read from the file itself.
    ///  Needed to work with the video in a YuvVideoHandler.
    /// </summary>
	public class YuvVideoInfo : IVideoInfo
	{

        YuvFormat _yuvFormat = YuvFormat.YUV444;
        int _width = 0;
        int _height = 0;
        int _frameCount = -1;


		public int width
		{
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
		}

		public int height
		{
            get
            {
                return _height;
            }
            set
            {
                _height = value;
            }
		}

		public int frameCount
		{
            get
            {
                return _frameCount;
            }
            set
            {
                _frameCount = value;
            }
		}

        public YuvFormat yuvFormat
        {
            get
            {
                return _yuvFormat;
            }
            set
            {
                _yuvFormat = value;
            }
        }


        public string videoCodecName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}

