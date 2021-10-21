using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ToDoListeWeb.API.Models
{
    public class ToDoLists
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ToDo> ToDos { get; set; }
    }
}