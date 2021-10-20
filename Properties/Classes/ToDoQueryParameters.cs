

namespace ToDoListeWeb.API.Classes
{
    public class ToDoQueryParameters : ToDoListQueryParameters
    {
        public System.DateTime? FromDate {get; set;}
        public System.DateTime? ToDate {get; set;}
        public int ListId { get; set; }
    }
}