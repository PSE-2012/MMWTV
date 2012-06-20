//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.PublicRessources.Plugin
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    using Oqat.PublicRessources.Model;

	public interface IPresentation  : IPlugin
	{
		PresentationPluginType presentationType { get;set; }

		void loadVideo(Video vid);

		void unloadVideo();

		void onFlushPresentationPlugins(object sender, EventArgs e);

	}
}

