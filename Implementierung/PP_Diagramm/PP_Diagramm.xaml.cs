﻿using System;
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
using PP_Presentation;
using Oqat.PublicRessources.Plugin;
using Plugins;

using Microsoft.CSharp;
using System.ComponentModel.Composition;

namespace PP_Diagramm
{
    /// <summary>
    /// Interaktionslogik für PP_Diagramm.xaml
    /// </summary>
    /// 
    [ExportMetadata("namePlugin", "PP_Diagramm")]
    [ExportMetadata("type", PluginType.Presentation)]
    [Export(typeof(IPlugin))]
  
    public partial class Diagramm :UserControl ,IPresentation   
    {
        private PresentationPluginType _presentationType=PresentationPluginType.Diagram;
        private string _namePlugin = "PP_Diagramm";
        private PluginType _type = PluginType.Presentation;

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

        public void setParentControll(System.Windows.Controls.Panel parent)
        {
            parent.Children.Add(this);

        }

        public Dictionary<EventType, List<Delegate>> getEventHandlers()
        {
            Dictionary<EventType, List<Delegate>> handlers = new Dictionary<EventType,List<Delegate>>();
            return handlers;
        }

        public Oqat.PublicRessources.Model.Memento getMemento()
        {
            throw new NotImplementedException();
        }

        public void setMemento(Oqat.PublicRessources.Model.Memento memento)
        {
            throw new NotImplementedException();
        }

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


        public void loadVideo(object sender, Oqat.PublicRessources.Model.VideoEventArgs vid) 
        {
            if (vid.video.frameMetricValue != null)
            {
                

                createDataSeries(vid.video.frameMetricValue);
            }
            else
            {
                System.ArgumentException illegalArgumentException = new System.ArgumentException("frameMetricValue ==NULL");
                throw illegalArgumentException;
            }
        }
    }
}