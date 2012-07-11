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
using AForge.Controls;

namespace PP_Presentation
{
    /// <summary>
    ///
    /// </summary>
    public partial class PlayerControl : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public PlayerControl()
        {
            InitializeComponent();
           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public VideoSourcePlayer getSourcePlayerControl() {
            return sourcePlayer;
        }
        
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            this.sourcePlayer.Stop();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            this.sourcePlayer.Start();
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void Resume_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
