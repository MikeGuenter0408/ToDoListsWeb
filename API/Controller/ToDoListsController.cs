using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToDoListeWeb.Domain.Entities;
using ToDoListeWeb.Infrastructure.Repositories;
using ToDoListeWeb.Infrastructure.QueryParameters;
using ToDoListWeb.Infrastructure;

namespace ToDoListeWeb.Controller
{
    [ApiVersion("1.0")]
    [Route("v{v:apiVersion}/ToDoLists")]        //  --> URL Versioning
    //[Route("ToDoLists")]                      //  --> For Querystring and HTTP Versioning
    [ApiController]
    public class ToDoListsV1_Controller : ControllerBase
    {
        private readonly ToDoListWebContext _context;
        private ToDoRepo toDoRepo;
        private ToDoListRepo toDoListRepo;
        
        public ToDoListsV1_Controller(ToDoListWebContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
            toDoListRepo = new ToDoListRepo(_context);
            toDoRepo = new ToDoRepo(_context);
        }

        // Get all ToDoLists
        [HttpGet]
        public IActionResult GetAllLists([FromQuery]ToDoListQueryParameters queryParameters)
        {
            var lists = toDoListRepo.FilterAndPageAllLists(queryParameters);
            if(lists!=null)
                return Ok(lists);
            else
                return NotFound();
        }

        // Get all ToDos (Search by ID, Filter and Order possible)
        [HttpGet("ToDos")]
        public IActionResult GetAllToDos([FromQuery] ToDoQueryParameters queryParameters)
        {
            var toDos = toDoRepo.FilterAndPageAllToDos(queryParameters);
            if(toDos!=null)
                return Ok(toDos);
            else
                return NotFound();
        }

        // Get a specific ToDoList by ID
        [HttpGet, Route("{id:int}")]
        public async Task<IActionResult> GetToDoList(int id)
        {
            var toDoList = await toDoListRepo.GetSpecificList(id);
            if(toDoList!=null)
                return Ok(toDoList);
            else
                return NotFound();
        }

        // Get a specific ToDo by ID
        [HttpGet, Route("ToDos/{id:int}")]
        public async Task<IActionResult> GetToDo(int id)
        {
            var ToDo = await toDoRepo.GetSpecificToDo(id);
            if(ToDo!=null)
                return Ok(ToDo);
            else
                return NotFound();
        }

        // Post a new ToDo
        [HttpPost("ToDos")]
        public async Task<IActionResult> PostToDo([FromBody]ToDo toDo)
        {
            await toDoRepo.PostToDo(toDo);
            return CreatedAtAction("GetToDo", toDo, toDo);
        }

        // Post a new ToDoList
        [HttpPost]
        public async Task<IActionResult> PostToDoList([FromBody]ToDoList list)
        {
            await toDoListRepo.PostToDoList(list);
            return CreatedAtAction("GetToDoList", list, list);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutToDoList([FromRoute] int id, [FromBody] ToDoList list)
        {
            await toDoListRepo.PutToDoList(id, list);
            return NoContent();
        }

        [HttpPut("ToDos/{id:int}")]
        public async Task<IActionResult> PutToDo([FromRoute] int id, [FromBody] ToDo toDo)
        {
            await toDoRepo.PutToDo(id, toDo);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteToDoList([FromRoute] int id)
        {
            var list = await toDoListRepo.DeleteToDoList(id);
            return Ok(list);
        }

        [HttpDelete("ToDos/{id:int}")]
        public async Task<IActionResult> DeleteToDo([FromRoute] int id)
        {
            var toDo = await toDoRepo.DeleteToDo(id);
            return Ok(toDo);
        }
    }
}