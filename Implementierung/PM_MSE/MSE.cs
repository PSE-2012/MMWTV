//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace PM_MSE
{
	using Oqat.PublicRessources.Plugin;
 
	
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
    using System.Drawing;
    using System.ComponentModel.Composition;
    using Oqat.PublicRessources.Model;

    [ExportMetadata("namePlugin", "PM_MSE")]
    [ExportMetadata("type", PluginType.IMetricOqat)]
    [Export(typeof(IPlugin))]

	public class MSE : IMetricOqat
	{
        private string _namePlugin = "PM_MSE";
        private PluginType _type = PluginType.IMetricOqat;
        
        VM_PM_MSE propertiesView;


        private int otto()
        {
            return 42;
        }
        public AnalysisInfo analyse(System.Drawing.Bitmap frameRef, System.Drawing.Bitmap frameProc)
        {

            Bitmap resultFrame = new Bitmap(frameRef.Width,frameRef.Height);
            double summe = 0;
            double summeR= 0;
            double summeG = 0;
            double summeB = 0;


           float[] resultValues = new float[4];
          
                for (int i = 0; i <frameRef.Height-1; i++)
                {
                    for (int j = 0; j <frameRef.Width-1; j++)
                    {
                        int newPixel = 0;
                   


                        Color colorProc = frameProc.GetPixel(j, i);

                        int alphaProc = colorProc.A;
                        int rotProc = colorProc.R;
                        int grunProc = colorProc.G;
                        int blauProc = colorProc.B;


                        Color colorRef = frameRef.GetPixel(j, i);

                        int alphaRef = colorRef.A;
                        int rotRef = colorRef.R;
                        int grunRef = colorRef.G;
                        int blauRef = colorRef.B;
                        //rgb
                       
                        
                        int newRot = (int)Math.Pow((rotProc - rotRef) , 2);
                        int newGrun = (int)Math.Pow((grunProc - grunRef) , 2);
                        int newBlau = (int)Math.Pow((blauProc - blauRef) , 2);



                        summe = summe + newBlau/3 + newGrun/3 + newRot/3;
                        summeR = summeR+newRot;
                        summeG = summeG +newGrun;
                        summeB = summeB +newBlau;

                        switch (propertiesView.getRb())
                        {
                            case 0:
                                newPixel = (((alphaProc + alphaRef) / 2) << 24) | (newRot << 16) | (newGrun << 8) | newBlau;
                                break;

                            case 1:
                                newPixel = (((alphaProc + alphaRef) / 2) << 24) | (newRot << 16) | (0 << 8) | 0;
                                 break;

                            case 2:
                                 newPixel = (((alphaProc + alphaRef) / 2) << 24) | (0 << 16) | (newGrun << 8) | 0;
                                 break;

                            case 3:
                                 newPixel = (((alphaProc + alphaRef) / 2) << 24) | (0<< 16) | (0 << 8) | newBlau;
                                 break;


                        }
            
                       resultFrame.SetPixel(j, i, Color.FromArgb(newPixel));
                    }
                }

                resultValues[0] = (float)summe / (frameProc.Height * frameProc.Width);
                resultValues[1] = (float)summeR / (frameProc.Height * frameProc.Width);
                resultValues[2] = (float)summeG / (frameProc.Height * frameProc.Width);
                resultValues[3] = (float)summeB / (frameProc.Height * frameProc.Width);

                AnalysisInfo analyse = new AnalysisInfo(resultFrame,resultValues);
 
            
            return analyse;
        }

        

        public string namePlugin
        {
            get
            {
               return _namePlugin;
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
                this._type=value;
            }
        }
        public MSE(){
            propertiesView = new VM_PM_MSE();
           
        }
        public void setParentControl(System.Windows.Controls.Panel parent)
        {
            parent.Children.Add(this.propertiesView);
        }

        public Dictionary<EventType, List<Delegate>> getEventHandlers()
        {
            Dictionary<EventType, List<Delegate>> handlers = new Dictionary<EventType, List<Delegate>>();
            return handlers;
        }

        public Oqat.PublicRessources.Model.Memento getMemento()
        {
            Memento mem = new Memento(this.namePlugin, this);

            return mem;
        }

        public void setMemento(Oqat.PublicRessources.Model.Memento memento)
        {
            Object obj = memento.state;
            MSE mse = (MSE)obj;
            this.propertiesView.setRb(mse.propertiesView.getRb())  ;
           
        }
    }
}

