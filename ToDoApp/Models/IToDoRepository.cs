using System.Collections.Generic;

namespace ToDoApp.Models
{
    public interface IToDoRepository
    {
        IEnumerable<ToDoModel> AllTodos();
        IEnumerable<ToDoModel> TodosByUser(string user);
        ToDoModel GetTodoById(string user, int id);
        void Update(object model);
        void AddEntity(object model);
        bool SaveAll();

    }
}
