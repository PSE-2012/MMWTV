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
using Oqat.PublicRessources.Model;
using System.Data;
using AC.AvalonControlsLibrary.Controls;
using System.Collections.ObjectModel;


namespace Oqat.ViewModel.Macro
{
    public partial class MacroMetricControl : UserControl
    {
        private PM_MacroMetric _macro;

        public PM_MacroMetric macro
        {
            get
            {
                return this._macro;
            }

            set
            {
                _macro = value;
            }
        }

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

        public MacroMetricControl(PM_MacroMetric macro, VM_Macro vmmacro)
        {
            this.macro = macro;
            this.vmmacro = vmmacro;
            InitializeComponent();
        }
    }
}
