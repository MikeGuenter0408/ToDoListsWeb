using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ToDoListeWeb.API.Models
{
    public class ToDo
    {
        public int Id { get; set; }
        public DateTime DateAndTime { get; set; }
        public string Description { get; set; }
        public int ToDoListId { get; set; }

        [JsonIgnore]
        public virtual ToDoLists TodoList {get; set; }
    }
}