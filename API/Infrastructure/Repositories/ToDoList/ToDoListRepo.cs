using System.Linq;
using Microsoft.EntityFrameworkCore;
using ToDoListeWeb.Infrastructure.QueryParameters;
using ToDoListeWeb.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoListeWeb.Infrastructure.Repositories
{
    public class ToDoListRepo
    {
        private IToDoListeWebContext context; 
        public ToDoListRepo(IToDoListeWebContext context)
        {
            this.context = context;
        }
        public List<ToDoLists> FilterAndPageAllLists(ToDoListQueryParameters queryParameters)
        {
            IQueryable<ToDoLists> lists = context.ToDoLists.Include(x => x.ToDos);
            
            if(queryParameters.Name!=null)
                lists = lists
                .Where(x=>x.Name.Equals(queryParameters.Name));

            lists = lists
            .Skip(queryParameters.SiteSize * (queryParameters.Page -1))
            .Take(queryParameters.SiteSize);

            return lists.ToList();
        }

        public async Task<ToDoLists> GetSpecificList(int id)
        {
            var lists = await context.GetToDoLists().Include(x=>x.ToDos).SingleOrDefaultAsync(x=>x.Id==id);
            return lists;
        }

        public async Task PostToDoList(ToDoLists list)
        {
            context.Add(list);
            await context.SaveChangesAsync();
        }

        public async Task PutToDoList(int id, ToDoLists list)
        {
            var toDoListToUpdate = context.ToDoLists.Find(id);
            toDoListToUpdate.Name = list.Name;
            
            await context.SaveChangesAsync();
        }

        public async Task<ToDoLists> DeleteToDoList(int id)
        {
            var list = await context.ToDoLists.FindAsync(id);
            
            context.ToDoLists.Remove(list);
            await context.SaveChangesAsync();

            return list;
        }
    }
}