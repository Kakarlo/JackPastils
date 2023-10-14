using System;

namespace JackPastil.Model {
    public class Product {
        public DateTime Timestamp { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public float ProductPrice { get; set; }
        public string ProductType { get; set; }

        public Product() { }

        public Product(string name, float price, string type) {
            ProductName = name;
            ProductPrice = price;
            ProductType = type;
        }

    }
}
