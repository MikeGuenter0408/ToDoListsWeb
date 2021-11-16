using System.Linq;
using Microsoft.EntityFrameworkCore;
using ToDoListeWeb.Infrastructure.QueryParameters;
using ToDoListeWeb.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoListWeb.Infrastructure;
using ToDoListWeb.Domain.Interfaces;
using ToDoListeWeb.Infrastructure.Extensions;

namespace ToDoListeWeb.Infrastructure.Repositories
{
    public class ToDoListRepo : IToDoListRepo
    {
        private IToDoListWebContext context; 
        public ToDoListRepo(IToDoListWebContext context)
        {
            this.context = context;
        }
        public List<ToDoList> FilterAndPageAllLists(ToDoListQueryParameters queryParameters)
        {
            IQueryable<ToDoList> lists = context.ToDoLists.Include(x => x.ToDos);

            // Order lists
            if(queryParameters.SortBy!=null && queryParameters.SortOrder!=null)
            {
                if(queryParameters.SortOrder == "asc")
                {
                    if(typeof(ToDoList).GetProperty(queryParameters.SortBy)!= null)
                    {
                        lists = lists.OrderByCustom(queryParameters.SortBy, queryParameters.SortOrder);
                    }
                }
                else
                {
                    if(typeof(ToDoList).GetProperty(queryParameters.SortBy)!= null)
                    {
                        lists = lists.OrderByCustom(queryParameters.SortBy, queryParameters.SortOrder);
                    }
                }
            }

            // paginate lists
            lists = lists
            .Skip(queryParameters.SiteSize * (queryParameters.Page - 1))
            .Take(queryParameters.SiteSize);

            return lists.ToList();
        }

        public async Task<ToDoList> GetSpecificList(int id)
        {
            var lists = context.GetToDoLists().Include(x=>x.ToDos);
            return  await lists.SingleOrDefaultAsync(x=>x.Id==id);
        }

        public async Task<int> PostToDoList(ToDoList list)
        {
            context.Add(list);
            return await context.SaveChangesAsync();
        }

        public async Task PutToDoList(int id, ToDoList list)
        {
            var toDoListToUpdate = context.ToDoLists.Find(id);
            toDoListToUpdate.Name = list.Name;
            
            await context.SaveChangesAsync();
        }

        public async Task<ToDoList> DeleteToDoList(int id)
        {
            var list = await context.ToDoLists.FindAsync(id);
            
            context.ToDoLists.Remove(list);
            await context.SaveChangesAsync();

            return list;
        }
    }
}