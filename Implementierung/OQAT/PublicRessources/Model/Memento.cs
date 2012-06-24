//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.PublicRessources.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
    using System.IO;

	/// <summary>
	/// This class is used for saving the internal state of an object, e.g. plugin settings.
	/// </summary>
    public class Memento
	{
        /// <summary>
        /// Name of the Memento.
        /// </summary>
		public virtual string name
		{
			get;
			set;
		}

        /// <summary>
        /// Object state to be saved.
        /// </summary>
		public virtual object state
		{
			get;
			set;
		}

        /// <summary>
        /// A path for saving the Memento to disk (if required).
        /// </summary>
		public virtual string mementoPath
		{
			get;
			set;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameMemento"></param> Name of the Memento
        /// <param name="state"></param> Object state to be saved
		public Memento(string nameMemento, object state)
		{
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameMemento"></param> Name of the Memento
        /// <param name="state"></param> Object state to be saved
        /// <param name="mementoPath"></param> A path to save the Memento to
		public Memento(string nameMemento, object state, string mementoPath)
		{
		}

	}
}

