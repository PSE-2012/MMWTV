﻿//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace PP_Presentation
{
	using AForge.Video;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class VideoSource : IVideoSource
	{
		public virtual void SignalToStop()
		{
			throw new System.NotImplementedException();
		}

		public virtual void Start()
		{
			throw new System.NotImplementedException();
		}

		public virtual void Stop()
		{
			throw new System.NotImplementedException();
		}

		public virtual void WaitForStop()
		{
			throw new System.NotImplementedException();
		}


        public long BytesReceived
        {
            get { throw new NotImplementedException(); }
        }

        public int FramesReceived
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsRunning
        {
            get { throw new NotImplementedException(); }
        }

        public event NewFrameEventHandler NewFrame;

        public event PlayingFinishedEventHandler PlayingFinished;

        public string Source
        {
            get { throw new NotImplementedException(); }
        }

        public event VideoSourceErrorEventHandler VideoSourceError;
    }
}
