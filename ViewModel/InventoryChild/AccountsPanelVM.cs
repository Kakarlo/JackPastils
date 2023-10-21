using JackPastil.Model;
using JackPastil.MVVM;
using JackPastil.Repository;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace JackPastil.ViewModel {
    public class AccountsPanelVM : ViewModelBase {

        // Variables
        private int currentPage = 0, pageLimit, index;
        private string _pageNum, username, password, firstname, lastname, role;
        private UserModel currentProduct;

        private ObservableCollection<UserModel> accounts;

        // Repository
        private readonly InventoryRepository inventoryRepository;

        // Commands
        public ICommand NextPage { get; }
        public ICommand PreviousPage { get; }
        public ICommand UpdateRow { get; }
        public ICommand AddRow { get; }
        public ICommand DeleteRow { get; }
        public ICommand ClearRow { get; }

        public AccountsPanelVM() {
            inventoryRepository = new InventoryRepository();
            RefreshTable();

            // Commands
            NextPage = new RelayCommand(Forward, CanForward);
            PreviousPage = new RelayCommand(Backward, CanBackward);
            UpdateRow = new RelayCommand(Update, CanUpdate);
            AddRow = new RelayCommand(Add, CanAdd);
            DeleteRow = new RelayCommand(Delete, CanDelete);
            ClearRow = new RelayCommand(Clear);

            Index = -1;
        }

        private void RefreshTable() {
            pageLimit = inventoryRepository.GetAccountRange();
            if (currentPage > pageLimit) currentPage = pageLimit;
            Accounts = inventoryRepository.GetAccounts(currentPage);
            PageNum = $"Page {currentPage + 1} / {pageLimit + 1}";

        }

        private void Clear(object arg) {
            ClearValues();
            currentProduct = null;
            Index = -1;
        }

        private void Delete(object arg) {
            inventoryRepository.DeleteAccount(currentProduct);
            RefreshTable();
        }

        private bool CanDelete(object arg) {
            return currentProduct != null;
        }

        private void Add(object arg) {
            UserModel user = new UserModel();
            user.Username = AccountUsername;
            user.Password = AccountPassword;
            user.FirstName = AccountFirstname;
            user.LastName = AccountLastname;
            user.Role = AccountRole;
            inventoryRepository.AddAccount(user);
            ClearValues();
            RefreshTable();
        }

        private bool CanAdd(object arg) {
            return currentProduct == null;
        }

        private void Update(object arg) {
            currentProduct.Username = AccountUsername;
            currentProduct.Password = AccountPassword;
            currentProduct.FirstName = AccountFirstname;
            currentProduct.LastName = AccountLastname;
            currentProduct.Role = AccountRole;
            inventoryRepository.UpdateAccount(currentProduct);
            ClearValues();
            RefreshTable();
        }

        private bool CanUpdate(object arg) {
            return currentProduct != null;
        }
        private void Forward(object arg) {
            currentPage++;
            RefreshTable();
        }

        private bool CanForward(object arg) {
            return currentPage < pageLimit;
        }

        private void Backward(object arg) {
            currentPage--;
            RefreshTable();
        }

        private bool CanBackward(object arg) {
            return currentPage > 0;
        }

        private void UpdateValues() {
            if (currentProduct != null) {
                AccountUsername = currentProduct.Username;
                AccountPassword = currentProduct.Password;
                AccountFirstname = currentProduct.FirstName;
                AccountLastname = currentProduct.LastName;
                AccountRole = currentProduct.Role;
            }
        }

        private void ClearValues() {
            AccountUsername = string.Empty;
            AccountPassword = string.Empty;
            AccountFirstname = string.Empty;
            AccountLastname = string.Empty;
            AccountRole = string.Empty;
        }

        public ObservableCollection<UserModel> Accounts { get => accounts; set { accounts = value; OnPropertyChanged(); } }
        public string PageNum { get => _pageNum; set { _pageNum = value; OnPropertyChanged(); } }
        public string AccountUsername { get => username; set { username = value; OnPropertyChanged(); } }
        public string AccountPassword { get => password; set { password = value; OnPropertyChanged(); } }
        public string AccountFirstname { get => firstname; set { firstname = value; OnPropertyChanged(); } }
        public string AccountLastname { get => lastname; set { lastname = value; OnPropertyChanged(); } }
        public string AccountRole { get => role; set { role = value; OnPropertyChanged(); } }
        public int Index { get => index; set { index = value; OnPropertyChanged(); } }
        public UserModel CurrentProduct { get => currentProduct; set { currentProduct = value; UpdateValues(); OnPropertyChanged(); } }
    }
}
