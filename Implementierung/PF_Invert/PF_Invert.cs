using AForge;
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

using System.Drawing.Imaging;

namespace PF_Invert
{

    [ExportMetadata("namePlugin", "Invert")]
    [ExportMetadata("type", PluginType.IFilterOqat)]
     [ExportMetadata("threadSafe", false)]
    [Export(typeof(IPlugin))]
    [Serializable()]
	public class Invert : IFilterOqat
	{
        public bool threadSafe
        {
            get { return false; }
        }

        private string _namePlugin = "Invert";
        private PluginType _type = PluginType.IFilterOqat;

        /// <summary>
        /// Generates the filtered Image.
        /// </summary>
        public Bitmap process(Bitmap frame)
        {
            Bitmap test = null;
            if (frame != null)
            {
                AForge.Imaging.Filters.Invert filter = new AForge.Imaging.Filters.Invert();
                // apply the filter
                test = new Bitmap(frame.Width, frame.Height, PixelFormat.Format24bppRgb);


                for (int i = 0; i < frame.Width; i++)
                {
                    for (int j = 0; j < frame.Height; j++)
                    {
                        test.SetPixel(i, j, frame.GetPixel(i, j));
                    }


                }
                /*  Graphics g = Graphics.FromImage(test);
                  g.DrawImage(frame, 0, 0);
                  g.Dispose();

                  frame = test; */
                filter.ApplyInPlace(test);
            }
            return test;
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
                return null;
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
            Memento mem = new Memento(this.namePlugin, this);
            return mem;
        }

        /// <summary>
        /// Sets a Memento as the current state of the Object.
        /// </summary>
        public void setMemento(Oqat.PublicRessources.Model.Memento memento)
        {
            
        }

        public IPlugin createExtraPluginInstance()
        {
            return new Invert();
        }
    }
}

