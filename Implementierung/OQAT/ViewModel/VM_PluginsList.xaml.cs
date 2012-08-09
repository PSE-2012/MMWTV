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
using System.ComponentModel;


namespace Oqat.ViewModel
{
    /// <summary>
    /// Interaktionslogik für VM_PluginsList.xaml
    /// </summary>
    public partial class VM_PluginsList : UserControl
    {

        ObservableCollection<PluginViewModel> _pluginList;
        PluginType pluginType;

        IPlugin selectedPlugin;
        PluginViewModel selectedPVM;
        bool copied;


        public VM_PluginsList(PluginType plugintype)
        {
            InitializeComponent();

            this.pluginType = plugintype;

            PluginManager.newMementoCreated += onNewMementoCreated;
            PluginManager.macroEntrySelected += onMacroFilterEntryClicked;

            loadPluginLists();

            this.treePlugins.ItemsSource = pluginList;
            //TODO: does it work?!
            this.gridPluginProperties.DataContext = selectedPlugin;
        }




        private ObservableCollection<PluginViewModel> pluginList
        {
            get
            {
                return _pluginList;
            }
        }

        private void loadPluginLists()
        {
            _pluginList = new ObservableCollection<PluginViewModel>();

            
            foreach (string name in PluginManager.pluginManager.getPluginNames(pluginType))
            {
                PluginViewModel pl = new PluginViewModel(name);

                List<string> mementos = PluginManager.pluginManager.getMementoNames(name);
                if(mementos != null)
                {
                    foreach (string m in mementos)
                    {
                        pl.children.Add(new PluginViewModel(m, pl));
                    }
                }

                pluginList.Add(pl);
            }
        }








		private void onMacroFilterEntryClicked( object sender, MementoEventArgs e)
		{
            selectedPVM = findPVM(e.pluginKey, e.mementoName);
            selectedPVM.selected = true;

            updatePropertiesView();
        }


        private void onNewMementoCreated(object sender, MementoEventArgs e)
        {
            //update the treeviews
            this.loadPluginLists();
        }





        private PluginViewModel findPVM(string pluginName, string mementoName)
        {
            foreach (PluginViewModel p in pluginList)
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
        



        /// <summary>
        /// Display propertiesView of the newly selected filter or metric plugin.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treePlugins_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.selectedPVM = (PluginViewModel)e.NewValue;

            updatePropertiesView();
        }

        private void updatePropertiesView()
        {
            if (selectedPVM == null)
            {
                this.panelMementoSave.Visibility = System.Windows.Visibility.Collapsed;
                return;
            }

            this.panelMementoSave.Visibility = System.Windows.Visibility.Visible;

            string pn = selectedPVM.getPluginName();
            selectedPlugin = PluginManager.pluginManager.getPlugin<IPlugin>(pn);

            if (selectedPVM.isMemento)
            {
                Memento m = PluginManager.pluginManager.getMemento(selectedPVM.parentName, selectedPVM.name);
                if (m != null)
                {
                    selectedPlugin.setMemento(m);

                    this.tbMementoName.Text = selectedPVM.name;
                }
                else
                {
                    MessageBox.Show("Die Einstellungen konnten nicht gefunden werden.");
                    this.loadPluginLists();
                    selectedPVM = new PluginViewModel(selectedPVM.parentName);
                    updatePropertiesView();
                }
            }
            else
            {
                this.tbMementoName.Text = selectedPVM.name + "_option";
                selectedPVM = selectedPVM.parent;
            }


            if (selectedPlugin.propertyView == null)
            {
                this.panelMementoSave.Visibility = System.Windows.Visibility.Collapsed;
                return;
            }
            this.gridPluginProperties.Content = selectedPlugin.propertyView;
        }



        private void bttSaveMemento_Click(object sender, RoutedEventArgs e)
        {
            saveCurrentMemento();
        }

        private void bttCopyMemento_Click(object sender, RoutedEventArgs e)
        {
            this.copied = true;

            this.tbMementoName.Text = this.tbMementoName.Text + "_copy";
        }

        private void bttDeleteMemento_Click(object sender, RoutedEventArgs e)
        {
            if (!selectedPVM.isMemento)
            {
                return;
            }

            Memento m = new Memento(selectedPVM.name, null);
            PluginManager.pluginManager.addMemento(selectedPVM.getPluginName(), m);

            //update treeviews
            this.loadPluginLists();
        }


        private void bttAddToMacro_Click(object sender, RoutedEventArgs e)
        {
            if(saveCurrentMemento())
            {
                PluginManager.pluginManager.raiseEvent(EventType.macroEntryAdd, 
                    new MementoEventArgs(selectedPVM.name, selectedPVM.getPluginName()));
            }
        }

        private bool saveCurrentMemento()
        {
            if(!selectedPVM.isMemento) copied = true;

            if (this.tbMementoName.Text == "")
            {
                System.Windows.MessageBox.Show("Bitte geben Sie den zu speichernden Einstellungen einen Namen.", "Speichern nicht möglich");
                return false;
            }
            else if (copied && findPVM(selectedPVM.getPluginName(), tbMementoName.Text) != null)
            {
                System.Windows.MessageBox.Show("Der Name der zu speichernden Einstellungen ist nicht eindeutig.", "Speichern nicht möglich");
                return false;
            }
            

            Memento mem = selectedPlugin.getMemento();
            mem.name = this.tbMementoName.Text;


            if (copied)
            {
                selectedPVM = new PluginViewModel(mem.name, selectedPVM);
                selectedPVM.parent.children.Add(selectedPVM);
                selectedPVM.parent.childrenChanged();
            }


            if (mem.name != selectedPVM.name)
            {
                Memento del = new Memento(selectedPVM.name, null);
                PluginManager.pluginManager.addMemento(selectedPVM.getPluginName(), del);
            }
            PluginManager.pluginManager.addMemento(selectedPVM.getPluginName(), mem);
            
            
            //TODO: use event instead
            //this.loadPluginLists();

            return true;
            //treeview is updated through the onNewMementoCreated handler
        }



    }


    public class PluginViewModel : INotifyPropertyChanged
    {
        public PluginViewModel parent
        {
            get;
            set;
        }

        string _name;
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != name)
                {
                    _name = value;
                    NotifyPropertyChanged("name");
                }
            }
        }

        public string parentName
        {
            get
            {
                if (parent != null)
                    return parent.name;
                else
                    return "";
            }
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

        bool _selected;
        public bool selected
        {
            get
            {
                return _selected;
            }
            set
            {
                if (value != selected)
                {
                    _selected = value;
                    NotifyPropertyChanged("selected");
                }
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

        public void childrenChanged()
        {
            NotifyPropertyChanged("children");
        }



        bool memento;
        public PluginViewModel(string pluginName)
        {
            this.name = pluginName;
            memento = false;

            this.children = new ObservableCollection<PluginViewModel>();
        }
        public PluginViewModel(string mementoName, PluginViewModel plugin)
        {
            this.name = mementoName;
            this.parent = plugin;
            memento = true;
        }



        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

    }
}
