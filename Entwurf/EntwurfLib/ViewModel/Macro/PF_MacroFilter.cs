﻿//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
namespace Oqat.ViewModel.Macro
{
    using Oqat.PublicRessources.Plugin;
    using Plugins;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class PF_MacroFilter : Macro, IFilterOqat
    {

        public System.Drawing.Bitmap process(System.Drawing.Bitmap frame)
        {
            throw new NotImplementedException();
        }

        public string namePlugin
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public PluginType type
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void setParentControll(System.Windows.Controls.Panel parent)
        {
            throw new NotImplementedException();
        }

        public Dictionary<EventType, List<Delegate>> getEventHandlers()
        {
            throw new NotImplementedException();
        }

        public PublicRessources.Model.Memento getMemento()
        {
            throw new NotImplementedException();
        }

        public void setMemento(PublicRessources.Model.Memento memento)
        {
            throw new NotImplementedException();
        }
    }
}

