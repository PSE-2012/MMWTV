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
using System.Data;
using AC.AvalonControlsLibrary.Controls;
using System.Collections.ObjectModel;


namespace Oqat.ViewModel.Macro
{
    public partial class MacroSaveDialog : Window
    {
        internal delegate void SaveClickEventHandler(object sender, EntryEventArgs e);
        internal event SaveClickEventHandler SaveClick;
        private string macroFilterName;
        private VM_Macro _vmmacro;

        public VM_Macro vmmacro
        {
            get
            {
                return this._vmmacro;
            }
            set
            {
                _vmmacro = value;
            }
        }

        public MacroSaveDialog()
        {
            InitializeComponent();
            textbox.DataContext = macroFilterName;
            SaveClick += new SaveClickEventHandler(saveClick);
        }

        private void onSaveButtonClick(object sender, RoutedEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
            EntryEventArgs ea = new EntryEventArgs(macroFilterName);
            onSaveClick(sender, ea);
        }

        private void onSaveClick(object sender, EntryEventArgs e)
        {
            SaveClick(this, e);
        }

        private void saveClick(object sender, EntryEventArgs e)
        {
            vmmacro.onMacroSave(this, e);
        }
       
    }
}
