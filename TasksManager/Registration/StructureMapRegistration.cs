using StructureMap;
using TasksManager.Repositories;

namespace TasksManager.Registration
{
    public class StructureMapRegistration : Registry
    {
        public StructureMapRegistration()
        {
            For<IDatabaseConnection>().Use<DatabaseConnection>();

            For<ITasksRepository>().Use<TasksRepository>();
        }
    }
}