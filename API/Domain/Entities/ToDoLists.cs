using System.Collections.Generic;

namespace ToDoListeWeb.Domain.Entities
{
    //ToDo: Singular benutzen: ToDoLists -> ToDoList
    public class ToDoLists
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ToDo> ToDos { get; set; }
    }
}