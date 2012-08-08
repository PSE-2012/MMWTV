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
        PluginViewModel propPVM;
        bool copied = false;


        public VM_PluginsList()
        {
            InitializeComponent();

            PluginManager.OqatToggleView += this.onToggleView;
            PluginManager.newMementoCreated += onNewMementoCreated;
            PluginManager.macroEntrySelected += onMacroFilterEntryClicked;


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


		private void onMacroFilterEntryClicked( object sender, MementoEventArgs e)
		{
            propPVM = findPVM(e.pluginKey, e.mementoName);

            updatePropertiesView();
        }


        private void onNewMementoCreated(object sender, MementoEventArgs e)
        {
            //update the treeviews
            this.loadPluginLists();
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

            this.propPVM = (PluginViewModel)e.NewValue;

            updatePropertiesView();
        }

        private void updatePropertiesView()
        {
            if (propPVM == null)
            {
                this.panelMementoSave.Visibility = System.Windows.Visibility.Collapsed;
                return;
            }

            this.panelMementoSave.Visibility = System.Windows.Visibility.Visible;

            string pn = propPVM.getPluginName();
            propPlugin = PluginManager.pluginManager.getPlugin<IPlugin>(pn);

            if (propPVM.isMemento)
            {
                Memento m = PluginManager.pluginManager.getMemento(propPVM.parentName, propPVM.name);
                if (m != null)
                {
                    propPlugin.setMemento(m);

                    this.tbMementoName.Text = propPVM.name;
                    copied = false;
                }
                else
                {
                    MessageBox.Show("Die Einstellungen konnten nicht gefunden werden.");
                    this.loadPluginLists();
                    propPVM = new PluginViewModel(propPVM.parentName);
                    updatePropertiesView();
                }
            }
            else
            {
                this.tbMementoName.Text = propPVM.name + "_option";
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

            this.tbMementoName.Text = this.tbMementoName.Text + "_copy";
            //TODO: update propPVM
        }

        private void bttDeleteMemento_Click(object sender, RoutedEventArgs e)
        {
            if (!propPVM.isMemento)
            {
                return;
            }

            Memento m = new Memento(propPVM.name, null);
            //TODO: PluginManager.pluginManager.addMemento(propPVM.parentName, m);

            //update treeviews
            this.loadPluginLists();
        }

        private void tab_SelectedChanged(object sender, SelectionChangedEventArgs e)
        {
            propPVM = this.getSelectedPVM();
            updatePropertiesView();

            ViewType t = ViewType.FilterView;
            if(this.tabMetricList.IsSelected)
                t = ViewType.MetricView;

            PluginManager.pluginManager.raiseEvent(EventType.toggleView, new ViewTypeEventArgs(t));
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

            return (PluginViewModel)curTV.SelectedItem;
        }

        private PluginViewModel findPVM(string pluginName, string mementoName)
        {
            foreach (PluginViewModel p in filterList.Union(metricList))
            {
                if (p.getPluginName() == pluginName)
                {
                    foreach (PluginViewModel pc in p.children)
                    {
                        if (pc.name == mementoName)
                            return pc;
                    }
                }
            }

            return null;
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
                System.Windows.MessageBox.Show("Bitte geben Sie den zu speichernden Einstellungen einen Namen.", "Speichern nicht möglich");
                return false;
            }
            else if (findPVM(propPVM.getPluginName(), tbMementoName.Text) != null)
            {
                System.Windows.MessageBox.Show("Der Name der zu speichernden Einstellungen ist nicht eindeutig.", "Speichern nicht möglich");
                return false;
            }


            Memento mem = propPlugin.getMemento();
            mem.name = this.tbMementoName.Text;


            //TODO: how is same name as existing memento avoided?

            if (copied)
            {
                //TODO: PluginManager.pluginManager.addMemento(propPVM.parentName, mem);
            }
            else
            {
                if (mem.name != propPVM.name)
                {
                    Memento del = new Memento(propPVM.name, null);
                    //TODO: PluginManager.pluginManager.addMemento(propPVM.parentName, del);

                    //TODO: PluginManager.pluginManager.addMemento(propPVM.parentName, mem);
                }
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

        public bool selected
        {
            get;
            set;
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
