//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.ViewModel.Macro
{
	using Oqat.PublicRessources.Plugin;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
    using System.Windows.Controls;
    using Oqat.PublicRessources.Model;
    using Oqat.Model;
    using System.Data;
    using AC.AvalonControlsLibrary.Controls;
    using System.Collections.ObjectModel;

    /// <summary>
    /// This class implements the IMacro interface, see <see cref="IMacro"/> for further information
    /// </summary>
    public abstract class Macro : IMacro
    {
        public string namePlugin
        {
            get
            {
                return "Macro";
            }
        }

        public PluginType type
        {
            get
            {
                return PluginType.IMacro;
            }
        }
                    

        public UserControl macroControl;
        internal List<RangeSelectionChangedEventHandler> delList;

        public Macro()
        {
            delList = new List<RangeSelectionChangedEventHandler>();
        }

        

        public UserControl propertyView
        {
            get
            {
                return macroControl;
            }
        }


        /// <summary>
        /// Get a Macro Memento
        /// </summary>
        /// <returns>A Macro Memento</returns>
        public abstract Memento getMemento();

        /// <summary>
        /// Set a Macro Memento
        /// </summary>
        /// <param name="memento">The Memento that should be set as Macro Memento</param>
        public abstract void setMemento(Memento memento);


    }
}



