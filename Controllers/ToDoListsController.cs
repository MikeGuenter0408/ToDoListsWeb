using Microsoft.AspNetCore.Mvc;

namespace ToDoListeWeb.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ToDoListsController : ControllerBase
    {
        [HttpGet]
        public string GetToDoLists()
        {
            return "OK";
        }
    }
}