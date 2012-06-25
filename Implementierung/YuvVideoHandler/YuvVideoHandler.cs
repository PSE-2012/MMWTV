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
    using Devcorp.Controls.Design;


	public class YuvVideoHandler : IVideoHandler
	{
        YuvVideoInfo _vidInfo;
        string _path;


        public YuvVideoHandler(string filepath, YuvVideoInfo info)
        {
            if (!System.IO.File.Exists(filepath))
            {
                throw new ArgumentException("Parameter filepath does not refer to an existing file.");
            }
            _path = filepath;

            vidInfo = info;


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
                if(value is YuvVideoInfo)
                {
                    throw new ArgumentException("Parameter vidInfo has to be of type YuvVideoInfo.");
                }
                _vidInfo =(YuvVideoInfo) value;
            }
        }

        public System.Drawing.Bitmap getFrame(int frameNm)
        {
            FileStream fs = File.OpenRead(_path);
            try
            {
                int blockbytes = bytesPerBlock(_vidInfo.yuvFormat);
                int blockpixels = pixelsPerBlock(_vidInfo.yuvFormat);

                Bitmap b = new Bitmap(_vidInfo.width, _vidInfo.height);

                for (int y = 0; y < _vidInfo.height; y++)
                {
                    for (int x = 0; x < _vidInfo.width; x++)
                    {
                        byte[] bytes = new byte[blockbytes];
                        fs.Read(bytes,  0, blockbytes);


                        Color color;
                        switch (_vidInfo.yuvFormat)
                        {
                            case YuvFormat.YUV444:
                                color = ColorSpaceHelper.YUVtoColor(Convert.ToDouble(bytes[0]), Convert.ToDouble(bytes[1]), Convert.ToDouble(bytes[2]));
                                break;
                            case YuvFormat.YUV422:
                                color = ColorSpaceHelper.YUVtoColor(Convert.ToDouble(bytes[0]), Convert.ToDouble(bytes[1]), Convert.ToDouble(bytes[2]));
                                break;
                            case YuvFormat.YUV411:
                                color = ColorSpaceHelper.YUVtoColor(Convert.ToDouble(bytes[1]), Convert.ToDouble(bytes[1]), Convert.ToDouble(bytes[2]));
                                break;
                            default:
                                throw new ArgumentException("Invalid YuvFormat set in VideoInfo.");
                        }

                        b.SetPixel(x, y, color);
                    }
                }
            }
            finally
            {
                fs.Close();
            }
        }


        public static int bytesPerBlock(YuvFormat format)
        {
            switch(format)
            {
                case YuvFormat.YUV444:
                    return 3;
                case YuvFormat.YUV422:
                    return 4;
                case YuvFormat.YUV411:
                    return 6;
                default:
                    throw new ArgumentException("Invalid YuvFormat set in VideoInfo.");
            }
        }
        public static int pixelsPerBlock(YuvFormat format)
        {
            switch(format)
            {
                case YuvFormat.YUV444:
                    return 1;
                case YuvFormat.YUV422:
                    return 2;
                case YuvFormat.YUV411:
                    return 4;
                default:
                    throw new ArgumentException("Invalid YuvFormat set in VideoInfo.");
            }
        }







        public System.Drawing.Bitmap[] getFrames(int frameNm, int offset)
        {
            throw new NotImplementedException();
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
    }
}

