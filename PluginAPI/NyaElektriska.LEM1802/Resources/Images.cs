using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Media.Imaging;

namespace NyaElektriska.LEM1802.Resources
{
    public static class Images
    {
        public static BitmapImage Font { get { return GetImage("NyaElektriska.LEM1802.Resources.Images.font.png"); } }
        public static BitmapImage Boot { get { return GetImage("NyaElektriska.LEM1802.Resources.Images.boot.png"); } }

        private static BitmapImage GetImage(string resourceName)
        {
            var thisassembly = Assembly.GetExecutingAssembly();
            System.IO.Stream imageStream = thisassembly.GetManifestResourceStream(resourceName);
            BitmapFrame bmp = BitmapFrame.Create(imageStream);
            var temp = Path.GetTempFileName();
            using (var f =  File.Create(temp))
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
    }
}
