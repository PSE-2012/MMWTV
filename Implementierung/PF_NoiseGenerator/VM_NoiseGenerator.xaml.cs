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
using System.Xml;
using System.IO;

namespace PF_NoiseGenerator
{
    /// <summary>
    /// Interaktionslogik für VM_Greyscale.xaml
    /// </summary>
    [Serializable()]
    public partial class VM_NoiseGenerator : UserControl
    {
        float uperBorder;
      
        
        
        public VM_NoiseGenerator()
        {
            InitializeComponent();
           
            uper.Value =0;
           
            
            uperBorder = (float)uper.Value;
           
            
            
        }

        private void ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            uperBorder = (float)uper.Value;
            
           
        }

        public void changeValue(double up){
            uper.Value =up;
          
           
            this.ValueChanged(this,null);
        }
        public float getUp(){
            return uperBorder;
        }

        public void local(String s)
        {
           try{
                String sFilename = Directory.GetCurrentDirectory() + "/" + s;
                XmlTextReader reader = new XmlTextReader(sFilename);
                reader.Read();
                reader.Read();
                String[] t = new String[1];
                String[] t2 = new String[1];
               
                    reader.Read();
                    reader.Read();
                    t[0] = reader.Name;
                    reader.MoveToNextAttribute();
                    t2[0] = reader.Value;
                
                label1.Content = t2[0];
           }
           catch (IndexOutOfRangeException e) { }
           catch (FileNotFoundException e) { }
           catch (XmlException e) { }
          
        }
       
    }
}
