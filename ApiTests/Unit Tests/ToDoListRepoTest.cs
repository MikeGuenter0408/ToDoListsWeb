using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using NUnit.Framework;
using ToDoListeWeb.Domain.Entities;
using ToDoListeWeb.Infrastructure.QueryParameters;
using ToDoListeWeb.Infrastructure.Repositories;
using ToDoListWeb.Infrastructure;

namespace ApiTests.UnitTests
{
    public class ToDoListRepoTest
    {
        private IToDoListWebContext context;
        private ToDoListRepo repo; 
        private List<ToDoList> list;
        private Moq.Mock dbSet;
        private ToDoListQueryParameters parameters;

        [SetUp]
        public void Setup()
        {
            context = Substitute.For<IToDoListWebContext>();
            repo = new ToDoListRepo(context);
            parameters = new ToDoQueryParameters();
        }

        [Test]
        public void ShouldFilterAndPageAllLists()
        {
            //Arrange
            parameters.SortBy = "Name";
            parameters.SiteSize = 1;
            parameters.Page = 2;

            var toDoListAsDbSet = CreateToDoLists().AsQueryable().BuildMockDbSet();
            context.ToDoLists.Returns(toDoListAsDbSet);
            
            //Act
            var lists = repo.FilterAndPageAllLists(parameters);
        
            //Assert
            Assert.That(lists[0].Name == "Series");
        }

        
        
        [Test]
        public async Task ShouldGetASpecificList()
        {
            //Arrange
            parameters.SortBy = "Name";
            parameters.Id = 2;
            var listAsQueryAble = CreateToDoLists().AsQueryable()
                .BuildMock();

            context.GetToDoLists().Returns(listAsQueryAble);

            //Act
            var list = await repo.GetSpecificList(parameters.Id);

            //Assert
            Assert.That(list.Name == "Series");
            Assert.That(list.ToDos[0].Description=="dummy");
        }

        [Test]
        public async Task ShouldAddToDoList()
        {
            //Arrange
            var list = CreateToDoList();

            //Act
            await repo.PostToDoList(list);

            //Assert
            context.Received(1).Add(Arg.Is(list));
            await context.Received(1).SaveChangesAsync();
        }

        [Test]
        public async Task ShouldPutToDoList()
        {
            //Arrange
            var lists = CreateToDoLists();
            int id = 1;
            var listUp = lists[0];
            var listToPut = lists[0];
            listToPut.ToDos[0].Description = "Put";
            context.ToDoLists.Find(id).Returns(listUp);

            //Act
            await repo.PutToDoList(id, listToPut);

            //Assert
            context.Received(2).ToDoLists.Find(id); 
            await context.Received(1).SaveChangesAsync();
        }

        [Test]
        public async Task ShouldDeleteToDoList()
        {
            //Arrange
            int id = 1;
            var listToDelete = CreateToDoList();
            context.ToDoLists.FindAsync(id).Returns(listToDelete);

            //Act
            await repo.DeleteToDoList(id);

            //Assert
            context.ToDoLists.Received(1).Remove(listToDelete);
            await context.Received(1).SaveChangesAsync();
        }

        private static List<ToDoList> CreateToDoLists()
        {
            return new List<ToDoList>
            {
                new ToDoList()
                {
                    Name = "Shopping",
                    Id = 1,
                    ToDos = new List<ToDo>
                    {
                        new ToDo
                        {
                            Description = "dummy",
                            Id = 1
                        }
                    }
                },
                new ToDoList()
                {
                    Name = "Series",
                    Id = 2,
                    ToDos = new List<ToDo>
                    {
                        new ToDo
                        {
                            Description = "dummy",
                            Id = 2
                        }
                    }
                }
            };
        }
        public static ToDoList CreateToDoList()
        {
            return new ToDoList()
            {
                Name = "Shopping",
                Id = 1,
                ToDos = new List<ToDo>
                {
                    new ToDo
                    {
                        Description = "dummy",
                        Id = 1
                    }
                }
            };
        }
    }
}