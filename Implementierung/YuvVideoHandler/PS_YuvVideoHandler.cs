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

        YuvVideoInfo _vidInfo;
        System.Windows.Controls.UserControl propertiesView = null;
        string _path;


        double lum2chrom;
        int ysize;
        int chromasize;
        byte[] data;
        byte[] framedata_lum;
        byte[] framedata_u;
        byte[] framedata_v;
        byte[] dataOri_lum;
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
            this.ysize = _vidInfo.width * _vidInfo.height;
            this.chromasize = (int)(ysize * lum2chrom);

            data = new byte[(int)(_vidInfo.width * _vidInfo.height * (1 + 2 * lum2chrom) * NUMFRAMESINMEM)];

            framedata_lum = new byte[ysize];
            framedata_u = new byte[chromasize];
            framedata_v = new byte[chromasize];
            dataOri_lum = new byte[ysize];

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
                return _vidInfo;
            }
            private set
            {
                _vidInfo =(YuvVideoInfo) value;
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
                this._vidInfo.frameCount = (int)(fs.Length / (_vidInfo.width * _vidInfo.height * (1 + 2 * lum2chrom)));
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


        /// <summary>Displays the UserControl of the handler for the user in order to change settings.
        /// These settings are updated directly to the videoInfo object "vidInfo".</summary>
        /// <param name="parent">the propertiesView is added as a child to this.</param>
        public void setParentControl(System.Windows.Controls.Panel parent)
        {
            if (this.propertiesView == null)
            {
                this.propertiesView = new PropertiesView();
            }

            if (vidInfo == null)
            {
                vidInfo = new YuvVideoInfo();
            }
            calculateFrameCount();

            //databinding between propertiesView and vidInfo
            this.propertiesView.DataContext = vidInfo;

            if (!parent.IsAncestorOf(this.propertiesView))
            {
                parent.Children.Add(this.propertiesView);
            }
        }


        /// <summary>
        ///  Passes a dictionary of eventHandlers this plugin uses to react to events of other modules.
        /// </summary>
        /// <returns>a dictionary of event types and their associated delegates this plugin uses to handle them.</returns>
        public Dictionary<EventType, List<Delegate>> getEventHandlers()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Memento getMemento()
        {
            throw new NotImplementedException();
        }

        public void setMemento(Memento memento)
        {
            throw new NotImplementedException();
        }


        #endregion







        //
        //<summary>reads the requested frame from the associated videofile</summary>
        public System.Drawing.Bitmap getFrame(int frameNm)
        {
            this.getFrameData(frameNm);

            Bitmap frame = new Bitmap(_vidInfo.width, _vidInfo.height);

            for (int y = 0; y < _vidInfo.height; y++)
            {
                for (int x = 0; x < _vidInfo.width; x++)
                {
                    int pixel = y * _vidInfo.width + x;
                    int chromP = Convert.ToInt32(Math.Floor(pixel * lum2chrom));

                    Color col = convertToRGB(framedata_lum[pixel], 0,0);//framedata_u[chromP],framedata_v[chromP]);
                    frame.SetPixel(x, y, col);
                
                
                }
            }

            return frame;
        }

        



        public System.Drawing.Bitmap[] getFrames(int frameNm, int offset)
        {
            throw new NotImplementedException();
        }


        private double getLum2Chrom()
        {
            switch (_vidInfo.yuvFormat)
            {
                case YuvFormat.YUV444:
                    return 1.0f;
                case YuvFormat.YUV422:
                    return 1.0f/2;
                case YuvFormat.YUV411:
                    return 1.0f/4;
                case YuvFormat.YUV420:
                    return 1.0f/4;
                default:
                    throw new ArgumentException("Invalid YuvFormat set in VideoInfo.");
            }
        }

        // Loads new frames from file into memory
        private bool Load(int startFrame)
        {
            FileStream fs;

            try
            {
                fs = new FileStream(_path, FileMode.Open);
                fs.Seek( (int)(startFrame * _vidInfo.width * _vidInfo.height * (1+2*lum2chrom)), SeekOrigin.Begin);
                fs.Read(data, 0, (int)(_vidInfo.width * _vidInfo.height * (1+2*lum2chrom) * NUMFRAMESINMEM));
                fs.Close();
            }
            catch (Exception) {return false; }


            firstFrameInMem = startFrame;
            return true;
        }

        // Get the luminance and color components of given frame and store them in arrays.s
        // Loads automatically unavailable data from file to memory.
        private void getFrameData(int frame)
        {
            // check if we have to load new data
            if (frame >= firstFrameInMem + NUMFRAMESINMEM)
            {
                Load(frame);
            }
            if (frame <  firstFrameInMem)
            {
                Load(Math.Max(frame - NUMFRAMESINMEM + 1, 0));
            }

            // calculate beginning offset of given frame in buffer
            int offset = (int)((frame - firstFrameInMem) * _vidInfo.height * _vidInfo.width * (1+2*lum2chrom));

            Array.Copy(data, offset, framedata_lum, 0, ysize);
            Array.Copy(data, offset, dataOri_lum, 0, ysize);
            Array.Copy(data, offset + ysize, framedata_u, 0, chromasize);
            Array.Copy(data, offset + ysize + chromasize, framedata_v, 0, chromasize);
        }

        private byte clampToByte(int val)
        {
            return (byte)((val < 0) ? 0 : ((val > 255) ? 255 : val));
        }
        private System.Drawing.Color convertToRGB(int y, int u, int v)
        {

            // conversion yuv > rgb according to http://msdn.microsoft.com/en-us/library/ms893078.aspx
            int c = y - 16;
            int d = u - 128;
            int e = v - 128;

            byte r = clampToByte((298 * c           + 409 * e   + 128) >> 8);
            byte g = clampToByte((298 * c - 100 * d - 208 * e   + 128) >> 8);
            byte b = clampToByte((298 * c + 516 * d             + 128) >> 8);

            /* 
            //alternative conversion formulas
            byte r = clampToByte(y + 1.402 * (v - 128));
            byte g = clampToByte(y - 0.344 * (u - 128) - 0.714 * (v - 128));
            byte b = clampToByte(y + 1.772 * (u - 128));
            */

            return System.Drawing.Color.FromArgb(r, g, b);
        }

        public static Color YUVtoRGB(double y, double u, double v)
        {
            int red = Convert.ToInt32((y + 1.139837398373983740 * v) * 255);
            red = (red > 255) ? 255 : ((red < 0) ? 0 : red);
            int green = Convert.ToInt32((y - 0.3946517043589703515 * u - 0.5805986066674976801 * v) * 255);
            green = (green > 255) ? 255 : ((green < 0) ? 0 : green);
            int blue = Convert.ToInt32((y + 2.032110091743119266 * u) * 255);
            blue = (blue > 255) ? 255 : ((blue < 0) ? 0 : blue);

            return Color.FromArgb(red, green, blue);
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

