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
        public IEnumerable<ToDoLists> GetAllLists()
        {
            return _context.ToDoLists;
        }

        [HttpGet("hello")]
        public IEnumerable<ToDo> GetAllToDos()
        {
            return _context.ToDos;
        }
    }
}