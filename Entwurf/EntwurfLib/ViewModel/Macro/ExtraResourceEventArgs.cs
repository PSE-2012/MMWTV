//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.Macro.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    using System.IO;
    using Oqat.PublicRessources.Plugin;

	public class ExtraResourceEventArgs : EventArgs
	{
		public virtual string resourcepath
		{
			get;
			set;
		}

		public virtual PresentationPluginType resourcetype
		{
			get;
			set;
		}

	}
}

