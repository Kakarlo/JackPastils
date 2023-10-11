using System.Data.SqlClient;

namespace JackPastil.Repository {
    public abstract class RepositoryBase {
        private readonly string _connectionString;

        public RepositoryBase() {
            _connectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=JackDB;Integrated Security=True";
        }
        protected SqlConnection GetConnection() {
            return new SqlConnection(_connectionString);
        }
    }
}
