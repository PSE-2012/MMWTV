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
      
       
    }
}
