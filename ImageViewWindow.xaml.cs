using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PickColors.Models;

namespace PickColors {
    public partial class ImageView {
        private SolidColorBrush _solidColorBrush;
        private PixelColor[,]   _pixelColor;
        public ImageView(string path) {
            InitializeComponent();
            _solidColorBrush = new SolidColorBrush(new Color {A = 0, B = 0, G = 0, R = 0});
            var bitmapImage = new BitmapImage(new Uri(path));
            _pixelColor = new PixelColor[bitmapImage.PixelWidth + 1, bitmapImage.PixelHeight + 1];
            PixelColor.CopyPixels(bitmapImage, _pixelColor, bitmapImage.PixelWidth * 4, 0);
            Image.Source                = bitmapImage;
            LbSourcePathPicture.Content = path;
        }

#region HeaderButtons
        private void BtnClose_OnClick(object sender, RoutedEventArgs e) { Application.Current.Shutdown(); }
        private void BtnFullWindow_OnClick(object sender, RoutedEventArgs e) {
            WindowState = WindowState == WindowState.Maximized
                              ? WindowState = WindowState.Normal
                              : WindowState = WindowState.Maximized;
        }
        private void BtnHide_OnClick(object sender, RoutedEventArgs routedEventArgs) {
            WindowState = WindowState.Minimized;
        }
        private void MoveGrid_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs buttonEventArgs) {
            this.DragMove();
        }
#endregion

        private Point GetImageCoordsAt(MouseEventArgs e) {
            var controlSpacePosition = e.GetPosition(Image);
            if (Image?.Source == null) return new Point(-1, -1);
            var x = Math.Floor(controlSpacePosition.X * Image.Source.Width / Image.ActualWidth);
            var y = Math.Floor(controlSpacePosition.Y * Image.Source.Height / Image.ActualHeight);
            return new Point(x, y);
        }
        private void Image_OnMouseMove(object sender, MouseEventArgs e) {
            var curPoint = GetImageCoordsAt(e);
            var x        = (int) curPoint.X;
            var y        = (int) curPoint.Y;
            _solidColorBrush.Color = new Color {
                A = _pixelColor[x, y].Alpha
              , B = _pixelColor[x, y].Blue
              , G = _pixelColor[x, y].Green
              , R = _pixelColor[x, y].Red
            };
            RectanglePixelColor.Fill = _solidColorBrush;
            LbRbgInfo.Content =
                $"RGB:( {_pixelColor[x, y].Red}; {_pixelColor[x, y].Blue}; {_pixelColor[x, y].Green}) | HEX ( {_solidColorBrush.Color} ) ";
        }
        static async void AsyncClosingToolTip(ToolTip toolTip) {
            await Task.Run(() => Thread.Sleep(500));
            toolTip.IsOpen = false;
        }
        private void Image_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e) {
            var toolTip = new ToolTip {
                Background = Brushes.DimGray
              , Placement  = PlacementMode.Mouse
              , IsOpen     = true
              , Content    = new TextBlock {Text = "Copy", Foreground = Brushes.WhiteSmoke,}
            };
            AsyncClosingToolTip(toolTip);
            Clipboard.SetText(_solidColorBrush.Color.ToString());
        }
    }
}