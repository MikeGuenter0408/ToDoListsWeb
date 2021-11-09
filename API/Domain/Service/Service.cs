
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoListeWeb.Domain.Entities;
using ToDoListeWeb.Infrastructure.QueryParameters;
using ToDoListWeb.Domain.Interfaces;

namespace ToDoListWeb.Domain.Service
{
    public class Service
    {
        private IToDoRepo toDoRepo;
        private IToDoListRepo toDoListRepo;
        
        public Service(IToDoListRepo tdlRepo, IToDoRepo tdRepo)
        {
            this.toDoRepo = tdRepo;
            this.toDoListRepo = tdlRepo;
        }

        public List<ToDoList> GetAllToDoLists(ToDoListQueryParameters parameters)
        {
            return toDoListRepo.FilterAndPageAllLists(parameters);
        }
        public List<ToDo> GetAllToDos(ToDoQueryParameters parameters)
        {
            return toDoRepo.FilterAndPageAllToDos(parameters);
        }
        public async Task<ToDoList> GetToDoList(int id)
        {
            return await toDoListRepo.GetSpecificList(id);
        }
        public async Task<ToDo> GetToDo(int id)
        {
            return await toDoRepo.GetSpecificToDo(id);
        }
        public async Task PostToDoList(ToDoList toDoList)
        {
            await toDoListRepo.PostToDoList(toDoList);
        }
        public async Task PostToDo(ToDo toDo)
        {
            await toDoRepo.PostToDo(toDo);
        }
        public async Task PutToDoList(int id, ToDoList toDoList)
        {
            await toDoListRepo.PutToDoList(id, toDoList);
        }
        public async Task PutToDo(int id, ToDo toDo)
        {
            await toDoRepo.PutToDo(id, toDo);
        }
        public async Task<ToDoList> DeleteToDoList(int id)
        {
            return await toDoListRepo.DeleteToDoList(id);
        }
        public async Task<ToDo> DeleteToDo(int id)
        {
            return await toDoRepo.DeleteToDo(id);
        }

    }
}