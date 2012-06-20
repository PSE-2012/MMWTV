//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
namespace Oqat.PublicRessources.Model
{
	using Plugins.Metric;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class VideoEventArgs : EventArgs
	{
		public virtual Video video
		{
			get;
			set;
		}

		public virtual bool isRefVid
		{
			get;
			set;
		}

	}
}

