using System.Threading.Tasks;
using NSubstitute;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ToDoListeWeb.Infrastructure.Repositories;
using ToDoListeWeb.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using ToDoListeWeb.Infrastructure;

namespace ApiTests.UnitTests
{
    public class ToDoListRepoTest
    {
        //public IToDoListeWebContext context;
        private ToDoListRepo repo; 
        private List<ToDoLists> list;
        private Moq.Mock dbSet;

        [SetUp]
        public void Setup()
        {
           //context = Substitute.For<IToDoListeWebContext>();
            repo = new ToDoListRepo();
            
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
            //context.ToDoLists.Returns(mockDbSet);
        }

        [Test]
        public void Test1()
        {
            //Arrange
            Assert.That(true);

            //Act

            //Assert
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