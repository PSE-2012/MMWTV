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

using Oqat.PublicRessources.Plugin;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Oqat.ViewModel.MacroPlugin
{
    /// <summary>
    /// Interaktionslogik für Macro_PropertyView.xaml
    /// </summary>
    public partial class Macro_PropertyView : UserControl, INotifyPropertyChanged
    {

        public ObservableCollection<MacroEntry> macroEntries
        { get; private set; }

        public Macro_PropertyView(ObservableCollection<MacroEntry> macroEntries)
        {
            this.macroEntries = macroEntries;
            InitializeComponent();
            self.DataContext = this;
        }


        private bool _filterMode = true;
        public bool filterMode
        {
            get { return _filterMode; }
            set
            {
                _filterMode = value;
                NotifyPropertyChanged("filterModeVisibility");
                NotifyPropertyChanged("filterModeActiveState");
            }
        }

        private bool _readOnly = false;
        public bool readOnly {
            get {
                return _readOnly;
            }
            set {
                _readOnly = value;
                NotifyPropertyChanged("readOnlyVisibility");
                NotifyPropertyChanged("readOnlyActiveState");
            }
        }

        private bool _inactive;
        public bool inactive
        {
            get
            {
                return _inactive;
            }
            set
            {
                _inactive = value;
                NotifyPropertyChanged("inactive");
            }
        }

        public bool inactiveActiveState
        {
            get { return !inactive; }
        }

        public Visibility inactiveVisibility
        {
            get
            {
                if (!inactive)
                    return System.Windows.Visibility.Visible;
                else
                    return System.Windows.Visibility.Collapsed;
            }
        }

        public Visibility filterModeVisibility
        {
            get
            {
                if (filterMode)
                    return System.Windows.Visibility.Visible;
                else
                    return System.Windows.Visibility.Collapsed;
            }
        }

        public Visibility readOnlyVisibility
        {
          
            get
            {
                if (readOnly)
                    return System.Windows.Visibility.Collapsed;
                else
                    return System.Windows.Visibility.Visible;
            }
        }

        public bool filterModeActiveState
        {
            get
            {
                return filterMode;
            }
        }

        private bool readOnlyActiveState
        {
            get
            {
                return !readOnly;
            }
        }

        internal UserControl getReadOnlyVersion()
        {
            throw new NotImplementedException();
        }

        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;
      
    }


    #region unusedTreeViewHack

    class StretchingTreeView : TreeView
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new StretchingTreeViewItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is StretchingTreeViewItem;
        }
    }

    class StretchingTreeViewItem : TreeViewItem
    {
        public StretchingTreeViewItem()
        {
            this.Loaded += new RoutedEventHandler(StretchingTreeViewItem_Loaded);
        }

        private void StretchingTreeViewItem_Loaded(object sender, RoutedEventArgs e)
        {
            // The purpose of this code is to stretch the Header Content all the way accross the TreeView. 
            if (this.VisualChildrenCount > 0)
            {
                Grid grid = this.GetVisualChild(0) as Grid;
                if (grid != null && grid.ColumnDefinitions.Count == 3)
                {
                    // Remove the middle column which is set to Auto and let it get replaced with the 
                    // last column that is set to Star.
                    grid.ColumnDefinitions.RemoveAt(1);
                }
            }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new StretchingTreeViewItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is StretchingTreeViewItem;
        }
    }
    #endregion
}
