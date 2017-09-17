using System.Collections.Generic;
using TasksManager.Models;
using Dapper;
using System.Linq;

namespace TasksManager.Repositories
{
    public interface ITasksRepository
    {
        void AddNewTasks(IEnumerable<Task> tasks);
        void UpdateExistingTasks(IEnumerable<Task> tasks);
        void DeleteTasks(params int[] tasksIds);
    }

    public class TasksRepository : Repository, ITasksRepository
    {
        public TasksRepository(IDatabaseConnection connection) : base(connection) { }

        public void AddNewTasks(IEnumerable<Task> tasks)
        {
            // Chunking tasks to not kill our database during inserting
            var tasksChanks = tasks.ChunkBy(chunkSize: 100);
            foreach (var chank in tasksChanks)
            {
                ExecuteWithinTransaction((connection, transaction) =>
                {
                    connection.Execute(
                    "INSERT INTO Task(Title, Value) VALUES (@Title, @Value);",
                    chank,
                    transaction);
                });
            }
        }

        public void UpdateExistingTasks(IEnumerable<Task> tasks)
        {
            var tasksChanks = tasks.ChunkBy(chunkSize: 100);
            foreach (var chank in tasksChanks) {
                ExecuteWithinTransaction((connection, transaction) =>
                {
                    foreach (var task in chank)
                    {
                        string sql = "UPDATE Task " +
                           "SET Title = @Title, Value = @Value " +
                           "WHERE Id = @Id";

                        connection.Execute(sql, task);
                    }
    
                });
            }
        }

        public void DeleteTasks(params int[] tasksIds)
        {
            var tasksIdsChanks = tasksIds.ChunkBy(chunkSize: 100);
            foreach (var idsChank in tasksIdsChanks)
            {
                ExecuteWithinTransaction((connection, transaction) =>
                {
                    connection.Execute(
                        string.Format("DELETE FROM Task WHERE Id IN ({0})", 
                        string.Join(",",tasksIds.ToArray())),
                    transaction);
                });
            }
        }
    }
}