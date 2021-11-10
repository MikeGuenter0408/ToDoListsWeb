using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ToDoListeWeb.Domain.Entities;
using ToDoListWeb.Infrastructure;


namespace ApiTests.IntegrationTest
{
    public class IntegrationTest
    {
        protected readonly HttpClient TestClient;
        public IntegrationTest()
        {
            
        }
        /*protected async Task AuthenticateAsync()
        {
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync());
        }
        private async Task<string> GetJwtAsync()
        {
            var response = await TestClient.PostAsJsonAsync("http://v1.0/ToDoLists", new ToDoList
            {
                Name = "Beispielliste",
                ToDos = new List<ToDo>
                {
                    new ToDo
                    {
                        Description = "BeispielToDo1"
                    },
                    new ToDo
                    {
                        Description = "BeispielToDo2"
                    }
                }
            });
            var registrationResponse = await response.Content.ReadAsAsync<AuthSuccessResponse>();
            return registrationResponse.
        }*/
    }
}