using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ToDoListeWeb.Domain.Entities;

namespace ToDoListeWeb.Infrastructure
{
    public interface IToDoListeWebContext
    {
        DbSet<ToDoLists> ToDoLists { get; set; }
        DbSet<ToDo> ToDos { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        EntityEntry<TEntity> Add<TEntity>([NotNullAttribute] TEntity entity) where TEntity : class;
        IQueryable<ToDoLists> GetToDoLists();
    }
}