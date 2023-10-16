using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JackPastil.Model {
    public class CartItem : Product, INotifyPropertyChanged {

        private int _productQuantity;
        private float _productTotal;
        public int ProductQuantity { get => _productQuantity; set { _productQuantity = value; OnPropertyChanged(); } }
        public float ProductTotal { get => _productTotal; set { _productTotal = value;  OnPropertyChanged(); } }

        public CartItem(Product product) : base(product.ProductName, product.ProductPrice, product.ProductType) {
            ProductId = product.ProductId;
            ProductQuantity = 1;
            ProductTotal = ProductPrice * ProductQuantity;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
