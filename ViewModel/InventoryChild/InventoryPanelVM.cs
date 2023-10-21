using JackPastil.Model;
using JackPastil.MVVM;
using JackPastil.Repository;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace JackPastil.ViewModel {
    public class InventoryPanelVM : ViewModelBase {

        // Variables
        private int currentPage = 0, pageLimit, stock, index;
        private string _pageNum, name, type;
        private Product currentProduct;

        private ObservableCollection<Product> inventory;

        // Repository
        private readonly InventoryRepository inventoryRepository;

        // Commands
        public ICommand NextPage { get; }
        public ICommand PreviousPage { get; }
        public ICommand UpdateRow { get; }
        public ICommand ClearRow { get; }

        public InventoryPanelVM() {
            inventoryRepository = new InventoryRepository();
            RefreshTable();

            // Commands
            NextPage = new RelayCommand(Forward, CanForward);
            PreviousPage = new RelayCommand(Backward, CanBackward);
            UpdateRow = new RelayCommand(Update, CanUpdate);
            ClearRow = new RelayCommand(Clear);

            Index = -1;
        }

        private void RefreshTable() {
            pageLimit = inventoryRepository.GetInventoryRange();
            if (currentPage > pageLimit) currentPage = pageLimit;
            Inventory = inventoryRepository.GetInventory(currentPage);
            PageNum = $"Page {currentPage + 1} / {pageLimit + 1}";

        }
        private void Clear(object arg) {
            ClearValues();
            currentProduct = null;
            Index = -1;
        }

        private void Update(object arg) {
            currentProduct.ProductStock = ProductStockBox;
            inventoryRepository.UpdateInventory(currentProduct);
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
                ProductNameBox = currentProduct.ProductName;
                ProductTypeBox = currentProduct.ProductType;
                ProductStockBox = currentProduct.ProductStock;
            }
        }

        private void ClearValues() {
            ProductNameBox = string.Empty;
            ProductTypeBox = string.Empty;
            ProductStockBox = 0;
        }

        public ObservableCollection<Product> Inventory { get => inventory; set { inventory = value; OnPropertyChanged(); } }
        public string PageNum { get => _pageNum; set { _pageNum = value; OnPropertyChanged(); } }
        public string ProductNameBox { get => name; set { name = value; OnPropertyChanged(); } }
        public string ProductTypeBox { get => type; set { type = value; OnPropertyChanged(); } }
        public int ProductStockBox { get => stock; set { stock = value; OnPropertyChanged(); } }
        public int Index { get => index; set { index = value; OnPropertyChanged(); } }
        public Product CurrentProduct { get => currentProduct; set { currentProduct = value; UpdateValues(); OnPropertyChanged(); } }
    }
}
