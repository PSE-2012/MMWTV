//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
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
    [ExportMetadata("namePlugin", "VideoPlayer")]
    [ExportMetadata("type", PluginType.IPresentation)]
    [Export(typeof(IPlugin))]
	public class PP_Player : IPresentation
	{

        private IVideoHandler _videohandler;
        private PlayerControl _playerControl;
        private VideoSource _videoSource;

        /// <summary>
        /// Initializes a new PP_Player instance with a given VideoHandler and a VideoSource
        /// </summary>
        /// <param name="videohandler"></param>
        /// <param name="videoSource"></param>
        public PP_Player(IVideoHandler videohandler, VideoSource videoSource)
        {
            this._videohandler = videohandler;
            this._videoSource = new VideoSource();
        }
        public PP_Player()
        {
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
                return PresentationPluginType.Player;
            }
            set
            {

            }
        }

        /// <summary>
        /// Unloads all currently loaded video frames from memory, as well as the video handler and the video source.
        /// Stops the player control.
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
        /// Clears the Player Control out of the PP_Player's View.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void onFlushPresentationPlugins(object sender, EventArgs e)
        {
            playerControl.getSourcePlayerControl().Dispose();
        }


        /// <summary>
        /// Creates a new Player Control, which returns an AForge Source Player Control instance.
        /// Sets the video source of the Source Player Control and adds the Player Control to
        /// the children of the parent panel.
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
            //added by Sebastian
            this._videohandler = vid.video.getVideoHandler();
            this._videoSource = new VideoSource();




            videoSource.NUMFRAMESINMEM = vid.video.vidInfo.frameCount;
            videoSource.bmp = new Bitmap[videoSource.NUMFRAMESINMEM];
            videoSource.bmp = videohandler.getFrames(0, videoSource.NUMFRAMESINMEM);
            // videoSource.bmp = vid.video.getVideoHandler().getFrames(0, videoSource.NUMFRAMESINMEM);
        }

        // private void videoSourcePlayer_NewFrame(object sender, ref Bitmap image)
        // {
            // throw new NotImplementedException();
            // this.videoSource.NewFrame += new VideoSourcePlayer.NewFrameHandler(this.videoSourcePlayer_NewFrame);
        // }

    }
}

