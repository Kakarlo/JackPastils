using System.Data.SqlClient;
using System.Windows.Interop;
using System.Windows.Markup;

namespace JackPastil.Repository {
    public abstract class RepositoryBase {
        private readonly string _connectionString;

        public RepositoryBase() {
            _connectionString = Properties.Settings.Default.JackDB;
        }
        protected SqlConnection GetConnection() {
            return new SqlConnection(_connectionString);
        }
    }
}
