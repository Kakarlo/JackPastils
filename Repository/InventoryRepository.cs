using JackPastil.Model;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace JackPastil.Repository {
    public class InventoryRepository : RepositoryBase {
        private int range = 10;

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
        public int GetDailySaleRange() {
            DateTime d = DateTime.Now;
            String date = d.ToString("yyyy-MM-dd");
            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand()) {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"SELECT COUNT(TransactionID) FROM tblSales WHERE TransactionDate='{date}'";
                using (SqlDataReader reader = command.ExecuteReader()) {
                    while (reader.Read())
                        return (int)reader[0] / range;
                }
            }
            return -1;
        }
        public int GetMonthlySaleRange() {
            DateTime d = DateTime.Now;
            String sDate = d.ToString("yyyy-MM") + "-01";
            String eDate = d.ToString("yyyy-MM") + "-31";
            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand()) {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"SELECT COUNT(TransactionID) FROM tblSales WHERE TransactionDate>='{sDate}' AND TransactionDate<='{eDate}'";
                using (SqlDataReader reader = command.ExecuteReader()) {
                    while (reader.Read())
                        return (int)reader[0] / range;
                }
            }
            return -1;
        }
        public int GetInventoryRange() {
            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand()) {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT COUNT(ProductID) FROM tblProduct";
                using (SqlDataReader reader = command.ExecuteReader()) {
                    while (reader.Read())
                        return (int)reader[0] / range;
                }
            }
            return -1;
        }
        public int GetProductsRange() {
            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand()) {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT COUNT(ProductID) FROM tblProduct";
                using (SqlDataReader reader = command.ExecuteReader()) {
                    while (reader.Read())
                        return (int)reader[0] / range;
                }
            }
            return -1;
        }
        public int GetAccountRange() {
            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand()) {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT COUNT(UserID) FROM tblUser";
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
                        s.TransactionDate = t.ToString("yyyy-MM-dd");
                        TimeSpan ts = (TimeSpan) reader[9];
                        s.TransactionTime = ts.ToString("c");
                        sales.Add(s);
                    }
                }
            }
            return sales;
        }
        public ObservableCollection<Sales> GetDailySales(int offset) {
            DateTime d = DateTime.Now;
            String date = d.ToString("yyyy-MM-dd");
            ObservableCollection<Sales> sales = new ObservableCollection<Sales>();
            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand()) {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"SELECT * FROM tblSales WHERE TransactionDate='{date}' ORDER BY TransactionDate Desc, TransactionTime DESC" +
                    $" OFFSET {(range * offset)} ROWS FETCH NEXT {range} ROWS ONLY";
                using (SqlDataReader reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        Sales s = new Sales();
                        s.TransactionID = (int)reader[0];
                        s.ProductName = (string)reader[2];
                        s.ProductQuantity = (int)reader[3];
                        s.ProductTotal = (float)Convert.ToDouble(reader[4]);
                        s.CashierUsername = (string)reader[5];
                        DateTime t = (DateTime)reader[8];
                        s.TransactionDate = t.ToString("yyyy-MM-dd");
                        TimeSpan ts = (TimeSpan)reader[9];
                        s.TransactionTime = ts.ToString("c");
                        sales.Add(s);
                    }
                }
            }
            return sales;
        }
        public ObservableCollection<Sales> GetMonthlySales(int offset) {
            DateTime d = DateTime.Now;
            String sDate = d.ToString("yyyy-MM-") + "01";
            String eDate = d.ToString("yyyy-MM-") + "31";
            ObservableCollection<Sales> sales = new ObservableCollection<Sales>();
            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand()) {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"SELECT * FROM tblSales WHERE TransactionDate>='{sDate}' AND TransactionDate<='{eDate}' ORDER BY TransactionDate Desc, TransactionTime DESC" +
                    $" OFFSET {(range * offset)} ROWS FETCH NEXT {range} ROWS ONLY";
                using (SqlDataReader reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        Sales s = new Sales();
                        s.TransactionID = (int)reader[0];
                        s.ProductName = (string)reader[2];
                        s.ProductQuantity = (int)reader[3];
                        s.ProductTotal = (float)Convert.ToDouble(reader[4]);
                        s.CashierUsername = (string)reader[5];
                        DateTime t = (DateTime)reader[8];
                        s.TransactionDate = t.ToString("yyyy-MM-dd");
                        TimeSpan ts = (TimeSpan)reader[9];
                        s.TransactionTime = ts.ToString("c");
                        sales.Add(s);
                    }
                }
            }
            return sales;
        }
        public ObservableCollection<Product> GetInventory(int offset) {
            ObservableCollection<Product> inventory = new ObservableCollection<Product>();
            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand()) {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM tblProduct ORDER BY Type DESC" +
                    $" OFFSET {(range * offset)} ROWS FETCH NEXT {range} ROWS ONLY";
                using (SqlDataReader reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        Product p = new Product();
                        p.ProductId = (int)reader[0];
                        p.ProductType = (string)reader[1];
                        p.ProductName = (string)reader[2];
                        p.ProductStock = (int)reader[4];
                        inventory.Add(p);
                    }
                }
            }
            return inventory;
        }
        public ObservableCollection<Product> GetProducts(int offset) {
            ObservableCollection<Product> products = new ObservableCollection<Product>();
            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand()) {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM tblProduct ORDER BY Type DESC" +
                    $" OFFSET {(range * offset)} ROWS FETCH NEXT {range} ROWS ONLY";
                using (SqlDataReader reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        Product p = new Product();
                        p.ProductId = (int)reader[0];
                        p.ProductType = (string)reader[1];
                        p.ProductName = (string)reader[2];
                        p.ProductPrice = (float)Convert.ToDouble(reader[3]);
                        p.ProductStock = (int)reader[4];
                        products.Add(p);
                    }
                }
            }
            return products;
        }
        public ObservableCollection<UserModel> GetAccounts(int offset) {
            ObservableCollection<UserModel> accounts = new ObservableCollection<UserModel>();
            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand()) {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM tblUser ORDER BY Role" +
                    $" OFFSET {(range * offset)} ROWS FETCH NEXT {range} ROWS ONLY";
                using (SqlDataReader reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        UserModel user = new UserModel();
                        user.Id = (int)reader[0];
                        user.Username = (string)reader[1];
                        user.Password = (string)reader[2];
                        user.Role = (string)reader[3];
                        user.FirstName = (reader[4] == DBNull.Value) ? "" : (string)reader[4];
                        user.LastName = (reader[5] == DBNull.Value) ? "" : (string)reader[5];
                        accounts.Add(user);
                    }
                }
            }
            return accounts;
        }

        public void DeleteSale(Sales sales) {
            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand()) {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"DELETE FROM tblSales WHERE TransactionID='{sales.TransactionID}' AND ProductName='{sales.ProductName}' AND" +
                    $" TransactionDate='{sales.TransactionDate}'";
                command.ExecuteNonQuery();
            }
        }
        public void DeleteProduct(Product product) {
            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand()) {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"DELETE FROM tblProduct WHERE ProductID='{product.ProductId}'";
                command.ExecuteNonQuery();
            }
        }
        public void DeleteAccount(UserModel user) {
            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand()) {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"DELETE FROM tblUser WHERE UserID='{user.Id}'";
                command.ExecuteNonQuery();
            }
        }

        public void UpdateInventory(Product product) {
            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand()) {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"UPDATE tblProduct SET Stock='{product.ProductStock}' WHERE ProductID='{product.ProductId}'";
                command.ExecuteNonQuery();
            }
        }
        public void UpdateProduct(Product product) {
            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand()) {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"UPDATE tblProduct SET Name='{product.ProductName}', Type='{product.ProductType}', " +
                    $" Price='{product.ProductPrice}' WHERE ProductID='{product.ProductId}'";
                command.ExecuteNonQuery();
            }
        }
        public void UpdateAccount(UserModel user) {
            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand()) {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"UPDATE tblUser SET Username='{user.Username}', Password='{user.Password}', Role='{user.Role}'," +
                    $" FirstName='{user.FirstName}', LastName='{user.LastName}' WHERE UserID='{user.Id}'";
                command.ExecuteNonQuery();
            }
        }

        public void AddProduct(Product product) {
            {
                using (SqlConnection connection = GetConnection())
                using (SqlCommand command = new SqlCommand()) {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = $"INSERT INTO tblProduct (Name, Type, Price) VALUES ('{product.ProductName}', '{product.ProductType}', '{product.ProductPrice}')";
                    command.ExecuteNonQuery();
                }
            }
        }
        public void AddAccount(UserModel user) {
            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand()) {
                connection.Open();
                command.Connection = connection;
                command.CommandText = $"INSERT INTO tblUser (Username, Password, Role, FirstName, LastName) VALUES " +
                    $"('{user.Username}', '{user.Password}', '{user.Role}', '{user.FirstName}', '{user.LastName}')";
                command.ExecuteNonQuery();
            }
        }
    }
}
