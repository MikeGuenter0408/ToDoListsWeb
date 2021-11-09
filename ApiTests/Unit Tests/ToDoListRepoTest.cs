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

        
        //ToDo prüfe ob die Todos mit geladen wurden
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
            var lists = CreateToDoLists();
            var list = lists[0];

            //Act
            await repo.PostToDoList(list);

            //Assert
            context.Received(1).Add(Arg.Is(list));
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

        private static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryableList = sourceList.AsQueryable();
            
            var dbSet = new Moq.Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableList.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableList.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableList.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryableList.GetEnumerator());

            return dbSet.Object;
        }
    }
}