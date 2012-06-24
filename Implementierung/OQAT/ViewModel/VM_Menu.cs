namespace Oqat.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
    using System.Windows.Controls;

    /// <summary>
    /// This class is the ViewModel for the menu bar. Nearly all system or plugin relevant properties
    /// can be maid here.
    /// </summary>
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

        /// <summary>
        /// VM_OptionsDialog instance, should be checked on actuality on every invocation.
        /// </summary>
		private VM_OptionsDialog vM_optionsDialog
		{
			get;
			set;
		}

        /// <summary>
        /// Assistent for creating projects.
        /// </summary>
		private VM_ProjectOpenDialog vM_ProjectOpenDialog
		{
			get;
			set;
		}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent">The panel where VM_Menu should display itself. Usually this will be at the top
        /// of the main window.(</param>
        public VM_Menu(Panel parent) { }

	}
}

