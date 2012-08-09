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
        private ObservableCollection<PluginViewModel> pluginList
        {
            get
            {
                return _pluginList;
            }
        }
        PluginType pluginType;

        IPlugin selectedPlugin;
        PluginViewModel selectedPVM
        {
            get
            {
                return (PluginViewModel)this.treePlugins.SelectedItem;
            }
        }
        bool copied;


        public VM_PluginsList(PluginType plugintype)
        {
            InitializeComponent();

            this.pluginType = plugintype;

            PluginManager.macroEntrySelected += onMacroFilterEntryClicked;

            loadPluginLists();
            this.treePlugins.ItemsSource = pluginList;
        }

        /// <summary>
        /// Fills the list of PluginViewModels with plugin and memento data from PluginManager.
        /// </summary>
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



        /// <summary>
        /// Finds the reference to a memento PluginViewModel in the pluginList with the given name.
        /// </summary>
        /// <param name="pluginName">Name of the plugin (the parent) of the memento.</param>
        /// <param name="mementoName">Name of the memento to search.</param>
        /// <returns>the PluginViewModel from pluginList or null if none was found.</returns>
        private PluginViewModel findPVM(string pluginName, string mementoName)
        {
            foreach (PluginViewModel p in pluginList)
            {
                if (p.parent.name == pluginName)
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
        /// Loads the propertyView of the selected plugin and manages visibility of the according pluginList buttons.
        /// </summary>
        private void updatePropertiesView()
        {
            if (selectedPVM == null)
            {
                this.panelMementoSave.Visibility = System.Windows.Visibility.Collapsed;
                return;
            }


            
            selectedPlugin = PluginManager.pluginManager.getPlugin<IPlugin>(selectedPVM.parent.name);
            if (selectedPVM.isMemento)
            {
                Memento m = PluginManager.pluginManager.getMemento(selectedPVM.parent.name, selectedPVM.name);
                if (m != null)
                {
                    selectedPlugin.setMemento(m);
                }
                else
                {
                    MessageBox.Show("Die Einstellungen konnten nicht gefunden werden.");

                    //remove the broken entry
                    selectedPVM.parent.children.Remove(selectedPVM);
                }
            }

            if (selectedPlugin.propertyView == null)
            {
                this.panelMementoSave.Visibility = System.Windows.Visibility.Collapsed;
                return;
            }
            this.gridPluginProperties.Content = selectedPlugin.propertyView;
            this.tbMementoName.Text = selectedPVM.name;
            this.panelMementoSave.Visibility = System.Windows.Visibility.Visible;
        }


        /// <summary>
        /// Copies the selected memento internally.
        /// </summary>
        private bool mementoCopy(PluginViewModel memento)
        {
            //find available new name
            string name = this.tbMementoName.Text;
            int nameext = 1;
            while (findPVM(memento.parent.name, (name + nameext)) != null)
            {
                nameext++;
            }
            name = name + nameext;
            this.tbMementoName.Text = name;


            PluginViewModel copymem = new PluginViewModel(name, memento.parent);
            memento.parent.children.Add(copymem);

            return mementoSave(copymem);
        }

        /// <summary>
        /// Deletes the selected memento.
        /// </summary>
        private void mementoDelete(PluginViewModel memento)
        {
            if (!memento.isMemento)
            {
                //if a root element (a plugin itself) was selected, don't delete
                //the changes in the proptertyView are only temporary anyway
                return;
            }

            Memento m = new Memento(memento.name, null);
            PluginManager.pluginManager.addMemento(memento.parent.name, m);

            //update the pluginList
            memento.parent.children.Remove(memento);
        }

        /// <summary>
        /// Saves the current settings permanently to the given memento.
        /// </summary>
        /// <returns>Returns true if the memento was successfully saved.</returns>
        private bool mementoSave(PluginViewModel memento)
        {
            //if plugin (no memento) itself is selected, copy it
            if (!memento.isMemento)
            {
                return mementoCopy(memento);
            }


            bool renaming = (memento.name != this.tbMementoName.Text);

            //check memento naming
            if (this.tbMementoName.Text == "")
            {
                System.Windows.MessageBox.Show("Bitte geben Sie den zu speichernden Einstellungen einen Namen.", "Speichern nicht möglich");
                return false;
            }
            else if (renaming && findPVM(memento.parent.name, this.tbMementoName.Text) != null)
            {
                //the new memento's name is already taken
                System.Windows.MessageBox.Show("Der Name der zu speichernden Einstellungen ist nicht eindeutig.", "Speichern nicht möglich");
                return false;
            }


            Memento mem = selectedPlugin.getMemento();
            mem.name = this.tbMementoName.Text;

            //adding a new memento
            PluginManager.pluginManager.addMemento(memento.parent.name, mem);

            //if renaming an existing memento, delete the memento by its old name
            if (renaming)
            {
                Memento m = new Memento(memento.name, null);
                PluginManager.pluginManager.addMemento(memento.parent.name, m);

                memento.name = mem.name;
            }

            //focus the saved memento
            memento.selected = true;

            copied = false;
            return true;
        }


        #region OQAT Events

        /// <summary>
        /// Handles the OQAT event (invoke through pluginManager) that signals that a macroentry
        /// was selected. Select this memento in the tree, therefore showing the propertyView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onMacroFilterEntryClicked(object sender, MementoEventArgs e)
        {
            PluginViewModel s = findPVM(e.pluginKey, e.mementoName);
            if (s != null) s.selected = true;
        }

        #endregion



        private void treePlugins_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            updatePropertiesView();
        }

        private void bttSaveMemento_Click(object sender, RoutedEventArgs e)
        {
            mementoSave(selectedPVM);
        }

        private void bttCopyMemento_Click(object sender, RoutedEventArgs e)
        {
            mementoCopy(selectedPVM);
        }

        private void bttDeleteMemento_Click(object sender, RoutedEventArgs e)
        {
            mementoDelete(selectedPVM);
        }



        private void bttAddToMacro_Click(object sender, RoutedEventArgs e)
        {
            if (mementoSave(selectedPVM))
            {
                PluginManager.pluginManager.raiseEvent(EventType.macroEntryAdd,
                    new MementoEventArgs(selectedPVM.name, selectedPVM.parent.name));
            }
        }

    }



    /// <summary>
    /// PluginViewModel is a rather generic wrapper to keep information about plugins and their mementos.
    /// In the PluginsList plugins are represented by PluginViewModels containing their name, and their
    /// mementos as children. Those mementos are represented as PluginViewModels as well.
    /// PluginViewModel implements INotifyPropertyChanged and can be used in a two way binding to the treeview.
    /// </summary>
    internal class PluginViewModel : INotifyPropertyChanged
    {
        PluginViewModel _parent;
        public PluginViewModel parent
        {
            get
            {
                if (_parent != null)
                    return _parent;
                else
                    return this;
            }
            private set
            {
                _parent = value;
            }
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

        public bool isMemento
        {
            get
            {
                return (_parent != null);
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



        bool memento;
        public PluginViewModel(string pluginName)
        {
            this.name = pluginName;

            this.children = new ObservableCollection<PluginViewModel>();
        }
        public PluginViewModel(string mementoName, PluginViewModel plugin)
        {
            this.name = mementoName;
            this.parent = plugin;
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
