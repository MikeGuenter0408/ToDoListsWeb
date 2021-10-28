using System.Linq;
using ToDoListeWeb.Infrastructure.QueryParameters;
using ToDoListeWeb.API.Models;
using ToDoListeWeb.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ToDoListeWeb.Infrastructure.Repositories
{
    class ToDoRepo
    {
        public List<ToDo> FilterAndPageAllToDos(IQueryable<ToDo> toDos, ToDoQueryParameters queryParameters)
        {
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

        
    }
}