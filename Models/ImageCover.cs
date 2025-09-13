using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ProgramManager.Models
{
    public class ImageCover
    {
        public ImageCover(string path)
        {
            Uri = path;
        }
        private SolidColorBrush _background;
        private SolidColorBrush _foreground;
        private BitmapSource _bitmapSource;
        private readonly DrawingBrush _iconBrush;
        public BitmapSource Source { get; set; }
        public Image Image { get; set; }
        public string Uri { get; set; }
        private int Size { get; set; }
        public string Name { get; set; }
        public int Dpi { get; set; }
        public int Id { get; set; }

        public ImageCover(IconModel icon)
        {
            _background = icon.BackgroundColor;
            _foreground = icon.ForegroundColor;
            _iconBrush = icon.Brush;
            Size = icon.Scale;
            Name = icon.Name;
            Dpi = 96;
            CreateImageFromBrush();
        }

        /// <summary>
        /// Saves the image to a file.
        /// </summary>
        public void Save(string format, string url)
        {
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(_bitmapSource));
            using (FileStream stream = new FileStream(url + Name + format, FileMode.Create))
                encoder.Save(stream);
        }
        
        /// <summary>
        /// Creates an image from a brush.
        /// </summary>
        private void CreateImageFromBrush()
        {
            BitmapSourceFromBrush();
            Image = new Image();
            Image.Width = Size;
            Image.Source = _bitmapSource;
            Source = _bitmapSource;
        }
        
        /// <summary>
        /// Converts a brush to images.
        /// </summary>
        /// <returns></returns>
        private void BitmapSourceFromBrush()
        {
            // RenderTargetBitmap = builds a bitmap rendering of a visual
            var pixelFormat = PixelFormats.Pbgra32;
            RenderTargetBitmap rtb = new RenderTargetBitmap(Size, Size, Dpi, Dpi, pixelFormat);

            // Drawing visual allows us to compose graphic drawing parts into a visual to render
            var drawingVisual = new DrawingVisual();
            using (DrawingContext context = drawingVisual.RenderOpen())
            {
                // Declaring drawing a rectangle using the input brush to fill up the visual
                context.DrawRectangle(_iconBrush, null, new Rect(0, 0, Size, Size));
            }
            // Actually rendering the bitmap
            rtb.Render(drawingVisual);
            _bitmapSource = rtb;
        }
    }
}
