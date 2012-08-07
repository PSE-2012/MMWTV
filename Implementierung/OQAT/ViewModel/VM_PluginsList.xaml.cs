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
using Oqat.Model;
using Oqat.PublicRessources.Plugin;
using System.Collections.ObjectModel;


namespace Oqat.ViewModel
{
    /// <summary>
    /// Interaktionslogik für VM_PluginsList.xaml
    /// </summary>
    public partial class VM_PluginsList : UserControl
    {

        ObservableCollection<PluginViewModel> _filterList;
        ObservableCollection<PluginViewModel> _metricList;

        IPlugin propPlugin;
        bool copied = false;


        public VM_PluginsList()
        {
            InitializeComponent();

            PluginManager.OqatToggleView += this.onToggleView;
            PluginManager.newMementoCreated += onNewMementoCreated;
            PluginManager.macroEntryClicked += onMacroFilterEntryClicked;

            loadPluginLists();

            this.treeFilters.DataContext = this.filterList;
            this.treeMetrics.DataContext = this.metricList;
        }




        public ObservableCollection<PluginViewModel> filterList
        {
            get
            {
                return _filterList;
            }
        }
        public ObservableCollection<PluginViewModel> metricList
        {
            get
            {
                return _metricList;
            }
        }

        

        private void loadPluginLists()
        {
            _filterList = new ObservableCollection<PluginViewModel>();
            _metricList = new ObservableCollection<PluginViewModel>();

            
            foreach (string name in PluginManager.pluginManager.getPluginNames(PluginType.IFilterOqat))
            {
                PluginViewModel pl = new PluginViewModel(name);

                List<string> mementos = PluginManager.pluginManager.getMementoNames(name);
                if(mementos != null)
                {
                    foreach (string m in mementos)
                    {
                        pl.children.Add(new PluginViewModel(m, name));
                    }
                }

                filterList.Add(pl);
            }


            foreach (string name in PluginManager.pluginManager.getPluginNames(PluginType.IMetricOqat))
            {
                PluginViewModel pl = new PluginViewModel(name);

                List<string> mementos = PluginManager.pluginManager.getMementoNames(name);
                if (mementos != null)
                {
                    foreach (string m in mementos)
                    {
                        pl.children.Add(new PluginViewModel(m, name));
                    }
                }

                metricList.Add(pl);
            }
        }



        /// <summary>
        /// Focus metric or filter list fitting the new ViewType
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onToggleView(object sender, ViewTypeEventArgs e)
        {
            if (e.viewType == ViewType.FilterView)
            {
                this.tabFilterList.IsSelected = true;
            }
            else if (e.viewType == ViewType.MetricView)
            {
                this.tabMetricList.IsSelected = true;
            }
        }


		private void onMacroFilterEntryClicked( object sender, EventArgs e)
		{
			//TODO: select the corresponding memento from the treeview in order to display the propertiesView
		}


        private void onNewMementoCreated(object sender, MementoEventArgs e)
        {
            //update the treeviews
            this.loadPluginLists();
        }

        /// <summary>
        /// Returns the currently selected and active plugin / memento&plugin from the PluginsList.
        /// </summary>
        /// <returns></returns>
        private PluginViewModel getSelectedPVM()
        {
            TreeView curTV;
            if (this.tabFilterList.IsSelected)
                curTV = this.treeFilters;
            else
                curTV = this.treeMetrics;

            return (PluginViewModel) curTV.SelectedItem;
        }


        /// <summary>
        /// Display propertiesView of the newly selected filter or metric plugin.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treePlugins_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue == null)
            {
                this.panelMementoSave.Visibility = System.Windows.Visibility.Hidden;
                return;
            }

            PluginViewModel val = (PluginViewModel)e.NewValue;

            updatePropertiesView(val);
        }

        private void updatePropertiesView(PluginViewModel selectedPlugin)
        {
            this.gridPluginProperties.Children.Clear();

            if (selectedPlugin == null)
            {
                this.panelMementoSave.Visibility = System.Windows.Visibility.Collapsed;
                return;
            }

            this.panelMementoSave.Visibility = System.Windows.Visibility.Visible;

            string pn = selectedPlugin.getPluginName();
            propPlugin = PluginManager.pluginManager.getPlugin<IPlugin>(pn);

            if (selectedPlugin.isMemento)
            {
                Memento m = PluginManager.pluginManager.getMemento(selectedPlugin.parentName, selectedPlugin.name);
                if (m != null)
                    propPlugin.setMemento(m);

                copied = false;
            }
            else
            {
                copied = true;
            }


            if (propPlugin.propertyView == null)
            {
                this.panelMementoSave.Visibility = System.Windows.Visibility.Collapsed;
                return;
            }
            
            this.gridPluginProperties.Children.Add(propPlugin.propertyView);
        }



        private void bttSaveMemento_Click(object sender, RoutedEventArgs e)
        {
            saveCurrentMemento();
        }

        private void bttCopyMemento_Click(object sender, RoutedEventArgs e)
        {
            this.copied = true;

            this.tbMementoName.Text = getSelectedPVM().name + "_copy";
        }

        private void bttDeleteMemento_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Delete the memento through pluginManager

            
            //update treeviews
            this.loadPluginLists();
        }

        private void tab_SelectedChanged(object sender, SelectionChangedEventArgs e)
        {
            updatePropertiesView(this.getSelectedPVM());

            ViewType t = ViewType.FilterView;
            if(this.tabMetricList.IsSelected)
                t = ViewType.MetricView;

            PluginManager.pluginManager.raiseEvent(EventType.toggleView, new ViewTypeEventArgs(t));
        }

        private void bttAddToMacro_Click(object sender, RoutedEventArgs e)
        {
            if(saveCurrentMemento())
            {
                PluginManager.pluginManager.raiseEvent(EventType.macroEntryAdd, 
                    new MementoEventArgs(tbMementoName.Text, getSelectedPVM().getPluginName()));
            }
        }

        private bool saveCurrentMemento()
        {
            if (this.tbMementoName.Text == "")
            {
                System.Windows.MessageBox.Show("Bitte geben Sie den zu speichernden Einstellungen einen Namen.");
                return false;
            }


            Memento mem = propPlugin.getMemento();
            mem.name = this.tbMementoName.Text;


            //TODO: how is same name as existing memento avoided?

            if (copied)
            {
                //TODO: write Memento through pluginManager
            }
            else
            {
                //TODO: overwrite selected Memento
            }


            return true;
            //treeview is updated through the onNewMementoCreated handler
        }



    }


    public class PluginViewModel
    {
        public string name
        {
            get;
            set;
        }
        public string parentName
        {
            get;
            set;
        }

        public string getPluginName()
        {
            if (memento)
                return parentName;
            else
                return name;
        }

        public bool isMemento
        {
            get
            {
                return memento;
            }
        }


        ObservableCollection<PluginViewModel> _children;
        public ObservableCollection<PluginViewModel> children
        {
            get
            {
                return _children;
            }
            private set
            {
                _children = value;
            }
        }

        bool memento;
        public PluginViewModel(string pluginName)
        {
            this.name = pluginName;
            memento = false;

            this.children = new ObservableCollection<PluginViewModel>();
        }
        public PluginViewModel(string mementoName, string pluginName)
        {
            this.name = mementoName;
            this.parentName = pluginName;
            memento = true;
        }

    }
}
