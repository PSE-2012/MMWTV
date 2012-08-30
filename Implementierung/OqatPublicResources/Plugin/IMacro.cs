namespace Oqat.PublicRessources.Plugin
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
    using System.Windows.Controls;


    /// <summary>
    /// Every macro has to implement this interface.
    /// Methods within this interface are used for actual processing (i.e. invoking plugins on a video).
    /// </summary>
	public interface IMacro : IPlugin
	{
        UserControl readOnlyPropertiesView
        {
            get;
        }
	}
}

