//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace PS_YuvVideoHandler
{
    using Oqat.PublicRessources.Model;
    using Oqat.PublicRessources.Plugin;
    using System;


    using System.ComponentModel.Composition;

    using System.Drawing.Imaging;
    using System.Drawing;
    using System.IO;
    using System.Windows.Controls;

    using System.Threading;
    using System.Collections;
    using System.ComponentModel;

    [ExportMetadata("namePlugin", "yuvVideoHandler")]
    [ExportMetadata("type", PluginType.IVideoHandler)]
    [Export(typeof(IPlugin))]
    public class YuvVideoHandler : IVideoHandler
    {

        #region general

        #region get/set
        /// <summary>
        /// Name the plugin exposes to the PluginManager.
        /// </summary>
        /// <remarks>
        /// The name of a videohandler is partially based on a naming 
        /// convention. In order for the PluginManager to find the corresponding
        /// handler for a given video format the prefix of the handler
        /// has to be the corresponding video format name. The
        /// prefix is "yuv" in this case.
        /// </remarks>
        public string namePlugin
        {
            get { return "yuvVideoHandler"; }
        }

        /// <summary>
        /// Type this plugin exposes to the PluginManager
        /// </summary>
        public PluginType type
        {
            get
            {
                return PluginType.IVideoHandler;
            }
        }

        /// <summary>
        /// See <see cref="YuvVideoInfo"/> for info.
        /// </summary>
        /// <remarks>
        /// This value has to be set (and consistent)
        /// in order to perform write/reads. If not
        /// set correctly the behaviour is undefined.
        /// If applicable check the consistancy
        /// and set the <see cref="consistent"/> flag
        /// accordingly.
        /// </remarks>
        private YuvVideoInfo _readVideoInfo;

        /// <summary>
        /// Absolute path to this video.
        /// </summary>
        private string readPath;



        int _frameByteSize = -1;
        /// <summary>
        /// This value is the byte wise frame size of the currently 
        /// set video context. It is computed on the first read
        /// of this property and will remain (event if 
        /// not latest) so. To achieve reinitialisation set
        /// this property to null and it will be reinitilized
        /// on the next read call.
        /// </summary>
        public int frameByteSize
        {
            get
            {
                if ((consistent) && !(_frameByteSize > 0))
                {
                    _frameByteSize = (int)(readVidInfo.width * readVidInfo.height * (1 + 2 * getLum2Chrom(_readVideoInfo.yuvFormat)));

                }

                return _frameByteSize;
            }
            set
            {
                _frameByteSize = value;
            }
        }

        /// <summary>
        /// Determines how many (.ca) frames will be read into Memory and how
        /// many frames has to be within the buffer to start writing to hard disk respectively.
        /// Do not set this value directly, it will be set according to the
        /// frameByteSize and the grantedMemory
        /// </summary>
        int NUMFRAMESINMEM = 1;

        int _grantedMemory = -1;

        /// <summary>
        /// A rough indicator on how much memory (in MB) this handler should use
        /// for writing/reading videos. 
        /// </summary>
        /// <remarks>
        /// This value has to be set by the user, however it will be checked on
        /// plausibility and adapted accordnigly.
        /// </remarks>
        int grantedMemory
        {
            get
            {
                if (_grantedMemory < 0)
                    _grantedMemory = frameByteSize * 5;

                return _grantedMemory;
            }
            set
            {
                _grantedMemory = value * (int)(Math.Pow(2.0, 20.0));
            }
        }

        /// <summary>Gets the current VideoInfo object, if a propertiesView is displayed through 
        /// "setParentControl()" the values are updated to the users settings.</summary>
        /// <returns>the current YuvVideoInfo instance of the handled video.</returns>
        public IVideoInfo readVidInfo
        {
            get
            {
                if (_readVideoInfo == null)
                    _readVideoInfo = new YuvVideoInfo(this.readPath);
                return _readVideoInfo;
            }
            private set
            {
                if (value != null)
                {
                    _readVideoInfo = (YuvVideoInfo)value;
                }
            }
        }

        public bool consistent
        {
            get
            {
                return (readVidInfo.frameCount < 0) ? false : true;
            }
        }



        private PropertiesView _propertyView;
        /// <summary>
        /// Contains the propertyView UserControl for this VideoHandler.
        /// </summary>
        public UserControl propertyView
        {
            get
            {
                if (_propertyView == null)
                {
                    _propertyView = new PropertiesView();
                    _propertyView.DataContext = readVidInfo;
                   
                }
                return _propertyView;
            }
        }

        private ReadOnlyPropertiesView _readOnlyInfoView;
        public UserControl readOnlyInfoView
        {
            get
            {
                if (_readOnlyInfoView == null)
                {
                    _readOnlyInfoView = new ReadOnlyPropertiesView(readVidInfo as YuvVideoInfo);
                }
                return _readOnlyInfoView;
            }
        }

        #endregion

        public YuvVideoHandler()
        {
            this.positionReaderLock = new Object();
        }

        /// <summary>
        /// Returns a new VideoHandler instance.
        /// </summary>
        /// <returns>
        /// Note that the returned handler is not initialized
        /// and is not tracked by the pluginManager, i.e.
        /// you have to dispose it yourself.
        /// </returns>
        public IVideoHandler createVideoHandlerInstance()
        {
            return new YuvVideoHandler();
        }

        public void setWriteContext(string filepath, IVideoInfo info)
        {
            // check if values ok (more or less)
            if (info == null)
                throw new ArgumentException("Problems occured by switching context to given video." +
                    "There could be different causes to this, the first is that the given path does " +
                    "not describe a valid yuv video file and the second if the given IVideoInfo object " +
                    "is not initialized properly.");

            this.writePath = filepath;
            writeVidInfo = info;

            if(consistent)
            // Flush has to happen after a VALID vidInfo is set.
            flushReader();
        }

        /// <summary>
        /// Sets a new video file as the context of this handler.
        /// </summary>
        /// <param name="filepath">Full path of the video file to initialize this
        /// handler to.</param>
        /// <param name="info">VideoInfo containing required informations like resolution and yuv format</param>
        public void setReadContext(string filepath, IVideoInfo info)
        {

            // check if values ok (more or less)
            if (!File.Exists(filepath) || info == null)
                throw new ArgumentException("Problems occured by switching context to given video." + 
                    "There could be different causes to this, the first is that the given path does " + 
                    "not describe a valid yuv video file and the second if the given IVideoInfo object " + 
                    "is not initialized properly.");
            this.readPath = filepath;
            readVidInfo = info;


            if (consistent)
                // Flush has to happen after a VALID vidInfo is set.
                flushWriter();

        }


        /// <summary>
        /// 
        /// Use this method with caution, it is intended for a video import procedure only and does
        /// not imply that the read or write context is set correctly.
        /// At video import time, there are usually not enough data available to provide a fully fledged
        /// video info object. Therefore you can set the context with this method, the only thing
        /// you have to ensure is that the propertyView is displayed to the user. After the user is
        /// done filling out the vidImport form you should check the consistancy flag to make sure
        /// all input was legal, however the consistancy flag CANNOT ensure the consistancy to 100 % so
        /// be prepared ;-).
        /// </summary>
        /// <param name="filepath"></param>
        public void setImportContext(string filepath)
        {
            // needs to be preset if calling setReadContext without
            // a valid info object so one can be constructed.
            this.readPath = filepath;
            this.setReadContext(filepath, readVidInfo);
        }


        /// <summary>
        /// Returns the relative number of chroma (u or v) to luma (y) samples according to yuv format.
        /// </summary>
        /// <param name="format">the yuv format</param>
        /// <returns>relative number of samples (1, 0.5 or 0.25)</returns>
        public static float getLum2Chrom(YuvFormat format)
        {
            switch (format)
            {
                case YuvFormat.YUV444:
                    return 1.0f;
                case YuvFormat.YUV422_UYVY:
                    return 0.5f;
                case YuvFormat.YUV411_Y41P:
                    return 0.25f;
                case YuvFormat.YUV420_IYUV:
                    return 0.25f;
                default:
                    throw new ArgumentException("Invalid YuvFormat set in VideoInfo.");
            }
        }


        /// <summary>
        /// Getter for properties to save.
        /// </summary>
        /// <returns>Properties to save</returns>
        /// <remarks>
        /// This is only a stub as VideoHandlers are not needed to keep their settings. 
        /// (Settings are rather saved with the Video object and its VideoInfo)
        /// </remarks>
        public Memento getMemento()
        {
            return new Memento("MementoStub", null);
        }
        /// <summary>
        /// Setter for properties (recoverd from disk by the Caretaker, passed by the PluginManager)
        /// </summary>
        /// <param name="memento">Properties to set.</param>
        /// <remarks>
        /// This is only a stub as VideoHandlers are not needed to keep their settings. 
        /// (Settings are rather saved with the Video object and its VideoInfo)
        /// </remarks>
        public void setMemento(Memento memento)
        {

        }

        /// <summary>
        /// cuts val down to a value between 0 and 255
        /// </summary>
        private byte clampToByte(double val)
        {
            return (byte)((val < 0) ? 0 : ((val > 255) ? 255 : val));
        }

#endregion


        #region read

        #region get/set
        private Queue _readQueue;
        /// <summary>
        /// This queue holds bitmaps produced by the "fillBuffer()" method.
        /// All bitmaps within this queue represent one frame of
        /// a corresponding yuvVideo file. It is assured that
        /// this bimpats are ordered(i.e. first frame comes bevor second and so on).
        /// </summary>
        /// <remarks>
        /// This queue is threadsafe, you do not have to lock it before
        /// concurrent access. If you lock it your thread would fall
        /// to a DeadLock state. Furthermore you shoulnt assign
        /// this property directly (by assigning to _readQueue for example)
        /// as it would break the threadsafety and result in a DeadLock
        /// if you are lucky.
        /// </remarks>
        private Queue readQueue
        {
            get
            {
                if (_readQueue == null)
                {
                    _readQueue = new Queue(NUMFRAMESINMEM);
                    _readQueue = Queue.Synchronized(_readQueue);
                }
                return _readQueue;
            }
        }


        private int _readerBuffPos = -1;
        /// <summary>
        /// This variable marks the number (whithin the
        /// yuvVideo accordingly to the <see cref="YuvVideoInfo"/> object)
        /// last frame which was successfully loaded into the
        /// readQueue. 
        /// </summary>
        /// <remarks>
        /// Do not meddle with this property as it is the responsibility
        /// of the currently active readerThread.
        /// </remarks>
        private int readerBuffPos
        {
            get
            {
                return _readerBuffPos;
            }
            set
            {
                if (value > readVidInfo.frameCount)
                {
                    #region obsolete
                    //////////////////////////////////////////////////////////////////
                    //// reached end of video, the flushProcedure will
                    //// clean up. It has to be invoked in a different
                    //// thread because it will stop the readerThread
                    //// which is usually (and should so) the caller
                    //// of this property
                    //waitForFlush.Reset();
                    //(new Thread(new ThreadStart(flushReader))).Start();
                    //waitForFlush.WaitOne();
                    ///////////////////////////////////////////////////////////////////
                    //
                    #endregion

                    // Let reader know to quit filling the buffer.
                    stopReaderThread = true;
                }
                else
                {
                    _readerBuffPos = value;
                }
            }
        }

        /// <summary>
        /// This flag signals the currently active readerThread(if active)
        /// to terminate itself.
        /// </summary>
        /// <remarks>
        /// You shouldnt call abort (as described in the corresponding doc entry)
        /// as the readerThread acquires some locks( i.e. on a file and a the
        /// <see cref="BitmapData"/> of a <see cref="Bimpap"/>) and couldnt
        /// release this if you kill it externally.
        /// </remarks>
        bool stopReaderThread = false;


        object positionReaderLock;
        private int _positionReader = -1;
        /// <summary>
        /// This property holds the number of the frame to be given on
        /// the nex getFrame() call. If it is set to a number
        /// less null or greater then vidInfo.frameCount a flush
        /// process will be initialized (all reader buffers and
        /// constants will be reseted).
        /// </summary>
        public int positionReader
        {
            get
            {
                if (_positionReader < 0)
                {
                    //            flushReader();
                    _positionReader = 0;
                }
                //if (readerThread.ThreadState == System.Threading.ThreadState.Unstarted)
                //    readerThread.Start();

                return _positionReader;
            }
           
            set
            {

                lock (positionReaderLock)
                {
                    // make sure not to call flushReader if a flush operation is already running
                    // this is assured with the tryEnter monitor operation.
                    if ((value != 1 + _positionReader) || (value < 0))
                    {

                        // once again, flush is invoked within a different Thread
                        // because the caller could be killed accidently as
                        // flush tries to clean up thoroughly
                        waitForFlush.Reset();
                        Thread flushThread = new Thread(new ThreadStart(flushReader));
                        flushThread.Name = "flushThread";
                        flushThread.IsBackground = true;
                        flushThread.Start();
                        waitForFlush.WaitOne();

                    }
                    if (value > readVidInfo.frameCount)
                    {
                        throw new ArgumentOutOfRangeException("Trying to fetch" +
                        " a non existing frame.");
                    }
                }
                _positionReader = value;
                OnPropertyChanged("positionReader");
            }
        }


#endregion

        private ManualResetEvent _waitForFlush;
        private ManualResetEvent waitForFlush
        {
            get
            {
                if (_waitForFlush == null)
                    _waitForFlush = new ManualResetEvent(false);
                return  _waitForFlush;
            }
        }
        /// <summary>
        /// Flushes the current video handler reader context. I.e. buffers,
        /// position variables and worker threads(reaerThread).
        /// </summary>
        /// <remarks>
        /// Invoke this method within a different thread as
        /// it could terminate the caller if it is on the
        /// list of thing to clean up.
        /// </remarks>
        public void flushReader()
        {
            stopReaderThread = true;
            readerWaitEvent.Set();
            Thread.Sleep(10); // should suffice as reader checkes stop flag every frame row.
            if (readerThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin)
            {
                try
                {
                    readerThread.Join(10);
                }
                catch (ThreadStateException)
                {
                    // looks like he finished before we could ask, good for him.
                }
            }
            if (readerThread.ThreadState != System.Threading.ThreadState.Stopped
                && readerThread.ThreadState != System.Threading.ThreadState.StopRequested
                && readerThread.ThreadState != System.Threading.ThreadState.Unstarted
                && readerThread.ThreadState != System.Threading.ThreadState.Aborted
                && readerThread.ThreadState != System.Threading.ThreadState.AbortRequested)
            {
                try
                {
                    readerThread.Abort();
                } catch (ThreadStateException) {
                    // lets hope he cleaned up ;-)
                }
            }

            #region obsolete
            //if (readerThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin)
            //{
            //    readerWaitEvent.Set();

            //    if (readerThread.ThreadState != System.Threading.ThreadState.Unstarted)
            //    {
            //        try
            //        {
            //            readerThread.Abort();
            //        } catch (ThreadStateException) {
            //            // cant handle this one.. but it shouldnt come this far either.
            //        }
            //    }

            //} else if (readerThread.ThreadState == System.Threading.ThreadState.Running) {
            //    waitReaderStopEvent.Reset();
            //    readerWaitEvent.Set();
            //    waitReaderStopEvent.WaitOne();
            //}
            //////////////////////////////////////////////////////////////////////////////////////
            //if ((readerThread.ThreadState == System.Threading.ThreadState.Running) ||
            //    (readerThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin))
            //    throw new ThreadStateException("YuvVideoHandler problem occured");
            // shouldnt be neccesary as even 4xHighDefinition frame would be computed
            // after 5 seconds...
            ///////////////////
            //if (readerThread.ThreadState == System.Threading.ThreadState.Running
            //    || readerThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin)
            //{
            //    try
            //    {
            //        if (readerThread.ThreadState == System.Threading.ThreadState.Running)
            //            readerThread.Abort();       // if reader not done after 50 ms it is
            //        // is probably deadLocked...
            //    }
            //    catch (ThreadStateException)
            //    {
            //        // nothing to do here
            //    }
            //    finally
            //    {

            //        readerThread = null;
            //    }
            //}
            #endregion


            
            readerThread = null;
            readerWaitEvent.Reset();
            stopReaderThread = false;

            //has to use the backdoor to prevent
            //ENDLESS LOOP
            _positionReader = 0;
            _readerBuffPos = 0;
            // dont have to reassign buffPos as it will be
            // overwritten by a starting readerThread

            /// Triggers reinitialization of this value, important
            /// if for some unknown reason someone tries
            /// to change the format and or the WIDTHxHEIGHT
            /// values (i.e. vidInfo) of the currently loaded
            /// context.
            frameByteSize = 0;

            // note: grantedMemory is user provided
            NUMFRAMESINMEM = grantedMemory / frameByteSize;

            readQueue.Clear();


            //signal that flush is done
            waitForFlush.Set();
        }

        private Thread _readerThread;
        private Thread readerThread
        {
            get
            {
                if ((_readerThread == null) || 
                    (_readerThread.ThreadState == System.Threading.ThreadState.Stopped) ||
                    (_readerThread.ThreadState == System.Threading.ThreadState.Aborted))
                {
                    _readerThread = new Thread(new ThreadStart(fillBuffer));
                    _readerThread.Name = "readerThread";
                }


                return _readerThread;
            }
            set
            {
                _readerThread = value;
            }
        }

        private ManualResetEvent _readerWaitEvent;
        private ManualResetEvent readerWaitEvent
        {
            get
            {
                if (_readerWaitEvent == null)
                    _readerWaitEvent = new ManualResetEvent(false);

                return _readerWaitEvent;
            }
        }
        private ManualResetEvent _waitReaderStopEvent;
        private ManualResetEvent waitReaderStopEvent
        {
            get
            {
                if (_waitReaderStopEvent == null)
                    _waitReaderStopEvent = new ManualResetEvent(false);

                return _waitReaderStopEvent;
            }
        }


        private ManualResetEvent _getFrameWaitEvent;
        private ManualResetEvent getFrameWaitEvent
        {
            get
            {
                if (_getFrameWaitEvent == null)
                    
                    _getFrameWaitEvent = new ManualResetEvent(false);
                return _getFrameWaitEvent;
            }
        }



        /// <summary>
        ///  Reads and returns the requested frame from the video file. Returns null if the video file cannot be read.
        /// </summary>
        /// <param name="frameNm">the number of the frame to return</param>
        /// <returns>the selected frame as a RGB Bitmap object</returns>
        public System.Drawing.Bitmap getFrame(bool buffer = true)
        {


            //if (!buffer && NUMFRAMESINMEM > 1)
            //{
            //    flushReader();
            //    NUMFRAMESINMEM = 1;
            //    var currentPosition = positionReader;
            //}
            this.buffer = buffer;
            getFrameWaitEvent.Reset();

            if (readerBuffPos < readVidInfo.frameCount)
            {
                if (((readerBuffPos - positionReader) < (NUMFRAMESINMEM / 2)) || !(readQueue.Count > 0))
                {
                    readerWaitEvent.Set();
                    if ((readerThread.ThreadState != System.Threading.ThreadState.Running) &&
                        (readerThread.ThreadState != System.Threading.ThreadState.WaitSleepJoin))
                    {
                        //readerThread.ThreadState != System.Threading.ThreadState.)
                        _readerBuffPos = _positionReader;       // avoid racecondition
                        readerThread.Start();
                    }
                }

                if (Math.Abs(readerBuffPos - positionReader) < 2)
                {
                    if (positionReader < readVidInfo.frameCount)
                    {

                        readerWaitEvent.Set();
                        getFrameWaitEvent.WaitOne();
                    }
                }
            }
            if (!(positionReader < readVidInfo.frameCount))
            {
                        // flush
                        waitForFlush.Reset();
                        Thread flushThread = new Thread(new ThreadStart(flushReader));
                        flushThread.Name = "flushThread";
                        flushThread.IsBackground = true;
                        flushThread.Start();
                        waitForFlush.WaitOne();
                        return null;
            }
            positionReader++;
            return (readQueue.Count > 0)?(Bitmap)readQueue.Dequeue():null;
        }


        private void fillBuffer()
        {

            readerBuffPos = positionReader;
            while (readerBuffPos < readVidInfo.frameCount)
            {

                int offset = readerBuffPos * frameByteSize;
                int count = NUMFRAMESINMEM - readQueue.Count;

                FileInfo file = new FileInfo(readPath);
                if (!file.Exists)
                    throw new FileNotFoundException();

                byte[] rawData = new byte[frameByteSize * count];
                using (FileStream fs = new FileStream(readPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    try
                    {
                        fs.Seek(frameByteSize * readerBuffPos, SeekOrigin.Begin);
                        fs.Read(rawData, 0, frameByteSize * count);
                    }
                    catch (IOException exc)
                    {
                        throw new FileLoadException("Problems occured while trying to load file (" + readPath + ")" +
                            " in memory. Given file is either not a valid file or the informations you" +
                            " provided are incorrect (i.e. resolution).", exc);
                    }
                }

                int pixelNum = readVidInfo.width * readVidInfo.height;
                int quartSize = readVidInfo.width * readVidInfo.height / 4;

                int i = 0;
                while ((i < count) && !(readerBuffPos > readVidInfo.frameCount) && !stopReaderThread)
                {

                    Bitmap _currFrame = new Bitmap(readVidInfo.width, readVidInfo.height);
                    BitmapData currFrameData = _currFrame.LockBits(new Rectangle(0, 0, _currFrame.Width, _currFrame.Height),
                                ImageLockMode.ReadOnly, _currFrame.PixelFormat);

                    try
                    {

                        int frameOffset = i * frameByteSize + pixelNum;
                        int pBmpBuffer = (System.Int32)currFrameData.Scan0;

                        for (int y = 0; (y < _currFrame.Height) && !stopReaderThread; y++)
                        {
                            int uvCoef = (_currFrame.Width / 2) * (y / 2);
                            int fastUvCoef = uvCoef + frameOffset;
                            int offCord = y * _currFrame.Width;

                            for (int x = 0; x < _currFrame.Width; x++)
                            {
                                int halfX = x / 2;
                                int rgb = convertToRGB(
                                 rawData[offCord + x + fastUvCoef - pixelNum - uvCoef],
                                 rawData[fastUvCoef + halfX],
                                 rawData[quartSize + fastUvCoef + halfX]);

                                unsafe
                                {
                                    *((System.Int32*)pBmpBuffer) = rgb;
                                }
                                pBmpBuffer += 4;

                            }
                        }
                    }
                    finally
                    {
                        _currFrame.UnlockBits(currFrameData);
                    }
                    readQueue.Enqueue(_currFrame);
                    getFrameWaitEvent.Set();
                    readerBuffPos++;

                    i++;
                    if (!buffer)
                    {
                        waitReaderStopEvent.Set();
                        getFrameWaitEvent.Set();
                        return;
                    }

                }

                readerWaitEvent.WaitOne();

                //check for stop request
                if (stopReaderThread || !buffer)
                {
                    waitReaderStopEvent.Set();
                    getFrameWaitEvent.Set();
                    return;
                }
                readerWaitEvent.Reset();
            }
        //    (new Thread(new ThreadStart(flushReader))).Start();

            getFrameWaitEvent.Set();
        }


        /// <summary>
        /// Converts yuv color values to a RGB.
        /// </summary>
        /// <param name="y">luminance (yuv y)</param>
        /// <param name="u">chroma (yuv u)</param>
        /// <param name="v">chroma (yuv v)</param>
        /// <returns>a Color with the according RGB values</returns>
        private int convertToRGB(int y, int u, int v)
        {
            int yCoefA = (y - 16);
            double yCoefB = 1.167 * yCoefA;
            int uCoefA = (u - 128);
            int vCoefA = (v - 128);

            int rgb = clampToByte(yCoefB + 1.596 * vCoefA) << 16;                              //r
            rgb |= clampToByte(1.169 * yCoefA - 0.393 * uCoefA - 0.816 * vCoefA) << 8;        //g
            rgb |= clampToByte(yCoefB + 2.018 * uCoefA);                                     //b

            return rgb;
        }

        #endregion

        #region write

        #region get/set
        private Queue _writeQueue;
        /// <summary>
        /// This queue holds bitmaps provided by a macroPlugin
        /// 
        /// All bitmaps within this queue represent one frame of
        /// a corresponding yuvVideo file. It HAS(by the caller) to be made safe
        /// that this bitmaps are ordered (i.e. first frame comes bevor second and so on).
        /// All frames within this buffer will be written to the path defined in the
        /// corresponding <see cref="YuvVideoInfo"/> object.
        /// </summary>
        /// <remarks>
        /// This queue is threadsafe, you do not have to lock it before
        /// concurrent access. If you lock it your thread would fall
        /// to a DeadLock state. Furthermore you shoulnt assign
        /// this property directly (by assigning to _readQueue for example)
        /// as it would break the threadsafety and result in a DeadLock
        /// if you are lucky.
        /// </remarks>

        private Queue writeQueue
        {
            get
            {
                if (_writeQueue == null)
                {
                    _writeQueue = new Queue(NUMFRAMESINMEM);
                    _writeQueue = Queue.Synchronized(_writeQueue);
                }
                return _writeQueue;
            }
        }

        /// <summary>
        /// This flag signals the currently active writerThread(if active)
        /// to terminate itself.
        /// </summary>
        /// <remarks>
        /// You shouldnt call abort (as described in the corresponding doc entry)
        /// as the writerThread acquires some locks( i.e. on a file and a the
        /// <see cref="BitmapData"/> of a <see cref="Bimpap"/>) and couldnt
        /// release this if you kill it externally.
        /// </remarks>
        bool stopWriterThread = false;

        private int _positionWriter = -1;
        public int positionWriter
        {
            get
            {
                if (_positionWriter < 0)
                    _positionWriter = 0;
                
                return _positionWriter;

            }
            set
            {
                _positionWriter = value;
            }
        }
        #endregion

        /// <summary>
        /// This operation shouldn't (in contrary to the readerFlush which does not
        /// have many consequences) be used if writing process is
        /// active as it would (in most cases) result in data loss if
        /// not worse.
        /// </summary>
        /// <remarks>
        /// Make sure that a writer flush will cause the writer
        /// to stop writing as soon as possible (usually within the next frame
        /// to write). If a currently written file is not written completely
        /// the video will be (it will be tried to..) truncated.
        /// </remarks>
        public void flushWriter()
        {
        // make sure writer thread is stopped
            stopWriterThread = true;
            if (writerThread.ThreadState == System.Threading.ThreadState.Running)
            {
                writerThread.Join(50);
                if (writerThread.ThreadState == System.Threading.ThreadState.Running)
                    writerThread.Abort();       // if reader not done after 50 ms it is
                // is probably deadLocked...
                writerThread = null;
            }
            positionWriter = 0;

            /// Triggers reinitialization of this value, important
            /// if for some unknown reason someone tries
            /// to change the format and or the WIDTHxHEIGHT
            /// values (i.e. vidInfo) of the currently loaded
            /// context.
            frameByteSize = 0;

            // note: grantedMemory is user provided
            NUMFRAMESINMEM = grantedMemory / frameByteSize;
            writeQueue.Clear();

        }

        private Thread _writerThread;
        private Thread writerThread
        {
            get
            {
                if (_writerThread == null)
                {
                    _writerThread = new Thread(new ThreadStart(emptyBuffer));
                    _writerThread.Name = "writerThread";
                }
                return _writerThread;
            }
            set
            {
                _writerThread = value;
            }
        }

        private ManualResetEvent _writerWaitEvent;
        private ManualResetEvent writerWaitEvent
        {
            get
            {
                if (_writerWaitEvent == null)
                    _writerWaitEvent = new ManualResetEvent(false);

                return _writerWaitEvent;
            }
        }
      

        public void writeFrame(bool buffer = true)
        {

        }


        private void emptyBuffer()
        {

        }


        public void writeFrames(int frameNum, System.Drawing.Bitmap[] frames)
        {
            //fill 2 dimensional buffer of data to write with yuv frames
            byte[][] wdata = new byte[frames.Length][];
            for (int i = 0; i < frames.Length; i++)
            {
                wdata[i] = frameToYUV(frames[i]);
            }

            FileStream fs;
            try
            {
                fs = new FileStream(writePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, ((YuvVideoInfo)writeVidInfo).frameSize * frames.Length);
                fs.Seek((int)(frameNum * ((YuvVideoInfo)writeVidInfo).frameSize), SeekOrigin.Begin);

                for (int i = 0; i < frames.Length; i++)
                {
                    fs.Write(wdata[i], 0, wdata[i].Length);
                }

                fs.Close();
            }
            catch (IOException e)
            {
                //TODO: handle writer exceptions?
                throw e;
            }
        }



        /// <summary>
        /// Converts the given RGB color to yuv format.
        /// </summary>
        /// <param name="rgbColor">the rgb color to be converted</param>
        /// <returns>an array containing y, u, and v values in this order</returns>
        private byte[] convertToYUV(Color rgbColor)
        {
            byte[] yuv = new byte[3];
            // conversion rgb > yuv according to http://msdn.microsoft.com/en-us/library/ms893078.aspx
            yuv[0] = (byte)(((66 * rgbColor.R + 129 * rgbColor.G + 25 * rgbColor.B + 128) >> 8) + 16);
            yuv[1] = (byte)(((-38 * rgbColor.R - 74 * rgbColor.G + 112 * rgbColor.B + 128) >> 8) + 128);
            yuv[2] = (byte)(((112 * rgbColor.R - 94 * rgbColor.G - 18 * rgbColor.B + 128) >> 8) + 128);

            return yuv;
        }

        /// <summary>
        /// Returns an array of bytes according to the set yuv format out of the given RGB Bitmap.
        /// </summary>
        /// <param name="frame">the frame to be converted to yuv</param>
        /// <returns>a byte array filled with yuv equivalents of the given frame</returns>
        private byte[] frameToYUV(Bitmap frame)
        {
            byte[] fdata = new byte[((YuvVideoInfo)writeVidInfo).frameSize];
            FrameDataPointers pointers = new FrameDataPointers((YuvVideoInfo)writeVidInfo);

            for (int y = 0; y < writeVidInfo.height; y++)
            {
                for (int x = 0; x < writeVidInfo.width; x++)
                {
                    byte[] col = convertToYUV(frame.GetPixel(x, y));

                    fdata[pointers.y_index] = col[0];
                    fdata[pointers.u_index] = col[1];
                    fdata[pointers.v_index] = col[2];

                    pointers.Next();
                }
            }

            return fdata;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private string writePath;

        private YuvVideoInfo _writeVidInfo;
        private bool buffer;
        public IVideoInfo writeVidInfo
        {
            get
            {
                if (_writeVidInfo == null)
                    _writeVidInfo = new YuvVideoInfo(this.writePath);
                return _writeVidInfo;
            }
            private set
            {
                if (value != null)
                {
                    _writeVidInfo = (YuvVideoInfo)value;
                }
            }
        }
    }


    


    /// <summary>
    /// Helper class managing the indices in an byte array of yuv frame data according to yuv format. 
    /// </summary>
    class FrameDataPointers
    {
        int framedata_lum_start;
        int framedata_u_start;
        int framedata_v_start;
        float chroma_step_horizontal;
        float chroma_step_vertical;
        int luma_step_horizontal;

        int indexY;
        float indexU;
        float indexV;

        YuvVideoInfo _videoInfo;
        int x = 0;
        int y = 0;

        /// <summary>
        /// Creates a new set of indices for the current data array.
        /// </summary>
        public FrameDataPointers(YuvVideoInfo info)
        {
            _videoInfo = info;

            setFrameDataPointers();

            indexY = framedata_lum_start;
            indexU = framedata_u_start;
            indexV = framedata_v_start;
        }


        /// <summary>
        ///  Set the offsets and pointers according to YuvFormat
        /// </summary>
        private void setFrameDataPointers()
        {
            switch (_videoInfo.yuvFormat)
            {
                case YuvFormat.YUV420_IYUV:
                    this.framedata_lum_start = 0;
                    this.framedata_u_start = (_videoInfo.width * _videoInfo.height);
                    this.framedata_v_start = (int)((_videoInfo.width * _videoInfo.height) * (1 + YuvVideoHandler.getLum2Chrom(_videoInfo.yuvFormat)));
                    this.chroma_step_horizontal = 0.5f;
                    this.chroma_step_vertical = 0.5f;
                    this.luma_step_horizontal = 1;
                    break;
                case YuvFormat.YUV444:
                    this.framedata_lum_start = 0;
                    this.framedata_u_start = 1;
                    this.framedata_v_start = 2;
                    this.chroma_step_horizontal = 3;
                    this.chroma_step_vertical = 3;
                    this.luma_step_horizontal = 3;
                    break;
                case YuvFormat.YUV422_UYVY:
                    this.framedata_lum_start = 1;
                    this.framedata_u_start = 0;
                    this.framedata_v_start = 2;
                    this.chroma_step_horizontal = 4;
                    this.chroma_step_vertical = 1;
                    this.luma_step_horizontal = 2;
                    break;
                case YuvFormat.YUV411_Y41P:
                    this.framedata_lum_start = 1;
                    this.framedata_u_start = 0;
                    this.framedata_v_start = 2;
                    this.chroma_step_horizontal = 4;
                    this.chroma_step_vertical = 1;
                    this.luma_step_horizontal = 2;
                    break;
            }
        }




        /// <summary>
        /// the index in the data buffer for the y value of the current pixel
        /// </summary>
        public int y_index
        {
            get
            {
                return this.indexY;
            }
        }

        /// <summary>
        /// the index in the data buffer for the u value of the current pixel
        /// </summary>
        public int u_index
        {
            get
            {
                return (int)Math.Floor(this.indexU);
            }
        }

        /// <summary>
        /// the index in the data buffer for the v value of the current pixel
        /// </summary>
        public int v_index
        {
            get
            {
                return (int)Math.Floor(this.indexV);
            }
        }


        /// <summary>
        /// Moves the index pointers to the next pixel. 
        /// Shifting rows from left to right, top to bottom.
        /// </summary>
        /// <returns>true if there was a next pixel in the frame to shift to</returns>
        public bool Next()
        {
            //next x
            this.x++;

            indexY += luma_step_horizontal;
            indexU += chroma_step_horizontal;
            indexV += chroma_step_horizontal;


            //next y
            if (x >= _videoInfo.width)
            {
                x = 0;
                y++;
                if (y >= _videoInfo.height) return false;

                float chromi = (float)(Math.Floor(y * chroma_step_vertical)
                        * Math.Floor(_videoInfo.width * chroma_step_horizontal));
                indexU = framedata_u_start + chromi;
                indexV = framedata_v_start + chromi;
            }

            return true;
        }
    }

}

//int[] y_Index = new int[vidInfo.height * vidInfo.width];
//int[] u_Index = new int[vidInfo.height * vidInfo.width];
//int[] v_Index = new int[vidInfo.height * vidInfo.width];
//for (int y = 0; y < vidInfo.height * vidInfo.width; y++)
//{

//    y_Index[y] = pointers.y_index;
//    u_Index[y] = pointers.u_index;
//    v_Index[y] = pointers.v_index;
//    pointers.Next();
//}
//var y_Index_Enumerator = y_Index.GetEnumerator();
//var u_Index_Enumerator = u_Index.GetEnumerator();
//var v_Index_Enumerator = v_Index.GetEnumerator();


// conversion yuv > rgb according to http://msdn.microsoft.com/en-us/library/ms893078.aspx
//int c = y - 16;
//int d = u - 128;
//int e = v - 128;

//byte r = clampToByte((298 * c           + 409 * e   + 128) >> 8);
//byte g = clampToByte((298 * c - 100 * d - 208 * e   + 128) >> 8);
//byte b = clampToByte((298 * c + 516 * d             + 128) >> 8);
//byte r = clampToByte(1.167 * (y - 16) + 1.596 * (v - 128));
//byte g = clampToByte(1.169 * (y - 16) - 0.393 * (u - 128) - 0.816 * (v - 128));
//byte b = clampToByte(1.167 * (y - 16) + 2.018 * (u - 128));

//y_Index_Enumerator.MoveNext();
//u_Index_Enumerator.MoveNext();
//v_Index_Enumerator.MoveNext();

//                            currCol = convertToRGB(rawData[0],
//                            rawData[1],
//                            rawData[2]);
//                            currCol = convertToRGB(rawData[frameOffset + (int)y_Index_Enumerator.Current],
//rawData[frameOffset + (int)u_Index_Enumerator.Current],
//rawData[frameOffset + (int)v_Index_Enumerator.Current]);

//                            currFrame.SetPixel(x, y, currCol);

//currFrame.SetPixel(x, y,
//    convertToRGB(
//    rawData[frameOffset + (int)y_Index_Enumerator.Current],
//    rawData[frameOffset + (int)u_Index_Enumerator.Current],
//    rawData[frameOffset + (int)v_Index_Enumerator.Current]));

/////////////////////////////////////////////////////////


//byte[] rawData = new byte[frameByteSize * count];
//fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
//fs.Seek(offset, SeekOrigin.Begin);

//if (fs.Read(rawData, 0, count*frameByteSize) == 0)
//   {
//    buffPos = vidInfo.frameCount;
//   break;
//}

//  byte[,,,] colArray = new byte[NUMFRAMESINMEM + 2,vidInfo.width,vidInfo.height,3];


//fs = new FileStream(path, FileMode.Open);
//fs.Seek(offset, SeekOrigin.Begin);

//if (fs.Read(rawData, 0, rawData.Count()) == 0)
//    this.readMode = Mode.Idle;
//fs.Close();


//    if (position == 0) {
//   Stopwatch sw = new Stopwatch();
//    sw.Start();
//    fillBuffer();
//    sw.Stop();
//    var fps = 150/sw.ElapsedMilliseconds / 1000.0;

//        getFrameWaitEvent.WaitOne();
//}
