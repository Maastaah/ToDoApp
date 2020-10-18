using System.Collections.Generic;

namespace ToDoApp.Models
{
    public interface IToDoRepository
    {
        IEnumerable<ToDoModel> AllTodos();
        IEnumerable<ToDoModel> TodosByUser(string user);


        void AddEntity(object model);
        bool SaveAll();

    }
}
