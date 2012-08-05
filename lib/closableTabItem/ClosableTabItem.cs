using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TestDemo {
	/// <summary>
	/// ========================================
	/// .NET Framework 3.0 Custom Control
	/// ========================================
	///
	/// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
	///
	/// Step 1a) Using this custom control in a XAML file that exists in the current project.
	/// Add this XmlNamespace attribute to the root element of the markup file where it is 
	/// to be used:
	///
	///     xmlns:MyNamespace="clr-namespace:TestDemo"
	///
	///
	/// Step 1b) Using this custom control in a XAML file that exists in a different project.
	/// Add this XmlNamespace attribute to the root element of the markup file where it is 
	/// to be used:
	///
	///     xmlns:MyNamespace="clr-namespace:TestDemo;assembly=CloseableTabItemDemo"
	///
	/// You will also need to add a project reference from the project where the XAML file lives
	/// to this project and Rebuild to avoid compilation errors:
	///
	///     Right click on the target project in the Solution Explorer and
	///     "Add Reference"->"Projects"->[Browse to and select this project]
	///
	///
	/// Step 2)
	/// Go ahead and use your control in the XAML file. Note that Intellisense in the
	/// XML editor does not currently work on custom controls and its child elements.
	///
	///     <MyNamespace:ClosableTabItem/>
	///
	/// </summary>
	/// <author>Adrian Alexander Pinter</author>
	public partial class ClosableTabItem : System.Windows.Controls.TabItem {
		static ClosableTabItem() {
			//This style is defined in themes\generic.xaml
			DefaultStyleKeyProperty.OverrideMetadata( typeof( ClosableTabItem ),
				new FrameworkPropertyMetadata( typeof( ClosableTabItem ) ) );

			CommandManager.RegisterClassCommandBinding( typeof( ClosableTabItem ),
				new CommandBinding( ClosableTabItem.StateChange, StateChangeExecuted ) );
		}

		public static readonly RoutedUICommand StateChange =
			new RoutedUICommand( "State Change", "StateChange", typeof(ClosableTabItem) );

		public static readonly RoutedEvent TabOpenEvent =
			EventManager.RegisterRoutedEvent( "TabOpen", RoutingStrategy.Bubble,
				typeof( RoutedEventHandler ), typeof( ClosableTabItem ) );

		public static readonly RoutedEvent TabCloseEvent =
			EventManager.RegisterRoutedEvent( "TabClose", RoutingStrategy.Bubble,
				typeof( RoutedEventHandler ), typeof( ClosableTabItem ) );

		private static void StateChangeExecuted( object sender, ExecutedRoutedEventArgs e ) {
			ClosableTabItem s = (ClosableTabItem)sender;
			bool parameter = (e.Parameter == null) ? false : (bool)e.Parameter;
			if ( parameter )
				s.RaiseEvent( new RoutedEventArgs(ClosableTabItem.TabOpenEvent) );
			else
				s.RaiseEvent( new RoutedEventArgs(ClosableTabItem.TabCloseEvent) );
		}

		public event RoutedEventHandler TabOpen {
			add { AddHandler( TabOpenEvent, value ); }
			remove { RemoveHandler( TabOpenEvent, value ); }
		}

		public event RoutedEventHandler TabClose {
			add { AddHandler( TabCloseEvent, value ); }
			remove { RemoveHandler( TabCloseEvent, value ); }
		}

		/// <summary>
		/// Because of a bug I found in WPF, I cannot get a OneWay binding to the
		/// IsVisible property working, so this additional property is used instead.
		/// </summary>
		public Boolean CIsVisible {
			get { return (Boolean)GetValue(CIsVisibleProperty); }
			set { SetValue(CIsVisibleProperty, value); }
		}
		public static readonly DependencyProperty CIsVisibleProperty =
			DependencyProperty.Register( "CIsVisible", typeof(Boolean), typeof(ClosableTabItem), new PropertyMetadata(true) );
	}
}
