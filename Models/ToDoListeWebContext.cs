using Microsoft.EntityFrameworkCore;

namespace ToDoListeWeb.API.Models
{
    public class ToDoListeWebContext : DbContext
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
        public DbSet<ToDoLists> ToDoLists {get; set;}
        public DbSet<ToDo> ToDos {get; set; }
    }
}