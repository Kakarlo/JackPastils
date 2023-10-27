using JackPastil.View;
using System;
using System.Windows;

namespace JackPastil {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        protected void ApplicationStart(object sender, StartupEventArgs e) {
            var loginView = new TestLogin();
            loginView.Show();
        }

    }
}
