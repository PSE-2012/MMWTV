//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    using Oqat.PublicRessources.Model;
    using System.IO;

	/// <summary>
	/// This class is the Model for the Caretaker, which is responsible for saving
    /// or loading the state of an object (Memento) to and from the hard disk drive,
    /// as well as keeping a table of saved object states for other classes to access.
	/// </summary>
    public class Caretaker
	{
        private static Caretaker cTaker;
		/// <summary>
		/// Getter and setter methods for a Caretaker.
		/// </summary>
        internal static Caretaker caretaker
        {
            get
            {
                if (cTaker == null)
                    cTaker = new Caretaker();
                return cTaker;
            }
        }

        /// <summary>
        /// A table of plugin names and a list of saved settings belonging to each of them.
        /// </summary>
		private Dictionary<string, List<Memento>> mementoTable
		{
			get;
			set;
		}

        /// <summary>
        /// Returns a new Caretaker.
        /// </summary>
        /// <returns></returns>
		public static Caretaker getCaretaker()
		{
			throw new System.NotImplementedException();
		}

        /// <summary>
        /// Removes saved settings of a plugin.
        /// </summary>
        /// <param name="origin"></param> Name of the plugin
        /// <param name="nameMemento"></param> Reference to the name of the settings
		public virtual void removeMemento(string origin, string nameMemento)
		{
			throw new System.NotImplementedException();
		}

        /// <summary>
        /// Saves all newly created Mementos to the hard disk drive.
        /// </summary>
		public virtual void toDisk()
		{
			throw new System.NotImplementedException();
		}

        /// <summary>
        /// Creates a new Caretaker.
        /// </summary>
		private Caretaker()
		{
		}

        /// <summary>
        /// Loads all Mementos located in a certain folder.
        /// </summary>
        /// <param name="mementoDir"></param> The folder to load the Mementos from.
		public virtual void fromDisk(string mementoDir)
		{
			throw new System.NotImplementedException();
		}

        /// <summary>
        /// Returns an array of all saved settings of a certain plugin.
        /// </summary>
        /// <param name="origin"></param> Plugin name
        /// <returns></returns>
		public virtual Memento[] getMementos(string origin)
		{
			throw new System.NotImplementedException();
		}

        /// <summary>
        /// Returns the settings of a plugin referenced by a name.
        /// </summary>
        /// <param name="origin"></param> Plugin name
        /// <param name="mementoname"></param> Name of the settings
        /// <returns></returns>
		public virtual Memento getMemento(string origin, string mementoname)
		{
			throw new System.NotImplementedException();
		}

        public virtual Memento getMemento(string path)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Adds new settings of a plugin.
        /// </summary>
        /// <param name="origin"></param> Plugin name
        /// <param name="memento"></param> Settings
		public virtual void addMemento(string origin, Memento memento)
		{
			throw new System.NotImplementedException();
		}

	}
}

