using System.Collections.Generic;

namespace ToDoApp.Models
{
    public interface IToDoRepository
    {
        ToDoModel GetAll();
        IEnumerable<ToDoModel> AllTodos { get; }
        IEnumerable<ToDoModel> TodosByUser(string user);
        ToDoModel GetByUser();
        ToDoModel Create();
        ToDoModel Update(int id);
        ToDoModel Delete(int id);

    }
}
