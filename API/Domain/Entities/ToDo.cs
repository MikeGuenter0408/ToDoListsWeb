using System;
using System.Text.Json.Serialization;

namespace ToDoListeWeb.Domain.Entities
{
    public class ToDo
    {
        public int Id { get; set; }
        public DateTime DueDateTime { get; set; }
        public string Description { get; set; }
        public int ToDoListId { get; set; }

        [JsonIgnore]
        public virtual ToDoLists TodoList {get; set; }
    }
}