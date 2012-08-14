//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.PublicRessources.Plugin
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
    using System.ComponentModel;

    /// <summary>
    /// This class is used to determine the history of a video object ( e.g. filtered by) and
    /// by classes inheriting from Macro (to reconstruct filter bound to a macro filter)
    /// </summary>
    [Serializable()]
	public class MacroEntryFilter : MacroEntry, INotifyPropertyChanged
	{
        private double _endFrameRelative;
        private double _startFrameRelative;

		public double endFrameRelative
		{
            get
            {
                return this._endFrameRelative;
            }
            set
            {
                this._endFrameRelative = value;
                NotifyPropertyChanged("endFrameRelative");
            }
		}

        public double startFrameRelative
        {
            get
            {
                return this._startFrameRelative;
            }
            set
            {
                this._startFrameRelative = value;
                NotifyPropertyChanged("startFrameRelative");
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

        public MacroEntryFilter(string pluginName, string mementoName, double endFrameRelative, double startFrameRelative)
        {
            this._pluginName = pluginName;
            this._mementoName = mementoName;
            this._endFrameRelative = endFrameRelative;
            this._startFrameRelative = startFrameRelative;            
        }
	}
}

