using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;

using Oqat.PublicRessources.Model;
namespace Oqat.Model
{
    class SmartNode : Collection<SmartNode>
    {
        private Video video
        {
            get;
            set;
        }

        public SmartNode()
        {
        }
        public SmartNode(Path vidPath)
        {
        }
    }
}
