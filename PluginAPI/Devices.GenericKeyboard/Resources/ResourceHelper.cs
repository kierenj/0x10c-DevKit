using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Devices.GenericKeyboard.Resources
{
    public static class ResourceHelper
    {
        public static string GetContent(string resourceName)
        {
            var thisassembly = Assembly.GetExecutingAssembly();
            System.IO.Stream dataStream = thisassembly.GetManifestResourceStream(resourceName);
            var temp = Path.GetTempFileName();
            using (var f = File.Create(temp))
            {
                dataStream.CopyTo(f);
            }
            var res = File.ReadAllText(temp);
            File.Delete(temp);
            return res;
        }
    }
}
