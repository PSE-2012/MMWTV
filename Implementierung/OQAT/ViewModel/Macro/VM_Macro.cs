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
                return this.macroFilter.macroControl;
            }
        }
        
        public MacroMetricControl macroMetricControl
        {
            get
            {
                return this.macroMetric.macroControl;
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

        internal delegate void MacroSaveEventHandler(object sender, EntryEventArgs e);
        internal event MacroSaveEventHandler MacroSave;
        internal delegate void EntrySelectEventHandler(object sender, EventArgs e);
        internal event EntrySelectEventHandler EntrySelect;
        internal delegate void StartProcessEventHandler(object sender, EventArgs e);
        internal event StartProcessEventHandler StartProcess;
        internal RangeSelectionChangedEventHandler del;
        internal List<RangeSelectionChangedEventHandler> delList;

        public VM_Macro()
        {
            PluginManager.OqatToggleView += this.onToggleView;
            PluginManager.macroEntryAdd += this.onEntrySelect;

            MacroSave += new MacroSaveEventHandler(macroSave);
            StartProcess += new StartProcessEventHandler(startProcess);

            delList = new List<RangeSelectionChangedEventHandler>();
            this.macroFilter = new PF_MacroFilter();
            this.macroMetric = new PM_MacroMetric();
            this.macroFilter.macroControl = new MacroFilterControl(macroFilter, this);
            this.macroMetric.macroControl = new MacroMetricControl(macroMetric, this);
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

        /// <summary>
        /// Raised if user clicks on the process button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Should contain informations about the result video i.e. name, format.</param>
        internal void onStartProcess(object sender, EventArgs e) // EventArgs are VideoEventArgs
        {
            StartProcess(sender, e);
        }

        private void startProcess(object sender, EventArgs e)
        {
            if (this.viewType == ViewType.MetricView)
            {
                PM_MacroMetric macroMetric = (PM_MacroMetric)this.macroMetric;
                arrayVidResult = new Video[macroMetric.macroQueue.Rows.Count]; // this line of code doesn't really belong in onStartProcess
                // what video info object to use for the result videos?
                // setting paths of result videos?
                // macroMetric.analyse(vidRef, vidProc, arrayVidResult); // arrayvidresult from eventargs?
            }
            if (this.viewType == ViewType.FilterView)
            {
                PF_MacroFilter macroFilter = (PF_MacroFilter)this.macroFilter;
                IVideoInfo vidInfo = vidRef.vidInfo;
                // vidResult = new Video(false, "C:/Documents/newvideo.yuv", vidInfo, null);
                vidResult = new Video(false, "C:/Documents/newvideo.yuv", vidInfo, this.macroFilter.getPluginMementoList());
                macroFilter.init(vidRef, vidResult);
                macroFilter.process(vidRef, vidResult); // vidresult from eventargs?
            }
        }

        public void onEntrySelect(object sender, MementoEventArgs e)
        {
            if (this.viewType == ViewType.FilterView)
            {
                long startValue = 0;
                long stopValue = 100;
                long startValueSlider = 0;
                long stopValueSlider = 500; // 500 only for testing
                // long stopValueSlider = vidRef.vidInfo.frameCount;

                MacroEntryFilter mfe = new MacroEntryFilter();
                mfe.pluginName = e.pluginKey;
                mfe.mementoName = e.mementoName;
                mfe.startFrameRelative = startValue;
                mfe.endFrameRelative = stopValue;

                // TODO: What does the slider do if the loaded plugin is a Macro Filter? Check for plugin name == "macro"?

                RangeSlider rs = new AC.AvalonControlsLibrary.Controls.RangeSlider();
                rs.RangeStart = startValueSlider;
                rs.RangeStop = stopValueSlider;
                rs.RangeStartSelected = startValueSlider;
                rs.RangeStopSelected = stopValueSlider;
                rs.MinRange = 1L;
                rs.Width = 150;
                rs.Height = 17.29;

                this.macroFilter.macroQueue.Rows.Add(mfe.pluginName, mfe.mementoName, mfe, startValue, stopValue);
                int j = this.macroFilter.macroQueue.Rows.Count - 1;
                del = delegate(object sender2, RangeSelectionChangedEventArgs e2)
                {
                    MacroEntryFilter mfeTemp = new MacroEntryFilter();
                    mfeTemp = (MacroEntryFilter)this.macroFilter.macroQueue.Rows[j]["Macro Entry"];
                    mfeTemp.startFrameRelative = ((double)e2.NewRangeStart / 500) * 100;
                    mfeTemp.endFrameRelative = ((double)e2.NewRangeStop / 500) * 100;
                    if (mfeTemp.startFrameRelative > 100) mfeTemp.startFrameRelative = 100; // slider values go out of range for some reason -> error handling
                    if (mfeTemp.endFrameRelative < 0) mfeTemp.endFrameRelative = 0;
                    this.macroFilter.macroQueue.Rows[j]["Macro Entry"] = mfeTemp;
                    this.macroFilter.macroQueue.Rows[j]["Start"] = mfeTemp.startFrameRelative;
                    this.macroFilter.macroQueue.Rows[j]["Stop"] = mfeTemp.endFrameRelative;
                };
                rs.RangeSelectionChanged += del;
                delList.Add(del);
                this.macroFilter.rsl.Add(rs);
            }
            if (this.viewType == ViewType.MetricView)
            {

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
                // we need the getpluginmementolist method of PF_MacroFilter -> type cast required
                PF_MacroFilter macroFilter = (PF_MacroFilter)this.macroFilter;
                //convert datatable macro entry column to list of macroEntrys
                List<MacroEntry> macroEntryList = macroFilter.getPluginMementoList();
                // save the macro filter
                macroFilter.createNewMemento(macroEntryList, e.Entry);
            }
            if (this.viewType == ViewType.MetricView)
            {

            }
        }
    }
}
