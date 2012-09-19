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
    /// <summary>
    /// This class is a readOnly version of the <see cref="PropertiesView"/> class, it can
    /// be used to display general informations about a video.
    /// </summary>
    public partial class ReadOnlyPropertiesView : UserControl
    {
        public ReadOnlyPropertiesView(YuvVideoInfo yuvInfo)
        {
            InitializeComponent();
            if (yuvInfo == null)
                throw new NullReferenceException("Given YuvVideoInfo object is not initialized.");
            this.DataContext = yuvInfo;
            this.local("YufVideoHandler_" + Thread.CurrentThread.CurrentCulture + ".xml");
        }

        private void local(string s)
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
                gb1.Header = t2[5];
              



            }
            catch (IndexOutOfRangeException) { }
            catch (FileNotFoundException) { }
            catch (XmlException) { }
        }
       
    }
}
