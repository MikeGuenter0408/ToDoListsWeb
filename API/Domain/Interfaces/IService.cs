using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoListeWeb.Domain.Entities;
using ToDoListeWeb.Infrastructure.QueryParameters;

namespace ToDoListWeb.Domain.Interfaces
{
    public interface IService
    {
        List<ToDoList> GetAllToDoLists(ToDoListQueryParameters parameters);
        List<ToDo> GetAllToDos(ToDoQueryParameters parameters);
        Task<ToDoList> GetToDoList(int id);
        Task<ToDo> GetToDo(int id);
        Task<int> PostToDoList(ToDoList toDoList);
        Task<int> PostToDo(ToDo toDo);
        Task PutToDoList(int id, ToDoList toDoList);
        Task PutToDo(int id, ToDo toDo);
        Task<ToDoList> DeleteToDoList(int id);
        Task<ToDo> DeleteToDo(int id);
    }
}