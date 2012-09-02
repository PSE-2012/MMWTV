using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading;
using Oqat.PublicRessources.Model;
using Oqat.PublicRessources.Plugin;
using System.Collections.ObjectModel;
using System.ComponentModel;

using System.IO;
using System.Xml;
using System.Globalization;

namespace Oqat.ViewModel
{
    /// <summary>
    /// Interaktionslogik für VM_PluginsList.xaml
    /// </summary>
    public partial class VM_PluginsList : UserControl
    {
        String msgText1 = "Die Einstellungen konnten nicht gefunden werden.";
        String msgText2 = "Bitte geben Sie den zu speichernden Einstellungen einen Namen." ;
        String msgText21 = "Speichern nicht möglich.";
        String msgText3 = "Der Name der zu speichernden Einstellungen ist nicht eindeutig.";
        String l2text = "Das momentan aktive Macro";

        /// <summary>
        /// Sets the Language Content and reads it from an XML File.
        /// </summary>
        private void local(String s)
        {
            try
            {
                String sFilename = Directory.GetCurrentDirectory() + "/" + s;
                XmlTextReader reader = new XmlTextReader(sFilename);
                reader.Read();
                reader.Read();
                String[] t = new String[9];
                String[] t2 = new String[9];
                for (int i = 0; i < 9; i++)
                {
                    reader.Read();
                    reader.Read();
                    t[i] = reader.Name;
                    reader.MoveToNextAttribute();
                    t2[i] = reader.Value;
                }
               // tb1.Text = t2[0];
                bt2.Content = t2[1];
               // bt3.Content = t2[2];
                bttAddToMacro.Content = t2[3];
                l2text = t2[4];
                msgText1 = t2[5];
                msgText2 = t2[6];
                msgText21 = t2[7];
                msgText3 = t2[8];

                //TODO: tbNoSettings.Text lokalisieren


            }
            catch (IndexOutOfRangeException e) { }
            catch (FileNotFoundException e) { }
            catch (XmlException e) { }
        }

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
        IMacro presentationMacro;

        //PluginViewModel _activeMacroPVM;
        //PluginViewModel activeMacroPVM
        //{
        //    get
        //    {
        //        if (_activeMacroPVM == null)
        //        {
        //            //select parent MacroPlugin (always last entry in list)
        //            _activeMacroPVM = pluginList.Last();
        //        }
        //        return _activeMacroPVM;
        //    }
        //    set
        //    {
        //        _activeMacroPVM = value;
        //    }
        //}

        public delegate void macroLoadHandler(object sender, MementoEventArgs e);
        public event macroLoadHandler macroLoaded;

       // Panel panelMacroPropertyViewCurrent;

        /// <summary>
        /// Constructor
        /// </summary>
        public VM_PluginsList(PluginType plugintype)
        {
            InitializeComponent();
            
            local("VM_PluginsList_"+ Thread.CurrentThread.CurrentCulture+".xml");
            this.pluginType = plugintype;

            PluginManager.macroEntrySelected += onMacroFilterEntryClicked;
            PluginManager.saveMacro += onMacroSave;
            PluginManager.saveMacroAs += onMacroSaveAs;

            loadPluginLists();
            this.treePlugins.ItemsSource = pluginList;

            //panelMacroPropertyViewCurrent = new StackPanel();
            //    TextBlock l2 = new TextBlock();
            //    l2.Text = l2text;
            //    l2.TextAlignment = TextAlignment.Center;
            //panelMacroPropertyViewCurrent.Children.Add(l2);
            
        }

