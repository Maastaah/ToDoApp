using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoApp.Models
{
    public interface IToDoRepository
    {
        IEnumerable<ToDoModel> AllTodos();
        IEnumerable<ToDoModel> TodosByUser(string user);
        Task<ToDoModel> GetTodoById(string user, int id);
        void Update(object model);
        void AddEntity(object model);
        Task<bool> SaveAll();

    }
}
