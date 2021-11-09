using System.Linq;
using Microsoft.EntityFrameworkCore;
using ToDoListeWeb.Infrastructure.QueryParameters;
using ToDoListeWeb.Domain.Entities;
using System.Collections.Generic;

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

            if(queryParameters.Id != 0)
            {
                lists = lists.Where(
                p => p.Id == queryParameters.Id
                );
            }  

            lists = lists
            .Skip(queryParameters.SiteSize * (queryParameters.Page -1))
            .Take(queryParameters.SiteSize);

            return lists.ToList();
        }

        public List<ToDoLists> FilterAndPageSpecificList(ToDoListQueryParameters queryParameters)
        {
            IQueryable<ToDoLists> lists = context.ToDoLists.Include(x=>x.ToDos);

            if(queryParameters.Id != 0)
            {
                lists = lists.Where(
                p => p.Id == queryParameters.Id
                );
            }  

            lists = lists
            .Skip(queryParameters.SiteSize * (queryParameters.Page -1))
            .Take(queryParameters.SiteSize);

            return lists.ToList();
        }
    }
}