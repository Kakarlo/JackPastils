using FontAwesome.Sharp;
using JackPastil.Model;
using JackPastil.MVVM;
using JackPastil.Repository;
using JackPastil.View.InventoryChild;
using System.Windows.Input;

namespace JackPastil.ViewModel {
    public class InventoryVM : ViewModelBase {

        // Fields
        private UserModel _currentUser;
        private ViewModelBase _currentChildView;
        private string _caption;
        private IconChar _icon;

        private UserRepository userRepository;

        // Property
        public string Caption { get => _caption; set { _caption = value; OnPropertyChanged(); } }
        public IconChar Icon { get => _icon; set { _icon = value; OnPropertyChanged(); } }
        public UserModel CurrentUser { get => _currentUser; set { _currentUser = value; OnPropertyChanged(); } }
        public ViewModelBase CurrentChildView { get => _currentChildView; set { _currentChildView = value; OnPropertyChanged(); } }

        // Commands
        public ICommand ShowSalesPanel { get; }
        public ICommand ShowInventoryPanel { get; }
        public ICommand ShowProductsPanel { get; }
        public ICommand ShowAccountsPanel { get; }
        public InventoryVM() {
            userRepository = new UserRepository();
            CurrentUser = new UserModel();

            // Command Initialization
            ShowSalesPanel = new RelayCommand(ExecuteShowSalesPanel);
            ShowInventoryPanel = new RelayCommand(ExecuteShowInventoryPanel);
            ShowProductsPanel = new RelayCommand(ExecuteShowProductsPanel);
            ShowAccountsPanel = new RelayCommand(ExecuteShowAccountsPanel);
            ExecuteShowSalesPanel(null);

            LoadCurrentUserData();
        }

        private void ExecuteShowSalesPanel(object obj) {
            CurrentChildView = new SalesPanelVM();
            Caption = "Sales";
            Icon = IconChar.Wallet;
        }
        private void ExecuteShowInventoryPanel(object obj) {
            CurrentChildView = new InventoryPanelVM();
            Caption = "Inventory";
            Icon = IconChar.PieChart;
        }
        private void ExecuteShowProductsPanel(object obj) {
            CurrentChildView = new ProductsPanelVM();
            Caption = "Products";
            Icon = IconChar.ShoppingBag;
        }
        private void ExecuteShowAccountsPanel(object obj) {
            CurrentChildView = new AccountsPanelVM();
            Caption = "Accounts";
            Icon = IconChar.UserGear;
        }

        private void LoadCurrentUserData() {
            // Make after bumalik ang internet
        }
    }
}
