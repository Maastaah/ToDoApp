using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApp.Models;
using ToDoApp.ViewModels;
using static ToDoApp.Helper;

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

        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View("/Views/Tasks/AddOrEdit.cshtml", new ToDoViewModel());
            else
            {
                var todoModel = await _toDoRepository.GetTodoById(User.Identity.Name, id);
                var updatedTodo = _mapper.Map<ToDoModel, ToDoViewModel>(todoModel);
                if (todoModel == null)
                {
                    return NotFound();
                }
                return View("/Views/Tasks/AddOrEdit.cshtml", updatedTodo);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("TaskId,Task,Deadline,Notes")] ToDoViewModel todoModel)
        {
            if (ModelState.IsValid)
            {
                var updatedTodo = _mapper.Map<ToDoViewModel, ToDoModel>(todoModel);
                //Update
                try
                {
                    updatedTodo.User = User.Identity.Name;
                    _toDoRepository.Update(updatedTodo);
                    await _toDoRepository.SaveAll();
                }
                catch (DbUpdateConcurrencyException)
                {
                    _logger.LogError("DB probleem");
                }
                return RedirectToAction("Index", "Home");
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", todoModel) });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ToDoViewModel toDo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newTask = _mapper.Map<ToDoViewModel, ToDoModel>(toDo);

                    newTask.User = User.Identity.Name;
                    _toDoRepository.AddEntity(newTask);
                    if (await _toDoRepository.SaveAll())
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    return View("Views/Forms/_CreateToDoForm.cshtml", toDo);
                }

            }
            catch (Exception ex)
            {

                _logger.LogError($"Failed to save new task {ex}");
            }
            return BadRequest("Failed to save new task");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                var task = await _toDoRepository.GetTodoById(User.Identity.Name, id);
                task.IsDone = true;

                _toDoRepository.Update(task);
                if (await _toDoRepository.SaveAll())
                {
                    return RedirectToAction("Index", "Home");
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Failed to update task {ex}");
            }
            return BadRequest("Failed to save new task");
        }
    }
}
