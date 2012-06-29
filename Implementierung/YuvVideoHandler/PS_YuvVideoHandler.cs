//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace YuvVideoHandler
{
	using Oqat.PublicRessources.Model;
	using Oqat.PublicRessources.Plugin;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    using System.Drawing.Imaging;
    using System.Drawing;
    using System.IO;
    using AForge.Imaging;



	public class PS_YuvVideoHandler : IVideoHandler
	{
        int NUMFRAMESINMEM = 1;

        YuvVideoInfo _videoInfo;
        string _path;


        double lum2chrom;
        int ysize;
        int chromasize;

        byte[] data;
        int framedata_lum;
        int framedata_u;
        int framedata_v;

        int firstFrameInMem;


        public PS_YuvVideoHandler(string filepath, YuvVideoInfo info)
        {
            if (!System.IO.File.Exists(filepath))
            {
                throw new ArgumentException("Parameter filepath does not refer to an existing file.");
            }
            _path = filepath;

            vidInfo = info;
            calculateFrameCount();

            initBuffers();
        }

        private void initBuffers()
        {
            this.lum2chrom = getLum2Chrom();
            this.ysize = _videoInfo.width * _videoInfo.height;
            this.chromasize = (int)(ysize * lum2chrom);

            data = new byte[(int)(_videoInfo.width * _videoInfo.height * (1 + 2 * lum2chrom) * NUMFRAMESINMEM)];

            Load(0);
        }


        #region getter/setter

        /// <summary>Gets to current VideoInfo object, if a propertiesView is displayed through 
        /// "setParentControl()" the values are updated to the users settings.</summary>
        /// <returns>the current YuvVideoInfo instance of the handled video.</returns>
        public IVideoInfo vidInfo
        {
            get
            {
                return _videoInfo;
            }
            private set
            {
                _videoInfo =(YuvVideoInfo) value;
            }
        }

        /// <summary>
        /// Calculates the number of frames of the video and writes this information into the vidInfo.
        /// </summary>
        /// <returns>true if operation was successful, false if an error occured</returns>
        private bool calculateFrameCount()
        {
            try
            {
                FileStream fs = new FileStream(_path, FileMode.Open);
                this._videoInfo.frameCount = (int)(fs.Length / (_videoInfo.width * _videoInfo.height * (1 + 2 * lum2chrom)));
                fs.Close();

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        /// <summary>Gets plugin name according to IPlugin.</summary>
        /// <returns>string of name of the plugin</returns>
        public string namePlugin
        {
            get
            {
                return "YuvVideoHandler";
            }
        }

        /// <summary>Gets plugin name according to IPlugin.</summary>
        /// <returns>the PluginType of the plugin</returns>
        public PluginType type
        {
            get
            {
                return PluginType.VideoHandler;
            }
        }


        /// <summary>Creates an instance of the propertiesView and
        /// displays the UserControl of this handler for the user in order to change settings.
        /// These settings are updated directly to the videoInfo object "vidInfo".</summary>
        /// <param name="parent">the propertiesView is added as a child to this.</param>
        public void setParentControl(System.Windows.Controls.Panel parent)
        {
            PropertiesView propertiesView = new PropertiesView();

            if (vidInfo == null)
            {
                vidInfo = new YuvVideoInfo();
            }
            calculateFrameCount();

            //databinding between propertiesView and vidInfo
            propertiesView.DataContext = vidInfo;

            parent.Children.Add(propertiesView);
        }


        /// <summary>
        ///  Passes a dictionary of eventHandlers this plugin uses to react to events of other modules.
        /// </summary>
        /// <returns>a dictionary of event types and their associated delegates this plugin uses to handle them.</returns>
        public Dictionary<EventType, List<Delegate>> getEventHandlers()
        {
            //TODO: are any events handled directly by VideoHandlers?
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



        //
        //<summary>reads the requested frame from the associated videofile</summary>
        public System.Drawing.Bitmap getFrame(int frameNm)
        {
            this.updateFrameDataPointers(frameNm);

            Bitmap frame = new Bitmap(_videoInfo.width, _videoInfo.height);

            for (int y = 0; y < _videoInfo.height; y++)
            {
                for (int x = 0; x < _videoInfo.width; x++)
                {
                    int pixel = y * _videoInfo.width + x;
                    int chromP = Convert.ToInt32(Math.Floor(pixel * lum2chrom));

                    Color col = convertToRGB(
                        data[(int)(this.framedata_lum   + y * _videoInfo.width              + x)],
                        data[(int)(this.framedata_u     + y * _videoInfo.width * lum2chrom  + x * lum2chrom * 2)],
                        data[(int)(this.framedata_v     + y * _videoInfo.width * lum2chrom  + x * lum2chrom * 2)]);
                    //data[y * width + x + offset],
                    //data[(y / 2) * (width / 2) + (x / 2) + ysize + offset],
                    //data[(y / 2) * (width / 2) + (x / 2) + ysize + (ysize / 4) + offset]
                    //Color col = convertToRGB(framedata_lum[pixel], 0,0);//framedata_u[chromP],framedata_v[chromP]);

                    frame.SetPixel(x, y, col);
                
                
                }
            }

            return frame;
        }


        public System.Drawing.Bitmap[] getFrames(int frameNm, int offset)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Loads new frames from file into the data array in memory.
        /// </summary>
        /// <param name="startFrame">the (zero-based) frame number to start the read.</param>
        /// <returns>true if loading was successful</returns>
        private bool Load(int startFrame)
        {
            FileStream fs;

            try
            {
                fs = new FileStream(_path, FileMode.Open);
                fs.Seek( (int)(startFrame * _videoInfo.width * _videoInfo.height * (1+2*lum2chrom)), SeekOrigin.Begin);
                fs.Read(data, 0, (int)(_videoInfo.width * _videoInfo.height * (1+2*lum2chrom) * NUMFRAMESINMEM));
                fs.Close();
            }
            catch (Exception) {return false; }


            firstFrameInMem = startFrame;
            return true;
        }

        // Get the luminance and color components of given frame and store them in arrays.s
        // Loads automatically unavailable data from file to memory.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="frame"></param>
        private void updateFrameDataPointers(int frame)
        {
            // check if we have to load new data
            if (frame >= firstFrameInMem + NUMFRAMESINMEM)
            {
                Load(frame);
            }
            if (frame < firstFrameInMem)
            {
                Load(Math.Max(frame - NUMFRAMESINMEM + 1, 0));
            }

            // calculate beginning offset of given frame in buffer
            int frameOffset = (int)((frame - firstFrameInMem) * _videoInfo.height * _videoInfo.width * (1+2*lum2chrom));


            switch (_videoInfo.yuvFormat)
            {

            }

            /*
            Array.Copy(data, frameOffset, framedata_lum, 0, ysize);
            Array.Copy(data, frameOffset, dataOri_lum, 0, ysize);
            Array.Copy(data, frameOffset + ysize, framedata_u, 0, chromasize);
            Array.Copy(data, frameOffset + ysize + chromasize, framedata_v, 0, chromasize);*/
        }
        private double getLum2Chrom()
        {
            switch (_videoInfo.yuvFormat)
            {
                case YuvFormat.YUV444:
                    return 1.0f;
                case YuvFormat.YUV422:
                    return 1.0f / 2;
                case YuvFormat.YUV411:
                    return 1.0f / 4;
                case YuvFormat.YUV420:
                    return 1.0f / 4;
                default:
                    throw new ArgumentException("Invalid YuvFormat set in VideoInfo.");
            }
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
            throw new NotImplementedException();
        }

        public void writeFrames(int frameNum, System.Drawing.Bitmap[] frames)
        {
            throw new NotImplementedException();
        }

    }
}

