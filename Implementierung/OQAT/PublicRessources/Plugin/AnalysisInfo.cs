//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.PublicRessources.Plugin
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    using System.Drawing;

    /// <summary>
    /// This class is used to pass results from a <see cref="IMetric"/> plugin 
    /// to the caller ( usually PM_MacroMetric/>.
    /// </summary>
	public class AnalysisInfo
	{
        /// <summary>
        /// The result frame wich will be compound to a video.
        /// </summary>
		public virtual Bitmap frame
		{
			get;
			set;
		}

        /// <summary>
        /// Possible results to use by a Diagramm Plugin.
        /// </summary>
		public virtual float[] values
		{
			get;
			set;
		}

	}
}

