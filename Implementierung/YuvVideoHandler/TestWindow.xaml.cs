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

namespace YuvVideoHandler
{
    
    /// <summary>
    /// Interaktionslogik für UserControl1.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        YuvVideoInfo info;
        PS_YuvVideoHandler handler;
        PS_YuvVideoHandler handler2;

        public TestWindow()
        {
            InitializeComponent();

            info = new YuvVideoInfo();
            info.width = 176;
            info.height = 144;
            info.yuvFormat = YuvFormat.YUV420_IYUV;

            handler = new PS_YuvVideoHandler("C:/Dokumente und Einstellungen/Sebastian/Eigene Dateien/PSE/Implementierung/YuvVideoHandler/akiyo_qcif.yuv", info);

            handler2 = new PS_YuvVideoHandler("C:/Dokumente und Einstellungen/Sebastian/Eigene Dateien/PSE/Implementierung/YuvVideoHandler/akiyo_qcif2.yuv", info);

        }

        int i = 0;
        public void BrowseButton_Click(object sender, EventArgs e)
        {
            i++;
            if (i >= 300) i = 0;


            

            Bitmap[] bmp = handler.getFrames(i, 20);

            handler2.writeFrame(i, bmp[0]);

            BitmapSource bmpsource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                       bmp[0].GetHbitmap(),
                       IntPtr.Zero,
                       System.Windows.Int32Rect.Empty,
                       BitmapSizeOptions.FromWidthAndHeight(info.width, info.height));
            BitmapSource bmpsource2 = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                       bmp[4].GetHbitmap(),
                       IntPtr.Zero,
                       System.Windows.Int32Rect.Empty,
                       BitmapSizeOptions.FromWidthAndHeight(info.width, info.height));

            ImageControl.Source = bmpsource;
            ImageControl2.Source = bmpsource2;
        }

        private void PropViewButton_Click(object sender, RoutedEventArgs e)
        {
            handler.setParentControl(this.propGrid);
            
        }

        private void WriteFrameButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }




    public class App : System.Windows.Application
    {
        public App()
        {
            StartupUri = new Uri("TestWindow.xaml", UriKind.Relative);
        }

        [System.STAThreadAttribute()]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.LoaderOptimization(LoaderOptimization.MultiDomainHost)]
        public static void Main()
        {
            //Application app = new Application();
            //EnsureApplicationResources();
            //app.Run();

            TestWindow t = new TestWindow();
            t.ShowDialog();
        }
    }


}
