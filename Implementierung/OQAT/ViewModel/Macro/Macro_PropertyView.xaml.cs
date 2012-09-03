using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Oqat.PublicRessources.Model;
using Oqat.PublicRessources.Plugin;

namespace Oqat.ViewModel.MacroPlugin
{
    /// <summary>
    /// Interaktionslogik für Macro_PropertyView.xaml
    /// </summary>
    partial class Macro_PropertyView : UserControl, INotifyPropertyChanged
    {

        public ObservableCollection<MacroEntry> macroEntries
        { get; private set; }

        //rootEntry is not the visual root
        //it is just the topLevel macro
        private MacroEntry rootEntry;
        private MacroViewDelegates macroViewDelegates;
        
        public Macro_PropertyView(MacroEntry rootEntry, MacroViewDelegates macroViewDelegates)
        {
            this.macroEntries = rootEntry.macroEntries;
            this.rootEntry = rootEntry;
            this.macroViewDelegates = macroViewDelegates;
            InitializeComponent();
            self.DataContext = this;

            dragControl = new MacroEntry_Control();
        }


        private bool _activeState = true;
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
            get { if(!readOnly) return _filterMode; else return false; }
            set
            {
                _filterMode = value;
                NotifyPropertyChanged("filterModeVisibility");
                NotifyPropertyChanged("filterModeActiveState");
            }
        }

        public bool allowDrop
        {
            get
            {
                return !readOnly;
            }
        }

