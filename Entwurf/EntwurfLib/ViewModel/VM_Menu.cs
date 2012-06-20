//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
    using System.Windows.Controls;

	public class VM_Menu
	{
		private MenuItem menuItem_Project
		{
			get;
			set;
		}

		private MenuItem menuItem_Properties
		{
			get;
			set;
		}

		private VM_OptionsDialog vM_optionsDialog
		{
			get;
			set;
		}

		private VM_ProjectOpenDialog vM_ProjectOpenDialog
		{
			get;
			set;
		}


        //public VM_Menu(Control parent, del onBuildProjectView)
        //{
        //}
        // lasse den Konstruktoraufruf, es ist zwar weder kompliziert noch besonders hässlich aber
        // unser Pluginmanager sollte unser einziges EventPass System sein.

        public VM_Menu(Panel parent) { }

	}
}

