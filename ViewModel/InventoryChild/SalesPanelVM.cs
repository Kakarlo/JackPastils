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
        private int currentPage = 0, pageLimit, pageType = 1;
        private string _pageNum;
        private Sales _salesIndex;

        private ObservableCollection<Sales> sales;
        public ObservableCollection<Sales> Sales { get => sales; set { sales = value; OnPropertyChanged(); } }

        public Sales SalesIndex { get => _salesIndex; set { _salesIndex = value; OnPropertyChanged(); } }
        public string PageNum { get => _pageNum; set { _pageNum = value; OnPropertyChanged(); } }

        // Repository
        private readonly InventoryRepository inventoryRepository;

        // Commands
        public ICommand DeleteItem { get; }
        public ICommand NextPage { get; }
        public ICommand PreviousPage { get; }
        public ICommand Daily { get; }
        public ICommand Monthly { get; }
        public ICommand Total { get; }
       

        public SalesPanelVM() {
            inventoryRepository = new InventoryRepository();
            RefreshTable();

            // Commands
            DeleteItem = new RelayCommand(Delete, CanDelete);
            NextPage = new RelayCommand(Forward, CanForward);
            PreviousPage = new RelayCommand(Backward, CanBackward);

            Daily = new RelayCommand(DailySales);
            Monthly = new RelayCommand(MonthlySales);
            Total = new RelayCommand(TotalSales);
            RefreshTable();
        }

        private void TotalSales(object arg) {
            pageType = 3;
            currentPage = 0;
            RefreshTable();
        }

        private void MonthlySales(object arg) {
            pageType = 2;
            currentPage = 0;
            RefreshTable();
        }

        private void DailySales(object arg) {
            pageType = 1;
            currentPage = 0;
            RefreshTable();
        }

        private void RefreshTable() {
            if (pageType == 1) {
                pageLimit = inventoryRepository.GetDailySaleRange();
                if (currentPage > pageLimit) currentPage = pageLimit;
                Sales = inventoryRepository.GetDailySales(currentPage);
                PageNum = $"Page {currentPage + 1} / {pageLimit + 1}";
            } else if (pageType == 2) {
                pageLimit = inventoryRepository.GetMonthlySaleRange();
                if (currentPage > pageLimit) currentPage = pageLimit;
                Sales = inventoryRepository.GetMonthlySales(currentPage);
                PageNum = $"Page {currentPage + 1} / {pageLimit + 1}";
            } else if (pageType == 3) {
                pageLimit = inventoryRepository.GetSaleRange();
                if (currentPage > pageLimit) currentPage = pageLimit;
                Sales = inventoryRepository.GetSales(currentPage);
                PageNum = $"Page {currentPage + 1} / {pageLimit + 1}";
            }
            
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

        private void Delete(object arg) {
            inventoryRepository.DeleteSale(SalesIndex);
            RefreshTable();
        }

        private bool CanDelete(object arg) {
            return SalesIndex != null;
        }
    }
}
