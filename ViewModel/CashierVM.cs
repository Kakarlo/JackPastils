using JackPastil.Model;
using JackPastil.MVVM;
using JackPastil.Repository;
using JackPastil.View;
using JackPastil.View.User_Control;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace JackPastil.ViewModel {

    public class CashierVM : ViewModelBase {
        // Variables
        private readonly int row = 4;
        private readonly int column = 6;
        private string _total, _numpadVal, currentPage = "";
        private int pageNum, prevIndex, currentIndex, nextIndex, _productsIndex;
        private bool _prevVis, _nextVis, withDecimal;
        private float totalAmount = 0f, numpadValue = 0f, min;

        private System.Windows.Media.Brush[] brArr = new System.Windows.Media.Brush[24];
        private bool[] vArr = new bool[24];
        private string[] tArr = new string[24];
        private float[] pArr = new float[24];

        // Repository
        private readonly UserRepository userRepository;
        private readonly UserModel userAccount;
        private readonly ProductRepository productRepository;

        // Products
        private List<Product> productList;
        // Styles
        private readonly ResourceDictionary resourceDict = new ResourceDictionary() {
            Source = new Uri("/Styles/GenStyles.xaml", UriKind.Relative)
        };

        private ObservableCollection<CartItem> products = new ObservableCollection<CartItem>();
        public ObservableCollection<CartItem> Products { get => products; set { products = value; OnPropertyChanged(); } }

        // Command
        public ICommand AddItem { get; }
        public ICommand ShowCat { get; }
        public ICommand PreviousPage { get; }
        public ICommand NextPage { get; }
        public ICommand PayOrder { get; }
        public ICommand SaveOrder { get; }
        public ICommand CancelOrder { get; }
        public ICommand ChangeQuantity { get; }
        public ICommand RemoveItem { get; }
        public ICommand AddNumber { get; }
        public ICommand DeleteNumber { get; }
        public ICommand EnterButton { get; }
        public ICommand AddDecimal { get; }
        public ICommand CloseNumberPad { get; }


        public CashierVM() {
            // Repository
            userRepository = new UserRepository();
            productRepository = new ProductRepository();
            userAccount = userRepository.GetAccount(Thread.CurrentPrincipal.Identity.Name);
            productList = productRepository.GetProducts();

            // Command Initialization
            AddItem = new RelayCommand(AddItems);
            ShowCat = new RelayCommand(ShowCategory);
            PreviousPage = new RelayCommand(Prev, CanPrev);
            NextPage = new RelayCommand(Next, CanNext);
            ChangeQuantity = new RelayCommand(ChangeQty, ProductIsSelected);
            RemoveItem = new RelayCommand(RemoveItm, ProductIsSelected);
            PayOrder = new RelayCommand(PayMenu, HasProduct);
            CancelOrder = new RelayCommand(CancelOrd, HasProduct);
            AddNumber = new RelayCommand(AddNum);
            DeleteNumber = new RelayCommand(DeleteNum);
            AddDecimal = new RelayCommand(AddNum, CanDec);
            EnterButton = new RelayCommand(AddNum, CanEnter);
            CloseNumberPad = new RelayCommand(CloseNumpad);

            // Values Initialize
            ProductsIndex = -1;
            Total = "Total: ";
            NumpadValue = "0";
            ShowCategory("All Items");
        }

        private bool CanEnter(object arg) {
            return numpadValue >= min;
        }
        private bool CanDec(object arg) {
            return withDecimal;
        }
        private void DeleteNum(object arg) {
            if (NumpadValue.Length > 1) {
                NumpadValue = NumpadValue.Remove(NumpadValue.Length - 1);
                numpadValue = (float)Convert.ToDouble(NumpadValue);
            } else if (NumpadValue.Length == 1) {
                NumpadValue = "0";
                numpadValue = (float)Convert.ToDouble(NumpadValue);
            }
        }
        private void AddNum(object arg) {
            if (arg == null) return;
            if (arg.ToString().Equals(".") && !NumpadValue.Contains(".")) {
                NumpadValue += arg.ToString();
                return;
            }
            if (numpadValue < 1000000f && !arg.ToString().Equals(".")) {
                numpadValue = (float)Convert.ToDouble( (NumpadValue + arg));
                NumpadValue = Convert.ToString(numpadValue);
            }
        }
        private void CloseNumpad(object arg) {
            numpadValue = 0;
            NumpadValue = "0";
        }
        private bool ProductIsSelected(object arg) {
            return ProductsIndex > -1;
        }
        private bool HasProduct(object arg) {
            return totalAmount > 0;
        }
        private void Numpad() {
            NumberPad np = new NumberPad();
            np.DataContext = this;
            np.ShowDialog();
        }
        private void RemoveItm(object obj) {
            totalAmount -= Products[ProductsIndex].ProductTotal;
            Products.RemoveAt(ProductsIndex);
            Total = "Total: " + String.Format("{0:0.00}", totalAmount);
        }

        private void PayMenu(object arg) {
            min = totalAmount;
            withDecimal = true;
            Numpad();
            if (numpadValue > 0) {
                PrintSale();
                //SaveOrdr();
                numpadValue = 0;
                NumpadValue = "0";
            }
        }

        private void ChangeQty(object obj) {
            min = 1;
            withDecimal = false;
            Numpad();
            if (numpadValue > 0) {
                int quantity = Convert.ToInt32(numpadValue);
                totalAmount -= Products[ProductsIndex].ProductTotal;
                Products[ProductsIndex].ProductQuantity = quantity;
                Products[ProductsIndex].ProductTotal = Products[ProductsIndex].ProductQuantity * Products[ProductsIndex].ProductPrice;
                totalAmount += Products[ProductsIndex].ProductTotal;
                Total = "Total: " + String.Format("{0:0.00}", totalAmount);
                numpadValue = 0;
                NumpadValue = "0";
            }
        }

        private bool CanNext(object arg) {
            return _nextVis;
        }

        private bool CanPrev(object arg) {
            return _prevVis;
        }

        public void ShowCategory(object arg) {
            if (!currentPage.Equals(arg)) {
                pageNum = 0;
                prevIndex = currentIndex = nextIndex = 0;
            }
            currentPage = (string)arg;
            NextVis = PrevVis = false;
            if (pageNum > 0) PrevVis = true;
            int i = currentIndex, j = 0;
            while (i < productList.Count) {
                if (j == row * column) {
                    NextVis = true;
                    nextIndex = i;
                    break;
                }
                if (arg.Equals("All Items")) {
                    SetButton(i, j++);
                }
                else {
                    if (productList[i].ProductType.Equals(arg)) {
                        SetButton(i, j++);
                    }
                }
                i++;
            }
            while (j < row * column) {
                vArr[j] = false;
                OnPropertyChanged("V" + (j++ + 1));
            }
        }

        public void AddItems(object arg) {
            int index = 0;
            for (int i = 0; i < productList.Count; i++) {
                if (productList[i].ProductName.Equals(arg)) {
                    index = i; break;
                }
            }
            CartItem test = new CartItem(productList[index]);
            bool found = false;
            for (int i = 0; i < Products.Count; i++) {
                if (Products[i].ProductName.Equals(arg)) {
                    totalAmount -= Products[i].ProductTotal;
                    Products[i].ProductQuantity = Products[i].ProductQuantity + 1;
                    Products[i].ProductTotal = Products[i].ProductQuantity * Products[i].ProductPrice;
                    totalAmount += Products[i].ProductTotal;
                    found = true;
                    break;
                }
            }
            if (!found) {
                totalAmount += test.ProductTotal;
                Products.Add(test);
            }
            Total = "Total: " + String.Format("{0:0.00}", totalAmount);
        }

        public void SaveOrdr() {
            productRepository.AddSale(Products, userAccount);
            Products.Clear();
            totalAmount = 0;
            Total = string.Empty;
        }

        public void CancelOrd(object arg) {
            Products.Clear();
            totalAmount = 0;
            Total = string.Empty;
        }

        public void Prev(object arg) {
            pageNum--;
            nextIndex = currentIndex;
            currentIndex = prevIndex;
            ShowCategory(currentPage);
        }

        public void Next(object arg) {
            pageNum++;
            prevIndex = currentIndex;
            currentIndex = nextIndex;
            ShowCategory(currentPage);
        }
        public void PrintSale() {
            string hold = "Transactional ID: " + productRepository.GetTransactionID() +
                "\n\n------------------| Jack's Pastil |------------------\n\n" +
                "No#\tProduct Name\tQty\tPrice\n";
            for (int i = 0; i < Products.Count; i++) {
                hold += $" {i,-5}\t{Products[i].ProductName,-20}\t{Products[i].ProductQuantity}\t{Products[i].ProductTotal}\n";
            }
            hold += "\n\tTotal: " + totalAmount;
            MessageBox.Show(hold);
        }

        public void SetButton(int i, int j) {
            // Color
            if (productList[i].ProductType.Equals("Main Dish")) {
                brArr[j] = resourceDict["menuCategory1"] as System.Windows.Media.Brush;
            }
            else if (productList[i].ProductType.Equals("Side Dish")) {
                brArr[j] = resourceDict["menuCategory2"] as System.Windows.Media.Brush;
            }
            else if (productList[i].ProductType.Equals("Drink")) {
                brArr[j] = resourceDict["menuCategory3"] as System.Windows.Media.Brush;
            }
            // Visibility
            vArr[j] = true;
            // Name
            tArr[j] = productList[i].ProductName;
            // Price
            pArr[j] = productList[i].ProductPrice;
            OnPropertyChanged("B" + (j + 1));
            OnPropertyChanged("V" + (j + 1));
            OnPropertyChanged("T" + (j + 1));
            OnPropertyChanged("P" + (j + 1));
        }

        //Property
        public System.Windows.Media.Brush B1 { get => brArr[0]; }
        public System.Windows.Media.Brush B2 { get => brArr[1]; }
        public System.Windows.Media.Brush B3 { get => brArr[2]; }
        public System.Windows.Media.Brush B4 { get => brArr[3]; }
        public System.Windows.Media.Brush B5 { get => brArr[4]; }
        public System.Windows.Media.Brush B6 { get => brArr[5]; }
        public System.Windows.Media.Brush B7 { get => brArr[6]; }
        public System.Windows.Media.Brush B8 { get => brArr[7]; }
        public System.Windows.Media.Brush B9 { get => brArr[8]; }
        public System.Windows.Media.Brush B10 { get => brArr[9]; }
        public System.Windows.Media.Brush B11 { get => brArr[10]; }
        public System.Windows.Media.Brush B12 { get => brArr[11]; }
        public System.Windows.Media.Brush B13 { get => brArr[12]; }
        public System.Windows.Media.Brush B14 { get => brArr[13]; }
        public System.Windows.Media.Brush B15 { get => brArr[14]; }
        public System.Windows.Media.Brush B16 { get => brArr[15]; }
        public System.Windows.Media.Brush B17 { get => brArr[16]; }
        public System.Windows.Media.Brush B18 { get => brArr[17]; }
        public System.Windows.Media.Brush B19 { get => brArr[18]; }
        public System.Windows.Media.Brush B20 { get => brArr[19]; }
        public System.Windows.Media.Brush B21 { get => brArr[20]; }
        public System.Windows.Media.Brush B22 { get => brArr[21]; }
        public System.Windows.Media.Brush B23 { get => brArr[22]; }
        public System.Windows.Media.Brush B24 { get => brArr[23]; }
        public System.Windows.Media.Brush B25 { get => brArr[24]; }
        public System.Windows.Media.Brush B26 { get => brArr[25]; }
        public bool V1 { get => vArr[0]; }
        public bool V2 { get => vArr[1]; }
        public bool V3 { get => vArr[2]; }
        public bool V4 { get => vArr[3]; }
        public bool V5 { get => vArr[4]; }
        public bool V6 { get => vArr[5]; }
        public bool V7 { get => vArr[6]; }
        public bool V8 { get => vArr[7]; }
        public bool V9 { get => vArr[8]; }
        public bool V10 { get => vArr[9]; }
        public bool V11 { get => vArr[10]; }
        public bool V12 { get => vArr[11]; }
        public bool V13 { get => vArr[12]; }
        public bool V14 { get => vArr[13]; }
        public bool V15 { get => vArr[14]; }
        public bool V16 { get => vArr[15]; }
        public bool V17 { get => vArr[16]; }
        public bool V18 { get => vArr[17]; }
        public bool V19 { get => vArr[18]; }
        public bool V20 { get => vArr[19]; }
        public bool V21 { get => vArr[20]; }
        public bool V22 { get => vArr[21]; }
        public bool V23 { get => vArr[22]; }
        public bool V24 { get => vArr[23]; }
        public bool V25 { get => vArr[24]; }
        public bool V26 { get => vArr[25]; }
        public string T1 { get => tArr[0]; }
        public string T2 { get => tArr[1]; }
        public string T3 { get => tArr[2]; }
        public string T4 { get => tArr[3]; }
        public string T5 { get => tArr[4]; }
        public string T6 { get => tArr[5]; }
        public string T7 { get => tArr[6]; }
        public string T8 { get => tArr[7]; }
        public string T9 { get => tArr[8]; }
        public string T10 { get => tArr[9]; }
        public string T11 { get => tArr[10]; }
        public string T12 { get => tArr[11]; }
        public string T13 { get => tArr[12]; }
        public string T14 { get => tArr[13]; }
        public string T15 { get => tArr[14]; }
        public string T16 { get => tArr[15]; }
        public string T17 { get => tArr[16]; }
        public string T18 { get => tArr[17]; }
        public string T19 { get => tArr[18]; }
        public string T20 { get => tArr[19]; }
        public string T21 { get => tArr[20]; }
        public string T22 { get => tArr[21]; }
        public string T23 { get => tArr[22]; }
        public string T24 { get => tArr[23]; }
        public string T25 { get => tArr[24]; }
        public string T26 { get => tArr[25]; }
        public float P1 { get => pArr[0]; }
        public float P2 { get => pArr[1]; }
        public float P3 { get => pArr[2]; }
        public float P4 { get => pArr[3]; }
        public float P5 { get => pArr[4]; }
        public float P6 { get => pArr[5]; }
        public float P7 { get => pArr[6]; }
        public float P8 { get => pArr[7]; }
        public float P9 { get => pArr[8]; }
        public float P10 { get => pArr[9]; }
        public float P11 { get => pArr[10]; }
        public float P12 { get => pArr[11]; }
        public float P13 { get => pArr[12]; }
        public float P14 { get => pArr[13]; }
        public float P15 { get => pArr[14]; }
        public float P16 { get => pArr[15]; }
        public float P17 { get => pArr[16]; }
        public float P18 { get => pArr[17]; }
        public float P19 { get => pArr[18]; }
        public float P20 { get => pArr[19]; }
        public float P21 { get => pArr[20]; }
        public float P22 { get => pArr[21]; }
        public float P23 { get => pArr[22]; }
        public float P24 { get => pArr[23]; }
        public float P25 { get => pArr[24]; }
        public float P26 { get => pArr[25]; }
        public bool PrevVis { get => _prevVis; set { _prevVis = value; OnPropertyChanged(); } }
        public bool NextVis { get => _nextVis; set { _nextVis = value; OnPropertyChanged(); } }
        public int ProductsIndex { get => _productsIndex; set { _productsIndex = value; OnPropertyChanged(); } }
        public string Total { get => _total; set { _total = value; OnPropertyChanged(); } }
        public string NumpadValue { get => _numpadVal; set { _numpadVal = value; OnPropertyChanged(); } }
    }
}
