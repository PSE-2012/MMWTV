//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.PublicRessources.Plugin
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    using System.Drawing;

	public class AnalysisInfo
	{
		public virtual Bitmap frame
		{
			get;
			set;
		}

		public virtual float[] values
		{
			get;
			set;
		}

	}
}

