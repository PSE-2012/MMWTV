using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Oqat.PublicRessources.Model;
using Oqat.PublicRessources.Plugin;

namespace Oqat.ViewModel
{
    /// <summary>
    /// Interaktionslogik für VM_PluginsList.xaml
    /// </summary>
    public partial class VM_PluginsList : UserControl
    {

        List<PluginViewModel> filterList;
        List<PluginViewModel> metricList;

        public VM_PluginsList(Panel parent)
        {
            InitializeComponent();
            parent.Children.Add(this);

            loadPluginLists();

            this.treeFilters.DataContext = this.filterList;
        }

        private void loadPluginLists()
        {
            this.filterList = new List<PluginViewModel>();
            this.metricList = new List<PluginViewModel>();




            /*
            foreach (string name in PluginManager.pluginManager.getPluginNames(PluginType.IFilterOqat))
            {
                filterList.Add(new PluginViewModel(name));
            }

            foreach (string name in PluginManager.pluginManager.getPluginNames(PluginType.IMetric))
            {
                metricList.Add(new PluginViewModel(name));
            }
            */
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


    public class PluginViewModel
    {
        string _pluginName;
        List<string> _mementos;

        public string pluginName
        {
            get
            {
                return _pluginName;
            }
        }

        public IList<string> mementos
        {
            get
            {
                return _mementos;
            }
        }


        public PluginViewModel(string name)
        {
            _pluginName = name;

            _mementos = PluginManager.pluginManager.getMementoNames(name);
        }

    }
}
