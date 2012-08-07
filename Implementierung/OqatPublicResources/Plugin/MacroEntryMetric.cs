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

        private IVideo _vidRef;
        private IVideo _vidProc;

        /// <summary>
        /// Reference video used for this particular analysis.
        /// </summary>
		public IVideo vidRef
		{
            get
            {
                return this._vidRef;
            }
            set
            {
                this._vidRef = value;
            }
		}

        /// <summary>
        /// Processed video used for this particular analysis.
        /// </summary>
        public IVideo vidProc
        {
            get
            {
                return this._vidProc;
            }
            set
            {
                this._vidProc = value;
            }
        }

	}
}

