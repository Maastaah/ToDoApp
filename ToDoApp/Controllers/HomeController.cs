using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region API Calls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.ToDo.ToListAsync() });
            //try
            //{
            //    return Ok(_toDoRepository.AllTodos());
            //}
            //catch(Exception ex)
            //{
            //    _logger.LogError($"Failed to get tasks: {ex}");
            //    return BadRequest("Failed to get tasks");
            //}
        }
        [HttpGet]
        public async Task<IActionResult> GetByUser()
        {
            return Json(new { data = await _db.ToDo.Where(b => b.User == User.Identity.Name).ToListAsync() });
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id)
        {
            var toDoFromDb = await _db.ToDo.FirstOrDefaultAsync(u => u.Id == id);
            if (toDoFromDb == null)
            {
                return Json(new { success = false, message = "Error while updating" });
            }

            toDoFromDb.IsDone = true;
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Update successful" });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var toDoFromDb = await _db.ToDo.FirstOrDefaultAsync(u => u.Id == id);

            if (toDoFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _db.ToDo.Remove(toDoFromDb);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion

    }
}
