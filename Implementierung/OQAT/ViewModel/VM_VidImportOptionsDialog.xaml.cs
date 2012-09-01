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
using System.Threading;
using Oqat.Model;
using Oqat.PublicRessources.Plugin;
using WPF_ClosableTabItem;
using System.Collections.Specialized;
using System.IO;
using System.Xml;

namespace Oqat.ViewModel
{
    /// <summary>
    /// Dialog to query user for information needed to add a video file to the project.
    /// </summary>
    public partial class VM_VidImportOptionsDialog : Window
    {

        private List<Video> _videoList;
        /// <summary>
        /// list from the videos wich should be imported
        /// </summary>
        internal List<Video> videoList
        {
            get
            {
                if (_videoList == null)
                    _videoList = new List<Video>();
                foreach (var entry in vidHandlerViews)
                {
                    if (!_videoList.Contains(entry.Value))
                        _videoList.Add(entry.Value);
                }
                return _videoList;
            }
        }
        private Dictionary<ClosableTabItem, Video> vidHandlerViews;

        /// <summary>
        /// Method to fit tho local content from the vm to an xml file.
        /// </summary>
        string videoerror = "Es wurde versucht ein unbekanntes Format einzulesen";
        private void local(String s)
        {
            try
            {
                String sFilename = Directory.GetCurrentDirectory() + "/" + s;
                XmlTextReader reader = new XmlTextReader(sFilename);
                reader.Read();
                reader.Read();
                String[] t = new String[4];
                String[] t2 = new String[4];
                for (int i = 0; i < 4; i++)
                {
                    reader.Read();
                    reader.Read();
                    t[i] = reader.Name;
                    reader.MoveToNextAttribute();
                    t2[i] = reader.Value;
                    if (t2[i] == "")
                    {
                        throw new XmlException("datei nicht lang genug");
                    }
                }
                this.Title = t2[0];
                btt_Import.Content = t2[1];
                btt_Cancel.Content = t2[2];
                videoerror = t2[3];


            }
            catch (IndexOutOfRangeException e) { }
            catch (FileNotFoundException e) { }
            catch (XmlException e) { }
        }


        /// <summary>
        /// construktor 
        /// </summary>
        public VM_VidImportOptionsDialog(StringCollection vidPathList)
        {
            InitializeComponent();
            local("VM_VidImportOptionsDialog_" + Thread.CurrentThread.CurrentCulture + ".xml");
     //       this.vidHandlerViews = new List<UserControl>();
            this.vidHandlerViews = new Dictionary<ClosableTabItem, Video>();
            Video video;
            IVideoHandler handler;

            bool msgboxShwon = false;
                foreach (string vidPath in vidPathList)
                {
                     try
            {
                    video = new Video(false, vidPath);
                    handler = video.handler;
                    handler.setImportContext(vidPath);


                    //   vidHandlerViews.Add(handler.propertyView);
                    presentHandlerView(video);
                    handler = null;
                }
                     catch (Exception e)
                     {
                         if (msgboxShwon == false)
                         {
                             MessageBox.Show(videoerror);
                             msgboxShwon = true;
                         }
                     }
            }
          
        }
        /// <summary>
        /// event to handel taps that should eb closed
        /// </summary>
        private void TabClosed(object source, RoutedEventArgs args) {
            args.Handled = true;
            ClosableTabItem tabItem = (ClosableTabItem)args.Source;
            vidHandlerViews.Remove(tabItem);
            vidHandlersPanel.Items.Remove(tabItem);
            tabItem.Content = null;
            tabItem = null;
            if (vidHandlerViews.Count == 0)
            {
                btt_Cancel_Click(this, new RoutedEventArgs());
            }
        }

        /// <summary>
        /// method to show the video format specific necessary information
        /// </summary>
        private void presentHandlerView(Video vid)
        {
            var tabItem = new ClosableTabItem();
            tabItem.Header = System.IO.Path.GetFileName(vid.vidPath);
            tabItem.Content = vid.handler.propertyView;
            vidHandlerViews.Add(tabItem, vid);
            this.vidHandlersPanel.Items.Add(tabItem);
        }


        /// <summary>
        /// event to import videos
        /// </summary>
        private void btt_Import_Click(object sender, RoutedEventArgs e)
        {
            foreach (Video vid in videoList)
            {
                vid.vidInfo = vid.handler.readVidInfo;
       
                if (!vid.handler.consistent)
                {
                    throw new Exception("Check your input, inconsistencies were detected.");
                }
            }
            e.Handled = true;
            this.DialogResult = true;
        }
        /// <summary>
        /// event to cancel the dialog.
        /// </summary>
        private void btt_Cancel_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            this.DialogResult = false;
        }

        

    }
}
