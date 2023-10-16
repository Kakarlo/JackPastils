using JackPastil.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace JackPastil.Repository {
    public class ProductRepository : RepositoryBase {
        private DateTime date;
        public List<Product> GetProducts() {
            List<Product> products = new List<Product>();
            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand()) {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM tblProduct ORDER BY Type DESC";
                using (SqlDataReader reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        Product p = new Product();
                        p.ProductId = (int)reader[0];
                        p.ProductType = (string)reader[1];
                        p.ProductName = (string)reader[2];
                        p.ProductPrice = (float)Convert.ToDouble(reader[3]);
                        products.Add(p);
                    }
                }
            }
            return products;
        }

        public int GetTransactionID() {
            date = DateTime.Now;
            string hold = "";
            bool present = false;
            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand()) {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT TOP 1 t.TransactionID FROM tblSales t WHERE t.TransactionDate=@date ORDER BY t.TransactionDate DESC, t.TransactionTime DESC";
                command.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
                using (SqlDataReader reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        present = true;
                        string tranID = Convert.ToString(reader[0]);
                        int ID = Convert.ToInt32(tranID.Substring(4)) + 1;
                        hold = date.ToString("MM") + date.ToString("dd") + string.Format("{0:0000}", ID);
                    }
                    if (!present) {
                        hold = date.ToString("MM") + date.ToString("dd") + "0001";
                    }
                }

            }

            return Convert.ToInt32(hold);
        }

        public void AddSale(ObservableCollection<CartItem> cartItem, UserModel account) {
            int transID = GetTransactionID();
            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand()) {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "INSERT INTO tblSales VALUES (@transactionID, @productID, @productName," +
                    " @productQuantity, @productTotalPrice, @cashierUser, @cashierFirstName, @cashierLastName," +
                    " @transactionDate, @transactionTime)";
                command.Parameters.Add("@transactionID", SqlDbType.Int);
                command.Parameters.Add("@productID", SqlDbType.Int);
                command.Parameters.Add("@productName", SqlDbType.NVarChar);
                command.Parameters.Add("@productQuantity", SqlDbType.Int);
                command.Parameters.Add("@productTotalPrice", SqlDbType.Float);
                command.Parameters.Add("@cashierUser", SqlDbType.NVarChar);
                command.Parameters.Add("@cashierFirstName", SqlDbType.NVarChar);
                command.Parameters.Add("@cashierLastName", SqlDbType.NVarChar);
                command.Parameters.Add("@transactionDate", SqlDbType.Date);
                command.Parameters.Add("@transactionTime", SqlDbType.Time);
                for (int i = 0; i < cartItem.Count; i++) {
                    command.Parameters[0].Value = transID;
                    command.Parameters[1].Value = cartItem[i].ProductId;
                    command.Parameters[2].Value = cartItem[i].ProductName;
                    command.Parameters[3].Value = cartItem[i].ProductQuantity;
                    command.Parameters[4].Value = cartItem[i].ProductTotal;
                    command.Parameters[5].Value = account.Username;
                    command.Parameters[6].Value = account.FirstName;
                    command.Parameters[7].Value = account.LastName;
                    command.Parameters[8].Value = date.ToString("dd/MM/yyyy");
                    command.Parameters[9].Value = date.ToString("HH:mm:ss");
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
