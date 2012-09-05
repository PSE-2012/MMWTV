using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oqat.PublicRessources.Model;
using Oqat.PublicRessources.Plugin;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Oqat.ViewModel;

namespace Oqat.Model
{
	/// <summary>
	/// This class is the Model for the Caretaker, which is responsible for saving
    /// or loading the state of an object (Memento) to and from the hard disk drive,
    /// as well as keeping a table of saved object states for other classes to access.
	/// </summary>
    public class Caretaker
	{
        /// <summary>
        /// Used to lock while writeMemento doing his job
        /// </summary>
        private static object writeDisk = new Object();

        /// <summary>
        /// Allows only a single thread to enter the critical area,
        /// which the lock block identifies, when no instance of Singleton has yet been created
        /// </summary>
        private static object syncRoot = new Object();


        /// <summary>
        /// The Caretaker Singleton instance.
        /// </summary>
        private static Caretaker _caretaker;

		/// <summary>
		/// Getter and setter methods for a Caretaker.
		/// </summary>
        public static Caretaker caretaker
        {
            get
            {
                // The lock to prevent Caretaker to exist twice.
                lock(syncRoot)
                {
                    //Check if Caretaker is already created.
                    if (_caretaker == null)
                    {
                       _caretaker = new Caretaker();
                    }
                    return _caretaker;
                }
            }
        }

        /// <summary>
        /// Creates a new Caretaker.
        /// </summary>
        private Caretaker()
        {
        }

        /// <summary>
        /// Loads binary Memento file. If file doesn't exist, null will be returned.
        /// </summary>
        /// <param name="fileName">The binary file to load.</param> 
        public virtual Memento getMemento(string fileName)
		{
            Memento objectToSerialize = null;
            //check if correct file exist
            if (File.Exists(fileName))
            {
                //Reading files and creates a list of mementos
                Stream stream;
  
                stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                //using binary format
                BinaryFormatter bFormatter = new BinaryFormatter();
                bFormatter.Binder = new OqatSerializationBinder();

                try
                {
                    objectToSerialize = (Memento)bFormatter.Deserialize(stream);
                }
                catch(SerializationException ex)
                {
                    stream.Close();
                    PluginManager.pluginManager.raiseEvent(EventType.failure, new ErrorEventArgs(ex));
                    return null;
                }

                stream.Close();
            }
            return objectToSerialize;
		}

        /// <summary>
        /// Saves newly created Memento to the hard disk drive.
        /// /// <param name="objectToSerialize">The Memento</param> 
        /// </summary>
        public virtual void writeMemento(Memento objectToSerialize)
        {
            try
            {
                lock (writeDisk)
                {
                    Stream stream = File.Open(objectToSerialize.mementoPath, FileMode.Create, FileAccess.Write, FileShare.None);
                    BinaryFormatter bFormatter = new BinaryFormatter();
                    bFormatter.Serialize(stream, objectToSerialize);
                    stream.Close();
                }
            }
            catch (Exception exc)
            {
                PluginManager.pluginManager.raiseEvent(EventType.failure, new ErrorEventArgs(exc));
            }
            
        }
	}
}

