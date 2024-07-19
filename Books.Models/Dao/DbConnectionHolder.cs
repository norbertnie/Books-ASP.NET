using System.Data;
using System.Data.SqlClient;

namespace Books.Models.Dao
{
    public class DbConnectionHolder
    {
        private readonly string _sqlconnectionstring;

        public DbConnectionHolder(string sqlconnectionstring)
        {
            _sqlconnectionstring = sqlconnectionstring;
        }
        public IDbConnection GetConnection()
        {
            var connection = new SqlConnection(_sqlconnectionstring);
            connection.Open();
            return connection;
        }
    }
}