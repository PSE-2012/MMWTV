//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace OqatPublicRessources.Plugin
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    using System.Drawing;

	public interface IFilterOqat  : IPlugin
	{
		Bitmap process(Bitmap frame);

	}
}

