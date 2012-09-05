using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ComponentModel.Composition;
using Oqat.PublicRessources.Plugin;
using Oqat.PublicRessources.Model;
using System.Threading;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;



namespace PP_Player
{
    /// <summary>
    /// Interaktionslogik für PP_Player.xaml
    /// </summary>
    [ExportMetadata("namePlugin", "PP_Player")]
    [ExportMetadata("type", PluginType.IPresentation)]
     [ExportMetadata("threadSafe", false)]
    [Export(typeof(IPlugin))]
    public partial class Player : System.Windows.Controls.UserControl, IPresentation, INotifyPropertyChanged
    {

        public Player()
        {
            InitializeComponent();

            //init
            pausePlayTicker.Reset();
            //waitPlayTickerThreadStop.Reset();
            stopPlayTickerThread = false;
            setVideoContextLock = new Object();

        }

        public PresentationPluginType presentationType
        {
            get
            {
                return PresentationPluginType.Player;
            }
        }

        private void setVideo(object video)
        {
            setVideo(video as IVideo, 0);
        }

        public void setVideo(Oqat.PublicRessources.Model.IVideo video, int position = 0)
        {

            lock (setVideoContextLock)
            {
                if (video == null || video.vidInfo == null || video.vidPath == null)
                    throw new ArgumentException("Given video was not initialized properly.");
                flush();

                this.video = video;
                //// meddling with the GUI -> Invoke gui thread
                // if (!i.Dispatcher.CheckAccess())
                // {   
                //     i.Dispatcher.Invoke(new MethodInvoker(init));
                //     return;
                // }



                RenderOptions.SetBitmapScalingMode(i, BitmapScalingMode.NearestNeighbor);
                RenderOptions.SetEdgeMode(i, EdgeMode.Aliased);


                // image
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




                positionSlider.Maximum = video.vidInfo.frameCount;
                playerControls.DataContext = this;

                Binding jtfReadPosbind = new Binding("positionReader");
                jtfReadPosbind.Mode = System.Windows.Data.BindingMode.OneWay;
                jtfReadPosbind.Converter = new intStringConverter();
                jumpToFrameTextBox.SetBinding(System.Windows.Controls.TextBox.TextProperty, jtfReadPosbind);


                Binding slReadPosBind = new Binding("positionReader");
                slReadPosBind.Mode = System.Windows.Data.BindingMode.OneWay;
                slReadPosBind.Converter = new intDoubleConverter();
                positionSlider.SetBinding(System.Windows.Controls.Slider.ValueProperty, slReadPosBind);

                video.handler.PropertyChanged += OnPropertyChanged;


                // draw Frame on panel
                getFrame(position);

                // prepare playTickerThread + Sync
                stopPlayTickerThread = false;
                pausePlayTicker.Reset();
                //    waitPlayTickerThreadStop.Set();
                if (playTickerThread.ThreadState != System.Threading.ThreadState.Running)
                    playTickerThread.Start();
            }
        }

        public void flush()
        {
            stopPlayTickerThread = true;
            pausePlayTicker.Set();      // allow to go one if blocked so playTicker can stop
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
            if (playTickerThread.ThreadState == System.Threading.ThreadState.Running
                || playTickerThread.ThreadState == ThreadState.WaitSleepJoin)
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
            playTickerThread = null;
            stopPlayTickerThread = false;
            pausePlayTicker.Reset();
            this.i.Source = null;
            //this.video = null;
        }


        /// <summary>
        /// 
        /// </summary>
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
            System.Diagnostics.Stopwatch fpsTimer = new System.Diagnostics.Stopwatch();
            while ((video.handler.positionReader < video.vidInfo.frameCount) && !stopPlayTickerThread)
            {
                pausePlayTicker.WaitOne();
                fpsTimer.Start();
                Thread.Sleep(playTickerTimeout);
                getFrame();
                fpsTimer.Stop();
                fpsIndicatorValue = 1000 / (int)((fpsTimer.ElapsedMilliseconds > 0) ? fpsTimer.ElapsedMilliseconds : 1);
                fpsTimer.Reset();
            }
            //    waitPlayTickerThreadStop.Set();
        }

        private ManualResetEvent pausePlayTicker
        {
            get
            {
                if (_pausePlayTicker == null)
                    _pausePlayTicker = new ManualResetEvent(false);
                return _pausePlayTicker;
            }
        }

        //private ManualResetEvent waitPlayTickerThreadStop
        //{
        //    get
        //    {
        //        if (_waitPlayTickerThreadStop == null)
        //            _waitPlayTickerThreadStop = new ManualResetEvent(false);
        //        return _waitPlayTickerThreadStop;
        //    }
        //}

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

