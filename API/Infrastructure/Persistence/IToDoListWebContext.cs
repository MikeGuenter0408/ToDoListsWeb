using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ToDoListeWeb.Domain.Entities;

namespace ToDoListWeb.Infrastructure
{
    public interface IToDoListWebContext
    {
        DbSet<ToDoList> ToDoLists { get; set; }
        DbSet<ToDo> ToDos { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        EntityEntry<TEntity> Add<TEntity>([NotNullAttribute] TEntity entity) where TEntity : class;
        IQueryable<ToDoList> GetToDoLists();
    }
}