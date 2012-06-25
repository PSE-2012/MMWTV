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


    /// <summary>
    /// Interaktionslogik für UserControl1.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        public TestWindow()
        {
            InitializeComponent();

            YuvVideoInfo info = new YuvVideoInfo();
            info.width = 176;
            info.height = 144;

            PS_YuvVideoHandler hand = new PS_YuvVideoHandler("C:/akiyo_qcif.yuv", info);

            Bitmap bmp = hand.getFrame(0); 
            

            //bmp = new Bitmap("C:/test.jpg");
            //info.width = 550;
            //info.height = 785;

            BitmapSource bmpsource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
   bmp.GetHbitmap(),
   IntPtr.Zero,
   System.Windows.Int32Rect.Empty,
   BitmapSizeOptions.FromWidthAndHeight(info.width, info.height));

            ImageControl.Source = bmpsource;

        }
    }
}
