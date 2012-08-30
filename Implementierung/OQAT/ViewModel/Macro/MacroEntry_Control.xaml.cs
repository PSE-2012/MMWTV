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
using Oqat.PublicRessources.Plugin;
using System.ComponentModel;
using System.Windows.Markup;

namespace Oqat.ViewModel.MacroPlugin
{
    /// <summary>
    /// Interaktionslogik für MacroEntry_Control.xaml
    /// </summary>
    public partial class MacroEntry_Control : UserControl
    {
       
        public MacroEntry_Control()
        {
            InitializeComponent();
        }

    }


    #region unusedvisibilityBoolConverter
    //public class ControlVisibility : BaseConverter, IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        // incoming: filterMode
    //        // outgoing: Visibility

    //        bool filterMode = System.Convert.ToBoolean(value);
    //        return (filterMode) ? Visibility.Visible : Visibility.Collapsed;

    //    }


    //    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public abstract class BaseConverter : MarkupExtension
    //{
    //    public override object ProvideValue(IServiceProvider serviceProvider)
    //    {
    //        return this;
    //    }
    //}
    #endregion
}
