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
    public partial class Diagramm : UserControl ,IPresentation   
    {
        public Diagramm()
        {
            InitializeComponent();
        }

        #region getter/setter

        private PlotModel MyPlotModel
        {
            get;
            set;
        }

        public PresentationPluginType presentationType
        {
            get
            {
                return PresentationPluginType.Diagram;
            }
        }

        public string namePlugin
        {
            get
            {
                return "PP_Diagram";
            }
        }

        public PluginType type
        {
            get
            {
                return PluginType.IPresentation;
            }
        }

        public UserControl propertyView
        {
            get
            {
                return this;
            }
        }

        #endregion


        /// <summary>
        /// Returns a new instance of Diagram Presentation.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new Diagramm();
        }


        /// <summary>
        /// Loads the given video to display its metric data.
        /// </summary>
        /// <param name="video">video to load metric results from.</param>
        /// <param name="position">framenumber to focus - not used</param>
        /// <exception cref="IllegalArgumentException">thrown if video is null</exception>
        public void setVideo(IVideo video, int position = 0)
        {
            if (video == null)
                throw new ArgumentException("Video to load must not be null.");

            if (video.frameMetricValue != null)
            {
                string plugin;
                try
                {
                    plugin = video.processedBy[0].pluginName;

                }
                catch (Exception e)
                {
                    plugin = "";
                }
                createDataSeries(video.frameMetricValue, plugin);
            }
            else
            {
                //ignore setVideo request, if video has no metric data
                //throw new System.ArgumentException("frameMetricValue ==NULL");
            }
        }

        /// <summary>
        /// Helper method to create content of the Plotmodel
        /// </summary>
        private void createDataSeries(float[][] series, String pluginname)
        {
            var plotModel1 = new PlotModel();
            plotModel1.LegendSymbolLength = 24;
            plotModel1.Title = pluginname;
            var linearAxis1 = new LinearAxis();
            linearAxis1.Position = AxisPosition.Bottom;
            plotModel1.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            plotModel1.Axes.Add(linearAxis2);

            StemSeries[] oxySeries = new StemSeries[series[0].Length];
            for (int i = 0; i < oxySeries.Length; i++)
            {
                oxySeries[i] = new StemSeries();
                oxySeries[i].MarkerSize = 2;
                //oxySeries[i].MarkerStroke = OxyColors.White;
                //oxySeries[i].MarkerStrokeThickness = 1.5;
                oxySeries[i].MarkerType = MarkerType.Circle;
                oxySeries[i].StrokeThickness = 0.1;
            }

            for (int j = 0; j < series.Length; j++)
            { //for each frame
                for (int i = 0; i < oxySeries.Length; i++)
                {
                    oxySeries[i].Points.Add(new DataPoint(j, series[j][i]));
                }
            }

            foreach (Series s in oxySeries)
            {
                plotModel1.Series.Add(s);
            }
            this.plotModel.Model = plotModel1;
        }

        /// <summary>
        /// Resets the plotModel
        /// </summary>
        public void flush()
        {
            plotModel.Model = null;
        }

        

        /// <summary>
        /// Creates a memento saving the current settings.
        /// </summary>
        /// <returns>Memento containing the current settings</returns>
        /// <remarks>Not implemented!</remarks>
        public Oqat.PublicRessources.Model.Memento getMemento()
        {
            return new Memento("Diagram_settings", null);
            //throw new NotImplementedException();
        }
        /// <summary>
        /// Loads the settings from the given memento.
        /// </summary>
        /// <remarks>Not implemented!</remarks>
        public void setMemento(Oqat.PublicRessources.Model.Memento memento)
        {
            //throw new NotImplementedException();
        }

        
    }
}