namespace Oqat.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    using System.IO;
    using Oqat.PublicRessources.Plugin;

    /// <summary>
    /// This class is used to pass informations about extra Resources the user
    /// wants to visualize.
    /// </summary>
	public class ExtraResourceEventArgs : EventArgs
	{

        /// <summary>
        /// Path to the ressource. The specific plugin to load will be determined from the filenameextension.
        /// </summary>
		public virtual string resourcepath
		{
			get;
			set;
		}
        /// <summary>
        /// Type of the Ressource i.e. Video, Diagramm, Custom.
        /// <remarks>This is only usefull if there are third party plugins of this type available.</remarks>
        /// </summary>
		public virtual PresentationPluginType resourcetype
		{
			get;
			set;
		}

	}
}

