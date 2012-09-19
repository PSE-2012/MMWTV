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

namespace PS_YuvVideoHandler
{
    public partial class PropertiesView : UserControl
    {
        // Collection property used to fill the ComboBox with a list
        List<ComboBoxYuvFormats> formatList;
      

        public PropertiesView()
        {
            InitializeComponent();

            initFormatValues();
            this.cb_format.ItemsSource = formatList;
            this.local("YufVideoHandler_" + Thread.CurrentThread.CurrentCulture + ".xml");
        }
        private void local(String s)
        {
            try
            {
                String sFilename = Directory.GetCurrentDirectory() + "/" + s;
                XmlTextReader reader = new XmlTextReader(sFilename);
                reader.Read();
                reader.Read();
                String[] t = new String[6];
                String[] t2 = new String[6];
                for (int i = 0; i < 6; i++)
                {
                    reader.Read();
                    reader.Read();
                    t[i] = reader.Name;
                    reader.MoveToNextAttribute();
                    t2[i] = reader.Value;
                }
                gb1.Header = t2[0];
                l1.Content = t2[1];
                l2.Content = t2[2];
                l3.Content = t2[3];
                l4.Content = t2[4];


            }
            catch (IndexOutOfRangeException) { }
            catch (FileNotFoundException) { }
            catch (XmlException) { }
        }

        /// <summary>
        /// Set up the collection properties used to bind the ItemsSource 
        /// properties to display the list of items in the dropdown lists.
        /// </summary>
        private void initFormatValues()
        {
            formatList = new List<ComboBoxYuvFormats>()
            {
                new ComboBoxYuvFormats(){ formatEnum = YuvFormat.YUV420_IYUV, 
			        formatString = "Yuv 4:2:0 (IYUV)" },
                new ComboBoxYuvFormats(){ formatEnum = YuvFormat.YUV444, 
			        formatString = "Yuv 4:4:4" },
                new ComboBoxYuvFormats(){ formatEnum = YuvFormat.YUV422_UYVY, 
			        formatString = "Yuv 4:2:2 (UYVY)" },
                new ComboBoxYuvFormats(){ formatEnum = YuvFormat.YUV411_Y41P, 
			        formatString = "Yuv 4:1:1 (Y41P)" },
            };
        }

        
    }


    /// <summary>
    /// This class provides us with an object to fill a ComboBox with
    /// that can be bound to 'ViewModelEnum.Colors' enum fields in the binding
    /// object while displaying a string values in the to user in the ComboBox.
    /// </summary>
    public class ComboBoxYuvFormats
    {
        YuvFormat _formatEnum =  YuvFormat.YUV420_IYUV;
        string _formatString;

        public YuvFormat formatEnum 
        { 
            get
            {
                return _formatEnum;
            }
            set
            {
                _formatEnum = value;
            }
        }
        public string formatString
        {
            get
            {
                return _formatString;
            }
            set
            {
                _formatString = value;
            }
        }
    }



}
