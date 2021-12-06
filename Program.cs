using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsDisplayScale
{
    class Program
    {
        static void Main(string[] args)
        {
            var scaling = GetScreenScalingFactor();

            Bitmap bmp = TakingScreenshotEx1(scaling);
            bmp.Save("Screenshot1.png", ImageFormat.Png);

        }

        [DllImport("gdi32.dll", EntryPoint = "GetDeviceCaps", SetLastError = true)]
        public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);


        enum DeviceCap
        {
            VERTRES = 10,
            PHYSICALWIDTH = 110,
            SCALINGFACTORX = 114,
            DESKTOPVERTRES = 117,

            // http://pinvoke.net/default.aspx/gdi32/GetDeviceCaps.html
        }

        private static double GetScreenScalingFactor()
        {
            var g = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr desktop = g.GetHdc();
            var physicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);

            var screenScalingFactor = (double)physicalScreenHeight / Screen.PrimaryScreen.Bounds.Height;//SystemParameters.PrimaryScreenHeight;

            return screenScalingFactor;
        }

        private static Bitmap TakingScreenshotEx1(double scaling = 1)
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            var width = (int)(bounds.Width * scaling);
            var height = (int)(bounds.Height * scaling);
            Bitmap bitmap = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(Point.Empty, Point.Empty, new Size { Width = width, Height = height });
            }

            return bitmap;
        }
    }
}
