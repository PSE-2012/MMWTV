using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oqat.PublicRessources.Plugin;
using Oqat.PublicRessources.Model;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.Reflection;
using System.ComponentModel;
using System.Threading;
using System.Drawing;

using Oqat.ViewModel;
using Oqat.Model;
using System.IO;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Documents;
using System.Diagnostics;

namespace Oqat.ViewModel.MacroPlugin
{
    [ExportMetadata("namePlugin", "Macro")]
    [ExportMetadata("type", PluginType.IMacro)]
    [ExportMetadata("threadSafe", false)]
    [Export(typeof(IPlugin))]

    class Macro : IMacro
    {

        /// <summary>
        /// This is the TopLevel macro. All processing
        /// is happening inside this macro, 
        /// </summary>
        private MacroEntry macroEntry
        { get; set; }

        public bool threadSafe
        {
            get { return false; }
        }

        

        [field:NonSerialized]
        private Macro_PropertyView _propertyView;


        private ViewType viewType;

        public UserControl propertyView
        {
            get
            {
                return _propertyView;
            }
            private set
            {
                _propertyView = value as Macro_PropertyView;
            }
        }
        public Macro()
        {
            // init first entry
            this.macroEntry = new MacroEntry(this.namePlugin, PluginType.IMacro, "anonymousMacro");
            this.macroEntry.frameCount = 100;
            this.macroEntry.startFrameAbs = 0;
            this.macroEntry.endFrameAbs = 100;

 
          // seqMacroEntryList = new List<MacroEntry>();


            propertyView = new Macro_PropertyView(this.macroEntry.macroEntries);
         //   (propertyView as Macro_PropertyView).cancelProcessing.Click += onCancelButtonClick;
            (propertyView as Macro_PropertyView).clearEntries.Click += onClearButtonClick;


            _propertyView.MacroEntryTreeView.PreviewDragEnter += new DragEventHandler(MacroEntryTreeView_PreviewDragEnter);
            _propertyView.MacroEntryTreeView.DragLeave += new DragEventHandler(MacroEntryTreeView_DragLeave);
            _propertyView.MacroEntryTreeView.PreviewDrop += new DragEventHandler(MacroEntryTreeView_PreviewDrop);
            _propertyView.MacroEntryTreeView.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(MacroEntryTreeView_PreviewMouseLeftButtonDown);
            _propertyView.MacroEntryTreeView.PreviewMouseMove += new System.Windows.Input.MouseEventHandler(MacroEntryTreeView_PreviewMouseMove);
            _propertyView.MacroEntryTreeView.KeyDown += new KeyEventHandler(MacroEntryTreeView_KeyDown);
            _propertyView.MacroEntryTreeView.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(MacroEntryTreeView_PreviewMouseLeftButtonUp);
            _propertyView.MacroEntryTreeView.PreviewDragOver += new DragEventHandler(MacroEntryTreeView_PreviewDragOver);
            _propertyView.startProcessing.Click += onStartProcessButtonClick;

            PluginManager.macroEntryAdd += this.addMacroEntry;
            PluginManager.OqatToggleView += onToggleView;


            dragControl = new MacroEntry_Control();
        }
        private System.Windows.Point startPoint;
        private bool isMouseDown = false;
        private bool isDragging = false;
        private MacroEntry dragData;
        private MacroEntry_Control dragControl;

