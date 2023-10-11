using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace JackPastil.View {
    /// <summary>
    /// Interaction logic for Inventory.xaml
    /// </summary>
    public partial class Inventory : Window {
        public Inventory() {
            InitializeComponent();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e) {
            WindowState = WindowState.Minimized;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            DragMove();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e) {
            var mainWindow = Application.Current.Windows.OfType<TestLogin>().FirstOrDefault();
            if (mainWindow != null)
                mainWindow.Visibility = Visibility.Visible;
            Application.Current.Windows.OfType<Inventory>().FirstOrDefault().Close();
        }

        private void BtnMaximize_Click(object sender, RoutedEventArgs e) {
            if (WindowState == WindowState.Maximized) {
                WindowState = WindowState.Normal;
            }
            else { WindowState = WindowState.Maximized; }
        }
    }
}