        Thread _playTickerThread;
        IVideo _video;
        IVideo video
        {
            get
            {
                return _video;
            }
            set
            {
                _video = value;

                // enable/disable controls
                if (_video == null)
                {
                    this.playerControls.IsEnabled = false;
                }
                else
                {
                    this.playerControls.IsEnabled = true;
                }
            }
        }
        WriteableBitmap writeableBitmap;
        bool stopPlayTickerThread = false;
        //   ManualResetEvent _waitPlayTickerThreadStop;
        int _playTickerTimeout = 1000 / 25;
        int playTickerTimeout
        {
            get
            {
                return _playTickerTimeout;
            }
            set
            {
                _playTickerTimeout = (value < 0) ? 10 : ((value > 800) ? Timeout.Infinite : value);
                //refresh fpsIndicator
                fpsUpdateBlocker = 0;
            }
        }
        ManualResetEvent _pausePlayTicker;
        Bitmap bmpHand;
        BitmapData bmpData;
        //   private object getFrameLock;
        private object setVideoContextLock;
        private int _positionReader;

        private int fpsUpdateBlocker = 10;
        private int _fpsIndicatorValue;
        public int fpsIndicatorValue
        {
            get
            {
                return _fpsIndicatorValue;
            }
            private set
            {
                if (fpsUpdateBlocker-- < 0)
                {
                    fpsUpdateBlocker = _fpsIndicatorValue / 2;
                    _fpsIndicatorValue = value;
                    OnPropertyChanged(this, new PropertyChangedEventArgs(fpsIndProName));
                }
            }
        }

        private readonly string randomJumpPositionUpdate = "rjpu";
        private readonly string nextFramePositionUpdate = "nfpu";
        private readonly string posReadProName = "positionReader";
        private readonly string fpsIndProName = "fpsIndicatorValue";

        public int positionReader
        {
            get
            {
                return _positionReader;
            }
            set
            {
                if (value != _positionReader)
                {
                    _positionReader = value;
                    OnPropertyChanged(null, new PropertyChangedEventArgs(posReadProName));
                }

            }
        }

