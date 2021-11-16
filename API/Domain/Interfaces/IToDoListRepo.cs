using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoListeWeb.Domain.Entities;
using ToDoListeWeb.Infrastructure.QueryParameters;

namespace ToDoListWeb.Domain.Interfaces
{
    public interface IToDoListRepo
    {
        List<ToDoList> FilterAndPageAllLists(ToDoListQueryParameters queryParameters);
        Task<ToDoList> GetSpecificList(int id);
        Task<int> PostToDoList(ToDoList list);
        Task PutToDoList(int id, ToDoList list);
        Task<ToDoList> DeleteToDoList(int id);
    }
}