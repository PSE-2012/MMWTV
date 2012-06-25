//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace YuvVideoHandler
{
	using Oqat.PublicRessources.Model;
	using Oqat.PublicRessources.Plugin;
	using Plugins;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class PS_YuvVideoHandler : IVideoHandler
	{
        YuvVideoInfo _vidInfo;


        public PS_YuvVideoHandler(string filepath, YuvVideoInfo info)
        {
            if (!System.IO.File.Exists(filepath))
            {
                throw new ArgumentException("Parameter filepath does not refer to an existing file.");
            }
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
            throw new NotImplementedException();
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

