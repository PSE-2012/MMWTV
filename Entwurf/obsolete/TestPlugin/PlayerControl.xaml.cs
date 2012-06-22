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

namespace ClassLibrary1
{
    /// <summary>
    /// Interaktionslogik für UserControl2.xaml
    /// </summary>
    public partial class PlayerControl : UserControl
    {

        public PlayerControl()
        {
            InitializeComponent();
           
        }
        public AForge.Controls.VideoSourcePlayer getSourcePlayerControl() {
            return sourcePlayer;
        }
    }
}
