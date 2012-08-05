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

namespace WPF_ClosableTabItem
{
    /// <summary>
    /// Führen Sie die Schritte 1a oder 1b und anschließend Schritt 2 aus, um dieses benutzerdefinierte Steuerelement in einer XAML-Datei zu verwenden.
    ///
    /// Schritt 1a) Verwenden des benutzerdefinierten Steuerelements in einer XAML-Datei, die im aktuellen Projekt vorhanden ist.
    /// Fügen Sie dieses XmlNamespace-Attribut dem Stammelement der Markupdatei 
    /// an der Stelle hinzu, an der es verwendet werden soll:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WPF_ClosableTabItem"
    ///
    ///
    /// Schritt 1b) Verwenden des benutzerdefinierten Steuerelements in einer XAML-Datei, die in einem anderen Projekt vorhanden ist.
    /// Fügen Sie dieses XmlNamespace-Attribut dem Stammelement der Markupdatei 
    /// an der Stelle hinzu, an der es verwendet werden soll:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WPF_ClosableTabItem;assembly=WPF_ClosableTabItem"
    ///
    /// Darüber hinaus müssen Sie von dem Projekt, das die XAML-Datei enthält, einen Projektverweis
    /// zu diesem Projekt hinzufügen und das Projekt neu erstellen, um Kompilierungsfehler zu vermeiden:
    ///
    ///     Klicken Sie im Projektmappen-Explorer mit der rechten Maustaste auf das Zielprojekt und anschließend auf
    ///     "Verweis hinzufügen"->"Projekte"->[Dieses Projekt auswählen]
    ///
    ///
    /// Schritt 2)
    /// Fahren Sie fort, und verwenden Sie das Steuerelement in der XAML-Datei.
    ///
    ///     <MyNamespace:ClosableTabItem/>
    ///
    /// </summary>
    public partial class ClosableTabItem : System.Windows.Controls.TabItem
    {
        static ClosableTabItem()
        {
            //This style is defined in themes\generic.xaml
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ClosableTabItem),
                new FrameworkPropertyMetadata(typeof(ClosableTabItem)));

            CommandManager.RegisterClassCommandBinding(typeof(ClosableTabItem),
                new CommandBinding(ClosableTabItem.StateChange, StateChangeExecuted));
        }

        public static readonly RoutedUICommand StateChange =
            new RoutedUICommand("State Change", "StateChange", typeof(ClosableTabItem));

        public static readonly RoutedEvent TabOpenEvent =
            EventManager.RegisterRoutedEvent("TabOpen", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(ClosableTabItem));

        public static readonly RoutedEvent TabCloseEvent =
            EventManager.RegisterRoutedEvent("TabClose", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(ClosableTabItem));

        private static void StateChangeExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ClosableTabItem s = (ClosableTabItem)sender;
            bool parameter = (e.Parameter == null) ? false : (bool)e.Parameter;
            if (parameter)
                s.RaiseEvent(new RoutedEventArgs(ClosableTabItem.TabOpenEvent));
            else
                s.RaiseEvent(new RoutedEventArgs(ClosableTabItem.TabCloseEvent));
        }

        public event RoutedEventHandler TabOpen
        {
            add { AddHandler(TabOpenEvent, value); }
            remove { RemoveHandler(TabOpenEvent, value); }
        }

        public event RoutedEventHandler TabClose
        {
            add { AddHandler(TabCloseEvent, value); }
            remove { RemoveHandler(TabCloseEvent, value); }
        }

        /// <summary>
        /// Because of a bug I found in WPF, I cannot get a OneWay binding to the
        /// IsVisible property working, so this additional property is used instead.
        /// </summary>
        public Boolean CIsVisible
        {
            get { return (Boolean)GetValue(CIsVisibleProperty); }
            set { SetValue(CIsVisibleProperty, value); }
        }
        public static readonly DependencyProperty CIsVisibleProperty =
            DependencyProperty.Register("CIsVisible", typeof(Boolean), typeof(ClosableTabItem), new PropertyMetadata(true));
    }
}
