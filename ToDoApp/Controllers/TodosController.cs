using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using ToDoApp.Models;
using ToDoApp.ViewModels;

namespace ToDoApp.Controllers
{
    public class TodosController : Controller
    {
        private readonly IToDoRepository _toDoRepository;
        private readonly ILogger<TodosController> _logger;
        private readonly IMapper _mapper;

        public TodosController(IToDoRepository toDoRepository, ILogger<TodosController> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _toDoRepository = toDoRepository;
        }


        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult Get()
        {
            try
            {
                var results = _toDoRepository.AllTodos();
                return Ok(_mapper.Map<IEnumerable<ToDoModel>, IEnumerable<ToDoViewModel>>(results));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get tasks: {ex}");
                return BadRequest("Failed to get tasks");
            }
        }

        [HttpGet("{username}")]
        public IActionResult Get(string username)
        {
            try
            {
                var results = _toDoRepository.TodosByUser(username);
                return Ok(_mapper.Map<IEnumerable<ToDoModel>, IEnumerable<ToDoViewModel>>(results));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get tasks: {ex}");
                return BadRequest("Failed to get tasks");
            }
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                var results = _toDoRepository.GetTodoById(User.Identity.Name, id);
                return new JsonResult(results);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTodo(ToDoViewModel toDo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _toDoRepository.Update(toDo);
                    if (_toDoRepository.SaveAll())
                    {
                        return Json(new { success = true, message = "Success while updating" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Error while updating" });
                    }
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"Failed to update task {ex}");
            }

            return BadRequest("Failed to update task");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm] ToDoViewModel toDo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newTask = _mapper.Map<ToDoViewModel, ToDoModel>(toDo);

                    newTask.User = User.Identity.Name;
                    _toDoRepository.AddEntity(newTask);
                    if (_toDoRepository.SaveAll())
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }

            }
            catch (Exception ex)
            {

                _logger.LogError($"Failed to save new task {ex}");
            }
            return BadRequest("Failed to save new task");
        }

    }
}
