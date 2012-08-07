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
        

        OqatApp()
        {
            errorConsole = new WindowErrorConsole();

            initPluginManager();
        }

        private void initPluginManager()
        {
            var pm = PluginManager.pluginManager;
        }
    }

   
}
