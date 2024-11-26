using backend.Controllers;
using backend.DTOs.Account;
using backend.Models;
using backend.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Tests.Integration;

namespace Tests.Integration
{

    public class ControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public ControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            using var scope = factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        }

        [Fact]
        public async Task GetAccount_ShouldReturnAccount_WhenAccountExists()
        {
            // Arrange
            var username = "existinguser";
            var password = "password123";
            var account = new AccountCreateDTO { Username = username, Password = password, score = 100 };
            var content = new StringContent(JsonSerializer.Serialize(account), Encoding.UTF8, "application/json");
            await _client.PostAsync("/api/Account", content);

            // Act
            var response = await _client.GetAsync($"/api/Account/{username}/{password}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var fetchedAccount = JsonSerializer.Deserialize<AccountDTO>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.Equal(username, fetchedAccount.Username);
        }

        [Fact]
        public async Task GetAccount_ShouldReturnNotFound_WhenAccountDoesNotExist()
        {
            // Act
            var response = await _client.GetAsync("/api/Account/nonexistentuser/password123");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateAccount_ShouldReturnUpdatedAccount()
        {
            // Arrange
            var username = "updateuser";
            var account = new AccountCreateDTO { Username = username, Password = "oldpassword", score = 100 };
            var content = new StringContent(JsonSerializer.Serialize(account), Encoding.UTF8, "application/json");
            await _client.PostAsync("/api/Account", content);

            var updatedAccount = new AccountUpdateDTO { Password = "newpassword", score = 150 };
            var updateContent = new StringContent(JsonSerializer.Serialize(updatedAccount), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"/api/Account/{username}", updateContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var fetchedAccount = JsonSerializer.Deserialize<AccountDTO>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.Equal(updatedAccount.Password, fetchedAccount.Password);
            Assert.Equal(updatedAccount.score, fetchedAccount.score);
        }

        [Fact]
        public async Task DeleteAccount_ShouldReturnNoContent()
        {
            // Arrange
            var username = "deleteuser";
            var account = new AccountCreateDTO { Username = username, Password = "password123", score = 100 };
            var content = new StringContent(JsonSerializer.Serialize(account), Encoding.UTF8, "application/json");
            await _client.PostAsync("/api/Account", content);

            // Act
            var response = await _client.DeleteAsync($"/api/Account/{username}");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }
        [Fact]
        public async Task GetGames_ShouldReturnOkResult_WithListOfGames()
        {
            // Act
            var response = await _client.GetAsync("/api/Games");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var games = JsonSerializer.Deserialize<List<Game>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotEmpty(games);
        }

        [Fact]
        public async Task CreateSession_ShouldReturnOkResult_WithSessionId()
        {
            // Arrange
            var session = new UserSession { Username = "user1" };
            var content = new StringContent(JsonSerializer.Serialize(session), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/Session/create", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = responseContent.Split(":")[1].Trim().Trim('"');
            Assert.NotEmpty(result);
        }


    }
}
