using JackPastil.Model;
using System.Data.SqlClient;

namespace JackPastil.Repository {
    public class UserRepository : RepositoryBase {
        public bool AuthenticateUser(string username, string password) {
            bool validUser;
            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand()) {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from tblUser where username=@username and password=@password";
                // command.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
                // command.Parameters.Add("@password", SqlDbType.NVarChar).Value = password;
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);
                validUser = command.ExecuteScalar() != null;
            }
            return validUser;
        }

        public string GetRole(string username) {
            string role = "";
            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand()) {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from tblUser where username=@username";
                command.Parameters.AddWithValue("username", username);
                using (var reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        role = reader[3].ToString();
                    }
                }
            }
            return role;
        }

        public UserModel GetAccount(string username) {
            UserModel acc = new UserModel();
            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand()) {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from tblUser where username=@username";
                command.Parameters.AddWithValue("username", username);
                using (var reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        acc.Username = reader[1].ToString();
                        acc.FirstName = reader[4].ToString();
                        acc.LastName = reader[5].ToString();
                    }
                }
            }
            return acc;
        }
    }
}
