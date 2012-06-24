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
    /// Every Oqat compatible Metric has to implement this interface.
    /// </summary>
	public interface IMetricOqat  : IPlugin
	{
        /// <summary>
        /// Processes both given Frames and result a <see cref="AnalysisInfo"/> object.
        /// </summary>
        /// <param name="frameRef">Reference frame.</param>
        /// <param name="frameProc">Processed frame.</param>
        /// <returns>Resulting AnalysisInfo object</returns>
		AnalysisInfo analyse(Bitmap frameRef, Bitmap frameProc);

	}
}

