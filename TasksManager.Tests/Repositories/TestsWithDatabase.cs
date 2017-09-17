using Moq;
using NUnit.Framework;
using System.Data;
using TasksManager.Repositories;

namespace TasksManager.Tests.Repositories
{
    public abstract class TestsWithDatabase<TRepository> 
        where TRepository : Repository
    {
        protected TRepository Sut;

        protected InMemoryDatabase Database { get; private set; }
        protected Mock<IDatabaseConnection> DbConnectionFactory { get; set; }
        public IDbConnection DbConnection { get; set; }

        [SetUp]
        public virtual void Init()
        {
            Database = new InMemoryDatabase();
            DbConnection = Database.OpenConnection();
            DbConnectionFactory = new Mock<IDatabaseConnection>();
            DbConnectionFactory
                .Setup(x =>  x.GetConnection())
                .Returns(WrapConnection(DbConnection));
            Sut = InstatiateDatabase(DbConnectionFactory);
        }

        [TearDown]
        public void Dispose()
        {
            DbConnection.Close();
            Database = null;
            DbConnectionFactory = null;
            DbConnection = null;
        }

        protected abstract TRepository InstatiateDatabase(Mock<IDatabaseConnection> connection);

        protected virtual IDbConnection WrapConnection(IDbConnection connection)
        {
            return connection;
        }
    }
}
