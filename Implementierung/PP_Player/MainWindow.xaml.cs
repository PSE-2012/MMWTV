using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PP_Player
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // Testing the program with Sebastian's YUV Video Handler
            YuvVideoInfo info = new YuvVideoInfo();
            info.width = 176;
            info.height = 144;
            info.yuvFormat = YuvFormat.YUV420_IYUV;
            info.frameCount = 500;
            YuvVideoHandler hand = new YuvVideoHandler("C:/Documents/akiyo_qcif.yuv", info);
            Oqat.PublicRessources.Model.Video video = new Oqat.PublicRessources.Model.Video(false, "C:/Documents/akiyo_qcif.yuv", info);
            Oqat.PublicRessources.Model.VideoEventArgs vidargs = new Oqat.PublicRessources.Model.VideoEventArgs(video, false);

            InitializeComponent();
            StackPanel myStackPanel = new StackPanel();
            PP_Presentation.PP_Player player = new PP_Presentation.PP_Player(hand, new PP_Presentation.VideoSource());
            player.setParentControl(myStackPanel);
            this.Content = myStackPanel;
            player.loadVideo(this, vidargs);
            player.playerControl.getSourcePlayerControl().Start();
            // player.unloadVideo();
            // player.onFlushPresentationPlugins(this, null);
        }
    }
}
