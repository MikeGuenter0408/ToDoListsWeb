using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToDoListeWeb.Domain.Entities;
using ToDoListeWeb.Infrastructure.QueryParameters;
using ToDoListWeb.Domain.Interfaces;

namespace ToDoListeWeb.Controller
{
    [ApiVersion("1.0")]
    [Route("v{v:apiVersion}/ToDoLists")]        //  --> URL Versioning
    //[Route("ToDoLists")]                      //  --> For Querystring and HTTP Versioning
    [ApiController]
    public class ToDoListsV1_Controller : ControllerBase
    {
        private readonly IService _service;
        
        public ToDoListsV1_Controller(IService service)
        {
            _service = service;
        }

        // Get all ToDoLists
        [HttpGet]
        public IActionResult GetAllToDoLists([FromQuery]ToDoListQueryParameters queryParameters)
        {
            var lists = _service.GetAllToDoLists(queryParameters);
            if(lists!=null)
                return Ok(lists);
            else
                return NotFound();
        }

        // Get all ToDos (Search by ID, Filter and Order possible)
        [HttpGet("ToDos")]
        public IActionResult GetAllToDos([FromQuery] ToDoQueryParameters queryParameters)
        {
            var toDos = _service.GetAllToDos(queryParameters);
            if(toDos!=null)
                return Ok(toDos);
            else
                return NotFound();
        }

        // Get a specific ToDoList by ID
        [HttpGet, Route("{id:int}")]
        public async Task<IActionResult> GetToDoList(int id)
        {
            var toDoList = await _service.GetToDoList(id);
            if(toDoList!=null)
                return Ok(toDoList);
            else
                return NotFound();
        }

        // Get a specific ToDo by ID
        [HttpGet, Route("ToDos/{id:int}")]
        public async Task<IActionResult> GetToDo(int id)
        {
            var ToDo = await _service.GetToDo(id);
            if(ToDo!=null)
                return Ok(ToDo);
            else
                return NotFound();
        }

        // Post a new ToDo
        [HttpPost("ToDos")]
        public async Task<IActionResult> PostToDo([FromBody]ToDo toDo)
        {
            int responseColumn = await _service.PostToDo(toDo);
            if(responseColumn!=0)
                return CreatedAtAction("GetToDo", toDo, toDo);
            else
                return BadRequest();
        }

        // Post a new ToDoList
        [HttpPost]
        public async Task<IActionResult> PostToDoList([FromBody]ToDoList list)
        {
            int responseColumn = await _service.PostToDoList(list);
            if(responseColumn!=0)
                return CreatedAtAction("GetToDoList", list, list);
            else
                return BadRequest();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutToDoList([FromRoute] int id, [FromBody] ToDoList list)
        {
            await _service.PutToDoList(id, list);
            return NoContent();
        }

        [HttpPut("ToDos/{id:int}")]
        public async Task<IActionResult> PutToDo([FromRoute] int id, [FromBody] ToDo toDo)
        {
            await _service.PutToDo(id, toDo);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteToDoList([FromRoute] int id)
        {
            var list = await _service.DeleteToDoList(id);
            return Ok(list);
        }

        [HttpDelete("ToDos/{id:int}")]
        public async Task<IActionResult> DeleteToDo([FromRoute] int id)
        {
            var toDo = await _service.DeleteToDo(id);
            return Ok(toDo);
        }
    }
}