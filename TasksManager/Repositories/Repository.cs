using System;
using System.Data;

namespace TasksManager.Repositories
{
    public abstract class Repository
    {
        IDatabaseConnection _connection;

        protected Repository(IDatabaseConnection connection)
        {
            _connection = connection;
        }

        protected void ExecuteWithinTransaction(Action<IDbConnection, IDbTransaction> action)
        {
            var connection = _connection.GetConnection();
            
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    action.Invoke(connection, transaction);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}