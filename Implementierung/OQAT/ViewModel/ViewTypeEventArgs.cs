//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    /// <summary>
    /// This class can be used to pass ViewTypes on a toggleView Event
    /// </summary>
	public class ViewTypeEventArgs : EventArgs
	{
		public virtual ViewType viewType
		{
			get;
			private set;
		}

        public ViewTypeEventArgs(ViewType viewType)
        {
            this.viewType = viewType;
        }

	}
}

