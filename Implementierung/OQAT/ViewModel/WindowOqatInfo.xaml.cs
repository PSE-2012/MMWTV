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
using System.Windows.Shapes;

namespace Oqat.ViewModel
{
    /// <summary>
    /// Interaktionslogik für WindowInfo.xaml
    /// </summary>
    public partial class WindowOqatInfo : Window
    {
        public WindowOqatInfo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// event to call the version number
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.tbVersion.Content = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        }
    }
}
