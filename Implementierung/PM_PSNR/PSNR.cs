//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace PM_PSNR
{
    using Oqat.PublicRessources.Plugin;


    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Drawing;
    using System.ComponentModel.Composition;
    using Oqat.PublicRessources.Model;


    [ExportMetadata("namePlugin", "PM_PSNR")]
    [ExportMetadata("type", PluginType.IMetricOqat)]
    [Export(typeof(IPlugin))]

	public class PSNR :  IMetricOqat
	{
        private string _namePlugin = "PM_PSNR";
        private PluginType _type = PluginType.IMetricOqat;

        public AnalysisInfo analyse(System.Drawing.Bitmap frameRef, System.Drawing.Bitmap frameProc)
        {
            Bitmap resultFrame = new Bitmap(frameRef.Width, frameRef.Height);
            double summe = 0;          

            float[] resultValues = new float[1];

            for (int i = 0; i < frameRef.Height - 1; i++)
            {
                for (int j = 0; j < frameRef.Width - 1; j++)
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
                    int newRot = (int)Math.Pow((rotProc - rotRef), 2);
                    int newGrun = (int)Math.Pow((grunProc - grunRef), 2);
                    int newBlau = (int)Math.Pow((blauProc - blauRef), 2);

                    summe = summe + newBlau  + newGrun + newRot ;
                     newPixel = (((alphaProc + alphaRef) / 2) << 24) | (newRot << 16) | (newGrun << 8) | newBlau;

                    resultFrame.SetPixel(j, i, Color.FromArgb(newPixel));
                }
            }

            float mse = (float)summe / (frameProc.Height * frameProc.Width*3);

            if(0<=mse&&mse<=1){
                resultValues[0] = -1;
            } else{
                resultValues[0] = (float)(20*Math.Log10(255)-10*Math.Log10(mse));
            }

            AnalysisInfo analyse = new AnalysisInfo(resultFrame, resultValues);


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
                this._type = value;
            }
        }

        public void setParentControl(System.Windows.Controls.Panel parent)
        {
           
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
            
        }

        public PSNR()
        {

        }
    }
}

