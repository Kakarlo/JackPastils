using System.Data.SqlClient;
using System.Windows.Interop;
using System.Windows.Markup;

namespace JackPastil.Repository {
    public abstract class RepositoryBase {
        private readonly string _connectionString;

        public RepositoryBase() {
            _connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Admin\\source\\repos\\JackPastils\\Database\\JacksDB.mdf;Integrated Security=True";
        }
        protected SqlConnection GetConnection() {
            return new SqlConnection(_connectionString);
        }
    }
}
