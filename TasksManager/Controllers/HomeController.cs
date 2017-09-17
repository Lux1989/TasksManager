using System.Web.Mvc;
using TasksManager.Repositories;
using TasksManager.Models;
using System.Collections.Generic;

namespace TasksManager.Controllers
{
    public class HomeController : Controller
    {
        ITasksRepository _tasksManagerRepository;

        public HomeController(ITasksRepository tasksManagerRepository)
        {
            _tasksManagerRepository = tasksManagerRepository;
        }

        public ActionResult Index()
        {
            var list = new List<Task>();
            for(int a = 1; a<1000000; a++)
            {
                list.Add(
                    new Task
                    {
                        Title = "Some Title " + a,
                        Value = "Some Value"
                    }
                );
            }
            _tasksManagerRepository.AddNewTasks(list);



            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}