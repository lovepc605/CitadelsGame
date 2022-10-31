using System;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Drawing;
using System.Windows.Threading;
using System.Windows.Interop;

namespace CitadelsGame
{
    public class GifAnimationControl:UserControl
    {

        delegate void OnFrameChangedDelegate();
        System.Drawing.Image m_animatedImage;
        HwndSource m_hwnd;
        System.Drawing.Rectangle m_rectangle;
        System.Windows.Point m_point;
        System.Drawing.Brush m_brush;

        public GifAnimationControl(Window p_window, Bitmap p_bitmap, System.Drawing.Brush p_brush)
        {
            m_animatedImage = p_bitmap;
            m_hwnd = PresentationSource.FromVisual((Visual)p_window) as HwndSource;
            m_brush = p_brush;
            Width = p_bitmap.Width;
            Height = p_bitmap.Height;
            m_point = p_window.PointToScreen(new System.Windows.Point(0, 0));

            ImageAnimator.Animate(m_animatedImage, new EventHandler(OnFrameChanged));

            Loaded += new RoutedEventHandler(AnimatedImageControl_Loaded);
        }

        void AnimatedImageControl_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Point point = this.PointToScreen(new System.Windows.Point(0, 0));
            m_rectangle = new System.Drawing.Rectangle((int)(point.X - m_point.X), (int)(point.Y - m_point.Y), (int)Width, (int)Height);
        }

        void OnFrameChangedTS()
        {
            InvalidateVisual();
        }

        void OnFrameChanged(object o, EventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new OnFrameChangedDelegate(OnFrameChangedTS));
        }

        protected override void OnRender(DrawingContext p_drawingContext)
        {
            //Get the next frame ready for rendering.
            ImageAnimator.UpdateFrames(m_animatedImage);
            //Draw the next frame in the animation.
            Bitmap drawableBMP = new Bitmap(m_animatedImage.Width, m_animatedImage.Height);
            Graphics gdc = Graphics.FromImage(drawableBMP);
            gdc.DrawImage(m_animatedImage, new System.Drawing.Rectangle(0, 0, m_rectangle.Width, m_rectangle.Height));
            MemoryStream outStream = TransparentGif.DrawTransparentGif(drawableBMP, m_animatedImage.Width, m_animatedImage.Height);
            System.Windows.Media.Imaging.BitmapImage bmpImage = new System.Windows.Media.Imaging.BitmapImage();
            bmpImage.BeginInit();
            bmpImage.StreamSource = outStream;
            bmpImage.EndInit();
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Source = bmpImage;
            p_drawingContext.DrawImage(image.Source, new Rect(m_rectangle.X, m_rectangle.Y, m_rectangle.Width, m_rectangle.Height));
            gdc.Dispose();
        }

        
    }
    public class TransparentGif
    {
        public static MemoryStream DrawTransparentGif(Bitmap drawableBMP, Int32 width, Int32 height)
        {
            try
            {
                MemoryStream memStream = new MemoryStream();
                drawableBMP.Save(memStream, ImageFormat.Gif);
                memStream.Seek(0, SeekOrigin.Begin);
                System.Drawing.Image gifed = System.Drawing.Image.FromStream(memStream, true, true);
                ColorPalette cp = gifed.Palette;
                Int32 alpha = 0;
                for (Int32 c = 0; c < cp.Entries.Length; c++)
                {
                    if (cp.Entries[c].A == 0)
                    {
                        alpha = c;
                        break;
                    }
                }
                BitmapData data = ((Bitmap)gifed).LockBits(
                new System.Drawing.Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, gifed.PixelFormat);
                long length = data.Stride * height;
                Byte[] buffer = new Byte[length];
                Marshal.Copy(data.Scan0, buffer, 0, (int)length);
                for (Int32 c = 0; c < buffer.Length; c++)
                {
                    if (cp.Entries[buffer[c]].ToArgb() == System.Drawing.Color.Black.ToArgb())
                    {
                        buffer[c] = (Byte)alpha;
                    }
                }
                Marshal.Copy(buffer, 0, data.Scan0, buffer.Length);
                ((Bitmap)gifed).UnlockBits(data);
                MemoryStream outStream = new MemoryStream();
                gifed.Save(outStream, ImageFormat.Gif);
                outStream.Seek(0, SeekOrigin.Begin);
                return outStream;
            }
            catch { return null; };
        }
    } 
}
