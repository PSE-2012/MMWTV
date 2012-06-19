//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace OqatPublicRessources.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class MementoEventArgs : EventArgs
	{
		private string pluginKey
		{
			get;
			set;
		}

		private string mementoName
		{
			get;
			set;
		}

	}
}

