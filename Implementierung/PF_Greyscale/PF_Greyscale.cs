using AForge;
using Oqat;
using Oqat.PublicRessources.Model;
using Oqat.PublicRessources.Plugin;
using System.Drawing;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.ComponentModel;


namespace PF_Greyscale
{

    [ExportMetadata("namePlugin", "Greyscale")]
    [ExportMetadata("type", PluginType.IFilterOqat)]
    [ExportMetadata("threadSafe", false)]
    [Export(typeof(IPlugin))]
    [Serializable()]
    public class Greyscale : IFilterOqat, INotifyPropertyChanged
    {
        public bool threadSafe
        {
            get { return false; }
        }
        private string _namePlugin = "Greyscale";
        private PluginType _type = PluginType.IFilterOqat;
        VM_Greyscale propertiesView;

        double _red = 0.2125;
        public double redValue
        {
            get
            {
                return _red;
            }
            set
            {
                if (_red != value)
                {
                    _red = value;
                    NotifyPropertyChanged("redValue");
                }
            }
        }
        double _green = 0.7154;
        public double greenValue
        {
            get
            {
                return _green;
            }
            set
            {
                if (_green != value)
                {
                    _green = value;
                    NotifyPropertyChanged("greenValue");
                }
            }
        }
        double _blue = 0.0721;
        public double blueValue
        {
            get
            {
                return _blue;
            }
            set
            {
                if (_blue != value)
                {
                    _blue = value;
                    NotifyPropertyChanged("blueValue");
                }
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        public Greyscale()
        {
            
        }

        /// <summary>
        /// Generates the filtered Image.
        /// </summary>
        public Bitmap process(Bitmap frame)
        {
            Bitmap grayImage;
            if (frame != null)
            {
                AForge.Imaging.Filters.Grayscale filter = new AForge.Imaging.Filters.Grayscale(redValue, greenValue, blueValue);
                grayImage = filter.Apply(frame);
            }
            else
            {
                grayImage = null;
            }
            return grayImage;
        }

        public string namePlugin
        {
            get
            {
                return this._namePlugin;

            }
            set
            {
                this._namePlugin = value;
            }
        }

        public PluginType type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        public UserControl propertyView
        {
            get
            {
                if (propertiesView == null)
                {
                    propertiesView = new VM_Greyscale();
                    propertiesView.local(_namePlugin + "_" + Thread.CurrentThread.CurrentCulture + ".xml");
                    propertiesView.DataContext = this;
                }
                return this.propertiesView;
            }
        }

        public Dictionary<EventType, List<Delegate>> getEventHandlers()
        {
            Dictionary<EventType, List<Delegate>> handlers = new Dictionary<EventType, List<Delegate>>();
            return handlers;
        }

        /// <summary>
        /// Returns a Memento with the current state of the Object.
        /// </summary>
        public Oqat.PublicRessources.Model.Memento getMemento()
        {
            double[] colorValues = new double[3];
            colorValues[0] = redValue;
            colorValues[1] = greenValue;
            colorValues[2] = blueValue;
            return new Memento(this.namePlugin , colorValues);
        }

        /// <summary>
        /// Sets a Memento as the current state of the Object.
        /// </summary>
        public void setMemento(Oqat.PublicRessources.Model.Memento memento)
        {
            if (memento.state is double[])
            {
                Object obj = memento.state;
                var colorValues = (double[])obj;
                redValue = colorValues[0];
                greenValue = colorValues[1];
                blueValue = colorValues[2];
            }
            
        }

        public void local(String s)
        {
            propertiesView.local(_namePlugin + s);
        }

        public IPlugin createExtraPluginInstance()
        {
            return new Greyscale();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
       
    }
}

