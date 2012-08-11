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


        private void local(String s)
        {
            try
            {
                String sFilename = Directory.GetCurrentDirectory() + "/" + s;
                XmlTextReader reader = new XmlTextReader(sFilename);
                reader.Read();
                reader.Read();
                String[] t = new String[3];
                String[] t2 = new String[3];
                for (int i = 0; i < 3; i++)
                {
                    reader.Read();
                    reader.Read();
                    t[i] = reader.Name;
                    reader.MoveToNextAttribute();
                    t2[i] = reader.Value;
                }
                this.Title = t2[0];
                btt_Import.Content = t2[1];
                btt_Cancel.Content = t2[2];
               


            }
            catch (IndexOutOfRangeException e) { }
            catch (FileNotFoundException e) { }
            catch (XmlException e) { }
        }
        public VM_VidImportOptionsDialog(StringCollection vidPathList)
        {
            InitializeComponent();
            local("VM_VidImportOptionsDialog_default.xml");
     //       this.vidHandlerViews = new List<UserControl>();
            this.vidHandlerViews = new Dictionary<ClosableTabItem, Video>();
            Video video;
            IVideoHandler handler;

            foreach (string vidPath in vidPathList)
            {
                video = new Video(false, vidPath);
                handler = video.handler;
                handler.setVideo(vidPath, null);


                 //   vidHandlerViews.Add(handler.propertyView);
                    presentHandlerView(video);
                    handler = null;
            }    
        }

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

        private void presentHandlerView(Video vid)
        {
            var tabItem = new ClosableTabItem();
            tabItem.Header = System.IO.Path.GetFileName(vid.vidPath);
            tabItem.Content = vid.handler.propertyView;
            vidHandlerViews.Add(tabItem, vid);
            this.vidHandlersPanel.Items.Add(tabItem);
        }



        private void btt_Import_Click(object sender, RoutedEventArgs e)
        {
            foreach (Video vid in videoList)
            {
                vid.vidInfo = vid.handler.vidInfo;
       
                if (!vid.handler.consistent)
                {
                    throw new Exception("Check your input, inconsistencies were detected.");
                }
            }
            e.Handled = true;
            this.DialogResult = true;
        }

        private void btt_Cancel_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            this.DialogResult = false;
        }

        

    }
}
