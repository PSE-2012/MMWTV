//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
using System.Windows.Controls;
    /// <summary>
    /// This viewmodel controls a modal window, which contains nearly all system or plugin relevant properties.
    /// </summary>
	public class VM_OptionsDialog
	{
		public virtual TreeView optionsTree
		{
			get;
			set;
		}

		public VM_OptionsDialog()
		{
			throw new System.NotImplementedException();
		}

	}
}

