namespace Oqat.PublicRessources.Plugin
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    /// <summary>
    /// This Enum contains different ( to <see cref="VM_Presentation"/> known) plugin types.
    /// </summary>
	public enum PresentationPluginType
	{
        /// <summary>
        /// This type is used to display Videos.
        /// </summary>
		Player,
        /// <summary>
        /// This type can display diagramm data, if a metric provided the information and stored it into the
        /// Video object. Diagramm will usually be bound to a video object.
        /// </summary>
		Diagram,

        /// <summary>
        /// This type can be used for nearly every presentation task, the drawback is that there will be no further integration (e.g. 
        /// binding to a Player plugin) trough the <see cref="VM_Presentation"/>
        /// </summary>
		Custom,
	}
}
