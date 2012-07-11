namespace Oqat.PublicRessources.Plugin
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    using System.Drawing;

    /// <summary>
    /// Every Oqat compatible Filter has to implement this interface.
    /// </summary>
	public interface IFilterOqat  : IPlugin
	{
        /// <summary>
        /// Processes the given frame and returns the
        /// result as a Bitmap
        /// </summary>
        /// <param name="frame">Frame to precess</param>
        /// <returns>Processed result</returns>
		Bitmap process(Bitmap frame);

	}
}

