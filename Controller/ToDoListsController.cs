using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToDoListeWeb.API.Models;
using Microsoft.EntityFrameworkCore;
using ToDoListeWeb.API.Classes;
using System.Linq;
using ToDoListeWeb.Domain.Entities;

namespace ToDoListeWeb.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{v:apiVersion}/ToDoLists")]        //  --> URL Versioning
    //[Route("ToDoLists")]                      //  --> For Querystring and HTTP Versioning
    [ApiController]
    public class ToDoListsV1_Controller : ControllerBase
    {
        private readonly ToDoListeWebContext _context;
        public ToDoListsV1_Controller(ToDoListeWebContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        // Get all ToDoLists
        [HttpGet]
        public async Task<IActionResult> GetAllLists([FromQuery]ToDoListQueryParameters queryParameters)
        {
            IQueryable<ToDoLists> Lists = _context.ToDoLists.Include(x => x.ToDos);
            if(Lists == null)
            {
                return NotFound();
            }

            if(queryParameters.Id != 0)
            {
                Lists = Lists.Where(
                p => p.Id == queryParameters.Id
                );
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
                    p => p.DueDateTime >= queryParameters.FromDate &&
                         p.DueDateTime <= queryParameters.ToDate
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

            // Search for a ToDo by List-ID
            if(queryParameters.ListId!=0){
                ToDos = ToDos
                .Where(
                    p => p.ToDoListId.Equals(queryParameters.ListId)
                );
            }

            // Order ToDos
            if(queryParameters.SortBy!=null && queryParameters.SortOrder!=null)
            {
                if(queryParameters.SortOrder == "asc")
                {
                    if(typeof(ToDo).GetProperty(queryParameters.SortBy)!= null)
                    {
                        ToDos = ToDos.OrderByCustom(queryParameters.SortBy, queryParameters.SortOrder);
                    }
                }
                else
                {
                    if(typeof(ToDo).GetProperty(queryParameters.SortBy)!= null)
                    {
                        ToDos = ToDos.OrderByCustom(queryParameters.SortBy, queryParameters.SortOrder);
                    }
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
            var toDoList = await _context.ToDoLists.Include(x=>x.ToDos).SingleOrDefaultAsync(x=>x.Id==id);
            if(toDoList == null)
            {
                return NotFound();
            }
            return Ok(toDoList);
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
            {
                return NotFound();
            }
            
            _context.ToDoLists.Remove(list);
            await _context.SaveChangesAsync();

            return Ok(list);
        }

        [HttpDelete("ToDos/{id:int}")]
        public async Task<IActionResult> DeleteToDo([FromRoute] int id)
        {
            var toDo = await _context.ToDos.FindAsync(id);
            
            if(toDo == null)
            {
                return NotFound();
            }
            
            _context.ToDos.Remove(toDo);
            await _context.SaveChangesAsync();

            return Ok(toDo);
        }
    }






    // New Version ahead

    [ApiVersion("2.0")]
    [Route("v{b:apiVersion}/ToDoLists")]      //--> URL Versioning
    //[Route("ToDoLists")]                    //--> For Querystring and HTTP Versioning
    [ApiController]
    public class ToDoListsV2_Controller : ControllerBase
    {
        private readonly ToDoListeWebContext _context;
        public ToDoListsV2_Controller(ToDoListeWebContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        // Get all ToDoLists
        [HttpGet]
        public async Task<IActionResult> GetAllLists([FromQuery]ToDoListQueryParameters queryParameters)
        {
            IQueryable<ToDoLists> Lists = _context.ToDoLists.Where(s => s.Id==1);
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
                    p => p.DueDateTime >= queryParameters.FromDate &&
                         p.DueDateTime <= queryParameters.ToDate
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
                        ToDos = ToDos.OrderByCustom(queryParameters.SortBy, queryParameters.SortOrder);
                    }
                }
                else
                {
                    if(typeof(ToDo).GetProperty(queryParameters.SortBy)!= null)
                    {
                        ToDos = ToDos.OrderByCustom(queryParameters.SortBy, queryParameters.SortOrder);
                    }
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
            {
                return NotFound();
            }
            
            _context.ToDoLists.Remove(list);
            await _context.SaveChangesAsync();

            return Ok(list);
        }

        [HttpDelete("ToDos/{id:int}")]
        public async Task<IActionResult> DeleteToDo([FromRoute] int id)
        {
            var toDo = await _context.ToDos.FindAsync(id);
            
            if(toDo == null)
            {
                return NotFound();
            }
            
            _context.ToDos.Remove(toDo);
            await _context.SaveChangesAsync();

            return Ok(toDo);
        }
    }
}
