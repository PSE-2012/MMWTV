namespace Oqat.PublicRessources.Plugin
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    using System.Windows.Controls;

    /// <summary>
    /// Every plugin has to implement this interface to be compatible with <see cref="PluginManager"/>.
    /// </summary>
    /// <remarks>
    /// a Plugin needs to export the following MEF stuff to be importable by PluginManager:
    /// [ExportMetadata("namePlugin", namePlugin)]
    /// [ExportMetadata("type", type)]
    /// [Export(typeof(IPlugin))]
    /// </remarks>
	public interface IPlugin  : IMemorizable, IPluginMetadata
	{

        /// <summary>
        /// Return an optional View a plugin might have implemented (i.e. options view), if
        /// plugin does not provide an additional view, this field can be left uninitialized 
        /// </summary>
        UserControl propertyView
        {
            get;
        }

        IPlugin createExtraPluginInstance();




	}
}

