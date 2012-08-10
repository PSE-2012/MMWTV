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
    public abstract class Macro : IMacro, IPlugin
    {
        string namePlugin
        {
            get
            {
                return "Macro";
            }
        }


        public UserControl macroControl;


        public Macro()
        {

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
        public Memento getMemento()
        {
            return null;
            //return new Memento(this.namePlugin, this.macroQueue.ToArray());
        }

        /// <summary>
        /// Set a Macro Memento
        /// </summary>
        /// <param name="memento">The Memento that should be set as Macro Memento</param>
        public void setMemento(Memento memento)
        {
            //this.macroEntryList = (List<MacroEntry>)memento.state;
        }


    }
}



