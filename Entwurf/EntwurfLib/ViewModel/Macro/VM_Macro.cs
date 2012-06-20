//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.ViewModel.Macro
{
	using Plugins.Metric;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Oqat.ViewModel;
    using Oqat.PublicRessources.Model;

	public class VM_Macro
	{
		private ViewType viewType
		{
			get;
			set;
		}

		private Macro macro
		{
			get;
			set;
		}

		private Video vidRef
		{
			get;
			set;
		}

		private Video vidProc
		{
			get;
			set;
		}

		private Video vidResult
		{
			get;
			set;
		}

		public VM_Macro()
		{
		}

        private void onPreviewLoad(object sender, EventArgs e) { }

		private void onToggleView(object sender, ViewTypeEventArgs e)
		{
			throw new System.NotImplementedException();
		}

		private void onStartProcess(object sender, EventArgs e)
		{
			throw new System.NotImplementedException();
		}

		private void onEntrySelect(object sender, MementoEventArgs e)
		{
			throw new System.NotImplementedException();
		}

		private void onMacroSave(object sender, EventArgs e)
		{
			throw new System.NotImplementedException();
		}

	}
}

