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

    [ExportMetadata("namePlugin", "PF_Convolution")]
    [ExportMetadata("type", PluginType.IFilterOqat)]
    [Export(typeof(IPlugin))]


    public class Convolution : IFilterOqat
    {

        private string _namePlugin = "PF_Convolution";
        private PluginType _type= PluginType.IFilterOqat;
        int[,] matrix = new int[5, 5];
        private VM_Convolution propertiesView;

        public Convolution()
        {
            propertiesView = new VM_Convolution();
            
        }
         bool setMatrix(String[] tbs)
        {
            bool success = true;
            try
            {
               

                for (int j = 0; j < 5; j++)
                {
                    if((tbs[j].Split(new Char[] { ',' }).Length)==5){
                    for (int i = 0; i < 5; i++)
                    {
                        matrix[j, i] = int.Parse(tbs[j].Split(new Char[] { ',' })[i]);

                    }}else{
                        propertiesView.resetPanel();
                        success = false;
                        j = 42;
                    }
                }
            }
            catch (FormatException)
            {
                propertiesView.resetPanel();
                success = false;
              MessageBox.Show("Die eingegebene Matrix enthält ungültige Zeichen.");  
            }
            return success;
        }

         void setPanelView()
         {
             String[] tbs = new String[5];

             for (int j = 0; j < 5; j++)
             {
                 for (int i = 0; i < 5; i++)
                 {
                     tbs[j] = tbs[j]+matrix[j,i];
                 }
                 if (j < 4)
                 {
                     tbs[j] = tbs[j] + ',';
                 }

             }
             propertiesView.setPanel(tbs);
         }


         public Bitmap process(Bitmap frame) 
        {
            if (setMatrix(propertiesView.getPanel()) == true)
            {
                // create filter
                AForge.Imaging.Filters.Convolution filter = new AForge.Imaging.Filters.Convolution(matrix);
                // apply the filter
                filter.ApplyInPlace(frame);
                
            }
            else
            {
               //frame = null;
                
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

        public void setParentControll(System.Windows.Controls.Panel parent)
        {
           
            parent.Children.Add(propertiesView);
        }

        public Dictionary<EventType, List<Delegate>> getEventHandlers()
        {
            Dictionary<EventType, List<Delegate>> handlers = new Dictionary<EventType, List<Delegate>>();
            return handlers;
        }

        public Memento getMemento()
        {
            Memento mem = new Memento(this.namePlugin,this);
            
            return mem;
        }

        public void setMemento(Memento memento)
        {
           Object obj= memento.state;
           
            Convolution otto = (Convolution)obj;


            for (int i = 0; i < 5; i++)
            {
                for( int j = 0; j<5; j++){
                    matrix[i, j] = otto.matrix[i, j];
                }

            }

            setPanelView();
        }
    }
}

