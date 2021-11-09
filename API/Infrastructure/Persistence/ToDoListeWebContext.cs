using System.Linq;
using Microsoft.EntityFrameworkCore;
using ToDoListeWeb.Domain.Entities;
using ToDoListeWeb.Infrastructure.Extensions;

namespace ToDoListeWeb.Infrastructure
{
    public class ToDoListeWebContext : DbContext, IToDoListeWebContext
    {
        public ToDoListeWebContext(DbContextOptions<ToDoListeWebContext> options) : base(options)  
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoLists>()
            .HasMany(c => c.ToDos)
            .WithOne(s => s.TodoList)
            .HasForeignKey(h => h.ToDoListId)
            .HasPrincipalKey(h => h.Id);

            modelBuilder.Entity<ToDoLists>()
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<ToDo>()
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();

            modelBuilder.Seed();
        }
        public DbSet<ToDoLists> ToDoLists {set; get;}
        public DbSet<ToDo> ToDos {set; get;}

        /*public void CreateTodo(ToDo toDo)
        {
            ToDos.Add(toDo);
        }
            
        public IQueryable<ToDoLists> GetToDoLists()
        {
            return ToDoLists.AsQueryable();
        }*/
    }
}