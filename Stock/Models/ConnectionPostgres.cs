
using Npgsql;

namespace Stock
{
    public class ConnectionPostgres
    {
        private string connectionString;
        public NpgsqlConnection connection;

        public ConnectionPostgres()
        {
            connectionString = "Host=localhost;Port=5432;Database=stock;Username=postgres;Password=1234";
            connection = new NpgsqlConnection(connectionString);
        }

        public void Open()
        {
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
        }

        public void Close()
        {
            if (connection.State != System.Data.ConnectionState.Closed)
            {
                connection.Close();
            }
        }

        public NpgsqlDataReader Execute(string query)
        {
            NpgsqlCommand command = new NpgsqlCommand(query, connection);
            return command.ExecuteReader();
        }

        public void ExecuteInsert(string query)
        {
            NpgsqlCommand command = new NpgsqlCommand(query, connection);
            command.ExecuteNonQuery();
        }
    }
}