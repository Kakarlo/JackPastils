using JackPastil.Model;
using JackPastil.MVVM;
using JackPastil.Repository;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace JackPastil.ViewModel {
    public class ProductsPanelVM : ViewModelBase {

        // Variables
        private int currentPage = 0, pageLimit, index;
        private string _pageNum, name, type;
        private float price;
        private Product currentProduct;

        private ObservableCollection<Product> products;

        // Repository
        private readonly InventoryRepository inventoryRepository;

        // Commands
        public ICommand NextPage { get; }
        public ICommand PreviousPage { get; }
        public ICommand UpdateRow { get; }
        public ICommand AddRow { get; }
        public ICommand DeleteRow { get; }
        public ICommand ClearRow { get; }

        public ProductsPanelVM() {
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
            pageLimit = inventoryRepository.GetProductsRange();
            if (currentPage > pageLimit) currentPage = pageLimit;
            Products = inventoryRepository.GetProducts(currentPage);
            PageNum = $"Page {currentPage + 1} / {pageLimit + 1}";

        }

        private void Clear(object arg) {
            ClearValues();
            currentProduct = null;
            Index = -1;
        }

        private void Delete(object arg) {
            inventoryRepository.DeleteProduct(currentProduct);
            RefreshTable();
        }

        private bool CanDelete(object arg) {
            return currentProduct != null;
        }

        private void Add(object arg) {
            Product p = new Product();
            p.ProductName = ProductNameBox;
            p.ProductType = ProductTypeBox;
            p.ProductPrice = ProductPriceBox;
            inventoryRepository.AddProduct(p);
            ClearValues();
            RefreshTable();
        }

        private bool CanAdd(object arg) {
            return currentProduct == null;
        }

        private void Update(object arg) {
            currentProduct.ProductName = ProductNameBox;
            currentProduct.ProductType = ProductTypeBox;
            currentProduct.ProductPrice = ProductPriceBox;
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
                ProductPriceBox = currentProduct.ProductPrice;
            }
        }

        private void ClearValues() {
            ProductNameBox = string.Empty;
            ProductTypeBox = string.Empty;
            ProductPriceBox = 0;
        }

        public ObservableCollection<Product> Products { get => products; set { products = value; OnPropertyChanged(); } }
        public string PageNum { get => _pageNum; set { _pageNum = value; OnPropertyChanged(); } }
        public string ProductNameBox { get => name; set { name = value; OnPropertyChanged(); } }
        public string ProductTypeBox { get => type; set { type = value; OnPropertyChanged(); } }
        public float ProductPriceBox { get => price; set { price = value; OnPropertyChanged(); } }
        public int Index { get => index; set { index = value; OnPropertyChanged(); } }
        public Product CurrentProduct { get => currentProduct; set { currentProduct = value; UpdateValues(); OnPropertyChanged(); } }
    }
}
