using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoListeWeb.Domain.Entities;
using ToDoListeWeb.Infrastructure.QueryParameters;

namespace ToDoListWeb.Domain.Interfaces
{
    public interface IToDoRepo
    {
        List<ToDo> FilterAndPageAllToDos(ToDoQueryParameters queryParameters);
        Task<ToDo> GetSpecificToDo(int id);
        Task PostToDo(ToDo toDo);
        Task PutToDo(int id, ToDo toDo);
        Task<ToDo> DeleteToDo(int id);
    }
}