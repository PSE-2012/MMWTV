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

namespace PM_MSE
{
    /// <summary>
    /// Interaktionslogik für VM_PM_MSE.xaml
    /// </summary>
    public partial class VM_PM_MSE : UserControl
    {
        public VM_PM_MSE()
        {
            InitializeComponent();
        }

        public int getRb()
        {
            int value = 0;
            if (rbRGB.IsChecked == true)
            {
                value = 0;
            }
            if (rbR.IsChecked == true)
            {
                value = 1;
            }
            if (rbG.IsChecked == true)
            {
                value = 2;
            }
            if (rbB.IsChecked == true)
            {
                value = 3;
            } 

            return value;

        }


        public void setRb(int i)
        {
            if (i ==0)
            {
                rbRGB.IsChecked = true;
            }
            if (i==1)
            {
                rbR.IsChecked = true;
            }
            if (i == 2)
            {
                rbG.IsChecked = true;
            }
            if (i == 3)
            {
                rbB.IsChecked = true;
            }
           
        }
    }
}

   

