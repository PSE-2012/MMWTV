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
            get { return (long)(frameCount / 100.0 * endFrameRelative); }
            set 
            {
                if (value < startFrameAbs)
                    value = startFrameAbs;

                endFrameRelative =value / (frameCount / 100.0);
                NotifyPropertyChanged("endFrameAbs");
            }
        }
       
        public long startFrameAbs
        {
            get { return (long)(frameCount / 100.0 * startFrameRelative); }
            set
            {
                if (value > endFrameAbs)
                    value = endFrameAbs;
                
                startFrameRelative = value / (frameCount / 100.0);

                NotifyPropertyChanged("startFrameAbs");
            }
        }

        // if this entry is a metric you can specify a custom path
        public string path;


        public double endFrameRelative
        { get; set;}

        public double startFrameRelative
        { get; set; }



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

    }
}

