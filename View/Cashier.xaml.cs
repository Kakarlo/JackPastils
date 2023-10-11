using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace JackPastil.View {
    /// <summary>
    /// Interaction logic for Cashier.xaml
    /// </summary>
    public partial class Cashier : Window {
        public Cashier() {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e) {
            // Access the Login Visibility
            var mainWindow = Application.Current.Windows.OfType<TestLogin>().FirstOrDefault();
            if (mainWindow != null)
                mainWindow.Visibility = Visibility.Visible;
            base.OnClosing(e);
        }
    }
}
