using System;
using System.Collections.Generic;
using System.Linq;

namespace ToDoApp.Models
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly ApplicationDbContext _db;

        public ToDoRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<ToDoModel> AllTodos
        {
            get
            {
                return _db.ToDo;
            }
        }

        public IEnumerable<ToDoModel> TodosByUser(string username)
        {
            return _db.ToDo.Where(b => b.User == username).ToList();
        }

        public ToDoModel Create()
        {
            throw new NotImplementedException();
        }

        public ToDoModel Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ToDoModel GetAll()
        {
            throw new NotImplementedException();
        }

        public ToDoModel GetByUser()
        {
            throw new NotImplementedException();
        }

        public ToDoModel Update(int id)
        {
            throw new NotImplementedException();
        }
    }
}
