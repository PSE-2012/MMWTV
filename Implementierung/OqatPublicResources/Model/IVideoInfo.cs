namespace Oqat.PublicRessources.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    /// <remarks>vlcht wäre es praktisches das Teil als Klasse zu implementieren</remarks>
	public interface IVideoInfo
	{
        string videoCodecName { get;set; }
        int frameCount { get; set; }
        int width { get; set; }
        int height { get; set; }

        bool Equals(object o);
	}
}

