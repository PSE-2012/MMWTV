//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace OqatPublicRessources.Plugin
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    using System.Drawing;
	public interface IMetricOqat  : IPlugin
	{
		AnalysisInfo analyse(Bitmap frame, Bitmap frame);

	}
}

