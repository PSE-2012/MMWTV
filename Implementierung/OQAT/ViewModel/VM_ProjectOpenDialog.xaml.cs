namespace Oqat.ViewModel
{
	using System;
    using System.Windows;
    using System.Threading;
    using Oqat.Model;
    using Microsoft.Win32;
    using System.Runtime.InteropServices;
    using System.Windows.Interop;
    using System.Windows.Media;
    using System.ComponentModel;
    using System.IO;
    using System.Xml;
    /// <summary>
    /// This component is responsible for creating new or openning existing projects.
    /// </summary>
	public partial class VM_ProjectOpenDialog : Window, INotifyPropertyChanged
	{

        /// <summary>
        /// method to localize the content of the vm.
        /// </summary>
        private void local(String s)
        {
            try
            {
                String sFilename = Directory.GetCurrentDirectory() + "/" + s;
                XmlTextReader reader = new XmlTextReader(sFilename);
                reader.Read();
                reader.Read();
                String[] t = new String[8];
                String[] t2 = new String[8];
                for (int i = 0; i < 8; i++)
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
                bt1.Content = t2[1];
                bt2.Content = t2[0];
                gb3.Header = t2[2];
                gb4.Header = t2[3];
                btnBrowse.Content = t2[4];
                tb6.Text = t2[5];
                this.Title = t2[6];
                msgboxovverid = t2[7];

            }
            catch (IndexOutOfRangeException e) { }
            catch (FileNotFoundException e) { }
            catch (XmlException e) { }
        }


        private string _title;
        public string title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                pathProject = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\" + title + ".oqatPrj";
                OnPropertyChanged("title");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                if (title == "")
                {
                    bt1.IsEnabled = false;
                }
                else
                {
                    bt1.IsEnabled = true;
                }
            }
        }

        private string _pathProject;
        public string pathProject {
            get
            {
                return _pathProject;
            }
            set
            {
                _pathProject = value;
                OnPropertyChanged("pathProject");

            }
        }
        internal Project project
        {
            get;
            private set;

        }
        private string _description;
        public string description {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnPropertyChanged("description");

            }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public VM_ProjectOpenDialog(string path = null)
        {
            
            InitializeComponent();
            local("VM_ProjectOpenDialog_" + Thread.CurrentThread.CurrentCulture + ".xml");
            title = "myOqatPrj";
            pathProject = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) +"\\" +title+ ".oqatPrj";
            description = "";
            this.prjProperties.DataContext = this;
                
        }
        /// <summary>
        /// event to open the search dialog
        /// </summary>
        private void ExplorerBrowser_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = title;
            dlg.DefaultExt = ".oqatPrj";
            dlg.Filter = "OQAT projects (.oqatPrj)|*.oqatPrj";

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                pathProject = dlg.FileName;
                String[] tmp = pathProject.Split(new Char[] {'\\'});
                String tmp2 = tmp[tmp.Length-1];
                title = tmp2.Substring(0,tmp2.Length-8);
            }
        }

        string msgboxovverid ="Datei existiert. Überschreiben?";
        string msgboxoverrid2 = "";
        
       
        private void buildProject_Click(object sender, RoutedEventArgs e)
        {
           MessageBoxResult writethrough =MessageBoxResult.Yes;
            if(!pathProject.Contains(title)){
                 pathProject = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) +"\\" +title+ ".oqatPrj";
            }


            if (!pathProject.EndsWith(".oqatPrj"))
            {
                pathProject = pathProject+".oqatPrj";
            }

            if (File.Exists(pathProject))
            {
                writethrough = MessageBox.Show(msgboxovverid,
            msgboxoverrid2, MessageBoxButton.YesNo);
            }

            if (writethrough == MessageBoxResult.Yes)
            {
                project = new Project(tbTitel.Text, pathProject, description);
                this.DialogResult = true;
            }
        }


        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {      
            this.DialogResult = false;
        }



        public event PropertyChangedEventHandler PropertyChanged;
    }
}

#region obsoleteTransparent
//[StructLayout(LayoutKind.Sequential)]
//private struct MARGINS
//{
//    public int cxLeftWidth;      // width of left border that retains its size
//    public int cxRightWidth;     // width of right border that retains its size
//    public int cyTopHeight;      // height of top border that retains its size
//    public int cyBottomHeight;   // height of bottom border that retains its size
//};

//[DllImport("DwmApi.dll")]
//private static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS pMarInset);

//private void Window_Loaded(object sender, RoutedEventArgs e)
//{
//    WindowInteropHelper windowInteropHelper = new WindowInteropHelper(this);
//    IntPtr myHwnd = windowInteropHelper.Handle;
//    HwndSource mainWindowSrc = System.Windows.Interop.HwndSource.FromHwnd(myHwnd);

//    mainWindowSrc.CompositionTarget.BackgroundColor = Color.FromArgb(0, 0, 0, 0);

//    MARGINS margins = new MARGINS()
//    {
//        cxLeftWidth = -1,
//        cxRightWidth = -1,
//        cyBottomHeight = -1,
//        cyTopHeight = -1
//    };

//    DwmExtendFrameIntoClientArea(myHwnd, ref margins);
//}
#endregion