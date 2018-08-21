using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CommonBaseUI.CommUtil
{
    public class ImageUtil
    {
        /// <summary>
        /// 从资源文件中取图片
        /// </summary>
        /// <param name="resourceFileFullName"></param>
        /// <param name="imageName"></param>
        /// <returns></returns>
        public static ImageSource GetImageFromResource(string resourceFileFullName, string imageName)
        {
            var rm = new ResourceManager(resourceFileFullName, Assembly.GetExecutingAssembly());
            var im = rm.GetObject(imageName) as System.Drawing.Bitmap;
            return ChangeBitmapToImageSource(im);
        }

        /// <summary>
        /// 从资源文件中取图片
        /// </summary>
        /// <param name="resourceFileFullName"></param>
        /// <param name="imageName"></param>
        /// <returns></returns>
        public static ImageSource GetImageFromResource(Assembly assembly, string resourceFileFullName, string imageName)
        {
            var rm = new ResourceManager(resourceFileFullName, assembly);
            var im = rm.GetObject(imageName) as System.Drawing.Bitmap;
            return ChangeBitmapToImageSource(im);
        }

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);
        public static ImageSource ChangeBitmapToImageSource(System.Drawing.Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            ImageSource wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            if (!DeleteObject(hBitmap))
            {
                throw new System.ComponentModel.Win32Exception();
            }

            return wpfBitmap;
        }
    }
}
