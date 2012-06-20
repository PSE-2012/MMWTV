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

	/// <remarks>an dieser stelle brauchen wir eine properties view für den VideoImportDialog und eine (oder de gleiche aber mit anderen parametern) die die infos in der smartlist darstellt</remarks>
	public class YuvVideoInfo : IVideoInfo
	{
		private int width
		{
			get;
			set;
		}

		private int height
		{
			get;
			set;
		}

		private long frameNum
		{
			get;
			set;
		}

	}
}

