using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToDoListeWeb.Domain.Entities;
using ToDoListeWeb.Infrastructure.Repositories;
using ToDoListeWeb.Infrastructure.QueryParameters;
using ToDoListWeb.Infrastructure;
using ToDoListWeb.Domain.Service;

namespace ToDoListeWeb.Controller
{
    [ApiVersion("1.0")]
    [Route("v{v:apiVersion}/ToDoLists")]        //  --> URL Versioning
    //[Route("ToDoLists")]                      //  --> For Querystring and HTTP Versioning
    [ApiController]
    public class ToDoListsV1_Controller : ControllerBase
    {
        private Service service;
        
        public ToDoListsV1_Controller(ToDoListWebContext context)
        {
            context.Database.EnsureCreated();
            ToDoRepo tdRepo = new ToDoRepo(context);
            ToDoListRepo tdlRepo = new ToDoListRepo(context);
            service = new Service(tdlRepo, tdRepo);
        }

        // Get all ToDoLists
        [HttpGet]
        public IActionResult GetAllToDoLists([FromQuery]ToDoListQueryParameters queryParameters)
        {
            var lists = service.GetAllToDoLists(queryParameters);
            return Ok(lists);
        }

        // Get all ToDos (Search by ID, Filter and Order possible)
        [HttpGet("ToDos")]
        public IActionResult GetAllToDos([FromQuery] ToDoQueryParameters queryParameters)
        {
            var toDos = service.GetAllToDos(queryParameters);
            return Ok(toDos);
        }

        // Get a specific ToDoList by ID
        [HttpGet, Route("{id:int}")]
        public async Task<IActionResult> GetToDoList(int id)
        {
            var toDoList = await service.GetToDoList(id);
            return Ok(toDoList);
        }

        // Get a specific ToDo by ID
        [HttpGet, Route("ToDos/{id:int}")]
        public async Task<IActionResult> GetToDo(int id)
        {
            var ToDo = await service.GetToDo(id);
            return Ok(ToDo);
        }

        // Post a new ToDo
        [HttpPost("ToDos")]
        public async Task<IActionResult> PostToDo([FromBody]ToDo toDo)
        {
            await service.PostToDo(toDo);
            return CreatedAtAction("GetToDo", toDo, toDo);
        }

        // Post a new ToDoList
        [HttpPost]
        public async Task<IActionResult> PostToDoList([FromBody]ToDoList list)
        {
            await service.PostToDoList(list);
            return CreatedAtAction("GetToDoList", list, list);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutToDoList([FromRoute] int id, [FromBody] ToDoList list)
        {
            await service.PutToDoList(id, list);
            return NoContent();
        }

        [HttpPut("ToDos/{id:int}")]
        public async Task<IActionResult> PutToDo([FromRoute] int id, [FromBody] ToDo toDo)
        {
            await service.PutToDo(id, toDo);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteToDoList([FromRoute] int id)
        {
            var list = await service.DeleteToDoList(id);
            return Ok(list);
        }

        [HttpDelete("ToDos/{id:int}")]
        public async Task<IActionResult> DeleteToDo([FromRoute] int id)
        {
            var toDo = await service.DeleteToDo(id);
            return Ok(toDo);
        }
    }
}