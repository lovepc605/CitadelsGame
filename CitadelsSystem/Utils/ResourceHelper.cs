using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CitadelsSystem.Utils
{
    public class ResourceHelper
    {
        public static BitmapSource LoadBitmap(string fileName)
        {
            var obj = (Bitmap)Properties.Resources.ResourceManager.GetObject(fileName);
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(obj.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty,
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
        }
    }

}
