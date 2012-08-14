//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
namespace PF_Convolution
{
    using Oqat;
    
  
    using Oqat.PublicRessources.Model;
    using Oqat.PublicRessources.Plugin;
    using Oqat.PublicRessources;
 
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Drawing;
    using AForge.Imaging;
    using System.Windows;
    using System.ComponentModel.Composition;
    using System.Windows.Controls;
    using System.ComponentModel;

    [ExportMetadata("namePlugin", "PF_Convolution")]
    [ExportMetadata("type", PluginType.IFilterOqat)]
    [Export(typeof(IPlugin))]

    [Serializable()]
    public class Convolution : IFilterOqat, INotifyPropertyChanged
    {
        private string _namePlugin = "PF_Convolution";
        private PluginType _type= PluginType.IFilterOqat;

        int[,] _matrix;
        public int[,] matrix
        {
            get
            {
                return _matrix;
            }
            set
            {
                if(_matrix != value)
                {
                    _matrix = value;
                    NotifyPropertyChanged("matrix");
                }
            }
        }

        private VM_Convolution propertiesView;

        /// <summary>
        /// constructor
        /// </summary>

        public Convolution()
        {
            matrix = new int[3, 3];

            propertiesView = new VM_Convolution();
            propertiesView.DataContext = this;
        }

        /// <summary>
        /// Returns a Memento with the current state of the Object.
        /// </summary>

        public Memento getMemento()
        {
            return new Memento(this.namePlugin, this.matrix);
        }

        /// <summary>
        /// Sets a Memento as the current state of the Object.
        /// </summary>

        public void setMemento(Memento memento)
        {
           this.matrix =(int[,]) memento.state;
        }

        /// <summary>
        /// Generates the filtered Image.
        /// </summary>

        public Bitmap process(Bitmap frame)
        {
            // create filter
            AForge.Imaging.Filters.Convolution filter = new AForge.Imaging.Filters.Convolution(matrix);
            // apply the filter
            filter.ApplyInPlace(frame);

            return frame;
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
                return this.propertiesView;
            }
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