//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class ViewTypeEventArgs : EventArgs
	{
		public virtual ViewType viewType
		{
			get;
			set;
		}

	}
}

