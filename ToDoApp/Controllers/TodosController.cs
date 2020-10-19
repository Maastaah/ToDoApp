using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using ToDoApp.Models;
using ToDoApp.ViewModels;

namespace ToDoApp.Controllers
{


    [Route("api/[Controller]")]
    [ApiController]
    [Produces("application/json")]
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
                        //return Created($"/api/todos/{newTask.Id}", _mapper.Map<ToDoModel, ToDoViewModel>(newTask));
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
