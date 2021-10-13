using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToDoListeWeb.API.Models;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> GetAllLists()
        {
            var Lists = await _context.ToDoLists.ToListAsync();
            if(Lists == null)
                return NotFound();
            return Ok(Lists);
        }

        [HttpGet("hello")]
        public async Task<IActionResult> GetAllToDos()
        {
            var ToDos = await _context.ToDos.ToListAsync();
            if(ToDos == null)
                return NotFound();
            return Ok(ToDos);
        }

        [HttpGet, Route("{id:int}")]
        public async Task<IActionResult> GetToDoList(int id)
        {
            var ToDoList = await _context.ToDoLists.FindAsync(id);
            if(ToDoList == null)
                return NotFound();
            return Ok(ToDoList);
        }
    }
}