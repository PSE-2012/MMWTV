//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.PublicRessources.Plugin
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    /// <summary>
    /// This class is used to determine the history of a video object ( e.g. filtered by) and
    /// by classes inheriting from Macro (to reconstruct filter bound to a macro filter)
    /// </summary>
    [Serializable()]
	public class MacroEntryFilter : MacroEntry
	{
        private double _endFrameRelative;
        private double _startFrameRelative;

		public double endFrameRelative
		{
            get
            {
                return this._endFrameRelative;
            }
            set
            {
                this._endFrameRelative = value;
            }
		}

        public double startFrameRelative
        {
            get
            {
                return this._startFrameRelative;
            }
            set
            {
                this._startFrameRelative = value;
            }
        }

	}
}

