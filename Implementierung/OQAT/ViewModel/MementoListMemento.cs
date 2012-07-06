using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oqat.PublicRessources.Model;
namespace Oqat.ViewModel
{
    

    class MementoListMemento : Memento
    {
        
        MementoListMemento(string nameMemento, List<Memento> memList, string mementoPath) 
            : base(nameMemento, memList, mementoPath)
        {
        }

        
    }
}
