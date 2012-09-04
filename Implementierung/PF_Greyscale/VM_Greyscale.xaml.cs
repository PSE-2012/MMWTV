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

namespace PF_Greyscale
{
    /// <summary>
    /// Interaktionslogik für VM_Greyscale.xaml
    /// </summary>
    public partial class VM_Greyscale : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public VM_Greyscale()
        {
            InitializeComponent();

            resetValues();
        }




        /// <summary>
        /// Sets the Language Content and reads it from an XML File.
        /// </summary>
        public void local(String s)
        {
            try
            {
                String sFilename = Directory.GetCurrentDirectory() + "/" + s;
                XmlTextReader reader = new XmlTextReader(sFilename);
                reader.Read();
                reader.Read();
                String[] t = new String[3];
                String[] t2 = new String[3];
                for (int i = 0; i < 3; i++)
                {
                    reader.Read();
                    reader.Read();
                    t[i] = reader.Name;
                    reader.MoveToNextAttribute();
                    t2[i] = reader.Value;
                    if (t2[i] == "")
                    {
                        throw new XmlException("datei nicht lang genug");
                    }
                }
                label1.Content = t2[0];
                label2.Content = t2[1];
                label3.Content = t2[2];
            }
            catch (IndexOutOfRangeException e) { }
            catch (FileNotFoundException e) { }
            catch (XmlException e) { }
        }

        private void bttReset_Click(object sender, RoutedEventArgs e)
        {
            resetValues();
        }

        private void resetValues()
        {
            this.red.Value = 0.2125;
            this.green.Value = 0.7154;
            this.blue.Value = 0.0721;
        }

    }
}
