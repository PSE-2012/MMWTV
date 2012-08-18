using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OxyPlot;
using Oqat;


using Oqat.PublicRessources.Plugin;
using Oqat.PublicRessources.Model;


using Microsoft.CSharp;
using System.ComponentModel.Composition;

namespace PP_Diagramm
{
    /// <summary>
    /// Interaktionslogik für PP_Diagramm.xaml
    /// </summary>
    /// 
    [ExportMetadata("namePlugin", "PP_Diagram")]
    [ExportMetadata("type", PluginType.IPresentation)]
    [Export(typeof(IPlugin))]
  
    public partial class Diagramm :UserControl ,IPresentation   
    {
        private PresentationPluginType _presentationType=PresentationPluginType.Diagram;
        private string _namePlugin = "PP_Diagramm";
        private PluginType _type = PluginType.IPresentation;

        public object Clone()
        {
            return this.MemberwiseClone();
        }


        /// <summary>
        /// Constructor.
        /// </summary>

        public Diagramm()
        {
           
         
            InitializeComponent();
        }

        public PresentationPluginType presentationType
        {
            get
            {
                return _presentationType;
            }
            set
            {
               _presentationType= value;
            }
        }

        /// <summary>
        /// resets the plotModel
        /// </summary>
        public void unloadVideo()
        {
            plotModel.Model = null;

        }

        /// <summary>
        /// resets the plotModel
        /// </summary>
        public void flush()
        {
            plotModel.Model = null;
        }

        public string namePlugin
        {
            get
            {
                return _namePlugin;
            }
            set
            {
                _namePlugin = value;
            }
        }

        public PluginType type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }



        public UserControl propertyView
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// not necessary method
        /// </summary>

        public Oqat.PublicRessources.Model.Memento getMemento()
        {
            return new Oqat.PublicRessources.Model.Memento("defaultDiagramm", null, "");
        }
        /// <summary>
        /// not necessary method
        /// </summary>
        public void setMemento(Oqat.PublicRessources.Model.Memento memento)
        {
        }
        /// <summary>
        /// Helper method to create content of the Plotmodel
        /// </summary>

        private void createDataSeries(float[][] series)
        {


            var plotModel1 = new PlotModel();
            plotModel1.LegendSymbolLength = 24;
            plotModel1.Title = "";
            var linearAxis1 = new LinearAxis();
            linearAxis1.Position = AxisPosition.Bottom;
            plotModel1.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            plotModel1.Axes.Add(linearAxis2);


            for (int j = 0; j < series.Length; j++)
            {
                float[] einDimArray = series[j];

                var stemSeries1 = new StemSeries();
                stemSeries1.MarkerSize = 4;
                stemSeries1.MarkerStroke = OxyColors.White;
                stemSeries1.MarkerStrokeThickness = 1.5;
                stemSeries1.MarkerType = MarkerType.Circle;

                for (int i = 0; i < series[j].Length; i++)
                {
                    stemSeries1.Points.Add(new DataPoint(i, series[j][i]));
                }

                plotModel1.Series.Add(stemSeries1);
            }
           
            


            
           
            
            plotModel.Model = plotModel1;
        }

        /// <summary>
        /// loads the Video
        /// </summary>
        public void setVideo(IVideo video, int position = 0) 
        {
            if (video.frameMetricValue != null)
            {
                

                createDataSeries(video.frameMetricValue);
            }
            else
            {
                System.ArgumentException illegalArgumentException = new System.ArgumentException("frameMetricValue ==NULL");
                throw illegalArgumentException;
            }
        }
    }
}
