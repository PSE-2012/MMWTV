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
using System.Drawing;

namespace Oqat.ViewModel
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        VM_Presentation pres;
        public MainWindow()
        {
            InitializeComponent();

            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PluginManager p = PluginManager.pluginManager;

            pres = new VM_Presentation(this.gridPlayer);

            //path selected from DateiExplorer, pass it on
            Video importedVideo = new Video(false, "C:/Dokumente und Einstellungen/Sebastian/Eigene Dateien/PSE/Implementierung/akiyo_qcif.yuv", null);
            VM_VidImportOptionsDialog vidImport = new VM_VidImportOptionsDialog(importedVideo);

            bool? res = vidImport.ShowDialog();
            if( !(res.HasValue && res.Value))
            {
                //canceled import
                return;
            }



            /*
            //display in PP_Player
            VideoEventArgs vidargs = new Oqat.PublicRessources.Model.VideoEventArgs(importedVideo, false);

            //initializing example PP_Player
            IPresentation player = PluginManager.pluginManager.getPlugin<IPresentation>("VideoPlayer");
            player.loadVideo(this, vidargs);
            player.setParentControl(this.gridPlayer);
            
            // player.unloadVideo();
            // player.onFlushPresentationPlugins(this, null);
            */
        }




        
        #region OqatApp - Initialization
        /*

        /// <summary>
        /// A reference to the OQAT main ViewModel.
        /// </summary>
        private VM_Oqat vm_Oqat
        {
            get;
            set;
        }

        /// <summary>
        /// This is the only "not ViewModel" to listen
        /// for the toggleView event. Other components can 
        /// ask OqatApp for the current ViewState.
        /// </summary>
        /// <remarks>
        /// Currently this feature is no needed by any component,
        /// since every OqatIntern class can (and should) listen
        /// for the toggleView event. The only known reasen to have
        /// OqatApp to listen for toggleView is the shutdown process,
        /// i.e. ViewType = shutdown.
        /// </remarks>
        /// <param name="sender">Reference to the caller</param>
        /// <param name="e">Holds the new (global) ViewType.</param>
        public delegate void onToggleView(object sender, ViewTypeEventArgs e);


        /// <summary>
        /// Initializes the main ViewModel the <see cref="VM_Oqat"/>.
        /// 
        /// </summary>
        private void initOqat()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Initializes the <see cref="PluginManager"/>.
        /// </summary>
        private void initPluginManager()
        {
            throw new System.NotImplementedException();
        }

        */
        #endregion

    }
}
