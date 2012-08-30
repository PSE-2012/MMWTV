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
using Xceed.Wpf.Toolkit;

using Oqat.PublicRessources.Plugin;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows.Threading;
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

        private int _frameCount;
        public int frameCount
        {
            get{
                return _frameCount;
               }

            set
            {
                _frameCount = value;
                NotifyPropertyChanged("frameCount");
            }
        
        }


        private bool _activeState;
        public bool activeState
        {
            get
            {
                return _activeState;
            }
            set
            {
                _activeState = value;
                NotifyPropertyChanged("activeState");
                NotifyPropertyChanged("activeStateVisibility");
            }
        }

        public Visibility activeStateVisibility
        {
            get
            {
                if (activeState)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
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

        private bool _processing;
        public bool processing
        {
            get
            {
                return _processing;
            }
            set
            {
                if (value != _processing)
                {
                    _processing = value;
                    NotifyPropertyChanged("processing");
                }
            }
        }

        private double _processingStateValue;
        public double processingStateValue
        {
            get { return _processingStateValue; }
            set
            {
                _processingStateValue = value;
                NotifyPropertyChanged("processingStateValue");
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
            {
                if (OqatApp.uiDispatcher.CheckAccess())

                    PropertyChanged(this, new PropertyChangedEventArgs(property));

                else
                    OqatApp.uiDispatcher.BeginInvoke(new Action<string>(NotifyPropertyChanged), property);
            }
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
