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

    /// <summary>
    /// This ViewModel is responsible for displaying Filter and Metric plugins so the user can
    /// invocate (or change Properties, if a particular plugin provides the option)  them with keyboard or mouse.
    /// </summary>
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

        /// <summary>
        /// With this button the user can delete a particular memento of a filter / metric
        /// plugin.
        /// </summary>
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

