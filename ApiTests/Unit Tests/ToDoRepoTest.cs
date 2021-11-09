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
    public class ToDoRepoTest
    {
        private IToDoListWebContext context;
        private ToDoRepo repo; 
        private List<ToDoList> list;
        private Moq.Mock dbSet;
        private ToDoQueryParameters parameters;

        [SetUp]
        public void Setup()
        {
            context = Substitute.For<IToDoListWebContext>();
            repo = new ToDoRepo(context);
            parameters = new ToDoQueryParameters();
        }

        [Test]
        public void ShouldFilterAndPageAllToDos()
        {
            //Arrange
            parameters.SortBy = "Description";
            parameters.SiteSize = 1;
            parameters.Page = 2;

            var toDosLists = CreateToDoLists();
            List<ToDo> toDos = new List<ToDo>();
            toDos.Add(toDosLists[0].ToDos[0]); toDos.Add(toDosLists[1].ToDos[0]); 
            var toDosAsDb = toDos.AsQueryable().BuildMockDbSet();
            context.ToDos.Returns(toDosAsDb);
            
            //Act
            var filteredToDos = repo.FilterAndPageAllToDos(parameters);
        
            //Assert
            Assert.That(filteredToDos[0].Description == "dummy2");
        }

        /*
        
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
            var lists = CreateToDoLists();
            int id = 1;
            var listUp = lists[0];
            context.ToDoLists.Find(id).Returns(listUp);

            //Act
            await repo.DeleteToDoList(id);

            //Assert
            context.Received(2).ToDoLists.Find(id);
            context.Received(1).ToDoLists.Remove(listUp);
            await context.Received(1).SaveChangesAsync();
        }
*/
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
                            Description = "dummy1",
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
                            Description = "dummy2",
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