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
        //YUV420 not implemented at the moment
    }

	/// <remarks>an dieser stelle brauchen wir eine properties view für den VideoImportDialog und eine (oder de gleiche aber mit anderen parametern) die die infos in der smartlist darstellt</remarks>
	public class YuvVideoInfo : IVideoInfo
	{
		public int width
		{
			get;
			set;
		}

		public int height
		{
			get;
			set;
		}

		public long frameNum
		{
			get;
			set;
		}

        public YuvFormat yuvFormat
        {
            get;
            set;
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

