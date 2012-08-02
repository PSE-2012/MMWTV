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
		PresentationPluginType presentationType { get;set; }

        /// <summary>
        /// This method will be called if a new Video should be loaded into the plugin.
        /// </summary>
        /// <param name="sender">The caller</param>
        /// <param name="vid">Video to load in.</param>
		void loadVideo(object sender, VideoEventArgs vid);

        /// <summary>
        /// Signals to unload the currently loaded Video.
        /// </summary>
		void unloadVideo();

        /// <summary>
        /// Resets the plugin i.e. unloadVideo() and flush all buffers.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		void onFlushPresentationPlugins(object sender, EventArgs e);

	}
}

