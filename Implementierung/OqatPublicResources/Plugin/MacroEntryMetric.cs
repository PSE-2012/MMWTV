namespace Oqat.PublicRessources.Plugin
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    using Oqat.PublicRessources.Model;

    /// <summary>
    /// This class is used to determine the history of a analysis video object.
    /// </summary>
    [Serializable()]
	public class MacroEntryMetric : MacroEntry
	{
        /// <summary>
        /// Reference video used for this particular analysis.
        /// </summary>
		private IVideo vidRef
		{
			get;
			set;
		}

        /// <summary>
        /// Processed video used for this particular analysis.
        /// </summary>
		private IVideo vidProc
		{
			get;
			set;
		}

	}
}

