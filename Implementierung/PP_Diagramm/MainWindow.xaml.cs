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


namespace PP_Diagramm
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public PresentationPluginType presentationType
        {
            get
            {
                return this.presentationType;
            }
            set
            {
                this.presentationType = value;
            }
        }

        public void unloadVideo()
        {
            plotModel.Model = null;

        }

        public void onFlushPresentationPlugins(object sender, EventArgs e)
        {
            plotModel.Model = null;
        }

        public string namePlugin
        {
            get
            {
                return this.namePlugin;
            }
            set
            {
                this.namePlugin = value;
            }
        }

        public PluginType type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }

        public void setParentControll(System.Windows.Controls.Panel parent)
        {
            parent.Children.Add(this);

        }

        public Dictionary<EventType, List<Delegate>> getEventHandlers()
        {
            throw new NotImplementedException();
        }

        public Oqat.PublicRessources.Model.Memento getMemento()
        {
            throw new NotImplementedException();
        }

        public void setMemento(Oqat.PublicRessources.Model.Memento memento)
        {
            throw new NotImplementedException();
        }

        private void createDataSeries(float[] series)
        {
            var plotModel1 = new PlotModel();
            plotModel1.LegendSymbolLength = 24;
            plotModel1.Title = "StemSeries";
            var linearAxis1 = new LinearAxis();
            linearAxis1.Position = AxisPosition.Bottom;
            plotModel1.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            plotModel1.Axes.Add(linearAxis2);
            var stemSeries1 = new StemSeries();
            stemSeries1.Color = OxyColors.SkyBlue;
            stemSeries1.MarkerFill = OxyColors.SkyBlue;
            stemSeries1.MarkerSize = 6;
            stemSeries1.MarkerStroke = OxyColors.White;
            stemSeries1.MarkerStrokeThickness = 1.5;
            stemSeries1.MarkerType = MarkerType.Circle;

            for (int i = 0; i < series.Length; i++)
            {
                stemSeries1.Points.Add(new DataPoint(i, series[i]));
            }

            plotModel1.Series.Add(stemSeries1);
            plotModel.Model = plotModel1;
        }


        public void loadVideo(object sender, Oqat.PublicRessources.Model.VideoEventArgs vid)
        {
            float[] einDimArray = new float[vid.video.frameMetricValue.Length - 1];
            for (int i = 0; i < vid.video.frameMetricValue.Length - 1; i++)
            {
                einDimArray[i] = vid.video.frameMetricValue[i][0];
            }

            createDataSeries(einDimArray);
        }
    }
}
