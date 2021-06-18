using System.Windows.Media.Imaging;

namespace PickColors.Models {
    public struct PixelColor {
        public byte Blue;
        public byte Green;
        public byte Red;
        public byte Alpha;
        public static void CopyPixels(BitmapSource source, PixelColor[,] pixels, int stride, int offset) {
            var height     = source.PixelHeight;
            var width      = source.PixelWidth;
            var pixelBytes = new byte[height * width * 4];
            source.CopyPixels(pixelBytes, stride, offset);
            var y0 = offset / width;
            var x0 = offset - width * y0;
            for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                    pixels[x + x0, y + y0] = new PixelColor {
                        Blue  = pixelBytes[(y * width + x) * 4 + 0]
                      , Green = pixelBytes[(y * width + x) * 4 + 1]
                      , Red   = pixelBytes[(y * width + x) * 4 + 2]
                      , Alpha = pixelBytes[(y * width + x) * 4 + 3]
                    };
        }
    }
}