using Microsoft.EntityFrameworkCore;

namespace ToDoListeWeb.API.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoLists>().HasData(
                new ToDoLists { Id = 1, Name = "Shopping"},
                new ToDoLists { Id = 2, Name = "Watching Series"},
                new ToDoLists { Id = 3, Name = "Playing Games"},
                new ToDoLists { Id = 4, Name = "Workout"},
                new ToDoLists { Id = 5, Name = "Studying"});
            
            modelBuilder.Entity<ToDo>().HasData(
                new ToDo {Id = 1,   ToDoListId = 0, Description = "Cabbage",        DateAndTime = new System.DateTime()},
                new ToDo {Id = 2,   ToDoListId = 0, Description = "Garlic",         DateAndTime = new System.DateTime()},
                new ToDo {Id = 3,   ToDoListId = 0, Description = "Tomatoes",       DateAndTime = new System.DateTime()},

                new ToDo {Id = 4,   ToDoListId = 1, Description = "Breaking Bad",   DateAndTime = new System.DateTime(2021, 11, 1, 20, 0, 0)},
                new ToDo {Id = 5,   ToDoListId = 1, Description = "Squid Game",     DateAndTime = new System.DateTime(2021, 11, 2, 20, 0, 0)},
                new ToDo {Id = 6,   ToDoListId = 1, Description = "Karate Kid",     DateAndTime = new System.DateTime(2021, 11, 3, 20, 0, 0)},

                new ToDo {Id = 7,   ToDoListId = 2, Description = "Skyrim",         DateAndTime = new System.DateTime(2021, 11, 1, 22, 0, 0)},
                new ToDo {Id = 8,   ToDoListId = 2, Description = "Phasmophobia",   DateAndTime = new System.DateTime(2021, 11, 2, 22, 0, 0)},
                new ToDo {Id = 9,   ToDoListId = 2, Description = "CS:GO",          DateAndTime = new System.DateTime(2021, 11, 3, 22, 0, 0)},

                new ToDo {Id = 10,   ToDoListId = 3, Description = "3x16 Pullups",   DateAndTime = new System.DateTime()},
                new ToDo {Id = 11,  ToDoListId = 3, Description = "4x30 Push Ups",  DateAndTime = new System.DateTime()},
                new ToDo {Id = 12,  ToDoListId = 3, Description = "3x100 Squads",   DateAndTime = new System.DateTime()},

                new ToDo {Id = 13,  ToDoListId = 4, Description = "Math",           DateAndTime = new System.DateTime(2021, 12, 1, 7, 0, 0)},
                new ToDo {Id = 14,  ToDoListId = 4, Description = "Informatics",    DateAndTime = new System.DateTime(2021, 12, 1, 10, 0, 0)},
                new ToDo {Id = 15,  ToDoListId = 4, Description = "English",        DateAndTime = new System.DateTime(2021, 12, 1, 13, 0, 0)});
        }
    }
}