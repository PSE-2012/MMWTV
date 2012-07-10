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
        YuvFormat _formatEnum;
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
