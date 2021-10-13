using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToDoListeWeb.API.Models;
using Microsoft.EntityFrameworkCore;
using ToDoListeWeb.API.Classes;
using System.Linq;

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

        // Get all ToDoLists
        [HttpGet]
        public async Task<IActionResult> GetAllLists([FromQuery]ToDoListQueryParameters queryParameters)
        {
            IQueryable<ToDoLists> Lists = _context.ToDoLists;
            if(Lists == null)
            {
                return NotFound();
            }

            Lists = Lists
            .Skip(queryParameters.SiteSize * (queryParameters.Page -1))
            .Take(queryParameters.SiteSize);

            return Ok(await Lists.ToListAsync());
        }

        // Get all ToDos (Search by ID, Filter and Order possible)
        [HttpGet("ToDos")]
        public async Task<IActionResult> GetAllToDos([FromQuery] ToDoQueryParameters queryParameters)
        {
            IQueryable<ToDo> ToDos = _context.ToDos;
            if(ToDos == null)
            {
                return NotFound();
            }

            // Filter ToDos by time
            if (queryParameters.FromDate!=null && 
                queryParameters.ToDate!=null)
            {
                ToDos = ToDos
                .Where(
                    p => p.DateAndTime >= queryParameters.FromDate &&
                         p.DateAndTime <= queryParameters.ToDate
                );
            }

            // Search for a ToDo
            if(queryParameters.Name!=null)
            {
                ToDos = ToDos
                .Where(
                    p => p.Description.ToLower().Contains(
                        queryParameters.Name.ToLower()
                    )
                );
            }

            // Order ToDos
            if(queryParameters.SortBy!=null && queryParameters.SortOrder!=null)
            {
                if(queryParameters.SortOrder == "asc")
                {
                    if(typeof(ToDo).GetProperty(queryParameters.SortBy)!= null)
                    {
                        ToDos = ToDos.OrderBy(s => s.)
                    }
                }
                else
                {
                    ToDos = ToDos.OrderByDescending
                }
            }

            // paginate ToDos
            ToDos = ToDos
            .Skip(queryParameters.SiteSize * (queryParameters.Page - 1))
            .Take(queryParameters.SiteSize);

            return Ok(await ToDos.ToListAsync());
        }

        // Get a specific ToDoList by ID
        [HttpGet, Route("{id:int}")]
        public async Task<IActionResult> GetToDoList(int id)
        {
            var ToDoList = await _context.ToDoLists.FindAsync(id);
            if(ToDoList == null)
            {
                return NotFound();
            }
            return Ok(ToDoList);
        }

        // Get a specific ToDo by ID
        [HttpGet, Route("ToDos/{id:int}")]
        public async Task<IActionResult> GetToDo(int id)
        {
            var ToDo = await _context.ToDos.FindAsync(id);
            if(ToDo == null)
            {
                return NotFound();
            }
            return Ok(ToDo);
        }
    }
}