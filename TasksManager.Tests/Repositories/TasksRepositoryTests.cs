using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TasksManager.Repositories;
using Moq;
using System.Data;
using ServiceStack.OrmLite;

namespace TasksManager.Tests.Repositories
{
    [TestFixture]
    public class TasksRepositoryTests : TestsWithDatabase<TasksRepository>
    {
        [Test]
        public void Should_be_able_to_insert_new_tasks_to_database()
        {
            var list = new List<Models.Task>();
            for (int i = 0; i < 3210; i++)
                list.Add(new Models.Task()
                {
                    Title = "Title" + i,
                    Value = "Value" + i
                });           

            Sut.AddNewTasks(list);

            var allTasks = Database
                .SelectAll<Models.Task>();

            Assert.AreEqual(allTasks.Count(), list.Count(), 
                "Database should after inserting should contain proper count of entities");
        }

        [Test]
        public void Should_be_able_to_delete_tasks_from_database()
        {
            var list = new List<Models.Task>();
            for (int i = 1; i <= 3210; i++)
                list.Add(new Models.Task()
                {
                    Title = "Title" + i,
                    Value = "Value" + i
                });

            Sut.AddNewTasks(list);

            Sut.DeleteTasks(1, 51, 501);

            var allTasks = Database
                .SelectAll<Models.Task>();

            var values = allTasks
                .Select(x => x.Value)
                .ToList();

            Assert.AreEqual(list.Count() - 3, allTasks.Count(), "Database should have removed 3 entities");
            Assert.That(values, Does.Not.Contains("Value1"), "Database should contain 'Value1' entity");
            Assert.That(values, Does.Not.Contains("Value51"), "Database should contain 'Value51' entity");
            Assert.That(values, Does.Not.Contains("Value501"), "Database should contain 'Value501' entity");
        }

        [Test]
        public void Should_be_able_to_update_tasks_from_database()
        {
            var list = new List<Models.Task>();
            for (int i = 1; i <= 3210; i++)
                list.Add(new Models.Task()
                {
                    Title = "Title" + i,
                    Value = "Value" + i
                });

            Sut.AddNewTasks(list);

            var taskToUpdateIds = new List<int>() { 1, 444, 3210 };
            var tasksToUpdate = Database
                .SelectAll<Models.Task>()
                .Where(x => taskToUpdateIds.Contains(x.Id))
                .ToList();
            tasksToUpdate.ForEach(x => x.Title = x.Title + "_Updated");

            Sut.UpdateExistingTasks(tasksToUpdate);

            var updatedTasks = Database
                .SelectAll<Models.Task>()
                .Where(x => x.Title.EndsWith("_Updated"))
                .ToList();

            var titles = updatedTasks.Select(x => x.Title).ToList();

            Assert.AreEqual(updatedTasks.Count(), 3,
                "Should update proper count of Tasks");
        }

        protected override TasksRepository InstatiateDatabase(Mock<IDatabaseConnection> connection)
        {
            return new TasksRepository(connection.Object);
        }

        protected override IDbConnection WrapConnection(IDbConnection connection)
        {
            connection.CreateTableIfNotExists(typeof(Models.Task));
            return base.WrapConnection(connection);
        }
    }
}
