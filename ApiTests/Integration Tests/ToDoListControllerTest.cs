using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using ToDoListeWeb.Domain.Entities;
using ToDoListWeb.Infrastructure;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ToDoListeWeb;

namespace ApiTests.IntegrationTest
{
    public class ToDoListControllerTest : IntegrationTest
    {
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            var appFactory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder=>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor  = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ToDoListWebContext>));
                    if (descriptor  != null)
                    {
                        services.Remove(descriptor );
                        services.AddDbContext<ToDoListWebContext>(options => options.UseInMemoryDatabase("TestDB"));
                    }
                });
            });
            _client = appFactory.CreateClient();
        }
        [Test]
        public async Task ShouldCarryOutGetRequest()
        {
            //Arrange

            //Act
            var response = await GetRequest<List<ToDoList>>("https://localhost:5001/v1.0/Todolists");
            
            //Assert
            Assert.That(response.Count, Is.EqualTo(5));
            Assert.That(response[0].Name, Is.EqualTo("Shopping"));
            Assert.That(response[0].ToDos[0].Description, Is.EqualTo("Cabbage"));
        }
        [Test]
        public async Task ShouldCarryOutPostRequest()
        {
            //Arrange 

            ToDoList newList = new ToDoList
            {
                Name = "TestList",
                ToDos = new List<ToDo>
                {
                    new ToDo
                    {
                        Description = "TestToDo"
                    }
                }
            };
            string jsonString = System.Text.Json.JsonSerializer.Serialize(newList);
            

            //Act
            await PostRequest<ToDoList>("https://localhost:5001/v1.0/Todolists", newList);
            var newContent = await GetRequest<List<ToDoList>>("https://localhost:5001/v1.0/Todolists");

            //Assert
            Assert.That(newContent.Count, Is.EqualTo(6));
            Assert.That(newContent[5].Name, Is.EqualTo("TestList"));
            Assert.That(newContent[5].ToDos[0].Description, Is.EqualTo("TestToDo"));
        }
        [Test]
        public async Task ShouldCarryOutDeleteRequest()
        {
            //Arrange

            //Act
            await DeleteRequest("https://localhost:5001/v1.0/Todolists/5");
            var listWithDeletedItem = await GetRequest<List<ToDoList>>("https://localhost:5001/v1.0/Todolists");

            //Assert
            Assert.That(listWithDeletedItem.Count, Is.EqualTo(4));
            Assert.That(listWithDeletedItem.Any(p=>p.Id==5),Is.False);
        }
        private async Task<T> GetRequest<T>(string url)
        {
            using(HttpResponseMessage response = await _client.GetAsync(url))
            {
                using(HttpContent content = response.Content)
                {
                    var jsonContent = await content.ReadAsStringAsync();
                    var list = JsonConvert.DeserializeObject<T>(jsonContent);
                    return list;
                }
            }
        }
        private async Task PostRequest<T>(string url, T postingObj)
        {
            string jsonString = System.Text.Json.JsonSerializer.Serialize(postingObj);
            var response = await _client.PostAsync(url, new StringContent(jsonString, Encoding.UTF8, "application/json"));
        }
        private async Task DeleteRequest(string url)
        {
            var response = await _client.DeleteAsync(url);
        }
    }
}