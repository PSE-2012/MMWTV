using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

using Oqat.ViewModel;
namespace Oqat
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class OqatApp : Application
    {
        public static WindowErrorConsole errorConsole
        {
            get;
            private set;
        }

        internal static System.Windows.Threading.Dispatcher uiDispatcher;
        
        OqatApp()
        {
            errorConsole = new WindowErrorConsole();
            uiDispatcher = Current.Dispatcher;
            initPluginManager();
        }

        private void initPluginManager()
        {
            var pm = PluginManager.pluginManager;
        }
    }

   
}
