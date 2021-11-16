using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using ToDoListeWeb.Domain.Entities;
using System.Linq;

namespace ApiTests.IntegrationTest
{
    public class ToDoListControllerTest : IntegrationTest
    {
        [SetUp]
        public void Setup()
        {
            base.Setup();
        }
        [Test]
        public async Task ShouldCarryOutGetRequest()
        {
            //Arrange

            //Act
            var response = await GetRequest<List<ToDoList>>("https://localhost:5001/v1.0/Todolists");
            
            //Assert
            Assert.That(response.Count, Is.EqualTo(5));
            Assert.That(response.Any(p => p.Name=="Shopping"), Is.True);
            if(response.Any(p => p.Name=="Shopping"))
            {
                ToDoList list = response.Find(p=>p.Name=="Shopping");
                Assert.That(list.ToDos.Any(p=>p.Description=="Garlic"), Is.True);
            }
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