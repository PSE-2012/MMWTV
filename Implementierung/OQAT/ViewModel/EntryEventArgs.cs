using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oqat.ViewModel
{
    class EntryEventArgs : EventArgs
    {
        public EntryEventArgs(string entry)
        {
            this.entry = entry;
        }
        private string entry;
        public string Entry {
            get 
            {
                return entry; 
            }
        }
    }
}
