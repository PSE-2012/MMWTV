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

        /// <summary>
        /// Constructor
        /// </summary>
        
        public VM_NoiseGenerator()
        {
            InitializeComponent();
           
            //uper.Value =0;
            //uperBorder = (float)uper.Value; 
        }

        ///// <summary>
        ///// Listener for data Binding
        ///// </summary>

        //private void ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        //{
        //    uperBorder = (float)uper.Value;
            
           
        //}

        ///// <summary>
        ///// Sets the Slider of the Properties Views
        ///// </summary>

        //public void changeValue(double noise){
        //    uper.Value =noise;
          
           
        //    this.ValueChanged(this,null);
        //}

        ///// <summary>
        ///// Gets the Slider of the Properties Views
        ///// </summary>

        //public float getValue(){
        //    return uperBorder;
        //}

        /// <summary>
        /// Sets the Language Content and reads it from an XML File.
        /// </summary>
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
                    if (t2[0] == "")
                    {
                        throw new XmlException("datei nicht lang genug");
                    }
                
                label1.Content = t2[0];
           }
           catch (IndexOutOfRangeException e) { }
           catch (FileNotFoundException e) { }
           catch (XmlException e) { }
          
        }
       
    }
}
