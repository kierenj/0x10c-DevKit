using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Media.Imaging;

namespace HaroldInnovationTechnologies.HMD2043.Resources
{
    public static class ResourceHelper
    {
        public static BitmapImage Disk { get { return GetImage("HaroldInnovationTechnologies.HMD2043.Resources.Images.disk.png"); } }
        public static BitmapImage Drive { get { return GetImage("HaroldInnovationTechnologies.HMD2043.Resources.Images.drive.png"); } }

        private static BitmapImage GetImage(string resourceName)
        {
            var thisassembly = Assembly.GetExecutingAssembly();
            System.IO.Stream imageStream = thisassembly.GetManifestResourceStream(resourceName);
            BitmapFrame bmp = BitmapFrame.Create(imageStream);
            var temp = Path.GetTempFileName();
            using (var f = File.Create(temp))
            {
                imageStream.CopyTo(f);
            }
            var res = GetImage(new Uri(temp));
            File.Delete(temp);
            return res;
        }


        private static BitmapImage GetImage(Uri uri)
        {
            if (uri == null) return null;

            try
            {
                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.UriSource = uri;
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.EndInit();
                bmp.Freeze();
                return bmp;
            }
            catch (Exception)
            {
                return null;
            }
        }

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
