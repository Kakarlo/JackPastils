using JackPastil.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackPastil.Repository {
    public class InventoryRepository : RepositoryBase {
        private int range = 20;

        public int GetSaleRange() {
            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand()) {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT COUNT(TransactionID) FROM tblSales";
                using (SqlDataReader reader = command.ExecuteReader()) {
                    while (reader.Read())
                        return (int)reader[0] / range;
                }
            }
            return -1;
        }

        public ObservableCollection<Sales> GetSales(int offset) {
            ObservableCollection<Sales> sales = new ObservableCollection<Sales>();
            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand()) {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM tblSales ORDER BY TransactionDate Desc, TransactionTime DESC" +
                    $" OFFSET {(range * offset)} ROWS FETCH NEXT {range} ROWS ONLY";
                using (SqlDataReader reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        Sales s = new Sales();
                        s.TransactionID = (int) reader[0];
                        s.ProductName = (string) reader[2];
                        s.ProductQuantity = (int) reader[3];
                        s.ProductTotal = (float) Convert.ToDouble(reader[4]);
                        s.CashierUsername = (string) reader[5];
                        DateTime t = (DateTime) reader[8];
                        s.TransactionDate = t.ToString("dd-MM-yyy");
                        TimeSpan ts = (TimeSpan) reader[9];
                        s.TransactionTime = ts.ToString("c");
                        sales.Add(s);
                    }
                }
            }
            return sales;
        }
    }
}