        TreeViewItem dragSourceTrItem;
        private void MacroEntryTreeView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {

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
                e.Handled = true;
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
                    DataObject dObject = new DataObject(macroEntry.GetType(), dragData);
                  //  itemsControl.AllowDrop = false;

                    resetMacroTreeViewSelections();

                    DragDrop.DoDragDrop(_propertyView.MacroEntryTreeView, dObject,DragDropEffects.Copy | DragDropEffects.Move);

                    ResetState();

                    //_propertyView.MacroEntryTreeView.ItemContainerGenerator.ContainerFromItem(_propertyView.

                    //var treeViewItem =
                    //    getNearestFather<TreeViewItem>((DependencyObject)e.OriginalSource);

                    //if (treeView == null || treeViewItem == null)
                    //    return;

                    //var entry = treeView.SelectedItem as MacroEntry;
                    //if (entry == null)
                    //    return;

                    //var dragData = new DataObject(entry);
                    

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

                removeMacroEntry(itemToRemove);

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
            try {
            contains = propertiesViewAdornerLayer.GetAdorners(_propertyView.MacroEntryTreeView).Contains(drInsAdorner);
            } catch{}

            if (!contains)
            {
                if (e.Data.GetDataPresent(typeof(MacroEntry)))
                {

                    //init dragInsert adorner
                    var pos = e.GetPosition(_propertyView.MacroEntryTreeView);
                    initializeDragInsertAdorner(_propertyView.MacroEntryTreeView, dragData, pos);

                    //set opacity on (source) dragged item



                    //initHighLight adorner
                    var trItem = getNearestFather<TreeViewItem>((DependencyObject)e.OriginalSource) as TreeViewItem;
                    if(trItem != null) {


                        var newPosition = getReslTrItemPos(_propertyView.MacroEntryTreeView, trItem);

                        double yOffset;
                        System.Windows.Size size;

                        getHighLightSizeOffset(trItem, e, out yOffset, out size);
                        newPosition.Y += yOffset;


                        initializeHighLightAdorner(_propertyView.MacroEntryTreeView, pos, opacityHighLight, size);
                    }
                }
            }
                
            e.Handled = true;

        }

       // DrawingVisualHost highLightRect;
        double opacityHighLight = 0.1;
        double opacityDragInsertControl = 1;
        double opacityDragSourceItem = 0.5;

        private void initializeHighLightAdorner(UIElement adornedElemnt, System.Windows.Point position, double opacity,System.Windows.Size size)
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


            highLightAdorner = new HighLight_Adorner(adornedElemnt, position, size,opacityHighLight, propertiesViewAdornerLayer);
                highLightAdorner.IsHitTestVisible = false;


            


        }

