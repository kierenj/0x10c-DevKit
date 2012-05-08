using System;
using System.IO;
using System.Reflection;

namespace Devices.GenericClock.Resources
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
