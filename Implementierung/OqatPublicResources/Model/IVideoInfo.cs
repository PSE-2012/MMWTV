namespace Oqat.PublicRessources.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    /// <remarks>vlcht wäre es praktisches das Teil als Klasse zu implementieren</remarks>
    public interface IVideoInfo : ICloneable
	{
        string videoCodecName { get; }
        int frameCount { get;  }
        int width { get;  }
        int height { get;  }

        bool Equals(object o);
	}
}

