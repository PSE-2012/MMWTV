using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;
using System.IO;

using Oqat.ViewModel;
namespace Oqat.Model
{
    class OqatSerializationBinder : SerializationBinder
    {
      
        public override Type BindToType(string assemblyName, string typeName)
        {      

            string[] knwonExtAssemblies =  Directory.GetFiles(PluginManager.PLUGIN_PATH);

            var assemblys = from i in AppDomain.CurrentDomain.GetAssemblies()
                        where !i.IsDynamic && Path.GetDirectoryName(i.Location).Equals(PluginManager.PLUGIN_PATH)
                        && i.GetType(typeName) != null
                        select i.GetType(typeName);
            return assemblys.FirstOrDefault();



        }
    }
}
