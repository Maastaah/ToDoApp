using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using ToDoApp.Models;
using ToDoApp.ViewModels;
namespace ToDoApp.Controllers
{

    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IToDoRepository _toDoRepository;

        [BindProperty]
        public ToDoModel ToDo { get; set; }

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, IToDoRepository toDoRepository)
        {
            _logger = logger;
            _db = db;
            _toDoRepository = toDoRepository;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var results = _toDoRepository.AllTodos();
            ToDoViewModel taskList = new ToDoViewModel
            {
                AllTasks = _toDoRepository.TodosByUser(User.Identity.Name)
            };
            return View(taskList);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
