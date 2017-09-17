using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Sqlite;
using System.Collections.Generic;
using System.Data;

namespace TasksManager.Tests.Repositories
{
    public class InMemoryDatabase
    {
        private readonly OrmLiteConnectionFactory dbFactory =
            new OrmLiteConnectionFactory(":memory:", SqliteOrmLiteDialectProvider.Instance);

        public IDbConnection OpenConnection() => dbFactory.OpenDbConnection();

        public void Insert<T>(params T[] items)
        {
            using (var db = OpenConnection())
            {
                db.CreateTableIfNotExists(typeof(T));
                foreach (var item in items)
                {
                    db.Insert(item);
                }
            }
        }

        public IEnumerable<T> SelectAll<T>()
        {
            IEnumerable<T> selections;
            using (var db = OpenConnection())
            {
                selections = db.Select<T>();
            }
            return selections;
        }
    }
}
