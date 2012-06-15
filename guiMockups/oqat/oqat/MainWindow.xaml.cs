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
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Video.DirectShow;
using AForge.Video;
using System.IO;
using AForge.Video.FFMPEG;
using System.Threading;
using OxyPlot;

using System.ComponentModel.Composition;
namespace oqat
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    /// 
    public class BitMapSource : IVideoSource
    {
        private string source;
        private long bytesReceived;        
        private int framesReceived;
        private Thread thread = null;
        private ManualResetEvent stopEvent = null;

        public long BytesReceived
        {
            get
            {
                long bytes = bytesReceived;
                bytesReceived = 0;
                return bytes;
            }
        }


        public int FramesReceived
        {
            get
            {
                int frames = framesReceived;
                framesReceived = 0;
                return frames;
            }
        }
        public bool IsRunning
        {
			get
			{
				if ( thread != null )
				{
                    // check thread status
					if ( thread.Join( 0 ) == false )
						return true;

					// the thread is not running, free resources
					Free( );
				}
				return false;
			}
        }

        public event NewFrameEventHandler NewFrame;

        public event PlayingFinishedEventHandler PlayingFinished;

        public void SignalToStop()
        {
			if ( thread != null )
			{
				// signal to stop
				stopEvent.Set( );
			}
        }

        public string Source
        {
            get { return source; }
			set { source = value; }
        }

        public void Start()
        {
			if ( !IsRunning )
			{
                // check source
                if ( ( source == null ) || ( source == string.Empty ) )
                    throw new ArgumentException( "Video source is not specified." );

				framesReceived = 0;
				bytesReceived = 0;

				// create events
				stopEvent = new ManualResetEvent( false );

                // create and start new thread
				thread = new Thread( new ThreadStart( WorkerThread ) );
				thread.Name = source; // mainly for debugging
				thread.Start( );
			}
        }

        public void Stop()
        {
			if ( this.IsRunning )
			{
                stopEvent.Set( );
                thread.Abort( );
				WaitForStop( );
			}
        }

        public event VideoSourceErrorEventHandler VideoSourceError;

        private void Free( )
		{
			thread = null;

			// release events
			stopEvent.Close( );
			stopEvent = null;
		}

        public void WaitForStop()
        {
			if ( thread != null )
			{
				// wait for thread stop
				thread.Join( );

				Free( );
			}
        }


        public BitMapSource(String source, int width, int height, int numframes) {
            this.source = source;
            this.width = width;
            this.height = height;
            this.NUMFRAMESINMEM = numframes;
        }
        private void WorkerThread() {
             Sequence();
            for (int z = 0; z < NUMFRAMESINMEM; z++) {
                System.Threading.Thread.Sleep(50);
                onNewFrame(bmp[z]);
            }
        }

// // Get the luminance and color components of given frame and store them in arrays.s
//// Loads automatically unavailable data from file to memory.
//private void getFrameData(int frame)
//{
//// check if we have to load new data
//if (frame>= firstFrameInMem + NUMFRAMESINMEM)
//{
//Load(frame);
//firstFrameInMem = frame;
//}
//if (frame<  firstFrameInMem)
//{
//Load(Math.Max(frame - NUMFRAMESINMEM + 1, 0));
//firstFrameInMem = Math.Max(frame - NUMFRAMESINMEM + 1, 0);
//}

//// calculate beginning offset of given frame in buffer
//int offset = (int)((frame - firstFrameInMem) * height * width * 1.5);

//Array.Copy(data, offset, framedata_lum, 0, ysize);
//Array.Copy(data, offset, dataOri_lum, 0, ysize);
//Array.Copy(data, offset + ysize, framedata_u, 0, chromasize);
//Array.Copy(data, offset + ysize + chromasize, framedata_v, 0, chromasize);
//}




        private void onNewFrame(Bitmap image) {
            framesReceived++;
            bytesReceived += image.Width * image.Height * ( Bitmap.GetPixelFormatSize( image.PixelFormat ) >> 3 );

            if ( ( !stopEvent.WaitOne( 0, false ) ) && ( NewFrame != null ) )
                NewFrame( this, new NewFrameEventArgs( image ) );
        }

int width, height, ysize, chromasize, currentFrame;
int NUMFRAMESINMEM = 150;
byte[] data, framedata_lum, framedata_u, framedata_v, dataOri_lum;
Bitmap[] bmp;

        // Constructor - init data and load first frame
public void Sequence()
{

this.ysize = width * height;
this.chromasize = (int)(ysize * 0.25);
bmp = new Bitmap[NUMFRAMESINMEM];
data = new byte[(int)(width * height * 1.5 * NUMFRAMESINMEM)];

//framedata_lum = new byte[ysize];
//framedata_u = new byte[chromasize];
//framedata_v = new byte[chromasize];
//dataOri_lum = new byte[ysize];
int framesize = (int)(width * height * 1.5);
int offset;
Load(0);
for (int z = 0; z < NUMFRAMESINMEM; z++)
{
    bmp[z] = new Bitmap(width, height);
    offset = framesize * z;
    PointF[] polygon = new PointF[5];
    polygon[0] = new PointF(100, 20);
    polygon[1] = new PointF(110, 30);
        polygon[2] = new PointF(120, 40);
        polygon[3] = new PointF(130, 50);
        polygon[4] = new PointF(100, 20);
        YCbCr tmpPx;
    for (int x = 0; x < width; x++)
    {
        for (int y = 0; y < height; y++)
        {
           // tmpPx = new YCbCr(data[y * width + x + offset],
           //    data[(y / 2) * (width / 2) + (x / 2) + ysize + offset],
           //    data[(y / 2) * (width / 2) + (x / 2) + ysize + (ysize / 4) + offset]);
           //bmp[z].SetPixel(x, y, tmpPx.ToRGB().Color);
           bmp[z].SetPixel(x, y, convertToRGB(data[y * width + x + offset],
              data[(y / 2) * (width / 2) + (x / 2) + ysize + offset],
              data[(y / 2) * (width / 2) + (x / 2) + ysize + (ysize / 4) + offset]));

            
        }
    }
    Graphics g = Graphics.FromImage(bmp[z]);
    g.DrawString("Ich bin ein schickes\n OVERLAY", new Font("Tahoma", 20), System.Drawing.Brushes.Aqua, new PointF(30, 30));
}

}

private byte clampToByte(double val)
{
    return (byte)( (val < 0)?0:((val > 255)?255:val));
}
private System.Drawing.Color convertToRGB(int y, int u, int v) {

    // ntsc standart
    //byte c = clampToByte(y - 16);
    //byte d = clampToByte(u - 128);
    //byte e = clampToByte(v - 128);

    //byte r = clampToByte((298 * c + 409 * e + 128) >> 8);
    //byte g = clampToByte((298 * c - 100*d -208*e+128) >> 8);
    //byte b = clampToByte((298 * c - 516 * d + 128) >> 8);

    byte r = clampToByte(y + 1.402 * (v - 128));
    byte g = clampToByte(y - 0.344 * (u - 128) - 0.714 * (v - 128));
    byte b = clampToByte(y + 1.772 * (u - 128));



    return System.Drawing.Color.FromArgb(r, g, b);
}
int totalFrames, firstFrameInMem;
// Loads new frames from file into memory
private bool Load(int startFrame)
{
FileStream fs;

try
{
fs = new FileStream(source, FileMode.Open);
fs.Seek( (int)(startFrame * width * height * 1.5), SeekOrigin.Begin);
fs.Read(data, 0, (int)(width * height * 1.5 * NUMFRAMESINMEM));
totalFrames = (int)(fs.Length / (width * height * 1.5));
fs.Close();
}
catch (Exception) {return false; }
firstFrameInMem = 0;
return true;
}
    }
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //DropList.Items.Add(new Contact("Max", "Musterman"));
            //DropList.Items.Add(new Contact("Hans", "Peter"));

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {



            var temp = new PlotModel("Square wave");
            var ls = new LineSeries("sin(x)+sin(3x)/3+sin(5x)/5+...");
            int n = 10;
            for (double x = -10; x < 10; x += 0.0001)
            {
                double y = 0;
                for (int i = 0; i < n; i++)
                {
                    int j = i * 2 + 1;
                    y += Math.Sin(j * x) / j;
                }
                ls.Points.Add(new DataPoint(x, y));
            }
            temp.Series.Add(ls);
            temp.Axes.Add(new LinearAxis(AxisPosition.Left, -4, 4));
            temp.Axes.Add(new LinearAxis(AxisPosition.Bottom));
            myModel.Model = temp;         // this is raising the INotifyPropertyChanged event




//            VideoFileReader reader = new VideoFileReader();
//            reader.Open("C:\\Users\\Public\\Videos\\Sample Videos\\7_cif.yuv");
//            Console.WriteLine( "width:  " + reader.Width );
//Console.WriteLine( "height: " + reader.Height );
//Console.WriteLine( "fps:    " + reader.FrameRate );
//Console.WriteLine( "codec:  " + reader.CodecName );

            if (sourcePlayer.VideoSource != null)
            {
                sourcePlayer.SignalToStop();

                // wait ~ 3 seconds
                for (int i = 0; i < 30; i++)
                {
                    if (!sourcePlayer.IsRunning)
                        break;
                    System.Threading.Thread.Sleep(100);
                }

                if (sourcePlayer.IsRunning)
                {
                    sourcePlayer.Stop();
                }

                sourcePlayer.VideoSource = null;
            }
            //FileVideoSource fileSource = new FileVideoSource(System.IO.Path.GetFullPath("C:\\Users\\Public\\Videos\\Sample Videos\\7_cif.yuv"));
            //doSomeProcessing(fileSource);
            BitMapSource bmpSource = new BitMapSource("C:\\Users\\Public\\Videos\\Sample Videos\\7_cif.yuv", 352, 288, 150);
            sourcePlayer.VideoSource = bmpSource;
            sourcePlayer.Start();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           
           

        }



        //System.Windows.Point startPoint;
        //private void List_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    startPoint = e.GetPosition(null);
        //}


        //private void List_MouseMove(object sender, MouseEventArgs e)
        //{
        //    System.Windows.Point mousePos = e.GetPosition(null);
        //    Vector diff = startPoint - mousePos;

        //    if (e.LeftButton == MouseButtonState.Pressed &&
        //        (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
        //        Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
        //    {
        //        ListView listView = sender as ListView;
        //        ListViewItem listViewItem = FindAnchestor<ListViewItem>((DependencyObject)e.OriginalSource);

        //        Contact contact = (Contact)listView.ItemContainerGenerator.ItemFromContainer(listViewItem);

        //        DataObject dragData = new DataObject("myFormat", contact);
        //        DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
        //    }
        //}

        //private static T FindAnchestor<T>(DependencyObject current)
        //    where T : DependencyObject
        //{
        //    do
        //    {
        //        if (current is T)
        //        {
        //            return (T)current;
        //        }
        //        current = VisualTreeHelper.GetParent(current);
        //    }
        //    while (current != null);
        //    return null;
        //}
        //public void DropList_DragEnter(object sender, DragEventArgs e)
        //{
        //    if (!e.Data.GetDataPresent("myFormat") ||
        //        sender == e.Source)
        //    {
        //        e.Effects = DragDropEffects.None;
        //    }
        //}
        //public void DropList_Drop(object sender, DragEventArgs e)
        //{
        //    Contact contact = e.Data.GetData("myFormat") as Contact;
        //    ListView listView = sender as ListView;
        //    listView.Items.Add(contact);
        //}

        //public class Contact
        //{
        //    public Contact(string firstName, string lastName)
        //    {
        //        FirstName = firstName;
        //        LastName = lastName;
        //    }

        //    public string FirstName { get; set; }

        //    public string LastName { get; set; }

        //    public override string ToString()
        //    {
        //        return string.Format("FirstName: {0}, LastName: {1}",FirstName, LastName);
        //    }
        //}


        private CompositionTarget
        private void loadPlugins()
        {
            
        }
    }
}