        void getFrame(int position = -1)
        {
            if (!(position < 0))
            {
                video.handler.positionReader = position;
                bmpHand = video.handler.getFrame(false);
            }
            else
            {
                /// else use sequential access

                // obviously
                bmpHand = video.handler.getFrame();
            }
            if (bmpHand == null)
            {
                // reached the end not as planned, but
                // at least it is stopped now.
                (new Thread(new ParameterizedThreadStart(setVideo))).Start(this.video);
                stopPlayTickerThread = true;
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
            #region tryAndCatch zone..
            //lock data for exclusive -> fast :D access
            try
            {
                bmpData = bmpHand.LockBits(new System.Drawing.Rectangle(0, 0, bmpHand.Width, bmpHand.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                //use dispatcher as "writeToDisplay" need access to backbuffer of our writeableBitmap
                //wich is set (init) as source of i (image control we use as player, defined in xaml)
                i.Dispatcher.Invoke(new System.Windows.Forms.MethodInvoker(writeToDisplay));

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
            }
            catch (AccessViolationException)
            {
                // cant handle it, see comment within getFrame method
            }
            finally
            {

                // Specify the area of the bitmap that changed.
                writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, bmpData.Width, bmpData.Height));
                // Release the back buffer and make it available for display.
                writeableBitmap.Unlock();
            }
        }

        // import native function (unmanaged)
        [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory")]
        public static extern void CopyMemory(IntPtr Destination, IntPtr Source, System.Int32 Length);


        #region obsolete
        //public void flush()
        //{

        //    stopPlayTickerThread = true;
        //    pausePlayTicker.Set();
        //    Thread.Sleep(playTickerTimeout);
        //    if (playTickerThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin)
        //    {
        //        try
        //        {
        //            playTickerThread.Join(playTickerTimeout);
        //        }
        //        catch (ThreadStateException)
        //        {
        //            // looks like he finished before we could ask, good for him.
        //        }
        //    }
        //    if (playTickerThread.ThreadState != System.Threading.ThreadState.Stopped
        //       && playTickerThread.ThreadState != System.Threading.ThreadState.StopRequested
        //       && playTickerThread.ThreadState != System.Threading.ThreadState.Unstarted
        //       && playTickerThread.ThreadState != System.Threading.ThreadState.Aborted
        //       && playTickerThread.ThreadState != System.Threading.ThreadState.AbortRequested)
        //    {
        //        try
        //        {
        //            playTickerThread.Abort();
        //        }
        //        catch (ThreadStateException)
        //        {
        //            // lets hope he cleaned up ;-)
        //        }
        //    }

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

        //    playTickerThread = null;
        //    stopPlayTickerThread = false;
        //    pausePlayTicker.Reset();
        //    this.i.Source = null;
        //    this.video = null;
        //}
        #endregion



        public System.Windows.Controls.UserControl propertyView
        {
            get
            {
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
            setVideo(this.video, this._positionReader);
            pausePlayTicker.Set();
            playButton.Visibility = System.Windows.Visibility.Collapsed;
            pauseButton.Visibility = System.Windows.Visibility.Visible;

        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            setVideo(this.video, 0);
        }

        private void previousFrame_Click(object sender, RoutedEventArgs e)
        {
            OnPropertyChanged(null, new PropertyChangedEventArgs(randomJumpPositionUpdate));
        }

        private void nextFrame_Click(object sender, RoutedEventArgs e)
        {
            OnPropertyChanged(null, new PropertyChangedEventArgs(nextFramePositionUpdate));
        }

        private void return_Click(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                e.Handled = true;
                jumpToFrame_Click(this, new RoutedEventArgs());
            }
        }

        private void jumpToFrame_Click(object sender, RoutedEventArgs e)
        {
            OnPropertyChanged(this, new PropertyChangedEventArgs(randomJumpPositionUpdate));
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            if (e.PropertyName.Equals(fpsIndProName))
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, e);
            }
            else

                if (Monitor.TryEnter(setVideoContextLock))
                {
                    bool wasPlaying = false;
                    if (pauseButton.IsVisible)
                    {
                        Pause_Click(null, null);
                        wasPlaying = true;
                    }

                    if (e.PropertyName.Equals(randomJumpPositionUpdate)) // rjpu: randomJumpePositionUpdate
                    {




                        if ((Math.Abs((int)positionSlider.Value - _positionReader)) < 1)
                        {
                            int jumpTo;
                            Int32.TryParse(jumpToFrameTextBox.Text, out jumpTo);
                            jumpTo -= 2;
                            if ((Math.Abs(jumpTo - _positionReader) > 0))
                            {
                                wasPlaying = false;
                                setVideo(this.video, jumpTo);
                            }

                        }
                        else
                        {
                            setVideo(this.video, (int)positionSlider.Value);

                        }

                        if (wasPlaying)
                            Play_Click(null, null);

                    }
                    else if (e.PropertyName.Equals(nextFramePositionUpdate))// nextFrame jump 
                    {
                        if ((playTickerThread.ThreadState == System.Threading.ThreadState.Running)
                            || pauseButton.IsVisible)
                            Pause_Click(this, null);

                        // controlled racecondition xD
                        pausePlayTicker.Set();
                        Thread.Sleep(5);
                        pausePlayTicker.Reset();

                    }
                }
            if (sender is IVideoHandler &&
                    e.PropertyName.Equals(posReadProName) &&
                    (PropertyChanged != null))
            {
                if (_positionReader != video.handler.positionReader)
                {
                    _positionReader = video.handler.positionReader;
                    PropertyChanged(this, new PropertyChangedEventArgs(e.PropertyName));
                }
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        //private bool dragStarted = false;
        private void positionSlider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            if (pauseButton.IsVisible)
                Pause_Click(this, new RoutedEventArgs());
        }

        private void positionSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            //this.dragStarted = false;
            OnPropertyChanged(this, new PropertyChangedEventArgs(randomJumpPositionUpdate));
        }

        private void positionSlider_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (e.HorizontalChange > 1)
                OnPropertyChanged(this, new PropertyChangedEventArgs(posReadProName));

        }

        private void jumpToFrameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (pauseButton.IsVisible)
                Pause_Click(this, new RoutedEventArgs());
        }

        private void slowDownButton_Click(object sender, RoutedEventArgs e)
        {
            playTickerTimeout += 10;

        }

        private void speedUpButton_Click(object sender, RoutedEventArgs e)
        {
            playTickerTimeout -= 10;
        }

        public IPlugin createExtraPluginInstance()
        {
            return new Player();
        }


       public bool threadSafe
        {
            get { return false; }
        }
    }
    internal class intStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            string pos = (System.Convert.ToInt32(value)).ToString();
            return pos;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int positionReader;
            Int32.TryParse(value as string, out positionReader);
            return positionReader;
        }
    }

    internal class intDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double conv = System.Convert.ToDouble(value);
            return conv;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int conv = System.Convert.ToInt32(value);
            return conv;
        }
    }
}
