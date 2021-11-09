using System.Collections.Generic;

namespace ToDoListeWeb.Domain.Entities
{
    public class ToDoList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ToDo> ToDos { get; set; }
    }
}