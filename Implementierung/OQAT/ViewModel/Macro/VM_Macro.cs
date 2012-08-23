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

        internal PM_MacroMetric macroMetric
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
        public int idRef;

        /// <summary>
        /// Currently selected processed video.
        /// </summary>
        public Video vidProc
        {
            get;
            set;
        }
        public int idProc;

        /// <summary>
        /// This is were the results of filter process are placed in.
        /// </summary>
        private Video vidResult
        {
            get;
            set;
        }

        /// <summary>
        /// This is were the results of metric analysis are placed in.
        /// Every metric has a own video.
        /// </summary>
        private Video[] arrayVidResult
        {
            get;
            set;
        }

        /// <summary>
        /// Control for Macro in FilterView
        /// </summary>
        public MacroFilterControl macroFilterControl
        {
            get
            {
                return this.macroFilter.macroControl as MacroFilterControl;
            }
        }
        
        /// <summary>
        /// Control for Macro in MetricView
        /// </summary>
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


        public VM_Macro()
        {
            PluginManager.OqatToggleView += this.onToggleView;
            PluginManager.macroEntryAdd += this.onEntrySelect;

            this.macroFilter =(PF_MacroFilter) PluginManager.pluginManager.getPlugin<IMacro>("PF_MacroFilter");
            this.macroMetric =(PM_MacroMetric)PluginManager.pluginManager.getPlugin<IMacro>("PM_MacroMetric");
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
        /// Starts the process of PM_MacroMetric and PF_MacroFilter and initilize all Data needed.
        /// </summary>
        public void startProcess()
        {
            if (this.viewType == ViewType.MetricView)
            {
                macroMetricControl.macroTable.IsEnabled = false;
                arrayVidResult = new Video[macroMetric.macroQueue.Count];
                IVideoInfo vidInfo = (IVideoInfo)vidRef.vidInfo.Clone();
                //Name new Videos   "analysed" + macroMetric.macroQueue[i].mementoName?? maybe to long, or textboxes
                for (int i = 0; i < macroMetric.macroQueue.Count; i++)
                {
                    arrayVidResult[i] = new Video(true, getNewFileName(vidRef.vidPath, "analysed" + i), vidInfo, this.macroFilter.macroQueue.ToList<MacroEntry>());
                }
                //this.macroMetric.init(vidRef, vidProc, arrayVidResult);
                this.macroMetric.analyse(vidRef, vidProc,this.idProc, arrayVidResult);
                macroMetricControl.macroTable.IsEnabled = true;
            }
            if (this.viewType == ViewType.FilterView)
            {
                macroFilterControl.macroTable.IsEnabled = false;
                macroFilterControl.rangeSliders.IsEnabled = false;
                IVideoInfo vidInfo =(IVideoInfo) vidRef.vidInfo.Clone();
                vidResult = new Video(false, getNewFileName(vidRef.vidPath, "filtered"), vidInfo, this.macroFilter.macroQueue.ToList<MacroEntry>());
                this.macroFilter.init(vidRef, vidResult);
                this.macroFilter.process(vidRef,this.idRef, vidResult);
                macroFilterControl.macroTable.IsEnabled = true;
                macroFilterControl.rangeSliders.IsEnabled = true;
            }
        }

        /// <summary>
        /// Will be called if a Plugin with settings has to be added to macroQueue of Filter/Metric 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">A MacroEntry</param>
        public void onEntrySelect(object sender, MementoEventArgs e)
        {
            if (this.viewType == ViewType.FilterView)
            {
                this.macroFilter.addFilter(e);
            }
            if (this.viewType == ViewType.MetricView)
            {
                MacroEntryMetric mEntryMetric = new MacroEntryMetric(e.pluginKey, e.mementoName, this.vidRef, this.vidProc);
                macroMetric.macroQueue.Add(mEntryMetric);
            }
        }

        /// <summary>
        /// Generates a new filename, that is not taken yet. If the filename exists a number is added as suffix.
        /// </summary>
        /// <param name="originalFile">the originalfile used as base filename</param>
        /// <param name="suffix">a suffix added to the original filename</param>
        /// <returns>a filename similar to originalFile that does not exist yet.</returns>
        private string getNewFileName(string originalFile, string suffix)
        {
            string resultpath = "";
            int i = 0;
            do
            {
                resultpath = System.IO.Path.GetDirectoryName(originalFile)
                    + "\\" + System.IO.Path.GetFileNameWithoutExtension(originalFile)
                    + suffix;

                if (i > 0)
                {
                    resultpath += i;
                }
                i++;

                resultpath += System.IO.Path.GetExtension(originalFile);
            }
            while(System.IO.File.Exists(resultpath));

            return resultpath;
        }

    }
}
