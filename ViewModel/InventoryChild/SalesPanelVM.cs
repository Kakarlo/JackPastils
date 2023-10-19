using JackPastil.Model;
using JackPastil.MVVM;
using JackPastil.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace JackPastil.ViewModel {
    public class SalesPanelVM : ViewModelBase{
        // Variables
        private Sales _salesIndex;

        private ObservableCollection<Sales> sales;
        public ObservableCollection<Sales> Sales { get => sales; set { sales = value; OnPropertyChanged(); } }

        public Sales SalesIndex { get => _salesIndex; set { _salesIndex = value; OnPropertyChanged(); } }

        // Repository
        private readonly InventoryRepository inventoryRepository;

        // Commands
        public ICommand DeleteItem { get; }
        public SalesPanelVM() {
            inventoryRepository = new InventoryRepository();
            Sales = inventoryRepository.GetSales(0);
            // Commands
            Check = new RelayCommand(Delete);
        }

        private void Delete(object arg) {
            MessageBox.Show(SalesIndex + "");
        }
    }
}
