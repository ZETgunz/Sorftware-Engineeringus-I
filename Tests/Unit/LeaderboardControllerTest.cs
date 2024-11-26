using backend.Controllers;
using backend.DTOs.Leaderboard;
using backend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class LeaderboardControllerTest
    {
        private readonly Mock<ILeaderboardRepository> _mockLeaderboardRepository;
        private readonly LeaderboardController _leaderboardController;

        public LeaderboardControllerTest()
        {
            _mockLeaderboardRepository = new Mock<ILeaderboardRepository>();
            _leaderboardController = new LeaderboardController(_mockLeaderboardRepository.Object);
        }

        [Fact]
        public async Task GetLeaderboard_ShouldReturnOkResult_WithListOfLeaderboardAccounts()
        {
            // Arrange
            var leaderboard = new List<LeaderboardAccountDTO>
            {
                new LeaderboardAccountDTO { Username = "user1", score = 100 },
                new LeaderboardAccountDTO { Username = "user2", score = 200 }
            };
            _mockLeaderboardRepository.Setup(repo => repo.GetLeaderboard()).ReturnsAsync(leaderboard);

            // Act
            var result = await _leaderboardController.GetLeaderboard();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnLeaderboard = Assert.IsType<List<LeaderboardAccountDTO>>(okResult.Value);
            Assert.Equal(2, returnLeaderboard.Count);
        }

        [Fact]
        public async Task GetAccountPlace_ShouldReturnOkResult_WithAccountPlace()
        {
            // Arrange
            var username = "user1";
            var accountPlace = new LeaderboardAccountDTO { Username = username, score = 100 };
            _mockLeaderboardRepository.Setup(repo => repo.GetAccountPlace(username)).ReturnsAsync(accountPlace);

            // Act
            var result = await _leaderboardController.GetAccountPlace(username);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnAccountPlace = Assert.IsType<LeaderboardAccountDTO>(okResult.Value);
            Assert.Equal(username, returnAccountPlace.Username);
        }

        [Fact]
        public async Task GetAccountPlace_ShouldReturnNotFound_WhenAccountDoesNotExist()
        {
            // Arrange
            var username = "nonexistentuser";
            _mockLeaderboardRepository.Setup(repo => repo.GetAccountPlace(username)).ThrowsAsync(new KeyNotFoundException("Account not found"));

            // Act
            var result = await _leaderboardController.GetAccountPlace(username);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Account not found", notFoundResult.Value);
        }
    }
}