        private void initializeDragInsertAdorner(UIElement uiElement, Object data, System.Windows.Point position)
        {
            if(propertiesViewAdornerLayer == null) 
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

        // if drop operation on nonMacro entrys is performed
        // the drop date will be dropped at --successorIndex;
        //or as last entry if dropTarget is no treeViewItem
        int successorIndex = -1;
        private void MacroEntryTreeView_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(MacroEntry)))
            {
                TreeViewItem trItem = getNearestFather<TreeViewItem>((DependencyObject)e.OriginalSource);
         
                var addornContainter = propertiesViewAdornerLayer.GetAdorners(_propertyView.MacroEntryTreeView);
                if (addornContainter.Contains(drInsAdorner))
                {
                    drInsAdorner.UpdatePosition(e.GetPosition(_propertyView.MacroEntryTreeView).X, e.GetPosition(_propertyView.MacroEntryTreeView).Y);
                }

                if (trItem != null)
                {
                    if (addornContainter.Contains(highLightAdorner))
                    {

                        var newPosition = getReslTrItemPos(_propertyView.MacroEntryTreeView, trItem);

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
            var trItemPos = getReslTrItemPos(_propertyView.MacroEntryTreeView, trItem);
            int relPosInd = IsSlightlyAbove(trItemPos, e.GetPosition(trItem), trItem.ActualHeight);
            if (relPosInd > 0)
            {
                yOffset = 5 * -2  + 2;
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
            var selItem = _propertyView.MacroEntryTreeView.SelectedItem;

            if (selItem != null)
                (_propertyView.MacroEntryTreeView.ItemContainerGenerator.ContainerFromItem(selItem) as TreeViewItem).IsSelected = false;

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


        private int getInsertIndex(DragEventArgs e, TreeViewItem trItem, out TreeViewItem dropTarget) {

            dropTarget = null;
            // get Rel trItem postion
            var trItemPos = getReslTrItemPos(_propertyView.MacroEntryTreeView, trItem);

            int relPosInd = IsSlightlyAbove(trItemPos, e.GetPosition(trItem), trItem.ActualHeight);

            int insertAtIndex = -1; // drop at toplevel as last child

            //check relative drop position

            if (relPosInd > 0) // drop above dropTarget  
            {
               insertAtIndex =  getNextHigherItem(out dropTarget, trItem);
            }
            else if ((relPosInd == 0)
                && (trItem.DataContext as MacroEntry).type == PluginType.IMacro) // try to drop within the target (only if dropTarget is macro, else -> drop below)
            {
                dropTarget = trItem;
            }
            else if (relPosInd < 0) // drop below dropTarget
            {
                 insertAtIndex =  getNextLowerItem(out dropTarget, trItem);
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
                index = _propertyView.MacroEntryTreeView.ItemContainerGenerator.IndexFromContainer(supposedDropTarget);

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
                index = _propertyView.MacroEntryTreeView.ItemContainerGenerator.IndexFromContainer(supposedDropTarget);
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
                dropEntry = constructMacroFromMementoArg(e.Data.GetData(typeof(MementoEventArgs)) as MementoEventArgs);
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

                            if(dropTarget == null) // insert at toplevel
                            {
                                
                                moveMacroEntry(dropEntry, this.macroEntry, insertIndex);
                            }
                            else
                            {
                                entryToDropIn = dropTarget.DataContext as MacroEntry;
                                moveMacroEntry(dropEntry, entryToDropIn, insertIndex);
                            }

                        }
                        else // no dropTarget -> move at topLeven (as last child)
                        {
                            moveMacroEntry(dropEntry, this.macroEntry);
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
                            addMacroEntry(dropEntry, this.macroEntry, insertIndex);
                        }
                        else
                        {
                            addMacroEntry(dropEntry, entryToDropIn, insertIndex);
                        }

                    }
                    else // no dropTarget -> move at topLeven
                    {
                        if((this.macroEntry.macroEntries.Count < 1) && (dropEntry.type == PluginType.IMacro))
                            addMacroEntry(dropEntry, null);
                        else
                            addMacroEntry(dropEntry, this.macroEntry);
                    }
                }
 
                }
      
            #region obsolete
            //if (e.Data.GetDataPresent(typeof(MacroEntry)))
            //{



            //    var treeView = sender as TreeView;
            //    var treeViewItem =
            //        getNearestFather<TreeViewItem>((DependencyObject)e.OriginalSource);

            //    if (treeView != null && treeViewItem != null)
            //    {


            //        var entryToDropIn = treeViewItem.DataContext as MacroEntry;

            //        moveMacroEntry(dropEntry, entryToDropIn);
            //    }

            //}
            //else if (e.Data.GetDataPresent(typeof(MementoEventArgs)))
            //{
            //    var treeView = sender as TreeView;
            //    var treeViewItem = getNearestFather<TreeViewItem>((DependencyObject)e.OriginalSource);
            //    if (treeViewItem != null)
            //    {
            //        addMacroEntry(treeViewItem.DataContext as MacroEntry,
            //            e.Data.GetData(typeof(MementoEventArgs)) as MementoEventArgs);
            //    }
            //    else
            //    {
            //        addMacroEntry(this, e.Data.GetData(typeof(MementoEventArgs)) as MementoEventArgs);
            //    }
            //    DetachDragAdorner();

            //}


            //if (e.Data.GetDataPresent(typeof(MacroEntry)))
            //{
            //    var dropEntry = e.Data.GetData(typeof(MacroEntry)) as MacroEntry;


            //    var treeView = sender as TreeView;
            //    var treeViewItem =
            //        getNearestFather<TreeViewItem>((DependencyObject)e.OriginalSource);

            //    if (treeView != null && treeViewItem != null)
            //    {


            //        var entryToDropIn = treeViewItem.DataContext as MacroEntry;

            //        moveMacroEntry(dropEntry, entryToDropIn);
            //    }

            //}
            //else if (e.Data.GetDataPresent(typeof(MementoEventArgs)))
            //{
            //    var treeView = sender as TreeView;
            //    var treeViewItem = getNearestFather<TreeViewItem>((DependencyObject)e.OriginalSource);
            //    if (treeViewItem != null)
            //    {
            //        addMacroEntry(treeViewItem.DataContext as MacroEntry,
            //            e.Data.GetData(typeof(MementoEventArgs)) as MementoEventArgs);
            //    }
            //    else
            //    {
            //        addMacroEntry(this, e.Data.GetData(typeof(MementoEventArgs)) as MementoEventArgs);
            //    }
            //    DetachDragAdorner();

            //}
            //else
            //{
            //   e.Effects = DragDropEffects.None;
            //}
            #endregion
            if (dropEntry != null)
            {
                DetachAdorner(drInsAdorner, propertiesViewAdornerLayer);
                DetachAdorner(highLightAdorner, propertiesViewAdornerLayer);
            }
            e.Handled = true;
            //  propertyView.Refresh();
        }

        private void removeMacroEntry(MacroEntry entry)
        {
           
            lock (this.macroEntry)
            {
                if (macroEntry.macroEntries.Contains(entry))
                {
                    macroEntry.macroEntries.Remove(entry);

                }
                else
                {
                    List<MacroEntry> seqMacroEntryList = new List<MacroEntry>();
                    recursiveFilterExplorer(this.macroEntry, seqMacroEntryList);

                    foreach (var listEntry in seqMacroEntryList)
                    {
                        if (listEntry.macroEntries.Contains(entry))
                        {
                            listEntry.macroEntries.Remove(entry);
                            break;
                        }
                    }
                }
            }
        }

        private void addMacroEntry(MacroEntry child, MacroEntry father, int index = -1) {
                        lock (this.macroEntry)
            {
                if (father == null) // child is new TL macro
                {
                    macroEntry.mementoName = child.mementoName;

                    // these are alway default for topLevel macros
                    //macroEntry.startFrameAbs = child.startFrameAbs;
                    //macroEntry.endFrameAbs = child.endFrameAbs;
                    macroEntry.macroEntries.Clear();
                    macroEntry.macroEntries = (macroEntry.macroEntries.Concat(child.macroEntries)) 
                        as ObservableCollection<MacroEntry>;
                }
                else
                {
                    if(index <0) {
                        father.macroEntries.Add(child);
                    } else {
                        father.macroEntries.Insert(index, child);
                    }
                }
            }
        }
        //// couldnt come up with a better name,
        //// the reason why we are not using 
        //private void concatCollection<T>(Collection<T> first, Collection<T> second)
        //{

        //}
        
       

        private void moveMacroEntry(MacroEntry toMoveMacro, MacroEntry target, int index = -1)
        {
            removeMacroEntry(toMoveMacro);

            addMacroEntry(toMoveMacro, target, index);

        }

        private void onToggleView(object sender, ViewTypeEventArgs e)
        {
            if (e.viewType == viewType)
                return;

            viewType = e.viewType;
            switch (viewType)
            {
                case ViewType.FilterView:
                    _propertyView.activeState = false;
                    _propertyView.filterMode = true;
                    flush();
                    break;
                case ViewType.MetricView:
                    _propertyView.activeState = false;
                    _propertyView.filterMode = false;
                    flush();
                    break;    
                default:
                    _propertyView.activeState = true;
                    break;
            }
        }

        public Memento getMemento()
        {
            if (this.macroEntry.macroEntries.Count > 0)
                return new Memento(macroEntry.mementoName, macroEntry as IMacroEntry);
            else
                return new Memento(macroEntry.mementoName, null);
        }


        public void setMemento(PublicRessources.Model.Memento memento)
        {
            var newTLMacroEnry = memento.state as MacroEntry;

            Debug.Assert(newTLMacroEnry != null);
            Debug.Assert(newTLMacroEnry.mementoName.Equals(memento.name));
            Debug.Assert(newTLMacroEnry.macroEntries.Count > 0);

            flush();
            addMacroEntry(newTLMacroEnry, null);
        
            
        }

        public virtual string namePlugin
        {
            get { return "Macro"; }
        }


        public virtual PluginType type
        {
            get { return PluginType.IMacro; }
        }

        // this will not only return a readOnlyView but actually
        // set the (only one view exists at a time) view to readOnly mode
        public UserControl readOnlyPropertiesView
        {
            get 
            {
                _propertyView.readOnly = true;

                return this.propertyView; 
            }
        }

        #region fields

       // private List<MacroEntry> seqMacroEntryList;

        private IVideoHandler handRef;
        private IVideoHandler handProc;
        private Video vidRes;
        private int idRes;

        private BackgroundWorker worker;

        #endregion


        public void setFilterContext(int idRef, IVideo vidRef)
        {
            flush();

            this.idRes = idRef;
            handRef = vidRef.getExtraHandler();

            // construct result video and init handler writing context
            var tmpVidInfo = handRef.readVidInfo.Clone() as IVideoInfo;
            Video vidRes = new  Video(isAnalysis:false,
                vidPath:findValidVidPath(vidRef.vidPath, this.macroEntry.mementoName),
                vidInfo:tmpVidInfo);


            this.vidRes = vidRes;
            handRef.setWriteContext(vidRes.vidPath, vidRes.vidInfo);
            

            (propertyView as Macro_PropertyView).filterMode = true;

            this.macroEntry.frameCount = handRef.readVidInfo.frameCount;

            //RemoveClickEvent((propertyView as Macro_PropertyView).startProcessing);
            
        }

        public void setMetricContext(IVideo vidRef, int idProc, IVideo vidProc)
        {
         
            flush();
            
            this.idRes = idProc;
            if (vidRef != null)
            {
                handRef = vidRef.getExtraHandler();
                
            }
            if (vidProc != null)
            {
                handProc = vidProc.getExtraHandler();
            }


            (propertyView as Macro_PropertyView).filterMode = false;
           // RemoveClickEvent((propertyView as Macro_PropertyView).startProcessing);
            //if ((handRef != null) && (handProc != null))
            //    (propertyView as Macro_PropertyView).startProcessing.Click += onStartMetricProcessButtonClick;
        }



        public void addMacroEntry(object sender, MementoEventArgs e)
        {
            addMacroEntry(e, macroEntry);
        }

        public void addMacroEntry(MementoEventArgs e, MacroEntry father, int index = -1)
        {
            var entryToAdd = constructMacroFromMementoArg(e);


            if ((entryToAdd.type == PluginType.IMacro) && (this.macroEntry.macroEntries.Count() == 0))
                addMacroEntry(entryToAdd, null);

            else    
                addMacroEntry(entryToAdd, father, index);
        }



        private MacroEntry constructMacroFromMementoArg(MementoEventArgs e)
        {

            PluginType entryType = PluginManager.pluginManager.getPlugin<IPlugin>(e.pluginKey).type;
            MacroEntry entryToAdd;
            if (entryType != PluginType.IMacro)
            {
                entryToAdd = new MacroEntry(e.pluginKey, entryType, e.mementoName);
                entryToAdd.frameCount = this.macroEntry.frameCount;       
            }
            else
            {
                var tmpMem = PluginManager.pluginManager.getMemento(e.pluginKey, e.mementoName);
                entryToAdd = tmpMem.state as MacroEntry;
            }

            return entryToAdd;
        }


       
        private void onStartProcessButtonClick(object sender, RoutedEventArgs e) {
            e.Handled = true;


            _propertyView.processingStateValue = 0;
            _propertyView.processing = true;


            worker = new BackgroundWorker();

            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;

            if (_propertyView.filterMode)
            {
                worker.DoWork += filterProcess;
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(filterProcessCompleted);
            }
            else
            {
                worker.DoWork += metricProcess;
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(metricProcessCompleted);

            }

            worker.ProgressChanged += delegate(object s, ProgressChangedEventArgs args)
            {
                Debug.Assert(_propertyView.MacroEntryTreeView.Dispatcher.CheckAccess());
                _propertyView.MacroEntryTreeView.Dispatcher.VerifyAccess();

                _propertyView.processingStateValue = args.ProgressPercentage;
                _propertyView.processingStateMessage = "Processed " + this.handRef.positionReader + " of " + this.macroEntry.frameCount + " frames.";

            };


            worker.RunWorkerAsync();
        }

        private void filterProcessCompleted(object s, RunWorkerCompletedEventArgs e)
        {
            _propertyView.processing = false;
            if (e.Cancelled == true)
            {
                //cancelled
            }
            else if (e.Error != null)
            {
                //error + e.Error.Message;
            }
            else
            {
                PluginManager.pluginManager.raiseEvent(
                EventType.macroProcessingFinished, new VideoEventArgs(this.vidRes, this.idRes));
            }
        }

        private void metricProcessCompleted(object s, RunWorkerCompletedEventArgs e)
        {
            _propertyView.processing = false;
            if (e.Cancelled == true)
            {
                //cancelled
            }
            else if (e.Error != null)
            {
                //error + e.Error.Message;
            }
            else
            {
                Debug.Assert(e.Result is List<metricResultContext>);

                foreach (var subEntry in e.Result as List<metricResultContext>)
                {
                    PluginManager.pluginManager.raiseEvent(
                        EventType.macroProcessingFinished, new VideoEventArgs(subEntry.vidRes, this.idRes));
                }
            }
        }
 
        private void filterProcess(object s, DoWorkEventArgs e)
        {

            List<MacroEntry> seqMacroEntryList = new List<MacroEntry>();
            recursiveFilterExplorer(this.macroEntry, seqMacroEntryList);

            while (handRef.positionReader < handRef.readVidInfo.frameCount)
            {
                Bitmap bmp = handRef.getFrame();

                foreach (var filterEntry in seqMacroEntryList)
                {

                    if (!(filterEntry.startFrameAbs > handRef.positionReader)
                        && (filterEntry.endFrameAbs > handRef.positionReader))
                    {
                        try
                        {
                            IFilterOqat actPlugin = PluginManager.pluginManager.getPlugin<IFilterOqat>(filterEntry.pluginName)
                                as IFilterOqat;
                            actPlugin = actPlugin.createExtraPluginInstance() as IFilterOqat;
                            actPlugin.setMemento(PluginManager.pluginManager.getMemento(
                                filterEntry.pluginName, filterEntry.mementoName));
                            bmp = actPlugin.process(bmp);

                        }
                        catch (Exception)  
                        {
                            //TODO
                            // set plugin to blackList if something went wrong
                        }
                    }
                }
                var tmpBmpArray = new Bitmap[1];
                tmpBmpArray[0] = bmp;
                handRef.writeFrames(handRef.positionReader, tmpBmpArray);

                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    worker.ReportProgress((int)(handRef.positionReader / ( macroEntry.frameCount / 100.0)));
                }
            }



                   
            

        }

      
        private void recursiveFilterExplorer(MacroEntry entry, List<MacroEntry> seqMacroList)
        {
            Debug.Assert(seqMacroList != null, "seqMacroList is null");
            foreach (var subEntry in entry.macroEntries)
            {
                    if (subEntry.type != PluginType.IMacro)
                    {
                        seqMacroList.Add(subEntry);
                    }
                    else
                    {
                        // go down to first found filter
                        recursiveFilterExplorer(subEntry, seqMacroList);
                    }
            }
        }

        private void metricProcess(object s, DoWorkEventArgs e)
        {

            List<metricResultContext> seqMetricResultCtxList = new List<metricResultContext>();
            
            recursiveMetricExplorer(this.macroEntry, seqMetricResultCtxList);

            while (handRef.positionReader < handRef.readVidInfo.frameCount)
            {
                Bitmap bmpRef = handRef.getFrame();
                Bitmap bmpProc = handProc.getFrame();
       
                foreach (var subEntry in seqMetricResultCtxList)
                {
                    // check if set as active
                    if (!(subEntry.entry.startFrameAbs > handRef.positionReader)
                        && (subEntry.entry.endFrameAbs > handRef.positionReader))
                    {
                        // init plugin
                        IMetricOqat curPlugin =
                            PluginManager.pluginManager.getPlugin<IMetricOqat>(
                            subEntry.entry.pluginName).createExtraPluginInstance() as IMetricOqat;

                        curPlugin.setMemento(PluginManager.pluginManager.getMemento(
                            subEntry.entry.pluginName,subEntry.entry.mementoName));
                        
                        var info = curPlugin.analyse(bmpRef, bmpProc);

                        // write acquired frame
                        (subEntry.vidRes as Video).frameMetricValue[
                            handRef.positionReader - subEntry.entry.startFrameAbs] = info.values;
                        var tmpArray = new Bitmap[1];
                        tmpArray[0] = info.frame;
                        subEntry.handRes.writeFrames(handRef.positionReader - (int)subEntry.entry.startFrameAbs, tmpArray);
                      
                    }
                }
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    worker.ReportProgress((int)(handRef.positionReader / macroEntry.frameCount / 100.0));
                }

            }
            e.Result = seqMetricResultCtxList;
            
            //foreach (var subEntry in seqMacroEntryList)
            //{
            //    PluginManager.pluginManager.raiseEvent(
            //        EventType.macroProcessingFinished, new VideoEventArgs(subEntry.vidRes, this.idRes));
            //}
        }

        struct metricResultContext
        {
            public IVideo vidRes;
            public IVideoHandler handRes;
            public MacroEntry entry;
        }

        private void recursiveMetricExplorer(MacroEntry entry, List<metricResultContext> seqMacroEntryList)
        {
            Debug.Assert(seqMacroEntryList != null, "Given seqMacroEntryList is null.");
            foreach (var subEntry in entry.macroEntries)
            {
                    if (subEntry.type != PluginType.IMacro)
                    {
                        metricResultContext metResContext;
                        metResContext.entry = subEntry;
                        string path;

                    
                       
                        if (subEntry.path != null)
                            path = subEntry.path;
                        else
                        {
                            path = findValidVidPath(handProc.readPath, subEntry.pluginName);
                           
                        }
                    

                        var tmpEntryList = new List<IMacroEntry>();
                        tmpEntryList.Add(subEntry);

                        var tmpVidInfo = handProc.readVidInfo.Clone() as IVideoInfo;
                        tmpVidInfo.frameCount =(int)(subEntry.endFrameAbs - subEntry.startFrameAbs);

                        metResContext.vidRes = new Video(isAnalysis:true, 
                                                        vidPath:path, 
                                                        vidInfo:tmpVidInfo,
                                                        processedBy:tmpEntryList);

                        (metResContext.vidRes as Video).frameMetricValue = new float[metResContext.vidRes.vidInfo.frameCount][];
                        metResContext.handRes = metResContext.vidRes.getExtraHandler();
                        metResContext.handRes.setWriteContext(metResContext.vidRes.vidPath, metResContext.vidRes.vidInfo);
                        seqMacroEntryList.Add(metResContext);
                    }
                    else
                    {
                        // go down to first found non macro
                        recursiveMetricExplorer(subEntry, seqMacroEntryList);
                    }
            }
            
        }

        private string findValidVidPath(string possiblePath, string suffix)
        {
            string path = "";


                string ext = Path.GetExtension(possiblePath);
                path = Path.GetDirectoryName(possiblePath) + Path.GetFileNameWithoutExtension(possiblePath);
                bool pathValid = false;
                int n = 0;
                string counter = "";
                while (!pathValid)
                {
                    if (n > 0)
                        counter = n.ToString();

                    string tmpPath = path + suffix + counter + ext;
                    if (!File.Exists(tmpPath))
                    {
                        pathValid = true;
                        path = tmpPath;
                    }
                    else
                        n++;
                
                }
                return path;
        }
        

        //private void disableControlWhileProcessing()
        //{
        //}

        //private void enableControlAfterProcessing()
        //{
        //}


        //private void onStartMetricProcessButtonClick(object sender, RoutedEventArgs e)
        //{
        // //   e.Handled = true;
        // ////   disableControlWhileProcessing();
        // //   worker = new Thread(new ThreadStart(metricProcess));
        // //   worker.Name = workerName + "metric";
        // //   worker.Start();
        //}

        private void onCancelButtonClick(object sender, RoutedEventArgs e)
        {
        }

        private void onClearButtonClick(object sender, RoutedEventArgs e)
        {
        }


        #region obsolete
        #region removeClickEvents
        ///// <summary>
        ///// Gets the list of routed event handlers subscribed to the specified routed event.
        ///// </summary>
        ///// <param name="element">The UI element on which the event is defined.</param>
        ///// <param name="routedEvent">The routed event for which to retrieve the event handlers.</param>
        ///// <returns>The list of subscribed routed event handlers.</returns>
        //public static RoutedEventHandlerInfo[] GetRoutedEventHandlers(UIElement element, RoutedEvent routedEvent)
        //{
        //    // Get the EventHandlersStore instance which holds event handlers for the specified element.
        //    // The EventHandlersStore class is declared as internal.
        //    var eventHandlersStoreProperty = typeof(UIElement).GetProperty(
        //        "EventHandlersStore", BindingFlags.Instance | BindingFlags.NonPublic);
        //    object eventHandlersStore = eventHandlersStoreProperty.GetValue(element, null);
        //    if (eventHandlersStore == null)
        //        return null;
        //        // Invoke the GetRoutedEventHandlers method on the EventHandlersStore instance 
        //        // for getting an array of the subscribed event handlers.
        //        var getRoutedEventHandlers = eventHandlersStore.GetType().GetMethod(
        //            "GetRoutedEventHandlers", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        //        var routedEventHandlers = (RoutedEventHandlerInfo[])getRoutedEventHandlers.Invoke(
        //            eventHandlersStore, new object[] { routedEvent });
            
        //    return routedEventHandlers;
        //}

        //private void RemoveClickEvent(Button b)
        //{
            
        //    var routedEventHandlers = GetRoutedEventHandlers(b, ButtonBase.ClickEvent);
        //    if(routedEventHandlers != null) 
        //        foreach (var routedEventHandler in routedEventHandlers)
        //            b.Click -= (RoutedEventHandler)routedEventHandler.Handler;
        //}
        #endregion

        #region drawingVisualHost
        //public class DrawingVisualHost : FrameworkElement
        //{
        //    private DrawingVisual drawingVisual;

        //    public DrawingVisualHost(DrawingVisual drawingVisual)
        //        : base()
        //    {
        //        this.drawingVisual = drawingVisual;
        //    }

        //    // EllipseAndRectangle instance is our only visual child
        //    protected override Visual GetVisualChild(int index)
        //    {
        //        return drawingVisual;
        //    }

        //    protected override int VisualChildrenCount
        //    {
        //        get
        //        {
        //            return 1;
        //        }
        //    }
        //}
        #endregion

        #endregion

        public void flush()
        {
            if (handProc != null)
            {
                handProc.flushReader();
                handProc.flushWriter();
                handProc = null;
            }

            if (handRef != null)
            {
                handRef.flushReader();
                handRef.flushWriter();
                handRef = null;
            }
            this.macroEntry.macroEntries.Clear();
            this.macroEntry.mementoName = "anonymousMacro";
            this.macroEntry.frameCount = 100;
            this.macroEntry.startFrameAbs = 0;
            this.macroEntry.endFrameAbs = 100;

        }


        public IPlugin createExtraPluginInstance()
        {
            return new Macro();
        }
    }


}
