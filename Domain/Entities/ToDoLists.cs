using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ToDoListeWeb.Domain.Entities
{
    public class ToDoLists
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ToDo> ToDos { get; set; }
    }
}