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
using System.Drawing;

namespace PF_RelativeColor
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        RelativeColor otto;
        public MainWindow()
        {
            InitializeComponent();
            otto = new RelativeColor();
            otto.setParentControll(grid1);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Bitmap b = new Bitmap("C:/Users/GRILLEDSHEEP/Desktop/sample1.bmp", true);


            otto.process(b);

            b.Save("C:/Users/GRILLEDSHEEP/Desktop/output.bmp");
        }

      
    }
}
