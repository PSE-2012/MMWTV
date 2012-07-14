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

namespace PF_Greyscale
{
    /// <summary>
    /// Interaktionslogik für VM_Greyscale.xaml
    /// </summary>
    public partial class VM_Greyscale : UserControl
    {
        double redValue;
        double greenValue;
        double blueValue;
        
        public VM_Greyscale()
        {
            InitializeComponent();

            red.Value =0.2125;
            green.Value = 0.7154;
            blue.Value = 0.0721;
            redValue = red.Value;
            greenValue = green.Value;
            blueValue = blue.Value;
            
        }

        private void ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            redValue = red.Value;
            greenValue = green.Value;
            blueValue = blue.Value;
        }

        public void changeValue(double r,double g, double b){
            red.Value =r;
            green.Value = g;
            blue.Value = b;
            this.ValueChanged(this,null);
        }
        public double getRed(){
            return redValue;
        }
        public double getGreen()
        {
            return greenValue;
        }
        public double getBlue()
        {
            return blueValue;
        }
    }
}
