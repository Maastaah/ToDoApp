using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public ToDoModel ToDo { get; set; }

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }


        public IActionResult Index()
        {
            return View();
        }

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
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create()
        {
            if (ModelState.IsValid)
            {
                _db.ToDo.Add(ToDo);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
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
                return Json(new { success = false, message = "Error while Deleting" });
            }
            _db.ToDo.Remove(toDoFromDb);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion

    }
}