        private bool _readOnly = false;
        public bool readOnly
        {
            get {
                return _readOnly;
            }
            set {
                    _readOnly = value;

                    macroViewDelegates.registerOnMacroEvents();
                    NotifyPropertyChanged("allowDrop");
                    NotifyPropertyChanged("readOnlyVisibility");
                    NotifyPropertyChanged("readOnlyActiveState");
                    NotifyPropertyChanged("filterModeVisibility");
                    NotifyPropertyChanged("filterModeActiveState");

                // notify children.
                    this.rootEntry.readOnly = _readOnly;
                
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
                    NotifyPropertyChanged("processingStateMessage");
                }
            }
        }

        private string _processingStateMessage;
        public string processingStateMessage
        {
           
            get
            {
                return _processingStateMessage;
            }
            set
            {
                _processingStateMessage = value;
                NotifyPropertyChanged("processingStateMessage");
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

        public bool readOnlyActiveState
        {
            get
            {
                return !readOnly;
            }
        }


        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(property));
        }


        public event PropertyChangedEventHandler PropertyChanged;


        #region coppiedInd

        private System.Windows.Point startPoint;
        private bool isMouseDown = false;
        private bool isDragging = false;
        private MacroEntry dragData;
        private MacroEntry_Control dragControl;

        TreeViewItem dragSourceTrItem;
        private void MacroEntryTreeView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            TreeView trView = sender as TreeView;

            AC.AvalonControlsLibrary.Controls.RangeSlider rSlider =
                getNearestFather<AC.AvalonControlsLibrary.Controls.RangeSlider>((DependencyObject)e.OriginalSource)
                as AC.AvalonControlsLibrary.Controls.RangeSlider;

            if ((sender != null) && (rSlider == null))
            {
                var treeViewItem =
                    getNearestFather<TreeViewItem>((DependencyObject)e.OriginalSource);

                if (trView == null || treeViewItem == null)
                    return;

                // opaque (source) draggedItem
                treeViewItem.Opacity = opacityDragSourceItem;

                dragData = treeViewItem.DataContext as MacroEntry;
                if (dragData != null)
                {
                    isMouseDown = true;
                    dragSourceTrItem = treeViewItem;
                    startPoint = e.GetPosition(trView);
                }
               
            }
        }

        private void MacroEntryTreeView_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                ItemsControl itemsControl = sender as ItemsControl;
                var mousePos = e.GetPosition(itemsControl);
                var diff = startPoint - mousePos;

                if (!isDragging && ((Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance
                    || Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)))
                {
                    if (dragData.GetType() != typeof(MacroEntry))
                        return;

                    isDragging = true;
                    DataObject dObject = new DataObject(rootEntry.GetType(), dragData);
                    //  itemsControl.AllowDrop = false;

                     resetMacroTreeViewSelections();

                    DragDrop.DoDragDrop(this.MacroEntryTreeView, dObject, DragDropEffects.Copy | DragDropEffects.Move);

                    ResetState();
                }
            }
        }

        private void ResetState()
        {
            isMouseDown = false;
            isDragging = false;
            if (dragSourceTrItem != null)
            {
                dragSourceTrItem.Opacity = 1;
                dragSourceTrItem = null;
            }
            //   dragData = null;
            //  itemsControl.AllowDrop = true;
        }

        private void MacroEntryTreeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var treeView = sender as TreeView;
                var treeViewItem = getNearestFather<TreeViewItem>((DependencyObject)e.OriginalSource);

                if ((treeView == null) || (treeViewItem == null))
                    return;

                var itemToRemove = treeViewItem.DataContext as MacroEntry;

                if (itemToRemove == null)
                    return;

                macroViewDelegates.removeMacro(itemToRemove);
                //removeMacroEntry(itemToRemove);

            }
        }



        private void MacroEntryTreeView_PreviewMouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            ResetState();
            //  DetachDragAdorner();
            e.Handled = true;
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
        private AdornerLayer propertiesViewAdornerLayer;
        private DragInsertAdorner drInsAdorner;
        private void MacroEntryTreeView_PreviewDragEnter(object sender, DragEventArgs e)
        {

            bool contains = false;
            try
            {
                contains = propertiesViewAdornerLayer.GetAdorners(this.MacroEntryTreeView).Contains(drInsAdorner);
            }
            catch { }

            if (!contains)
            {
                if (e.Data.GetDataPresent(typeof(MacroEntry)))
                {

                    //init dragInsert adorner
                    var pos = e.GetPosition(this.MacroEntryTreeView);
                    initializeDragInsertAdorner(this.MacroEntryTreeView, dragData, pos);

                    //set opacity on (source) dragged item



                    //initHighLight adorner
                    var trItem = getNearestFather<TreeViewItem>((DependencyObject)e.OriginalSource) as TreeViewItem;
                    if (trItem != null)
                    {


                        var newPosition = getReslTrItemPos(this.MacroEntryTreeView, trItem);

                        double yOffset;
                        System.Windows.Size size;

                        getHighLightSizeOffset(trItem, e, out yOffset, out size);
                        newPosition.Y += yOffset;


                        initializeHighLightAdorner(this.MacroEntryTreeView, pos, opacityHighLight, size);
                    }
                }
            }

            e.Handled = true;

        }

        // DrawingVisualHost highLightRect;
        double opacityHighLight = 0.1;
        double opacityDragInsertControl = 1;
        double opacityDragSourceItem = 0.5;

        private void initializeHighLightAdorner(UIElement adornedElemnt, System.Windows.Point position, double opacity, System.Windows.Size size)
        {
            if (propertiesViewAdornerLayer == null)
                propertiesViewAdornerLayer = AdornerLayer.GetAdornerLayer(adornedElemnt);

            //if (highLightRect == null)
            //{
            //    //// create visual content to show on screen
            //    //DrawingVisual drawingVisual = new DrawingVisual();
            //    //DrawingContext drawingContext = drawingVisual.RenderOpen();
            //    //Rect rect = new Rect(size);
            //    //Rectangle recfdat = new Rectangle(position, size);
            //    //recfdat.Dra

            //    //drawingContext.DrawRectangle(System.Windows.SystemColors.HighlightBrush, (System.Windows.Media.Pen)null, rect);
            //   // drawingContext.DrawRectangle(System.Windows.Media.Brushes.Black, (System.Windows.Media.Pen)null, rect);

            //   // drawingVisual.Opacity = opacity;

            //   // highLightRect = new DrawingVisualHost(drawingVisual);
            //}


            highLightAdorner = new HighLight_Adorner(adornedElemnt, position, size, opacityHighLight, propertiesViewAdornerLayer);
            highLightAdorner.IsHitTestVisible = false;





        }

        private void initializeDragInsertAdorner(UIElement uiElement, Object data, System.Windows.Point position)
        {
            if (propertiesViewAdornerLayer == null)
                propertiesViewAdornerLayer = AdornerLayer.GetAdornerLayer(uiElement);
            if (dragControl.DataContext != data)
            {
                dragControl = new MacroEntry_Control();
                dragControl.DataContext = data as MacroEntry;
            }
            //  macroControlCopy.startEndFrameSlider.Visibility = Visibility.Collapsed;
            dragControl.Opacity = opacityDragInsertControl;
            drInsAdorner = new DragInsertAdorner(uiElement, position, dragControl, propertiesViewAdornerLayer);
            drInsAdorner.IsHitTestVisible = false;
        }

        private HighLight_Adorner highLightAdorner;

        private void MacroEntryTreeView_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(MacroEntry)))
            {
                TreeViewItem trItem = getNearestFather<TreeViewItem>((DependencyObject)e.OriginalSource);

                var addornContainter = propertiesViewAdornerLayer.GetAdorners(this.MacroEntryTreeView);
                if (addornContainter.Contains(drInsAdorner))
                {
                    drInsAdorner.UpdatePosition(e.GetPosition(this.MacroEntryTreeView).X, e.GetPosition(this.MacroEntryTreeView).Y);
                }

                if (trItem != null)
                {
                    if (addornContainter.Contains(highLightAdorner))
                    {

                        var newPosition = getReslTrItemPos(this.MacroEntryTreeView, trItem);

                        double yOffset;
                        System.Windows.Size size;

                        getHighLightSizeOffset(trItem, e, out yOffset, out size);
                        newPosition.Y += yOffset;

                        highLightAdorner.UpdateSizePosition(newPosition, size);
                    }
                }
                //else
                //{
                //    successorIndex = -1;
                //}

            }
            e.Handled = true;
        }

        private void getHighLightSizeOffset(TreeViewItem trItem, DragEventArgs e,
            out double yOffset, out System.Windows.Size size)
        {
            var trItemPos = getReslTrItemPos(this.MacroEntryTreeView, trItem);
            int relPosInd = IsSlightlyAbove(trItemPos, e.GetPosition(trItem), trItem.ActualHeight);
            if (relPosInd > 0)
            {
                yOffset = 5 * -2 + 2;
                size = new System.Windows.Size(trItem.Width, 5 * 2 - 4);
            }
            else if (relPosInd < 0)
            {
                yOffset = 5 * 2 - 2;
                size = new System.Windows.Size(trItem.Width, 5 * 2 - 4);
            }
            else
            {
                yOffset = 0;
                size = trItem.RenderSize;
            }

        }

        private System.Windows.Point getReslTrItemPos(TreeView trView, TreeViewItem trItem)
        {
            GeneralTransform genTrans = trItem.TransformToAncestor(trView);
            System.Windows.Point newPosition = genTrans.Transform(new System.Windows.Point(0, 0));
            return newPosition;
        }

        private void DetachAdorner(Adorner adorner, AdornerLayer adornerLayer)
        {
            try { adornerLayer.Remove(adorner); }
            catch { }
        }

        private void MacroEntryTreeView_DragLeave(object sender, DragEventArgs e)
        {
            //var trView = sender as TreeView;
            //if (trView == null)
            DetachAdorner(drInsAdorner, propertiesViewAdornerLayer);
            DetachAdorner(highLightAdorner, propertiesViewAdornerLayer);




            e.Handled = true;

        }

        private void resetMacroTreeViewSelections()
        {
            var selItem = this.MacroEntryTreeView.SelectedItem;

            if (selItem != null)
                (this.MacroEntryTreeView.ItemContainerGenerator.ContainerFromItem(selItem) as TreeViewItem).IsSelected = false;

        }

        //true => slightly above
        //false => slightly below
        //slightly => a third of ActualHeight
        private static int IsSlightlyAbove(System.Windows.Point parentPosition,
                                                System.Windows.Point mousePosition,
                                            double parentHeight)
        {

            int result = 0;

            double parentY = parentHeight / 3;
            double mouseY = mousePosition.Y;


            if (mouseY > parentY * 2)
            {
                result = -1;
            }
            else if (mouseY > parentY)
            {
                result = 0;
            }
            else
            {
                result = 1;
            }

            return result;
        }


        private int getInsertIndex(DragEventArgs e, TreeViewItem trItem, out TreeViewItem dropTarget)
        {

            dropTarget = null;
            // get Rel trItem postion
            var trItemPos = getReslTrItemPos(this.MacroEntryTreeView, trItem);

            int relPosInd = IsSlightlyAbove(trItemPos, e.GetPosition(trItem), trItem.ActualHeight);

            int insertAtIndex = -1; // drop at toplevel as last child

            //check relative drop position

            if (relPosInd > 0) // drop above dropTarget  
            {
                insertAtIndex = getNextHigherItem(out dropTarget, trItem);
            }
            else if ((relPosInd == 0)
                && (trItem.DataContext as MacroEntry).type == PluginType.IMacro) // try to drop within the target (only if dropTarget is macro, else -> drop below)
            {
                dropTarget = trItem;
            }
            else if (relPosInd < 0) // drop below dropTarget
            {
                insertAtIndex = getNextLowerItem(out dropTarget, trItem);
            }

            return insertAtIndex;
        }

        private int getNextHigherItem(out TreeViewItem nextHigher, TreeViewItem supposedDropTarget)
        {
            int index = -1;
            if (supposedDropTarget.Parent != null)
            {
                nextHigher = null;
            }
            else // topLevel -> set trItem to null and return inserIndex
            {
                nextHigher = null;
                index = this.MacroEntryTreeView.ItemContainerGenerator.IndexFromContainer(supposedDropTarget);

            }

            return index;
        }

        private int getNextLowerItem(out TreeViewItem nextLower, TreeViewItem supposedDropTarget)
        {
            int index = -1;
            if (supposedDropTarget.Parent != null)
            {
                nextLower = null;
            }
            else // topLevel -> set trItem to null and return inserIndex
            {
                nextLower = null;
                index = this.MacroEntryTreeView.ItemContainerGenerator.IndexFromContainer(supposedDropTarget);
                index++;

            }

            return index;
        }

        private void MacroEntryTreeView_PreviewDrop(object sender, DragEventArgs e)
        {
            // data to drop
            MacroEntry dropEntry = null;
            MacroEntry entryToDropIn = null;
            bool moveOperation = false;
            if (e.Data.GetDataPresent(typeof(MacroEntry)))
            {
                dropEntry = e.Data.GetData(typeof(MacroEntry)) as MacroEntry;
                moveOperation = true;
            }

            else if (e.Data.GetDataPresent(typeof(MementoEventArgs)))
            {
                dropEntry = macroViewDelegates.constrMacroFromMem(e.Data.GetData(typeof(MementoEventArgs)) as MementoEventArgs); 
              //  dropEntry = constructMacroFromMementoArg(e.Data.GetData(typeof(MementoEventArgs)) as MementoEventArgs);
                moveOperation = false;
            }


            if (dropEntry != null)
            {
                var treeViewItem =
                        getNearestFather<TreeViewItem>((DependencyObject)e.OriginalSource);

                if (moveOperation)
                { //move operation(macro initialized)

                    // var treeView = sender as TreeView;
                    //if (treeView != null)
                    //{

                    if (treeViewItem != dragSourceTrItem)
                    {

                        if (treeViewItem != null)
                        {
                            // entryToDropIn = treeViewItem.DataContext as MacroEntry;
                            TreeViewItem dropTarget;
                            var insertIndex = getInsertIndex(e, treeViewItem, out dropTarget);

                            if (dropTarget == null) // insert at toplevel
                            {
                                macroViewDelegates.moveMacro(dropEntry, rootEntry, insertIndex);
                               // moveMacroEntry(dropEntry, rootEntry, insertIndex);
                            }
                            else
                            {
                                entryToDropIn = dropTarget.DataContext as MacroEntry;
                                macroViewDelegates.moveMacro(dropEntry, entryToDropIn, insertIndex);
                               // moveMacroEntry(dropEntry, entryToDropIn, insertIndex);
                            }

                        }
                        else // no dropTarget -> move at topLeven (as last child)
                        {
                            macroViewDelegates.moveMacro(dropEntry, rootEntry);
                           // moveMacroEntry(dropEntry, rootEntry);
                        }
                    }
                }
                else
                {
                    //drop operation (pluginList initilized)

                    if (treeViewItem != null)
                    {
                        // entryToDropIn = treeViewItem.DataContext as MacroEntry;
                        TreeViewItem dropTarget;
                        var insertIndex = getInsertIndex(e, treeViewItem, out dropTarget);

                        entryToDropIn = dropTarget.DataContext as MacroEntry;

                        if (entryToDropIn == null) // insert at toplevel
                        {
                            macroViewDelegates.addMacro(dropEntry, rootEntry, insertIndex);
                           // addMacroEntry(dropEntry, rootEntry, insertIndex);
                        }
                        else
                        {
                            macroViewDelegates.addMacro(dropEntry, entryToDropIn, insertIndex);
                           // addMacroEntry(dropEntry, entryToDropIn, insertIndex);
                        }

                    }
                    else // no dropTarget -> move at topLeven
                    {
                        if ((this.rootEntry.macroEntries.Count < 1) && (dropEntry.type == PluginType.IMacro))
                            macroViewDelegates.addMacro(dropEntry, null);
                        // addMacroEntry(dropEntry, null);
                        else
                            macroViewDelegates.addMacro(dropEntry, rootEntry);
                           // addMacroEntry(dropEntry, rootEntry);
                    }
                }

            }

  
            if (dropEntry != null)
            {
                DetachAdorner(drInsAdorner, propertiesViewAdornerLayer);
                DetachAdorner(highLightAdorner, propertiesViewAdornerLayer);
            }
            e.Handled = true;
         
        }
        

        #endregion

        private void clearEntries_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            macroViewDelegates.clearEntries();
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            macroViewDelegates.cancelProcessing();
        }

        private void startProcessing_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            macroViewDelegates.startProcessing();
        }


        private void saveMacro_Click(object sender, RoutedEventArgs e)
        {
            if (this.rootEntryMem_TextBox.Text == "")
                showMementoNameInvalidDialog(rootMemNameEmptyWarning);
            else
            {
                rootEntry.mementoName = this.rootEntryMem_TextBox.Text;
                macroViewDelegates.saveSaveAs(EventType.saveMacro);
            }
        }
        private string rootMemNameEmptyWarning = "You must specifie a name of at least one caracter in " + 
                                 "order to be able to save this macro for later use.";
        private string rootMemNameAmbigousWarning = "The name you specified is reserved for " + 
                                 "an another memento, delelte the existing first or choose a different name please.";
        private void saveMacroAs_Click(object sender, RoutedEventArgs e)
        {
            if (this.rootEntryMem_TextBox.Text == "")
                showMementoNameInvalidDialog(rootMemNameEmptyWarning);
            else
            {
                rootEntry.mementoName = this.rootEntryMem_TextBox.Text;
                macroViewDelegates.saveSaveAs(EventType.saveMacroAs);
            }
        }


        private void showMementoNameInvalidDialog(string message)
        {
          
            string caption = "OQAT Macro";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Exclamation;
            MessageBox.Show(message, caption, button, icon);
        }
    }


}
