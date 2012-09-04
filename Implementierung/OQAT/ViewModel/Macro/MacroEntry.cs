namespace Oqat.ViewModel.MacroPlugin
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
    using System.Runtime.Serialization;
    using System.Collections.ObjectModel;

    using Oqat.PublicRessources.Plugin;
using System.ComponentModel;
    using System.Windows;

    /// <summary>
    /// This class represents a particular plugin and its memento wich enables a way to store such
    /// processing informatin within a PF_Macro or PM_Metric object.
    /// See MacroEntryMetric and MacroEntryFilter for usecases.
    /// </summary>
    [Serializable()]
	public class MacroEntry : IMacroEntry, INotifyPropertyChanged
	{

        // oneway binding
        // notify if mementoName changed
        public string namMemConcat
        { get { return pluginName + " : " + mementoName; } }

        public string pluginName
        { get; private set; }

        public PluginType type
        { get; private set; }

        private string _mementoName;
        public string mementoName
        {
            get { return _mementoName; }
            set
            {
                _mementoName = value;
                NotifyPropertyChanged("mementoName");
                NotifyPropertyChanged("namMemConcat");
            }
        }

        private int defaultRangeSliderWidth = 250;

        [field: NonSerialized]
        private int _frameCount;
        public int frameCount
        {
            get { return (_frameCount != 0)?_frameCount:100; }
            set
            {
                if (_frameCount != value)
                {
                    _frameCount = value;
                    NotifyPropertyChanged("frameCount");
                    NotifyPropertyChanged("tickSize");
                    foreach (var entry in this.macroEntries)
                    {
                        entry.frameCount = value;
                    }
                }
            }
        }

        public ObservableCollection<MacroEntry> macroEntries
        { get; set;}

        List<IMacroEntry> IMacroEntry.macroEntries
        {
            get { return new List<IMacroEntry>(macroEntries); }
        }

 
        public long endFrameAbs
        {
            get {
                if (endFrameRelative == 0)
                    return 0;
                return (long)(frameCount / 100.0 * endFrameRelative); }
        }


        public long startFrameAbs
        {
            get {


                if (startFrameRelative == 0)
                    return 0;
                return (long)(frameCount / 100.0 * startFrameRelative); }
        }

        // if this entry is a metric you can specify a custom path
        public string path;


        public double _endFrameRelative = 100;
        public double endFrameRelative
        {
            get
            {
                return _endFrameRelative;
            }
            set
            {
                _endFrameRelative = value;
                NotifyPropertyChanged("endFrameRelative");
                NotifyPropertyChanged("endFrameAbs");
            }
        }
        public double _startFrameRelative = 0;
        public double startFrameRelative
        {
            get
            {
                return _startFrameRelative;     
            }
            set
            {
                _startFrameRelative = value;
                NotifyPropertyChanged("startFrameRelative");
                NotifyPropertyChanged("startFrameAbs");
            }
        }



        // oneway bound
        // change frameCount value -> propertyChanged event 
        public long tickSize
        { 
            get 
            { 
                return (long) (frameCount / 100.0); 
            }
        }

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public MacroEntry(string pluginName,PluginType type, string mementoName)
        {
            this.pluginName = pluginName;
            this.type = type;
            this.mementoName = mementoName;
            this.macroEntries = new ObservableCollection<MacroEntry>();
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
        public bool readOnlyActiveState
        {
            get
            {
                return !readOnly;
            }
        }

        [field: NonSerialized]
        private bool _readOnly = false;
        public bool readOnly
        {
            get
            {
                return _readOnly;
            }
            set
            {
                
                _readOnly = value;
                NotifyPropertyChanged("readOnlyVisibility");
                NotifyPropertyChanged("readOnlyActiveState");
                NotifyPropertyChanged("rangeSliderWidth");

                if (this.macroEntries != null)
                    foreach (var entry in this.macroEntries)
                        entry.readOnly = _readOnly;

            }
        }

        public bool allowDrop
        {
            get
            {
                return !readOnly;
            }
        }
        public int rangeSliderWidth {
            get
            {
                if (readOnly)
                    return 0;
                else 
                    return defaultRangeSliderWidth;
            }
            
    }
    }
}

