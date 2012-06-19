//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace OqatPublicRessources.Model
{
	using OqatPublicRessources;
	using Plugins;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

using System.IO;
using OqatPublicRessources.Plugin;
	public class Video : IMemorizable
	{
		private IVideoInfo vidInfo
		{
			get;
			set;
		}

		private bool isAnalysis
		{
			get;
			set;
		}

		private Path vidPath
		{
			get;
			set;
		}

		private float[][] frameMetricValue
		{
			get;
			set;
		}

		private List<MacroEntry> processedBy
		{
			get;
			set;
		}

		private Dictionary<PresentationPluginType, List<string>> extraRessources
		{
			get;
			set;
		}

        private delegate void videoObjectCreatedEventHandler(Object sender, VideoEventArgs e);
		private event videoObjectCreatedEventHandler videoObjectCreated;

		public Video(bool isAnalysis,  Path vidPath, IVideoInfo vidInfo)
		{
		}

		public virtual IVideoHandler getVideoHandler()
		{
			throw new System.NotImplementedException();
		}

		public virtual Memento getMemento()
		{
			throw new System.NotImplementedException();
		}

		public virtual void setMemento(Memento memento)
		{
			throw new System.NotImplementedException();
		}

	}
}

