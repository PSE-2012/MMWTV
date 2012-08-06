namespace PP_Presentation
{
	using AForge.Controls;
	using Oqat.PublicRessources.Plugin;
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
    /// where the video can be played. It can be exported as a plugin.
    /// </summary>
    [ExportMetadata("namePlugin", "PP_Player")]
    [ExportMetadata("type", PluginType.IPresentation)]
    [Export(typeof(IPlugin))]
	public class PP_Player : IPresentation, ICloneable
	{

        private IVideoHandler _videohandler;
        private PlayerControl _playerControl;
        private VideoSource _videoSource;

        private PresentationPluginType _presentationType = PresentationPluginType.Player;
        private PluginType _type = PluginType.IPresentation;
        private string _namePlugin = "PP_Player";

        public object Clone()
        {
            return new PP_Player();
        }

        public PP_Player()
        {
            this._videoSource = new VideoSource();
        }

        /// <summary>
        /// The video handler which is responsible for getting frames from the video.
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
        /// The PlayerControl is the View Model for PP_Player
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
        /// The VideoSource is responsible for operations like Start, Stop, Pause, Play
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
                return _presentationType;
            }
            set
            {
                _presentationType = value;
            }
        }

        public PluginType type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        public string namePlugin
        {
            get
            {
                return _namePlugin;
            }
            set
            {
                _namePlugin = value;
            }
        }

        /// <summary>
        /// Unloads all currently loaded video frames from memory, as well as the video handler and the video source.
        /// Stops the player control.
        /// </summary>
        public void unloadVideo()
        {
            if (videoSource != null)
            {
                for (int i = 0; i < videoSource.bmp.Length; i++)
                {
                    videoSource.bmp[i] = null;
                }
            }

            videohandler = null;
            videoSource = null;
            playerControl.getSourcePlayerControl().Stop();
        }

        /// <summary>
        /// Clears the Player Control out of the PP_Player's View.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void onFlushPresentationPlugins(object sender, EventArgs e)
        {
            if(playerControl != null && playerControl.getSourcePlayerControl() != null)
                playerControl.getSourcePlayerControl().Dispose();
        }



       /// <summary>
       /// This property holds the actual player you can put in your View in. 
       /// </summary>
        public UserControl propertyView
        {
            get
            {
                              
                playerControl.getSourcePlayerControl().VideoSource = videoSource;
                return playerControl;
            }
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
        /// This function is responsible for loading the frames of a video into memory.
        /// First it gets the frame count X of the video from its VideoInfo object and gives
        /// the information to our Video Source. Our Video Source then creates a new array
        /// of X BitMaps, these are the frames to be loaded into memory. With the getFrames
        /// method of our VideoHandler, the Video Source loads the frames into the BitMap array.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="vid"></param>
        public void loadVideo(object sender, VideoEventArgs vid)
        {
            this._videohandler = vid.video.handler;
            videoSource.NUMFRAMESINMEM = vid.video.vidInfo.frameCount;
            videoSource.bmp = new Bitmap[videoSource.NUMFRAMESINMEM];
            videoSource.bmp = videohandler.getFrames(0, videoSource.NUMFRAMESINMEM);
        }

        // private void videoSourcePlayer_NewFrame(object sender, ref Bitmap image)
        // {
            // throw new NotImplementedException();
            // this.videoSource.NewFrame += new VideoSourcePlayer.NewFrameHandler(this.videoSourcePlayer_NewFrame);
        // }

    }
}

