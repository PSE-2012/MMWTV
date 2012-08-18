namespace Oqat.PublicRessources.Plugin
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    using Oqat.PublicRessources.Model;

    /// <summary>
    /// This interface has to be implemented in order to be recognized
    /// as a PresentationPlugin ( Pluginmanager)
    /// </summary>
    public interface IPresentation : IPlugin, ICloneable
	{
        /// <summary>
        /// See <see cref="PresentationPluginType"/> for a complete list.
        /// </summary>
        PresentationPluginType presentationType
        {
            get;
        }

        /// <summary>
        /// This method will be called if a new Video should be loaded into the plugin.
        /// </summary>
        void setVideo(IVideo video, int position = 0);

        /// <summary>
        /// Resets the plugin to construction time state
        /// </summary>
		void flush();

	}
}

