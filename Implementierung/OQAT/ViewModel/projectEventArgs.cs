namespace Oqat.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    using Oqat.Model;

    /// <summary>
    /// This class can be used to pass projects trough events, i.e. after construction.
    /// </summary>
	public class ProjectEventArgs : EventArgs
	{
		private Project project
		{
			get;
			set;
		}

	}
}

