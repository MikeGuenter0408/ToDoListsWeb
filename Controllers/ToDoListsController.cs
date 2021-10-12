using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ToDoListeWeb.API.Models;

namespace ToDoListeWeb.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ToDoListsController : ControllerBase
    {
        private readonly ToDoListeWebContext _context;
        public ToDoListsController(ToDoListeWebContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }
        [HttpGet]
        public IActionResult GetAllLists()
        {
            var Lists = _context.ToDoLists;
            return Ok(Lists);
        }

        [HttpGet("hello")]
        public IActionResult GetAllToDos()
        {
            var ToDos = _context.ToDos;
            return Ok(ToDos);
        }

        [HttpGet, Route("{id:int}")]
        public IActionResult GetToDoList(int id)
        {
            var ToDoList = _context.ToDoLists.Find(id);
            if(ToDoList == null)
            {
                return NotFound();
            }
            return Ok(ToDoList);
        }
    }
}