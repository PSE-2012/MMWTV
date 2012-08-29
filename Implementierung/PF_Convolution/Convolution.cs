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

        const int MIN_DIM = 3;
        const int MAX_DIM = 25;
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
                    //matrix needs quadratic and in limited dimensions for AForge filter
                    if (value.GetLength(0) != value.GetLength(1) || value.GetLength(0) < MIN_DIM || value.GetLength(0) > MAX_DIM)
                    {
                        //dimensions not valid, need to fix

                        int dim = value.GetLength(0);
                        if (value.GetLength(1) > dim) dim = value.GetLength(1);

                        //a valid matrix for convolutionfilter has to be between 3 and 25
                        if (dim < MIN_DIM) dim = MIN_DIM;
                        else if (dim > MAX_DIM) dim = MAX_DIM;

                        _matrix = new int[dim, dim];

                        for (int i = 0; (i < value.GetLength(0) && i < MAX_DIM); i++)
                        {

                            for (int j = 0; (j < value.GetLength(1) && j < MAX_DIM); j++)
                            {
                                _matrix[i, j] = value[i, j];
                            }
                        }
                    }
                    else
                    {
                        _matrix = value;
                    }

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
            if (!(memento.state is int[,]))
                return;

            this.matrix = (int[,]) memento.state;
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