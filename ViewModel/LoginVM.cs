using JackPastil.MVVM;
using JackPastil.Repository;
using JackPastil.View;
using System.Windows;
using System.Windows.Input;

namespace JackPastil.ViewModel {
    public class LoginVM : ViewModelBase {

        // Fields
        private string _username;
        private string _password;
        private string _errorMessage;
        private bool _isViewVisible = true;

        private readonly UserRepository userRepository;

        // Properties
        public string Username { get => _username; set { _username = value; OnPropertyChanged(); } }
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }
        public string ErrorMessage { get => _errorMessage; set { _errorMessage = value; OnPropertyChanged(); } }
        public bool IsViewVisible { get => _isViewVisible; set { _isViewVisible = value; OnPropertyChanged(); ErrorMessage = string.Empty; } }

        // Commands
        public ICommand LoginCommand { get; }
        public ICommand RecoverPasswordCommand { get; }

        // Constructor
        public LoginVM() {
            userRepository = new UserRepository();
            LoginCommand = new RelayCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
        }

        private bool CanExecuteLoginCommand(object arg) {
            bool validData;
            if (string.IsNullOrWhiteSpace(Username) || Username.Length < 3 ||
                Password == null || Password.Length < 6) {
                validData = false;
            }
            else {
                validData = true;
            }
            return validData;
        }

        private void ExecuteLoginCommand(object obj) {
            bool isValidUser = userRepository.AuthenticateUser(Username, Password);
            if (isValidUser) {
                string role = userRepository.GetRole(Username);
                if (role.Equals("ADMIN")) {
                    var mainWindow = new Inventory();
                    mainWindow.Show();
                }
                else if (role.Equals("CASHIER")) {
                    var mainWindow = new Cashier();
                    mainWindow.Show();
                }
                else {
                    MessageBox.Show("INVALID ROLE");
                }
                // Application.Current.MainWindow.Close();
                IsViewVisible = false;
            }
            else {
                ErrorMessage = "Invalid username or password";
            }
            ClearFields();
        }

        private void ClearFields() {
            Username = string.Empty;
            Password = string.Empty;
        }
    }
}
