using System.Threading.Tasks;
using NSubstitute;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ToDoListeWeb.Infrastructure.Repositories;
using ToDoListeWeb.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using ToDoListeWeb.Infrastructure;
using ToDoListeWeb.Infrastructure.QueryParameters;

namespace ApiTests.UnitTests
{
    public class ToDoListRepoTest
    {
        public IToDoListeWebContext context;
        private ToDoListRepo repo; 
        private List<ToDoLists> list;
        private Moq.Mock dbSet;
        private ToDoListQueryParameters parameters;

        [SetUp]
        public void Setup()
        {
           context = Substitute.For<IToDoListeWebContext>();
            repo = new ToDoListRepo(context);
            
            list = new List<ToDoLists>{new ToDoLists()
                {
                    Name = "Shopping",
                    Id = 1
                },
                new ToDoLists()
                {
                    Name = "Series",
                    Id = 2
                }
            };
            var mockDbSet = GetQueryableMockDbSet(list);
            context.ToDoLists.Returns(mockDbSet);

            parameters = new ToDoListQueryParameters();
        }

        [Test]
        public void ShouldFilterAndPageAllLists()
        {
            //Arrange
            parameters.SortBy = "Name";
            parameters.SiteSize = 1;
            parameters.Page = 2;

            //Act
            List<ToDoLists> listen = repo.FilterAndPageAllLists(parameters);

            //Assert
            Assert.That(listen[0].Name == "Series");
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