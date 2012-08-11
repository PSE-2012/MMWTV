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
    /// <summary>
    /// This class is a readOnly version of the <see cref="PropertiesView"/> class, it can
    /// be used to display general informations about a video.
    /// </summary>
    public partial class ReadOnlyPropertiesView : UserControl
    {
        public ReadOnlyPropertiesView(YuvVideoInfo yuvInfo)
        {
            InitializeComponent();
            if (yuvInfo != null)
                throw new NullReferenceException("Given YuvVideoInfo object is not initialized.");
            this.DataContext = yuvInfo;
        }
       
    }
}
