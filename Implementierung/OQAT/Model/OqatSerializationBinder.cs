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

       //     Type typeToDesirialize = null;
         //   Assembly targetAssembly;
            string[] knwonExtAssemblies =  Directory.GetFiles(PluginManager.pluginManager.PLUGIN_PATH);

            var assemblys = from i in AppDomain.CurrentDomain.GetAssemblies()
                        where !i.IsDynamic && Path.GetDirectoryName(i.Location).Equals(PluginManager.pluginManager.PLUGIN_PATH)
                        && i.GetType(typeName) != null
                        select i.GetType(typeName);
            var hmm = assemblys.FirstOrDefault();
            return assemblys.FirstOrDefault();

            //foreach (string entry in knwonExtAssemblies)
            //{

            //    if (Path.GetExtension(entry).Equals(".dll"))
            //    {
            //        try
            //        {
            //            targetAssembly = Assembly.LoadFile(entry);
            //        }
            //        catch (BadImageFormatException exc)
            //        {
            //            continue;
            //        }
            //        var hmm = targetAssembly.GetTypes();
            //        if (targetAssembly.FullName.Equals(assemblyName) && !AppDomain.CurrentDomain.GetAssemblies().Contains(targetAssembly))
            //        {

            //            var typeList = from type in targetAssembly.GetTypes()
            //                           where type.FullName.Equals(typeName)
            //                           select type;

            //            typeToDesirialize = typeList.FirstOrDefault();
            //            if (typeToDesirialize != null)
            //                break;
            //        }
            //    }
            //}

           
            
        //    return typeToDesirialize;

        }
    }
}
