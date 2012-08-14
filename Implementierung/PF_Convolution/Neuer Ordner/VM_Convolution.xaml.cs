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

namespace PF_Convolution
{
    /// <summary>
    /// Interaktionslogik für VM_Convolution.xaml
    /// </summary>
    [Serializable()]
    public partial class VM_Convolution : UserControl
    {
        public VM_Convolution()
        {
            InitializeComponent();
            resetPanel();
        }

        public void resetPanel()
        {
            tB1.Text = "0,0,0,0,0";
            tB2.Text = "0,0,0,0,0";
            tB3.Text = "0,0,0,0,0";
            tB4.Text = "0,0,0,0,0";
            tB5.Text = "0,0,0,0,0";
        }
        public void setPanel(String[] text)
        {
            tB1.Text = text[0];
            tB2.Text = text[1];
            tB3.Text = text[2];
            tB4.Text = text[3];
            tB5.Text = text[4];
        }

        public string[] getPanel()
        {
            return new String[] { tB1.Text, tB2.Text,tB3.Text,tB4.Text,tB5.Text };
        }
    }
}
