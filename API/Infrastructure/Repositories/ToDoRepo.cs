using System.Linq;
using ToDoListeWeb.Infrastructure.QueryParameters;
using ToDoListeWeb.Domain.Entities;
using System.Collections.Generic;
using ToDoListeWeb.Infrastructure.Extensions;
using System.Threading.Tasks;
using ToDoListWeb.Infrastructure;
using ToDoListWeb.Domain.Interfaces;

namespace ToDoListeWeb.Infrastructure.Repositories
{
    public class ToDoRepo : IToDoRepo
    {
        private IToDoListWebContext context; 
        public ToDoRepo(IToDoListWebContext context)
        {
            this.context = context;
        }
        public List<ToDo> FilterAndPageAllToDos(ToDoQueryParameters queryParameters)
        {
            IQueryable<ToDo> toDos = context.ToDos;

            // Filter ToDos by time
            if (queryParameters.FromDate!=null && 
                queryParameters.ToDate!=null)
            {
                toDos = toDos
                .Where(
                    p => p.DueDateTime >= queryParameters.FromDate &&
                         p.DueDateTime <= queryParameters.ToDate
                );
            }

            // Search for a ToDo
            if(queryParameters.Name!=null)
            {
                toDos = toDos
                .Where(
                    p => p.Description.ToLower().Contains(
                        queryParameters.Name.ToLower()
                    )
                );
            }

            // Search for a ToDo by List-ID
            if(queryParameters.ListId!=0){
                toDos = toDos
                .Where(
                    p => p.ToDoListId.Equals(queryParameters.ListId)
                );
            }

            // Order ToDos
            if(queryParameters.SortBy!=null && queryParameters.SortOrder!=null)
            {
                if(queryParameters.SortOrder == "asc")
                {
                    if(typeof(ToDo).GetProperty(queryParameters.SortBy)!= null)
                    {
                        toDos = toDos.OrderByCustom(queryParameters.SortBy, queryParameters.SortOrder);
                    }
                }
                else
                {
                    if(typeof(ToDo).GetProperty(queryParameters.SortBy)!= null)
                    {
                        toDos = toDos.OrderByCustom(queryParameters.SortBy, queryParameters.SortOrder);
                    }
                }
            }

            // paginate ToDos
            toDos = toDos
            .Skip(queryParameters.SiteSize * (queryParameters.Page - 1))
            .Take(queryParameters.SiteSize);

            return toDos.ToList();
        }

        public async Task<ToDo> GetSpecificToDo(int id)
        {
            var toDos = await context.ToDos.FindAsync(id);
            return toDos;
        }

        public async Task<int> PostToDo(ToDo toDo)
        {
            context.Add(toDo);
            return await context.SaveChangesAsync();
        }

        public async Task PutToDo(int id, ToDo toDo)
        {
            var toDoToUpdate = context.ToDos.Find(id);

            toDoToUpdate.Description = toDo.Description;

            await context.SaveChangesAsync();
        }

        public async Task<ToDo> DeleteToDo(int id)
        {
            var toDo = await context.ToDos.FindAsync(id);
            
            context.ToDos.Remove(toDo);
            await context.SaveChangesAsync();
            return toDo;
        }
    }
}