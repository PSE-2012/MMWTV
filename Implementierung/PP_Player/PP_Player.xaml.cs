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
using System.ComponentModel.Composition;
using Oqat.PublicRessources.Plugin;
using Oqat.PublicRessources.Model;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace PP_Player
{
    /// <summary>
    /// Interaktionslogik für PP_Player.xaml
    /// </summary>
    [ExportMetadata("namePlugin", "PP_Player")]
    [ExportMetadata("type", PluginType.IPresentation)]
    [Export(typeof(IPlugin))]
    public partial class PP_Player : System.Windows.Controls.UserControl, IPresentation, ICloneable
    {

        public PP_Player()
        {
            InitializeComponent();
            pausePlayTicker.Reset();
            waitPlayTickerThreadStop.Reset();
            stopPlayTickerThread = false;
            

        }

        public object Clone()
        {
            return new PP_Player();
        }

        public PresentationPluginType presentationType
        {
            get
            {
                return PresentationPluginType.Player;
            }
        }

        public void setVideo(Oqat.PublicRessources.Model.IVideo video)
        {
            flush();
            this.video = video;
            init();
        }
        private void init()
        {
            // meddling with the GUI -> Invoke gui thread
            if (!i.Dispatcher.CheckAccess())
            {   
                i.Dispatcher.Invoke(new MethodInvoker(init));
                return;
            }

            RenderOptions.SetBitmapScalingMode(i, BitmapScalingMode.NearestNeighbor);
            RenderOptions.SetEdgeMode(i, EdgeMode.Aliased);

            writeableBitmap = new WriteableBitmap(
                video.vidInfo.width,
                video.vidInfo.height,
                96,
                96,
                PixelFormats.Bgr32,
                null);

            i.Source = writeableBitmap;
            playButton.Visibility = System.Windows.Visibility.Visible;
            pauseButton.Visibility = System.Windows.Visibility.Collapsed;


            // draw first Frame on panel
            getFrame(0);

            // prepare playTickerThread + Sync
            stopPlayTickerThread = false;
            pausePlayTicker.Reset();
            waitPlayTickerThreadStop.Set();
            playTickerThread.Start();
        }

        private void playTicker()
        {
            #region fpsCounter

            //int[] times = new int[video.vidInfo.frameCount];
            //Stopwatch sw = new Stopwatch();
            //int i = 0;

            //sw.Start();

            //sw.Stop();
            //times[i] =(int) sw.ElapsedMilliseconds;
            //sw.Reset();
            //i++;

            //int sum = 0;
            //for(int n = 0; n < times.Count();n++) {
            //    sum += times[n];
            //}
            //double fps = (i /( sum/1000.0));
            #endregion

            video.handler.positionReader = 0;
            while ((video.handler.positionReader < video.vidInfo.frameCount) && !stopPlayTickerThread)
            {
                // terminate current thread

                pausePlayTicker.WaitOne();
                Thread.Sleep(playTickerTimeout);
                getFrame();
            }
            waitPlayTickerThreadStop.Set();
        }

        private  ManualResetEvent pausePlayTicker
        {
            get
            {
                if (_pausePlayTicker == null)
                    _pausePlayTicker = new ManualResetEvent(false);
                return _pausePlayTicker;
            }
        }
        private ManualResetEvent waitPlayTickerThreadStop
        {
            get
            {
                if (_waitPlayTickerThreadStop == null)
                    _waitPlayTickerThreadStop = new ManualResetEvent(false);
                return _waitPlayTickerThreadStop;
            }
        }

        private Thread playTickerThread
        {
            get
            {
                if ((_playTickerThread == null) ||
                    (_playTickerThread.ThreadState == System.Threading.ThreadState.Stopped) ||
                    (_playTickerThread.ThreadState == System.Threading.ThreadState.Aborted))
                {
                    _playTickerThread = new Thread(new ThreadStart(playTicker));
                    _playTickerThread.Name = "playTicker";
                }


                return _playTickerThread;
            }
            set
            {
                _playTickerThread = value;
            }
        }
                //    pausePlayTicker = new ManualResetEvent(false);
           // waitPlayTickerThreadStop = new ManualResetEvent(false);
           // playTickerThread = new Thread(new ThreadStart(playTicker));

        Thread _playTickerThread;
        IVideo video;
        WriteableBitmap writeableBitmap;
        bool stopPlayTickerThread = false;
        ManualResetEvent _waitPlayTickerThreadStop;
        int playTickerTimeout = 50;
        ManualResetEvent _pausePlayTicker;
        Bitmap bmpHand;
        BitmapData bmpData;

        void getFrame(int position = -1)
        {
            if (!(position < 0))
                video.handler.positionReader = position;
            /// else use sequential access

            // obviously
            bmpHand = video.handler.getFrame();
            if (bmpHand == null)
            {
                // reached the end not as planned, but
                // at least it is stopped now.
                setVideo(this.video);
                return;
            }
           
            /// it is not possible (ok, I dont know how to do it ;-)
            /// to come along without this.
            /// The problem occurs if for some reason
            /// the handler provides corrupt frames.
            /// Such can happen if it was flushed before 
            /// it has completed a frame.
            /// There is no easy way to check this as
            /// the handler works directly on the bitmap
            /// data (unsafe mode).
            #region tryAndCat zone..
            //lock data for exclusive -> fast :D access
            try
            {
                bmpData = bmpHand.LockBits(new System.Drawing.Rectangle(0, 0, bmpHand.Width, bmpHand.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                //use dispatcher as "writeToDisplay" need access to backbuffer of our writeableBitmap
                //wich is set (init) as source of i (image control we use as player, defined in xaml)
                i.Dispatcher.Invoke(new MethodInvoker(writeToDisplay));

                // dispose
                bmpHand.UnlockBits(bmpData);
                bmpHand.Dispose();
            }
            catch (ArgumentException)
            {
                // nothing to handle, the user shouldnt notice one missing frame..
            }
            #endregion
        }

        void writeToDisplay()
        {
            writeableBitmap.Lock();

            // magic -> do not touch ;-)
            try
            {
                CopyMemory(writeableBitmap.BackBuffer,
                        bmpData.Scan0,
                        (System.Int32)writeableBitmap.Height * writeableBitmap.BackBufferStride);
            } catch (AccessViolationException) {
                // cant handle it, see comment within getFrame method
            } finally {

            // Specify the area of the bitmap that changed.
            writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, bmpData.Width, bmpData.Height));
            // Release the back buffer and make it available for display.
            writeableBitmap.Unlock();
            }
        }
        // import native function (unmanaged)
        [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory")]
        public static extern void CopyMemory(IntPtr Destination, IntPtr Source, System.Int32 Length);

        public void flush()
        {

            stopPlayTickerThread = true;
            pausePlayTicker.Set();
            Thread.Sleep(playTickerTimeout);
            if (playTickerThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin)
            {
                try
                {
                    playTickerThread.Join(playTickerTimeout);
                }
                catch (ThreadStateException)
                {
                    // looks like he finished before we could ask, good for him.
                }
            }
            if (playTickerThread.ThreadState != System.Threading.ThreadState.Stopped
               && playTickerThread.ThreadState != System.Threading.ThreadState.StopRequested
               && playTickerThread.ThreadState != System.Threading.ThreadState.Unstarted
               && playTickerThread.ThreadState != System.Threading.ThreadState.Aborted
               && playTickerThread.ThreadState != System.Threading.ThreadState.AbortRequested)
            {
                try
                {
                    playTickerThread.Abort();
                }
                catch (ThreadStateException)
                {
                    // lets hope he cleaned up ;-)
                }
            }
            #region obsolete
            //if(playTickerThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin) {
            //     playTickerThread.Join(playTickerTimeout);
            //     if (playTickerThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin)
            //     {
            //         try
            //         {
            //             playTickerThread.Abort();
            //         }
            //         catch (ThreadStateException)
            //         {
            //             // cant handle this one, let hope all resources were cleaned up..
            //         }
            //     }
            //} else if (playTickerThread.ThreadState == System.Threading.ThreadState.Running)
            //{
            //    // allow  to go on if blocked
            //    pausePlayTicker.Set();
            //}
            //if ((playTickerThread.ThreadState == System.Threading.ThreadState.Running)
            //    || (playTickerThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin))
            //{
            //    // let know to terminate himself
                
            //    waitPlayTickerThreadStop.Reset();
            //    // allow to go on if blocked
            //    pausePlayTicker.Set();
            //    waitPlayTickerThreadStop.WaitOne(playTickerTimeout);
            //    #region obsolete
            //    //try
            //    //{
            //    //    playTickerThread.Join(1000);
            //    //    if (playTickerThread.ThreadState == System.Threading.ThreadState.Running)
            //    //    {
            //    //        playTickerThread.Abort();
            //    //    }
            //    //}
            //    //catch (ThreadStateException)
            //    //{
            //    //    //should never come to this..
            //    //}
            //    //finally
            //    //{
            //    #endregion
                
            //    pausePlayTicker.Reset();
            //    playTickerThread = null;
            //    //}

            //}
            #endregion

           
            playTickerThread = null;
            stopPlayTickerThread = false;
            pausePlayTicker.Reset();
            this.i.Source = null;
            this.video = null;
        }

        public System.Windows.Controls.UserControl propertyView
        {
            get { 
                return this; 
                }
        }

        public Oqat.PublicRessources.Model.Memento getMemento()
        {
            return new Memento("dd", null, "dd");
        }

        public void setMemento(Oqat.PublicRessources.Model.Memento memento)
        {
            
        }

        public string namePlugin
        {
            get { return "PP_Player"; }
        }

        public PluginType type
        {
            get { return PluginType.IPresentation; }
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            pausePlayTicker.Reset();
            pauseButton.Visibility = System.Windows.Visibility.Collapsed;
            playButton.Visibility = System.Windows.Visibility.Visible;
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            pausePlayTicker.Set(); 
            playButton.Visibility = System.Windows.Visibility.Collapsed;
            pauseButton.Visibility = System.Windows.Visibility.Visible;
            
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
         //   Pause_Click(this, null);
            setVideo(this.video);
            //// some call it cheating, but in fact this is redundancy avoidance ;-)
            //getFrame(0);
        }

        private void previousFrame_Click(object sender, RoutedEventArgs e)
        {

        }

        private void nextFrame_Click(object sender, RoutedEventArgs e)
        {

        }

        private void return_Click(object sender, System.Windows.Input.KeyEventArgs e)
        {

        }

        private void jumpToFrame_Click(object sender, RoutedEventArgs e)
        {

        }

}
}
