//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
    using System.IO;

    /// <summary>
    /// This class is the Model for the global settings of OQAT.
    /// </summary>
	public class OqatProperties
	{
		/// <summary>
		/// Language settings of OQAT.
		/// </summary>
        private string language
		{
			get;
			set;
		}

        /// <summary>
        /// Folder where the program plugins are located.
        /// </summary>
		private string pluginFolder
		{
			get;
			set;
		}

	}
}

