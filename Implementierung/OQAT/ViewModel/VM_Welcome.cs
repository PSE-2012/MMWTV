namespace Oqat.ViewModel
{
	using Oqat.Model;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    using Oqat.PublicRessources.Model;
    using System.Windows.Controls;

    /// <summary>
    /// This component is displayed whenever no project is open.
    /// </summary>
	public class VM_Welcome
	{
        /// <summary>
        /// A list of recently used projects.
        /// </summary>
		public virtual Memento[] lastUsedProjects
		{
			get;
			set;
		}
        /// <summary>
        /// This delegate will be called if the user chooses a project from the "Recently used projects" List and
        /// is responsible for initializing the choosen project.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onSelectProject(object sender, EventArgs e) { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parent">A panel where VM_Welcome can place itself.</param>
		public VM_Welcome(Panel parent)
		{
		}

	}
}

