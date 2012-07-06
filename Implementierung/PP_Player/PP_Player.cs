//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace PP_Presentation
{
	using AForge.Controls;
	using Oqat.PublicRessources.Plugin;
	using Plugins;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Windows.Controls;
    using System.ComponentModel.Composition;
    using Oqat.PublicRessources.Model;
    
    /// <summary>
    /// This class is responsible for loading a video to play from disk and setting up a container
    /// where the video can be played.
    /// </summary>
    [Export(typeof(IPlugin))]
    [ExportMetadata("Video Player", PluginType.IPresentation)]
	public class PP_Player : IPresentation
	{

        private IVideoHandler _videohandler;
        private PlayerControl _playerControl;
        private VideoSource _videoSource;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="videohandler"></param>
        /// <param name="videoSource"></param>
        public PP_Player(IVideoHandler videohandler, VideoSource videoSource)
        {
            this._videohandler = videohandler;
            this._videoSource = new VideoSource();
        }

        /// <summary>
        /// 
        /// </summary>
        public IVideoHandler videohandler
        {
            get
            {
                return _videohandler;
            }
            set
            {
                _videohandler = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public PlayerControl playerControl
        {
            get {
                return _playerControl;
            }
            set
            {
                _playerControl = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public VideoSource videoSource
        {
            get
            {
                return _videoSource;
            }
            set
            {
                _videoSource = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public PresentationPluginType presentationType
        {
            get
            {
                return PresentationPluginType.Player;
            }
            set
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void unloadVideo()
        {
            for (int i = 0; i < videoSource.bmp.Length; i++)
            {
                videoSource.bmp[i] = null;
            }
            videohandler = null;
            videoSource = null;
            playerControl.getSourcePlayerControl().Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void onFlushPresentationPlugins(object sender, EventArgs e)
        {
            playerControl.getSourcePlayerControl().Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        public string namePlugin
        {
            get
            {
                return "Video Player";
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public PluginType type
        {
            get
            {
                return PluginType.IPresentation;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        public void setParentControl(System.Windows.Controls.Panel parent)
        {
            playerControl = new PlayerControl();
            playerControl.getSourcePlayerControl().VideoSource = videoSource;
            parent.Children.Add(playerControl);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memento"></param>
        public void setMemento(Memento memento)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="vid"></param>
        public void loadVideo(object sender, VideoEventArgs vid)
        {
            videoSource.NUMFRAMESINMEM = vid.video.vidInfo.frameCount;
            videoSource.bmp = new Bitmap[videoSource.NUMFRAMESINMEM];
            // videoSource.bmp = vid.video.getVideoHandler().getFrames(0, videoSource.NUMFRAMESINMEM);

            for (int i = 0; i < videoSource.NUMFRAMESINMEM; i++)
            {
              videoSource.bmp[i] = videohandler.getFrame(i);
            }
        }

        // private void videoSourcePlayer_NewFrame(object sender, ref Bitmap image)
        // {
            // throw new NotImplementedException();
            // this.videoSource.NewFrame += new VideoSourcePlayer.NewFrameHandler(this.videoSourcePlayer_NewFrame);
        // }

    }
}

