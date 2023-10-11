using System;

namespace JackPastil.Model {
    public class Product {
        public DateTime EventStarted { get; set; }
        public DateTime EventCompleted { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public float ProductPrice { get; set; }
        public string ProductType { get; set; }
        public int ProductStock { get; set; }

    }
}
