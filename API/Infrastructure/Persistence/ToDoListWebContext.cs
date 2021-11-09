using System.Linq;
using Microsoft.EntityFrameworkCore;
using ToDoListeWeb.Domain.Entities;
using ToDoListeWeb.Infrastructure.Extensions;

namespace ToDoListWeb.Infrastructure
{
    public class ToDoListWebContext : DbContext, IToDoListWebContext
    {
        public ToDoListWebContext(DbContextOptions<ToDoListWebContext> options) : base(options)  
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoList>()
            .HasMany(c => c.ToDos)
            .WithOne(s => s.TodoList)
            .HasForeignKey(h => h.ToDoListId)
            .HasPrincipalKey(h => h.Id);

            modelBuilder.Entity<ToDoList>()
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<ToDo>()
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();

            modelBuilder.Seed();
        }
        public DbSet<ToDoList> ToDoLists {set; get;}
        public DbSet<ToDo> ToDos {set; get;}
            
        public IQueryable<ToDoList> GetToDoLists()
        {
            return ToDoLists.AsQueryable();
        }
    }
}