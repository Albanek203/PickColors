using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace PickColors {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        public MainWindow() { InitializeComponent(); }

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
        private void MoveGrid_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs) {
            this.DragMove();
        }
#endregion

        private void DropImage_OnDrop(object sender, DragEventArgs e) {
            var dropPath  = ((string[]) e.Data.GetData(DataFormats.FileDrop))?[0];
            var extension = Path.GetExtension(dropPath);
            if (extension != ".png" && extension != ".jpg" && extension != ".jpeg") return;
            new ImageView(dropPath).Show();
            Close();
        }
        private void OpenStorage_OnClick(object sender, RoutedEventArgs e) {
            
        }
    }
}