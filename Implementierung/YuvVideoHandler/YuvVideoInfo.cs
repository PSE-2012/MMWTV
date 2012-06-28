//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace YuvVideoHandler
{
	using Oqat.PublicRessources.Model;
	using Plugins;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    public enum YuvFormat
    {
        YUV444,
        YUV422,
        YUV411,
        YUV420
    }

	/// <remarks>an dieser stelle brauchen wir eine properties view für den VideoImportDialog und eine (oder de gleiche aber mit anderen parametern) die die infos in der smartlist darstellt</remarks>
	public class YuvVideoInfo : IVideoInfo
	{

        YuvFormat _yuvFormat;
        int _width;
        int _height;


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

		public long frameNum
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

