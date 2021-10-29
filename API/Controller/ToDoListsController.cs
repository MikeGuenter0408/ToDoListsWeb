using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ToDoListeWeb.Domain.Entities;
using ToDoListeWeb.Infrastructure.Repositories;
using ToDoListeWeb.Infrastructure.QueryParameters;
using ToDoListeWeb.Infrastructure;

namespace ToDoListeWeb.Controller
{
    [ApiVersion("1.0")]
    [Route("v{v:apiVersion}/ToDoLists")]        //  --> URL Versioning
    //[Route("ToDoLists")]                      //  --> For Querystring and HTTP Versioning
    [ApiController]
    public class ToDoListsV1_Controller : ControllerBase
    {
        private readonly ToDoListeWebContext _context;
        private ToDoRepo toDoRepo = new ToDoRepo();
        private ToDoListRepo toDoListRepo = new ToDoListRepo();
        
        public ToDoListsV1_Controller(ToDoListeWebContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        // Get all ToDoLists
        [HttpGet]
        public IActionResult GetAllLists([FromQuery]ToDoListQueryParameters queryParameters)
        {
            var lists =  _context.ToDoLists.Include(x=>x.ToDos);
             if(lists == null)
                return NotFound();

             return Ok(toDoListRepo.FilterAndPageAllLists(_context, queryParameters));
        }

        // Get all ToDos (Search by ID, Filter and Order possible)
        [HttpGet("ToDos")]
        public IActionResult GetAllToDos([FromQuery] ToDoQueryParameters queryParameters)
        {
            IQueryable<ToDo> toDos = _context.ToDos;
            if(toDos == null)
                return NotFound();

            return Ok(toDoRepo.FilterAndPageAllToDos(_context, queryParameters));
        }

        // Get a specific ToDoList by ID
        [HttpGet, Route("{id:int}")]
        public async Task<IActionResult> GetToDoList(int id)
        {
            var toDoList = await _context.ToDoLists.Include(x=>x.ToDos).SingleOrDefaultAsync(x=>x.Id==id);
            if(toDoList == null)
                return NotFound();

            return Ok(toDoList);
        }

        // Get a specific ToDo by ID
        [HttpGet, Route("ToDos/{id:int}")]
        public async Task<IActionResult> GetToDo(int id)
        {
            var ToDo = await _context.ToDos.FindAsync(id);
            if(ToDo == null)
                return NotFound();

            return Ok(ToDo);
        }

        // Post a new ToDo
        [HttpPost("ToDos")]
        public async Task<IActionResult> PostToDo([FromBody]ToDo toDo)
        {
            _context.Add(toDo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetToDo", toDo, toDo);
        }

        // Post a new ToDoList
        [HttpPost]
        public async Task<IActionResult> PostToDoList([FromBody]ToDoLists list)
        {
            _context.Add(list);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetToDoList", list, list);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutToDoList([FromRoute] int id, [FromBody] ToDoLists list)
        {
            if(_context.ToDoLists.Find(id) == null)
                return NotFound();

            var toDoListToUpdate = _context.ToDoLists.Find(id);
            toDoListToUpdate.Name = list.Name;

            try
            {
                await _context.SaveChangesAsync();
            }catch(DbUpdateConcurrencyException)
            {
                if(_context.ToDoLists.Find(id) == null)
                    return NotFound();

                throw;
            }
            return NoContent();
        }

        [HttpPut("ToDos/{id:int}")]
        public async Task<IActionResult> PutToDo([FromRoute] int id, [FromBody] ToDo toDo)
        { 
            var toDoToUpdate = _context.ToDos.Find(id);

            if(toDoToUpdate == null)
                return NotFound();

            toDoToUpdate.Description = toDo.Description;

            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteToDoList([FromRoute] int id)
        {
            var list = await _context.ToDoLists.FindAsync(id);
            
            if(list == null)
                return NotFound();
            
            _context.ToDoLists.Remove(list);
            await _context.SaveChangesAsync();

            return Ok(list);
        }

        [HttpDelete("ToDos/{id:int}")]
        public async Task<IActionResult> DeleteToDo([FromRoute] int id)
        {
            var toDo = await _context.ToDos.FindAsync(id);
            
            if(toDo == null)
                return NotFound();
            
            _context.ToDos.Remove(toDo);
            await _context.SaveChangesAsync();

            return Ok(toDo);
        }
    }
}