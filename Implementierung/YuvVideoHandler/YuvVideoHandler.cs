//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace PS_YuvVideoHandler
{
	using Oqat.PublicRessources.Model;
	using Oqat.PublicRessources.Plugin;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;


    using System.ComponentModel.Composition;

    using System.Drawing.Imaging;
    using System.Drawing;
    using System.IO;
    using System.Windows.Controls;


    [ExportMetadata("namePlugin", "yuvVideoHandler")]
    [ExportMetadata("type", PluginType.IVideoHandler)]
    [Export(typeof(IPlugin))]
	public class YuvVideoHandler : IVideoHandler
	{
        const int NUMFRAMESINMEM = 5;
        const int THREADS = 4;

        YuvVideoInfo _videoInfo = null;
        string _path = "";

        private byte[] data = null;
        int bufferSizeFrames = -1;

        int firstFrameInMem = int.MinValue;


        private string path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
                if(_videoInfo != null) _videoInfo.path = _path;
            }
        }
       

        /// <summary>
        /// Constructs a YuvVideoHandler without filepath or VideoInfoObject. 
        /// This instance can be used to display a propertiesview and create a YuvVideoInfo object for example.
        /// </summary>
        public YuvVideoHandler()
        {
        }

        /// <summary>
        /// Returns a new VideoHandler instance.
        /// </summary>
        /// <returns>a new VideoHandler instance</returns>
        public IVideoHandler createVideoHandlerInstance()
        {
            return new YuvVideoHandler();
        }

        /// <summary>
        /// Sets a new video file as the context of this handler.
        /// </summary>
        /// <param name="filepath">location of the videofile to read</param>
        /// <param name="info">VideoInfo containing needed information like resolution and yuv format</param>
        public void setVideo(string filepath, IVideoInfo info)
        {
            vidInfo = info;
            path = filepath;

            //init buffer
            if(info != null)
                initBuffer(NUMFRAMESINMEM);
        }




        /// <summary>
        /// Creates the buffer array for the given number of frames.
        /// </summary>
        /// <param name="frames">the buffersize in frames of the video of this handler</param>
        private void initBuffer(int frames)
        {
            bufferSizeFrames = frames;
            data = new byte[(int)(_videoInfo.width * _videoInfo.height * (1 + 2 * getLum2Chrom(_videoInfo.yuvFormat)) * bufferSizeFrames)];
        }


        #region getter/setter

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


        /// <summary>Gets the current VideoInfo object, if a propertiesView is displayed through 
        /// "setParentControl()" the values are updated to the users settings.</summary>
        /// <returns>the current YuvVideoInfo instance of the handled video.</returns>
        public IVideoInfo vidInfo
        {
            get
            {
                if (_videoInfo == null)
                    _videoInfo = new YuvVideoInfo(this.path);
                return _videoInfo;
            }
            private set
            {
                if (value != null)
                {
                    _videoInfo = (YuvVideoInfo)value;
                }
            }
        }

        public bool consistent
        {
            get 
            {
                return (vidInfo.frameCount < 0) ? false : true;
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
                    _propertyView.DataContext = vidInfo;
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
                    _readOnlyInfoView = new ReadOnlyPropertiesView(vidInfo as YuvVideoInfo);
                }
                return _readOnlyInfoView;
            }
        }


        /// <summary>
        ///  Passes a dictionary of eventHandlers this plugin uses to react to events of other modules.
        /// </summary>
        /// <returns>a dictionary of event types and their associated delegates this plugin uses to handle them.</returns>
        public Dictionary<EventType, List<Delegate>> getEventHandlers()
        {
            return new Dictionary<EventType,List<Delegate>>();
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


        #endregion



        /// <summary>
        ///  Reads and returns the requested frame from the video file. Returns null if the video file cannot be read.
        /// </summary>
        /// <param name="frameNm">the number of the frame to return</param>
        /// <returns>the selected frame as a RGB Bitmap object</returns>
        public System.Drawing.Bitmap getFrame(int frameNm)
        {
            if (data == null) initBuffer(NUMFRAMESINMEM);

            if (!Load(frameNm)) return null;
                


            int frameOffset = (frameNm - firstFrameInMem) * ((YuvVideoInfo)vidInfo).frameSize;
            FrameDataPointers pointers = new FrameDataPointers(_videoInfo);

            Bitmap frame = new Bitmap(_videoInfo.width, _videoInfo.height);

            //convert every pixel of the frame from yuv to rgb
            for (int y = 0; y < _videoInfo.height; y++)
            {
                for (int x = 0; x < _videoInfo.width; x++)
                {
                    Color col = convertToRGB(data[frameOffset + pointers.y_index], data[frameOffset + pointers.u_index], data[frameOffset+pointers.v_index]);
                    frame.SetPixel(x, y, col);
                    
                    pointers.Next();
                }
            }

            return frame;
        }

        /// <summary>
        ///  Reads and returns the requested frames from the video file. Returns null if the video file cannot be read.
        /// </summary>
        /// <param name="frameNm">the number of the first frame to return</param>
        /// <param name="count">number of frames to get</param>
        /// <returns>the selected frame as a RGB Bitmap object</returns>
        public System.Drawing.Bitmap[] getFrames(int frameNm, int count)
        {
            //if buffer is smaller than requested number of frames, resize buffer
            //if (count > this.bufferSizeFrames) initBuffer(count);

            if (!Load(frameNm)) return null;

            Bitmap[] frames = new Bitmap[count];
            int n = 0;

            while(n < count)
            {
                frames[n] = getFrame(frameNm + n);
                n++;

                /*
                for (int i = 0; i < this.THREADS; i++)
                {
                    //TODO: implement concurrent calls to getFrame()
                    //Thread MyThread = new Thread(new ThreadStart(getFrameT));

                }
                */
            }
            

            return frames;
        }


        /// <summary>
        /// Loads new frames from file into the data array in memory.
        /// </summary>
        /// <param name="startFrame">the (zero-based) frame number to start the read.</param>
        /// <returns>true if loading was successful</returns>
        private bool Load(int startFrame)
        {
            // check if we have to load new data into the buffer
            if(startFrame >= firstFrameInMem && startFrame < firstFrameInMem + bufferSizeFrames )
            {
                //data is already in buffer
                return true;
            }


            FileStream fs;

            try
            {
                fs = new FileStream(path, FileMode.Open);
                fs.Seek((int)(startFrame * ((YuvVideoInfo)vidInfo).frameSize), SeekOrigin.Begin);
                fs.Read(data, 0, ((YuvVideoInfo)vidInfo).frameSize * bufferSizeFrames);
                fs.Close();
            }
            catch (Exception) 
            {
                return false; 
            }

            firstFrameInMem = startFrame;
            return true;
        }



        /// <summary>
        /// Converts yuv color values to a RGB.
        /// </summary>
        /// <param name="y">luminance (yuv y)</param>
        /// <param name="u">chroma (yuv u)</param>
        /// <param name="v">chroma (yuv v)</param>
        /// <returns>a Color with the according RGB values</returns>
        private System.Drawing.Color convertToRGB(int y, int u, int v)
        {
            // conversion yuv > rgb according to http://msdn.microsoft.com/en-us/library/ms893078.aspx
            int c = y - 16;
            int d = u - 128;
            int e = v - 128;

            byte r = clampToByte((298 * c           + 409 * e   + 128) >> 8);
            byte g = clampToByte((298 * c - 100 * d - 208 * e   + 128) >> 8);
            byte b = clampToByte((298 * c + 516 * d             + 128) >> 8);

            return System.Drawing.Color.FromArgb(r, g, b);
        }
        /// <summary>
        /// cuts val down to a value between 0 and 255
        /// </summary>
        private byte clampToByte(int val)
        {
            return (byte)((val < 0) ? 0 : ((val > 255) ? 255 : val));
        }













        public void writeFrame(int frameNum, System.Drawing.Bitmap frame)
        {
            this.writeFrames(frameNum, new Bitmap[] { frame });
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
                fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, ((YuvVideoInfo)vidInfo).frameSize * frames.Length);
                fs.Seek((int)(frameNum * ((YuvVideoInfo)vidInfo).frameSize), SeekOrigin.Begin);

                for (int i = 0; i < frames.Length; i++)
                {
                    fs.Write(wdata[i], 0, wdata[i].Length);
                }

                fs.Close();
            }
            catch(IOException e)
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
            yuv[0] =(byte)( ((  66 * rgbColor.R + 129 * rgbColor.G +  25 * rgbColor.B + 128) >> 8) +  16);
            yuv[1] =(byte)( ((-38 * rgbColor.R - 74 * rgbColor.G + 112 * rgbColor.B + 128) >> 8) + 128);
            yuv[2] =(byte)( ((112 * rgbColor.R - 94 * rgbColor.G - 18 * rgbColor.B + 128) >> 8) + 128);

            return yuv;
        }

        /// <summary>
        /// Returns an array of bytes according to the set yuv format out of the given RGB Bitmap.
        /// </summary>
        /// <param name="frame">the frame to be converted to yuv</param>
        /// <returns>a byte array filled with yuv equivalents of the given frame</returns>
        private byte[] frameToYUV(Bitmap frame)
        {
            byte[] fdata = new byte[((YuvVideoInfo)vidInfo).frameSize];
            FrameDataPointers pointers = new FrameDataPointers(_videoInfo);
                        
            for (int y = 0; y < _videoInfo.height; y++)
            {
                for (int x = 0; x < _videoInfo.width; x++)
                {
                    byte[] col = convertToYUV(frame.GetPixel(x,y));

                    fdata[pointers.y_index] = col[0];
                    fdata[pointers.u_index] = col[1];
                    fdata[pointers.v_index] = col[2];

                    pointers.Next();
                }
            }
            
            return fdata;
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

