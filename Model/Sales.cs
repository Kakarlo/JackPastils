using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackPastil.Model {
    public class Sales {
        public int TransactionID { get; set; }
        public string ProductName { get; set; }
        public int ProductQuantity { get; set; }
        public float ProductTotal { get; set; }
        public string CashierUsername { get; set; }
        public string TransactionDate { get; set; }
        public string TransactionTime { get; set; }
    }
}
