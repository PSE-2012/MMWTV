﻿//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace PF_RelativeColor
{
    using Oqat;
    using Oqat.PublicRessources.Model;
    using Oqat.PublicRessources.Plugin;
    using System.Drawing;
   
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ComponentModel.Composition;


    [ExportMetadata("namePlugin", "PF_RelativeColor")]
    [ExportMetadata("type", PluginType.IFilterOqat)]
    [Export(typeof(IPlugin))]

	public class RelativeColor : IFilterOqat
	{
        private string _namePlugin = "PF_RelativeColor";
        private PluginType _type = PluginType.IFilterOqat;
        
        VM_RelativeColor propertiesView;
        public RelativeColor()
        {
            propertiesView = new VM_RelativeColor();

        }

        public Bitmap process(Bitmap frame)
        {
            double red = propertiesView.getRed();
            double green = propertiesView.getGreen();
            double blue = propertiesView.getBlue();

            for (int i = 0; i < frame.Height - 1; i++)
            {
                for (int j = 0; j < frame.Width - 1; j++)
                {
                    double newRed =frame.GetPixel(j, i).R*red;
                    double newGreen = frame.GetPixel(j, i).G*green;
                    double newBlue = frame.GetPixel(j, i).B*blue;

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

            RelativeColor otto = (RelativeColor)obj;
            this.propertiesView.changeValue(otto.propertiesView.getRed(), otto.propertiesView.getGreen(), otto.propertiesView.getBlue());
        }

    }
}
