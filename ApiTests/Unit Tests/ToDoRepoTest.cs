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

            var toDos = CreateToDos();
            var toDosAsDb = toDos.AsQueryable().BuildMockDbSet();
            context.ToDos.Returns(toDosAsDb);
            
            //Act
            var filteredToDos = repo.FilterAndPageAllToDos(parameters);
        
            //Assert
            Assert.That(filteredToDos[0].Description == "dummy2");
        }

        
        
        [Test]
        public async Task ShouldGetASpecificToDo()
        {
            //Arrange
            int id = 1;
            ToDo toDoMock = CreateToDo();

            context.ToDos.FindAsync(id).Returns(toDoMock);

            //Act
            ToDo toDo = await repo.GetSpecificToDo(id);

            //Assert
            await context.ToDos.Received(1).FindAsync(id);
            Assert.That(toDo.Id==1&&toDo.Description=="dummy1");
        }

        [Test]
        public async Task ShouldAddToDo()
        {
            //Arrange
            ToDo toDoMock = CreateToDo();

            //Act
            await repo.PostToDo(toDoMock);

            //Assert
            context.Received(1).Add(Arg.Is(toDoMock));
            await context.Received(1).SaveChangesAsync();
        }

        [Test]
        public async Task ShouldPutToDo()
        {
            //Arrange
            int id = 1;
            var toDos = CreateToDos();
            ToDo toDoUp = toDos[0];
            ToDo toDoToPut = toDos[1];

            context.ToDos.Find(id).Returns(toDoUp);
            
            //Act
            await repo.PutToDo(id, toDoToPut);

            //Assert
            context.ToDos.Received(1).Find(id);
            await context.Received(1).SaveChangesAsync();
        }

        [Test]
        public async Task ShouldDeleteToDo()
        {
            //Arrange
            int id = 1;
            var toDos = CreateToDos();

            context.ToDos.FindAsync(id).Returns(toDos[0]);

            //Act
            await repo.DeleteToDo(id);

            //Assert
            await context.ToDos.Received(1).FindAsync(id);
            context.ToDos.Received(1).Remove(toDos[0]);
            await context.Received(1).SaveChangesAsync();
        }

        private static List<ToDo> CreateToDos()
        {
            return new List<ToDo>
            {
                new ToDo
                    {
                        Description = "dummy2",
                        Id = 1
                    },
                new ToDo
                    {
                        Description = "dummy1",
                        Id = 2
                    }
            };
        }
        private static ToDo CreateToDo()
        {
            return new ToDo
                    {
                        Description = "dummy1",
                        Id = 1
                    };
            
        }
    }
}