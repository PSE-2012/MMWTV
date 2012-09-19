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
using System.IO;
using System.Xml;
using System.Threading;
using System.Globalization;

namespace PM_MSE
{
    /// <summary>
    /// Interaktionslogik für VM_PM_MSE.xaml
    /// </summary>
    [Serializable()]
    public partial class VM_PM_MSE : UserControl
    {
        public VM_PM_MSE()
        {
            InitializeComponent();
        }

        /// <summary>
        /// returns the value of the Radio Buttons.
        /// </summary>
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

        /// <summary>
        /// sets the value of the Radio Buttons.
        /// </summary>
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

        /// <summary>
        /// Sets lokal language to a the xml file
        /// </summary>
        public void local(String s)
        {
            try
            {
                String sFilename = Directory.GetCurrentDirectory() + "/" + s;
                XmlTextReader reader = new XmlTextReader(sFilename);
                reader.Read();
                reader.Read();
                String[] t = new String[4];
                String[] t2 = new String[4];
                for (int i = 0; i < 4; i++)
                {
                    reader.Read();
                    reader.Read();
                    t[i] = reader.Name;
                    reader.MoveToNextAttribute();
                    t2[i] = reader.Value;
                    if (t2[i] == "")
                    {
                        throw new XmlException ("datei nicht lang genug");
                    }
                }
                
                rbRGB.Content = t2[0];
                rbR.Content = t2[1];
                rbG.Content = t2[2];
                rbB.Content = t2[3];
            }
            catch (IndexOutOfRangeException) { }
            catch (FileNotFoundException) { }
            catch (XmlException) { }

        }


    }

    public class rbConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return ((int)value == (int)parameter);
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (!((bool)value)) return -1;
            else return parameter;
        }
    }
}

   

