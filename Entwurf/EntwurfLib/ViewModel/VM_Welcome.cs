//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.ViewModel
{
	using Oqat.Model;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    using Oqat.PublicRessources.Model;
    using System.Windows.Controls;
	public class VM_Welcome
	{
		public virtual Memento[] lastUsedProjects
		{
			get;
			set;
		}

        private void onSelectProject(object sender, EventArgs e) { }


		public VM_Welcome(Panel parent)
		{
		}

	}
}