        /// <summary>
        /// Fills the list of PluginViewModels with plugin and memento data from PluginManager.
        /// </summary>
        private void loadPluginLists()
        {
            _pluginList = new ObservableCollection<PluginViewModel>();
            var plList = PluginManager.pluginManager.getPluginNames(pluginType);
            plList =  plList.Concat(PluginManager.pluginManager.getPluginNames(PluginType.IMacro)).ToList();
            
            foreach (string name in plList)
            {
                PluginViewModel pl = new PluginViewModel(name);

                List<string> mementos = PluginManager.pluginManager.getMementoNames(name);
                if(mementos != null)
                {
                    foreach (string m in mementos)
                    {
                        if(m!="") // no invisible entries allowed
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
                    if (mementoName == null) return p.parent;
                    if ((p.children == null) || (p.children.Count() < 1)) return p.parent;
                    foreach (PluginViewModel pc in p.children)
                    {
                        if (pc.name == mementoName)
                            return pc;
                    }
                    return p.parent;
                }
            }

            return null;
        }

        /// <summary>
        /// Loads the propertyView of the selected plugin and manages visibility of the according pluginList buttons.
        /// </summary>
        /// 
       
        private void updatePropertiesView()
        {
            this.panelMementoSave.Visibility = System.Windows.Visibility.Collapsed;
            this.bttAddToMacro.Visibility = System.Windows.Visibility.Collapsed;
            this.panelMacroProp.Visibility = System.Windows.Visibility.Collapsed;
            this.tbNoSettings.Visibility = System.Windows.Visibility.Collapsed;

            if(selectedPVM.isMemento)
            { //real memento
                selectedPlugin = PluginManager.pluginManager.getPlugin<IPlugin>(selectedPVM.parent.name);
                Memento m = PluginManager.pluginManager.getMemento(selectedPVM.parent.name, selectedPVM.name);
                                    if (m != null)
                    {

                       
                if (selectedPlugin is IMacro)
                { //macro
                 //   if (selectedPVM == activeMacroPVM)
                //    {
                    selectedPlugin.setMemento(m);

                    this.gridPluginProperties.Content = (selectedPlugin as IMacro).readOnlyPropertiesView;

                    //    this.panelMementoSave.Visibility = System.Windows.Visibility.Visible;
                    //}
                    //else
                    //{
                        panelMacroProp.Visibility = System.Windows.Visibility.Visible;
                   // }
                }
                else
                { //filter/metric
                    selectedPlugin.setMemento(m);
                    this.gridPluginProperties.Content = selectedPlugin.propertyView;
                }
                this.panelMementoSave.Visibility = System.Windows.Visibility.Visible;
                    }
                                    else
                                    { //memento can't be found

                                        MessageBox.Show(msgText1);

                                        //remove the broken entry
                                        selectedPVM.parent.children.Remove(selectedPVM);
                                    }


                this.bttAddToMacro.Visibility = System.Windows.Visibility.Visible;

                this.tbMementoName.Text = selectedPVM.name;
                if (gridPluginProperties.Content == null
                    || (selectedPlugin is IMacro)) //&& selectedPVM != activeMacroPVM))
                {
                    this.panelMementoSave.Visibility = System.Windows.Visibility.Collapsed;

                    if (!(selectedPlugin is IMacro))
                    {
                        if (selectedPlugin.propertyView == null)
                        {
                            tbNoSettings.Visibility = System.Windows.Visibility.Visible;
                        }
                    }
                }
            }


            
            
        }


        /// <summary>
        /// Copies the selected memento internally.
        /// </summary>
        private bool mementoSaveAs(PluginViewModel memento, MementoEventArgs.getMemento_Delegate getMemento_Delegate = null)
        {
            //find available new name
            string oldName = memento.name;
            string newName = this.tbMementoName.Text;
            //int nameext = 1;
            //while (findPVM(memento.parent.name, (name + nameext)) != null)
            //{
            //    nameext++;
            //}
            //name = name + nameext;
            if (findPVM(memento.parent.name, newName) != memento.parent)
            {
                //the new memento's name is already taken
                System.Windows.MessageBox.Show(msgText3, msgText21);
                return false;
            }

            this.tbMementoName.Text = newName;


            PluginViewModel copymem = new PluginViewModel(newName, memento.parent);
            memento.parent.children.Add(copymem);

            return mementoSave(copymem, getMemento_Delegate);
        }

        /// <summary>
        /// Deletes the selected memento.
        /// </summary>
        private void mementoDelete(PluginViewModel memento)
        {
            if (!memento.isMemento)
            {
                //if a root element (a plugin itself) was selected there is nothing to be deleted
                return;
            }

            if (memento.parent.children.Count <= 1)
            {
                //don't delete the last memento so there is always one to copy and create new mementos
                MessageBox.Show("Das letzte Memento dieses Plugins kann nicht gelöscht werden.");
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
        private bool mementoSave(PluginViewModel memento, MementoEventArgs.getMemento_Delegate getMemento_Delegate = null)
        {
            //if plugin (no memento) itself is selected, change the
            if (!memento.isMemento)
            {
                return false;
            }


            bool renaming = (memento.name != this.tbMementoName.Text);
            var nameAmbiguityCheck = findPVM(memento.parent.name, this.tbMementoName.Text);
            //check memento naming
            if (this.tbMementoName.Text == "")
            {
                System.Windows.MessageBox.Show(msgText2,msgText21);
                return false;
            }
            else if (renaming && findPVM(memento.parent.name, this.tbMementoName.Text) != memento.parent)
            {
                //the new memento's name is already taken
                System.Windows.MessageBox.Show(msgText3, msgText21);
                return false;
            }

            Memento mem;

            if (getMemento_Delegate == null)
                mem = selectedPlugin.getMemento();
            else
                mem = getMemento_Delegate();

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
            return true;
        }

        /// <summary>
        /// Prepares the selected plugin memento and adds it to the active macro.
        /// </summary>
        /// <param name="memento"></param>
        private void mementoAddToMacro(PluginViewModel memento)
        {
            if (memento == null) return;

            //if (mementoSave(memento))
            //{
                PluginManager.pluginManager.raiseEvent(EventType.macroEntryAdd,
                    new MementoEventArgs(this.tbMementoName.Text, memento.parent.name));
            //}
        }

        private void onMacroSave(object sender, MementoEventArgs e) {

            PluginViewModel plVm;
            if(e.previousMementoName == "") { //just save
                plVm = findPVM(e.pluginKey, e.mementoName);
                if (plVm == null)
                    throw new ArgumentException("No such memento: " + e.mementoName + " found!");
            } else { // rename and save
                plVm = findPVM(e.pluginKey, e.previousMementoName);
                if (plVm == null)
                    throw new ArgumentException("No such memento: " + e.previousMementoName + " found!");   
            }
            this.tbMementoName.Text = e.mementoName;
            if (!mementoSave(plVm, e.getMemDel))
                throw new ArgumentException("Specified memento name is ambigous, try an another one.");
           
        }

        private void onMacroSaveAs(object sender, MementoEventArgs e) {
            PluginViewModel plVm = new PluginViewModel(e.previousMementoName, findPVM(e.pluginKey, null));
            this.tbMementoName.Text = e.mementoName;
            mementoSaveAs(plVm, e.getMemDel);
        }

        /// <summary>
        /// Prepares the selected macro memento and loads it as the active macro.
        /// </summary>
        /// <param name="memento"></param>
        private void mementoLoadAsMacro(PluginViewModel memento)
        {
            if (!memento.isMemento) return;

            //macroLoaded(this, new MementoEventArgs(selectedPVM.name, selectedPVM.parent.name));
           // activeMacroPVM = selectedPVM;
            updatePropertiesView();
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

        private void bttSaveAsMemento_Click(object sender, RoutedEventArgs e)
        {
            mementoSaveAs(selectedPVM);
        }

        private void bttDeleteMemento_Click(object sender, RoutedEventArgs e)
        {
            mementoDelete(selectedPVM);
        }



        private void bttAddToMacro_Click(object sender, RoutedEventArgs e)
        {
            mementoAddToMacro(selectedPVM);
        }

        private void treeitem_MouseDoubleClicked(object sender, RoutedEventArgs e)
        {
            if (selectedPVM == ((PluginViewModel)((TreeViewItem)e.Source).Header))
                mementoAddToMacro(selectedPVM);
        }

        private void bttLoadAsMacro_Click(object sender, RoutedEventArgs e)
        {
            mementoLoadAsMacro(selectedPVM);
        }

        //private void bttSwitchToCurrentMacro_Click(object sender, RoutedEventArgs e)
        //{
        //    activeMacroPVM.selected = true;
        //}

        Point dragOrigin;
        private void treePlugins_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            dragOrigin = e.GetPosition(null);
        }

        private void treePlugins_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var mousePos = e.GetPosition(null);
                var diff = dragOrigin - mousePos;
                if (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance
                || Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    var treeView = sender as TreeView;
                    var treeViewItem =
                        getNearestFather<TreeViewItem>((DependencyObject)e.OriginalSource);

                    if (treeView == null || treeViewItem == null)
                        return;

                    var selItem = treeView.SelectedItem as PluginViewModel;
                    if (selItem == null)
                        return;

                    if(!selItem.isMemento)
                        return;


                    MementoEventArgs dragData = new MementoEventArgs(selItem.name, selItem.parent.name);

                    DragDrop.DoDragDrop(treeViewItem, dragData, DragDropEffects.Move);
                }
            }
        }

        /// <summary>
        /// If a bubbling event occured it may be not on the element we
        /// need, therefore this method walks along the tree until
        /// a sought element (smartTree item) is found and returns it.
        /// </summary>
        /// <param name="element">Elemnt the event occured on.</param>
        /// <returns>The nearest father element of the given UIElement</returns>
        private T getNearestFather<T>(DependencyObject current)
             where T : DependencyObject
        {
            // Walk up the element tree to the nearest tree view item.
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
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
                if (value != _selected)
                {
                    _selected = value;
                    NotifyPropertyChanged("selected");

                    if (value && isMemento)
                    {
                        parent.expanded = true;
                    }
                }
            }
        }

        bool _expanded = true;
        public bool expanded
        {
            get
            {
                return _expanded;
            }
            set
            {
                if (value != expanded)
                {
                    _expanded = value;
                    NotifyPropertyChanged("expanded");
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





    

    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (((bool)value))
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (((Visibility)value) == Visibility.Visible)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
