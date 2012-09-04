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


namespace PF_RelativeColor
{

    [ExportMetadata("namePlugin", "RelativeColor")]
    [ExportMetadata("type", PluginType.IFilterOqat)]
    [ExportMetadata("threadSafe", false)]
    [Export(typeof(IPlugin))]
    [Serializable()]
    public class RelativeColor : IFilterOqat, INotifyPropertyChanged
    {
        /// <summary>
        /// constructor
        /// </summary>
        public RelativeColor()
        {

        }


        #region variables and properties

        private string _namePlugin = "RelativeColor";
        private PluginType _type = PluginType.IFilterOqat;
        
        

        double _red = 1;
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
        double _green = 1;
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
        double _blue = 1;
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


        VM_RelativeColor propertiesView;
        public UserControl propertyView
        {
            get
            {
                if (propertiesView == null)
                {
                    propertiesView = new VM_RelativeColor();
                    localize(_namePlugin + "_" + Thread.CurrentThread.CurrentCulture + ".xml");
                    propertiesView.DataContext = this;
                }
                return this.propertiesView;
            }
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

        public bool threadSafe
        {
            get { return false; }
        }

        #endregion

        

        /// <summary>
        /// Generates the filtered Image.
        /// </summary>
        public Bitmap process(Bitmap frame)
        {
            for (int i = 0; i < frame.Height - 1; i++)
            {
                for (int j = 0; j < frame.Width - 1; j++)
                {
                    double newRed =frame.GetPixel(j, i).R*redValue;
                    double newGreen = frame.GetPixel(j, i).G*greenValue;
                    double newBlue = frame.GetPixel(j, i).B*blueValue;

                    if (newRed > 255)
                    {
                        newRed = 255;
                    }
                    if (newGreen > 255)
                    {
                        newGreen = 255;
                    }
                    if (newBlue > 255)
                    {
                        newBlue = 255;
                    }

                    int newPixel = ((frame.GetPixel(j, i).A) << 24) | ((int)newRed << 16) | ((int)newGreen << 8) | (int)newBlue;
                    frame.SetPixel(j, i,Color.FromArgb(newPixel));

                }
            }
            return frame;
        }


        /// <summary>
        /// Returns a Memento with the current state of the Object.
        /// </summary>
        public Oqat.PublicRessources.Model.Memento getMemento()
        {
            double[] colorValues= new double[3];
            colorValues[0] = redValue;
            colorValues[1] = greenValue;
            colorValues[2] = blueValue;
            Memento mem = new Memento(this.namePlugin, colorValues);

            return mem;
        }

        /// <summary>
        /// Sets a Memento as the current state of the Object.
        /// </summary>
        public void setMemento(Oqat.PublicRessources.Model.Memento memento)
        {
            Object obj = memento.state;

            var colorValues = (double[])obj;
            redValue = colorValues[0];
            greenValue = colorValues[1];
            blueValue = colorValues[2];
        }

        private void localize(String s)
        {
            propertiesView.local(s);
        }

        public IPlugin createExtraPluginInstance()
        {
            return new RelativeColor();
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

