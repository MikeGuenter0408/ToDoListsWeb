using Microsoft.EntityFrameworkCore;
using ToDoListeWeb.Domain.Entities;

namespace ToDoListeWeb.Infrastructure.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoList>().HasData(
                new ToDoList { Id = 1, Name = "Shopping"},
                new ToDoList { Id = 2, Name = "Watching Series"},
                new ToDoList { Id = 3, Name = "Playing Games"},
                new ToDoList { Id = 4, Name = "Workout"},
                new ToDoList { Id = 5, Name = "Studying"});
            
            modelBuilder.Entity<ToDo>().HasData(
                new ToDo {Id = 1,   ToDoListId = 1, Description = "Cabbage",        DueDateTime = new System.DateTime()},
                new ToDo {Id = 2,   ToDoListId = 1, Description = "Garlic",         DueDateTime = new System.DateTime()},
                new ToDo {Id = 3,   ToDoListId = 1, Description = "Tomatoes",       DueDateTime = new System.DateTime()},

                new ToDo {Id = 4,   ToDoListId = 2, Description = "Breaking Bad",   DueDateTime = new System.DateTime(2021, 11, 1, 20, 0, 0)},
                new ToDo {Id = 5,   ToDoListId = 2, Description = "Squid Game",     DueDateTime = new System.DateTime(2021, 11, 2, 20, 0, 0)},
                new ToDo {Id = 6,   ToDoListId = 2, Description = "Karate Kid",     DueDateTime = new System.DateTime(2021, 11, 3, 20, 0, 0)},

                new ToDo {Id = 7,   ToDoListId = 3, Description = "Skyrim",         DueDateTime = new System.DateTime(2021, 11, 1, 22, 0, 0)},
                new ToDo {Id = 8,   ToDoListId = 3, Description = "Phasmophobia",   DueDateTime = new System.DateTime(2021, 11, 2, 22, 0, 0)},
                new ToDo {Id = 9,   ToDoListId = 3, Description = "CS:GO",          DueDateTime = new System.DateTime(2021, 11, 3, 22, 0, 0)},

                new ToDo {Id = 10,   ToDoListId = 4, Description = "3x16 Pullups",   DueDateTime = new System.DateTime()},
                new ToDo {Id = 11,  ToDoListId = 4, Description = "4x30 Push Ups",  DueDateTime = new System.DateTime()},
                new ToDo {Id = 12,  ToDoListId = 4, Description = "3x100 Squads",   DueDateTime = new System.DateTime()},

                new ToDo {Id = 13,  ToDoListId = 5, Description = "Math",           DueDateTime = new System.DateTime(2021, 12, 1, 7, 0, 0)},
                new ToDo {Id = 14,  ToDoListId = 5, Description = "Informatics",    DueDateTime = new System.DateTime(2021, 12, 1, 10, 0, 0)},
                new ToDo {Id = 15,  ToDoListId = 5, Description = "English",        DueDateTime = new System.DateTime(2021, 12, 1, 13, 0, 0)});
        }
    }
}