//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.PublicRessources.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
    using System.IO;

    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;


	/// <summary>
	/// This class is used for saving the internal state of an object, e.g. plugin settings.
	/// </summary>
    [Serializable()]
    public class Memento : ISerializable
	{
        /// <summary>
        /// Name of the Memento.
        /// </summary>
		public virtual string name
		{
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
		}

        /// <summary>
        /// Object state to be saved.
        /// </summary>
		public virtual object state
		{
            get
            {
                return this.state;
            }
            set
            {
                this.state = value;
            }
		}

        /// <summary>
        /// A path for saving the Memento to disk (if required).
        /// </summary>
		public virtual string mementoPath
		{
            get
            {
                return this.mementoPath;
            }
            set
            {
                this.mementoPath = value;
            }
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameMemento">Name of the Memento</param>
        /// <param name="state">Object state to be saved</param>
        public Memento(string nameMemento, object state)
		{
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameMemento">Name of the Memento</param>
        /// <param name="state">Object state to be saved</param>
        /// <param name="mementoPath">A path to save the Memento to</param>
		public Memento(string nameMemento, object state, string mementoPath)
		{
		}

        public Memento(SerializationInfo info, StreamingContext ctxt)
        {
            this.name = (string)info.GetValue("Name", typeof(string));
            this.state = (object)info.GetValue("State", typeof(object));
            this.name = (string)info.GetValue("Memento Path", typeof(string));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Name", this.name);
            info.AddValue("State", this.state);
            info.AddValue("Memento Path", this.mementoPath);
        }

	}
}

