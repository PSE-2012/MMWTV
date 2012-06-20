//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.ViewModel
{
	using Oqat.Model;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;


    using System.Windows.Controls;
    using Oqat.PublicRessources.Model;
	public class VM_PluginLists
	{
		private TabControl PluginLists
		{
			get;
			set;
		}

		private TabItem filterList
		{
			get;
			set;
		}

		private TabItem metricList
		{
			get;
			set;
		}

		private Button delete
		{
			get;
			set;
		}

		private PluginManager pluginManager
		{
			get;
			set;
		}

		private Caretaker caretaker
		{
			get;
			set;
		}




		public VM_PluginLists(Panel parent)
		{
		}

		private void onMacroFilterEntryClicked( object sender, EventArgs e)
		{
			throw new System.NotImplementedException();
		}
        private void onNewMementoCreated(object sender, MementoEventArgs e)
        {
            throw new System.NotImplementedException();
        }
        private void onEntryClicked(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }
        private void onEntrySelected(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void onToggleView(object sender, ViewTypeEventArgs e)
        {
            throw new System.NotImplementedException();
        }

	}
}

