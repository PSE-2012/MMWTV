//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace PP_Presentation
{
	using AForge.Video;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
    using System.Drawing;
    using System.Threading;

    /// <summary>
    /// 
    /// </summary>
	public class VideoSource : IVideoSource

	{

        private String source;
        private long bytesReceived;
        private int framesReceived;
        private Thread thread = null;
        private ManualResetEvent stopEvent = null;
        private ManualResetEvent suspendEvent = null;

        private int _NUMFRAMESINMEM;
        private Bitmap[] _bmp;

		/// <summary>
		/// 
		/// </summary>
        public virtual void SignalToStop()
		{
            if (thread != null)
            {
                // signal to stop
                stopEvent.Set();
            }
		}

		/// <summary>
		/// 
		/// </summary>
        public virtual void Start()
		{
            if (!IsRunning)
            {
                // check source
                //if ((source == null) || (source == string.Empty))
                  //  throw new ArgumentException("Video source is not specified.");

                framesReceived = 0;
                bytesReceived = 0;

                // create events
                stopEvent = new ManualResetEvent(false);
                suspendEvent = new ManualResetEvent(true);

                // create and start new thread
                thread = new Thread(new ThreadStart(WorkerThread));
                // thread.Name = source; // mainly for debugging
                thread.Start();
            }
		}

		/// <summary>
		/// 
		/// </summary>
        public virtual void Stop()
		{
            if (this.IsRunning)
            {
                stopEvent.Set();
                suspendEvent.Set();
                PlayingFinished(this, ReasonToFinishPlaying.StoppedByUser);
                thread.Abort();
                WaitForStop();
            }
		}

        /// <summary>
        /// 
        /// </summary>
        public virtual void Pause()
        {
            if (this.IsRunning)
            {
            suspendEvent.Reset();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Resume()
        {
            if (this.IsRunning)
            {
            suspendEvent.Set();
            }
        }

		/// <summary>
		/// 
		/// </summary>
        public virtual void WaitForStop()
		{
            if (thread != null)
            {
                // wait for thread stop
                thread.Join();
                Free();
            }
		}


        /// <summary>
        /// 
        /// </summary>
        public long BytesReceived
        {
            get
            {
                long bytes = bytesReceived;
                bytesReceived = 0;
                return bytes;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int FramesReceived
        {
            get
            {
                int frames = framesReceived;
                framesReceived = 0;
                return frames;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsRunning
        {
            get
            {
                if (thread != null)
                {
                    // check thread status
                    if (thread.Join(0) == false)
                        return true;

                    // the thread is not running, free resources
                    Free();
                }
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event NewFrameEventHandler NewFrame;

        /// <summary>
        /// 
        /// </summary>
        public event PlayingFinishedEventHandler PlayingFinished;

        /// <summary>
        /// 
        /// </summary>
        public event VideoSourceErrorEventHandler VideoSourceError;

        /// <summary>
        /// 
        /// </summary>
        public string Source
        {
            get { return source; }
            set { source = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int NUMFRAMESINMEM
        {
            get
            {
                return _NUMFRAMESINMEM;
            }
            set
            {
                _NUMFRAMESINMEM = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Bitmap[] bmp
        {
            get
            {
                return _bmp;
            }
            set
            {
                _bmp = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Free()
        {
            thread = null;

            // release events
            stopEvent.Close();
            stopEvent = null;
        }

        /// <summary>
        /// 
        /// </summary>
        private void WorkerThread()
        {
            for (int z = 0; z < NUMFRAMESINMEM; z++)
            {
                    System.Threading.Thread.Sleep(50);
                    onNewFrame(bmp[z]);
                    Graphics g = Graphics.FromImage(bmp[z]);
                    g.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        private void onNewFrame(Bitmap image)
        {
            suspendEvent.WaitOne(Timeout.Infinite);
            framesReceived++;
            bytesReceived += image.Width * image.Height * (Bitmap.GetPixelFormatSize(image.PixelFormat) >> 3);
           if ((!stopEvent.WaitOne(0, false)) && (NewFrame != null))
             NewFrame(this, new NewFrameEventArgs(image));
        }
    }
}

