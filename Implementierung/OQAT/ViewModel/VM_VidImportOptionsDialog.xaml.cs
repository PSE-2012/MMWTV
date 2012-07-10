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

        public VM_VidImportOptionsDialog(Video importTarget)
        {
            InitializeComponent();

            video = importTarget;
            handler = video.getVideoHandler();
            if (handler != null)
            {
                this.txt_NoHandler.Visibility = System.Windows.Visibility.Collapsed;

                handler.setParentControl(this.gridHandlerView);
                video.vidInfo = handler.vidInfo;
            }
            this.DataContext = video;
        }

        public Video importedVideo
        {
            get
            {
                return video;
            }
        }

        private void btt_Import_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void btt_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        

    }
}
