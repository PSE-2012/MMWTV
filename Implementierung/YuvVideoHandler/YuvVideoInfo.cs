//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace PS_YuvVideoHandler
{
	using Oqat.PublicRessources.Model;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
    using System.Runtime.Serialization;
    using System.Reflection;

    using System.IO;

    /// <summary>
    ///  All supported Yuv formats.
    /// </summary>
    /// <remarks>reference: http://www.fourcc.org/yuv.php#IYUV </remarks>
    [Serializable()]
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
    [Serializable()]
	public class YuvVideoInfo : IVideoInfo
	{

        YuvFormat _yuvFormat = YuvFormat.YUV444;
        int _width = 0;
        int _height = 0;
        int _frameCount = -1;
        int _framesize;
        string _path;

		public int width
		{
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                this.calculateFrameCount();
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
                this.calculateFrameCount();
            }
		}

        public int frameSize
        {
            get
            {
                return _framesize;
            }
            set
            {
                _framesize = value;
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
                this.calculateFrameCount();
            }
        }


        public string videoCodecName
        {
            get
            {
                return "yuv";
            }
        }

        public string path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
                this.calculateFrameCount();
            }
        }


        /// <summary>
        /// Calculates the number of frames of the video and writes this information into the vidInfo.
        /// </summary>
        /// <returns>true if operation was successful, false if an error occured</returns>
        private void calculateFrameCount()
        {
            frameSize = (int)(height * width * (1 + 2 * YuvVideoHandler.getLum2Chrom(yuvFormat)));

            if (File.Exists(_path))
            {
                FileInfo f = new FileInfo(_path);
                if (this.frameSize > 0)
                {
                    frameCount = (int)(f.Length / this.frameSize);
                }
            }
            else
            {
                frameCount = -1;
            }
        }

        public YuvVideoInfo() { }



        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Video return false.
            YuvVideoInfo p = obj as YuvVideoInfo;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (this._yuvFormat == p._yuvFormat
                && this._width == p._width && this._height == p._height);
        }

        public override int GetHashCode()
        {
            return (this.width ^ this.height << 4) | (int)this.yuvFormat;
        }

        public YuvVideoInfo(string path)
        {
            _path = path;

            // if Format names were found within the filename, set resolution 
            // and format accordingly
            if (Path.GetFileName(path).ToUpper().Contains("QCIF"))
            {
                height = 144;
                width = 176;
                yuvFormat = YuvFormat.YUV420_IYUV;
            }
            else if (Path.GetFileName(path).ToUpper().Contains("CIF"))
            {
                height = 288;
                width = 352;
                yuvFormat = YuvFormat.YUV420_IYUV;
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}

