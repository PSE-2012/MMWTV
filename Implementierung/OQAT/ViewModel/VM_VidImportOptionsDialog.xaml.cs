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

using Oqat.PublicRessources.Model;
using Oqat.PublicRessources.Plugin;

namespace Oqat
{
    /// <summary>
    /// Dialog to query user for information needed to add a video file to the project.
    /// </summary>
    public partial class VM_VidImportOptionsDialog : Window
    {
        Video video;
        IVideoHandler handler;

        public VM_VidImportOptionsDialog()
        {
            InitializeComponent();

                      
        }

        public Video importVideo(string path)
        {
            video = new Video();
            video.vidPath = path;
            IVideoHandler handler = video.getVideoHandler();
            if (handler != null)
            {
                this.txt_NoHandler.Visibility = System.Windows.Visibility.Collapsed;

                handler.setParentControl(this.gridHandlerView);
            }
            this.DataContext = video;

            this.ShowDialog();

            return video;
        }

        

    }
}
