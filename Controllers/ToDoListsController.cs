using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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