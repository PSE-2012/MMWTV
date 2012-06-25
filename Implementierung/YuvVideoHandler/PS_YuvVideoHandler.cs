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
        string _path;

        float lum2chrom;
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


        public IVideoInfo vidInfo
        {
            get
            {
                return _vidInfo;
            }
            private set
            {
                if (value == null)
                {
                    throw new ArgumentException("Parameter vidInfo may not be null.");
                }
                if(!(value is YuvVideoInfo))
                {
                    throw new ArgumentException("Parameter vidInfo has to be of type YuvVideoInfo.");
                }
                _vidInfo =(YuvVideoInfo) value;
            }
        }

        

        public void writeFrame(int frameNum, System.Drawing.Bitmap frame)
        {
            throw new NotImplementedException();
        }

        public void writeFrames(int frameNum, System.Drawing.Bitmap[] frames)
        {
            throw new NotImplementedException();
        }

        public string namePlugin
        {
            get
            {
                return "YuvVideoHandler";
            }
        }

        public PluginType type
        {
            get
            {
                return PluginType.VideoHandler;
            }
        }

        public void setParentControll(System.Windows.Controls.Panel parent)
        {
            throw new NotImplementedException();
        }

        public Dictionary<EventType, List<Delegate>> getEventHandlers()
        {
            throw new NotImplementedException();
        }

        public Memento getMemento()
        {
            throw new NotImplementedException();
        }

        public void setMemento(Memento memento)
        {
            throw new NotImplementedException();
        }










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
                    YCbCr c = new YCbCr();
                    c.Y = framedata_lum[pixel];
                    c.Cb = framedata_u[Convert.ToInt32(pixel * lum2chrom)];
                    c.Cr = framedata_v[Convert.ToInt32(pixel * lum2chrom)];

                    frame.SetPixel(x,y,c.ToRGB().Color);
                }
            }

            return frame;
        }


        public System.Drawing.Bitmap[] getFrames(int frameNm, int offset)
        {
            throw new NotImplementedException();
        }


        private float getLum2Chrom()
        {
            switch (_vidInfo.yuvFormat)
            {
                case YuvFormat.YUV444:
                    return 1.0f;
                case YuvFormat.YUV422:
                    return 1/2;
                case YuvFormat.YUV411:
                    return 1/4;
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
                //totalFrames = (int)(fs.Length / (width * height * (1+2*lum2chrom)));
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


    }
}

