namespace Oqat.ViewModel.Macro
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Oqat.ViewModel;
    using Oqat.PublicRessources.Model;
    using Oqat.PublicRessources.Plugin;
    using Oqat.Model;
    using System.Data;
    using System.Windows.Controls;
    using System.Windows.Data;
    using AC.AvalonControlsLibrary.Controls;

    /// <summary>
    /// This components provides the user a way to coordinate choosen filters or metrics (from the <see cref="PluginLists"/>)
    /// and invoke the according plugins (<see cref="PluginManager"/>) on the currently choosen Video/s.
    /// </summary>
    public class VM_Macro
    {
        /// <summary>
        /// Currently active view.
        /// VM_Macro is displayed if viewType == ( Filter || Metric)
        /// </summary>
        private ViewType viewType = ViewType.FilterView;

        internal PF_MacroFilter macroFilter
        {
            get;
            set;
        }

        private PM_MacroMetric macroMetric
        {
            get;
            set;
        }

        /// <summary>
        /// Currently selected reference video
        /// </summary>
        public Video vidRef
        {
            get;
            set;
        }

        /// <summary>
        /// Currently selected processed video.
        /// </summary>
        public Video vidProc
        {
            get;
            set;
        }

        /// <summary>
        /// This is were the results (filter or metric process) are placed in.
        /// </summary>
        private Video vidResult
        {
            get;
            set;
        }

        private Video[] arrayVidResult
        {
            get;
            set;
        }

        public MacroFilterControl macroFilterControl
        {
            get
            {
                return this.macroFilter.macroControl as MacroFilterControl;
            }
        }
        
        public MacroMetricControl macroMetricControl
        {
            get
            {
                return this.macroMetric.macroControl as MacroMetricControl;
            }
        }

        //propertiesView has to be wrapped in a grid in order for updates onToggleView to show immidiatelly
        Grid _propertiesView;
        public System.Windows.UIElement propertiesView
        {
            get
            {
                if (_propertiesView == null) _propertiesView = new Grid();
                return _propertiesView;
            }
            private set
            {
                if (_propertiesView == null) _propertiesView = new Grid();

                _propertiesView.Children.Clear();
                _propertiesView.Children.Add(value);
            }
        }

        internal delegate void EntrySelectEventHandler(object sender, EventArgs e);
        internal event EntrySelectEventHandler EntrySelect;
        //internal delegate void StartProcessEventHandler(object sender, EventArgs e);
        //internal event StartProcessEventHandler StartProcess;
        internal List<RangeSelectionChangedEventHandler> delList;

        public VM_Macro()
        {
            PluginManager.OqatToggleView += this.onToggleView;
            PluginManager.macroEntryAdd += this.onEntrySelect;

            //StartProcess += new StartProcessEventHandler(startProcess);

            delList = new List<RangeSelectionChangedEventHandler>();
            this.macroFilter = new PF_MacroFilter();
            this.macroMetric = new PM_MacroMetric();
        }

        /// <summary>
        /// Raised if user changes the macroQueue ( filters or metrics currently selected for execution).
        /// </summary>
        /// <param name="sender">Caller, should be the <see cref="VM_PluginLists"/></param>
        /// <param name="e">Has to contain information about wich filter or metric were selected or changed.</param>
        private void onPreviewLoad(object sender, EventArgs e) { }

        public void onToggleView(object sender, ViewTypeEventArgs e)
        {
            this.viewType = e.viewType;

            if (this.viewType == ViewType.FilterView)
            {
                propertiesView = this.macroFilter.macroControl;
            }
            else if (this.viewType == ViewType.MetricView)
            {
                propertiesView = this.macroMetric.macroControl;
            }
        }

        public void startProcess()
        {
            if (this.viewType == ViewType.MetricView)
            {
                arrayVidResult = new Video[macroMetric.macroQueue.Count]; // setting paths of result videos?
            }
            if (this.viewType == ViewType.FilterView)
            {
                macroFilterControl.macroTable.IsEnabled = false;
                macroFilterControl.rangeSliders.IsEnabled = false;
                // TODO: don't forget to enable the table after processing is finished
                IVideoInfo vidInfo =(IVideoInfo) vidRef.vidInfo.Clone();
                //TODO: where to save the new video?!?
                string resultpath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                vidResult = new Video(false, resultpath+"\\newvideo.yuv", vidInfo, this.macroFilter.getPluginMementoList());
                this.macroFilter.init(vidRef, vidResult);
                this.macroFilter.process(vidRef, vidResult);
                macroFilterControl.macroTable.IsEnabled = true;
                macroFilterControl.rangeSliders.IsEnabled = true;
            }
        }

        public void onEntrySelect(object sender, MementoEventArgs e)
        {
            if (this.viewType == ViewType.FilterView)
            {
                long startValue = 0;
                long stopValue = 100;
                long startValueSlider = 0;
                long stopValueSlider = 500;
                MacroEntryFilter mEntryFilter = new MacroEntryFilter(e.pluginKey, e.mementoName, stopValue, startValue);
                RangeSlider rs = new AC.AvalonControlsLibrary.Controls.RangeSlider();
                rs.RangeStart = startValueSlider;
                rs.RangeStop = stopValueSlider;
                rs.RangeStartSelected = startValueSlider;
                rs.RangeStopSelected = stopValueSlider;
                rs.MinRange = 1L;
                rs.Width = 270; // TODO: changing width at runtime
                rs.Height = 17.29; // this height fits the height of the data rows in the macro table
                this.macroFilter.macroQueue.Add(mEntryFilter);
                int j = this.macroFilter.macroQueue.Count - 1;
                this.macroFilter.macroControl.addDelegate(rs, j, delList);
                this.macroFilter.rsl.Add(rs);
                this.macroFilter.macroControl.updateSliders();
            }
            if (this.viewType == ViewType.MetricView)
            {
                MacroEntryMetric mEntryMetric = new MacroEntryMetric(e.pluginKey, e.mementoName, this.vidRef, this.vidProc);
                macroMetric.macroQueue.Add(mEntryMetric);
                //TODO add Textbox for name to save on disc???
            }
        }

        /// <summary>
        /// Raised if user wishes to save current macroQueue for later use.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">name the new macro should have.</param>
        internal void onMacroSave(object sender, EntryEventArgs e)
        {
            MacroSave(sender, e);
        }

        private void macroSave(object sender, EntryEventArgs e)
        {
            if (this.viewType == ViewType.FilterView)
            {
                //convert datatable macro entry column to list of macroEntrys
                List<MacroEntry> macroEntryList = this.macroFilter.getPluginMementoList();
                // save the macro filter
                this.macroFilter.createNewMemento(macroEntryList, e.Entry);
            }
            if (this.viewType == ViewType.MetricView)
            {

            }
        }
    }
}
