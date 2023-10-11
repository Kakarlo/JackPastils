using System.Windows;
using System.Windows.Input;

namespace JackPastil.View {
    /// <summary>
    /// Interaction logic for TestLogin.xaml
    /// </summary>
    public partial class TestLogin : Window {
        public TestLogin() {
            InitializeComponent();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e) {
            WindowState = WindowState.Minimized;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            DragMove();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        private void BtnMaximize_Click(object sender, RoutedEventArgs e) {
            if (WindowState == WindowState.Maximized) {
                WindowState = WindowState.Normal;
            }
            else { WindowState = WindowState.Maximized; }
        }
    }
}
