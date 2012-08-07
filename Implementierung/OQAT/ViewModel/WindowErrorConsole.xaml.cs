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
    /// Interaktionslogik für Window1.xaml
    /// </summary>
    public partial class WindowErrorConsole : Window
    {
        public WindowErrorConsole()
        {
            InitializeComponent();

            errors = new List<ErrorViewModel>();
            this.DataContext = errors;

            PluginManager.OqatInfo += onOqatInfo;
            PluginManager.OqatFailure += onOqatFailure;
            PluginManager.OqatPanic += onOqatPanic;

        }

        List<ErrorViewModel> errors
        {
            get;
            set;
        }

        private void onOqatInfo(object sender, System.IO.ErrorEventArgs e)
        {
            Exception ex = e.GetException();
            errors.Add(new ErrorViewModel(ex.Message, ex.Source, "info"));
        }
        private void onOqatFailure(object sender, System.IO.ErrorEventArgs e)
        {
            Exception ex = e.GetException();
            errors.Add(new ErrorViewModel(ex.Message, ex.Source, "failure"));
        }
        private void onOqatPanic(object sender, System.IO.ErrorEventArgs e)
        {
            Exception ex = e.GetException();
            errors.Add(new ErrorViewModel(ex.Message, ex.Source, "panic"));
        }
    }

    public class ErrorViewModel
    {
        public string priority
        {
            get;
            private set;
        }
        public string source
        {
            get;
            private set;
        }
        public DateTime time
        {
            get;
            private set;
        }
        public string message
        {
            get;
            private set;
        }

        public ErrorViewModel(string message, string source, string priority)
        {
            this.message = message;
            this.source = source;
            this.priority = priority;

            time = DateTime.Now;
        }

    }
}
