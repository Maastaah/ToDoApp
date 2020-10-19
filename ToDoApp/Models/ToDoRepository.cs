using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoApp.Models
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<ToDoRepository> _logger;

        public ToDoRepository(ApplicationDbContext db, ILogger<ToDoRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public IEnumerable<ToDoModel> AllTodos()
        {

            try
            {
                _logger.LogInformation("AllTodos was called");
                return _db.ToDo.OrderBy(p => p.Deadline).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all todos: {ex}");
                return null;
            }
        }

        public IEnumerable<ToDoModel> TodosByUser(string username)
        {
            try
            {
                _logger.LogInformation("TodosByUser was called");
                return _db.ToDo.OrderBy(p => p.Deadline).Where(i => i.User == username).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all todos: {ex}");
                return null;
            }
        }
        public async Task<IActionResult> EditTodo(int id)
        {
            var todo = await _db.ToDo.FirstOrDefaultAsync(p => p.Id == id);
        }
        public void AddEntity(object model)
        {
            _db.Add(model);
        }
        public bool SaveAll()
        {
            return _db.SaveChanges() > 0;
        }
    }
}
