using System.Web.Mvc;
using StructureMap;

namespace TasksManager.Registration
{
    public static class StructureMapBootstrapper
    {
        public static Container RegisteredContainer;

        public static void Run()
        {
            ControllerBuilder.Current
                .SetControllerFactory(new StructureMapControllerFactory());

            RegisteredContainer = new Container(x =>
                x.AddRegistry(new StructureMapRegistration()));
        }
    }
}