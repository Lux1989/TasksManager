using System;
using System.Data;
using System.Data.SqlClient;

namespace TasksManager.Repositories
{
    public interface IDatabaseConnection
    {
        IDbConnection GetConnection();
    }

    public class DatabaseConnection : IDatabaseConnection, IDisposable
    {
        private IDbConnection _connection;

        public IDbConnection GetConnection()
        {
            return _connection ?? InitializeDbConectionAndReturnInstance();
        }

        private void SetConnection()
        {
            var connectionString =
                "data source=localhost;Database=TasksManagerDatabase;Integrated Security=SSPI";
            _connection = new SqlConnection(connectionString);
        }

        private IDbConnection InitializeDbConectionAndReturnInstance()
        {
            SetConnection();
            _connection.Open();
            return _connection;
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}