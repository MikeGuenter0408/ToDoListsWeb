using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using NUnit.Framework;
using ToDoListeWeb.Domain.Entities;
using ToDoListeWeb.Infrastructure;
using ToDoListeWeb.Infrastructure.QueryParameters;
using ToDoListeWeb.Infrastructure.Repositories;

namespace ApiTests.UnitTests
{
    public class ToDoListRepoTest
    {
        private IToDoListeWebContext context;
        private ToDoListRepo repo; 
        private List<ToDoLists> list;
        private Moq.Mock dbSet;
        //ToDo parameter sollte nicht global in der Klasse sein
        //Tests überschreiben sich gegenseitig den Wert des Parameters
        //Kann zu unterschiedlichem  Verhalten führen, je nachdem in welcher Reihenfolge die Tests ausgeführt werden
        private ToDoListQueryParameters parameters;

        [SetUp]
        public void Setup()
        {
            //ToDo: Englische Bezeichnung für ToDoList benutzen. Bezeichnung der Entity konstant halten. Hier IToDoListeWebContext
            context = Substitute.For<IToDoListeWebContext>();
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

            var toDoListAsDbSet = CreateToDoList().AsQueryable().BuildMockDbSet();
            context.ToDoLists.Returns(toDoListAsDbSet);
            
            //Act
            //ToDo Englische Variablennamen
            var listen = repo.FilterAndPageAllLists(parameters);
            //ToDo: Erneutes ToList() nicht nötig. Return Wert ist schon vom Typ List<ToDoLists>
            var listenAsList = listen.ToList();
        
            //Assert
            Assert.That(listen[0].Name == "Series");
        }

        
        //ToDo prüfe ob die Todos mit geladen wurden
        [Test]
        public async Task ShouldGetASpecificList()
        {
            //Arrange
            parameters.SortBy = "Name";
            parameters.Id = 2;
            var listAsQueryAble = CreateToDoList().AsQueryable()
                .BuildMock();

            context.GetToDoLists().Returns(listAsQueryAble);

            //Act
            var list = await repo.GetSpecificList(parameters.Id);

            //Assert
            Assert.That(list.Name == "Series");
        }

        private static List<ToDoLists> CreateToDoList()
        {
            return new List<ToDoLists>
            {
                new ToDoLists()
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
                new ToDoLists()
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