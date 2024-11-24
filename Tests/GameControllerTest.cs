using backend.Controllers;
using backend.Models;
using backend.Interfaces;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using backend.Exceptions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class GameControllerTest
    {
        private readonly Mock<IGameRepository> _mockGameRepository;
        private readonly Mock<ILogger<GamesController>> _mockLogger;
        private readonly Mock<Validator<Game>> _mockValidator;
        private readonly GamesController _gamesController;

        public GameControllerTest()
        {
            _mockGameRepository = new Mock<IGameRepository>();
            _mockLogger = new Mock<ILogger<GamesController>>();
            _mockValidator = new Mock<Validator<Game>>();
            _gamesController = new GamesController(_mockGameRepository.Object, _mockLogger.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task GetGames_ShouldReturnOkResult_WithListOfGames()
        {
            // Arrange
            var games = new List<Game>
            {
                new Game { Id = 1, Name = "Game1", Description = "Description1", Route = "/route1" },
                new Game { Id = 2, Name = "Game2", Description = "Description2", Route = "/route2" }
            };
            _mockGameRepository.Setup(repo => repo.GetAllGames()).ReturnsAsync(games);

            // Act
            var result = await _gamesController.GetGames();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnGames = Assert.IsType<List<Game>>(okResult.Value);
            Assert.Equal(2, returnGames.Count);
        }

        [Fact]
        public async Task GetGame_ShouldReturnOkResult_WithGame()
        {
            // Arrange
            var game = new Game { Id = 1, Name = "Game1", Description = "Description1", Route = "/route1" };
            _mockGameRepository.Setup(repo => repo.GetGameById(1)).ReturnsAsync(game);

            // Act
            var result = await _gamesController.GetGame(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnGame = Assert.IsType<Game>(okResult.Value);
            Assert.Equal(1, returnGame.Id);
        }

        [Fact]
        public async Task GetGame_ShouldReturnNotFound_WhenGameDoesNotExist()
        {
            // Arrange
            _mockGameRepository.Setup(repo => repo.GetGameById(1)).ReturnsAsync((Game)null);

            // Act
            var result = await _gamesController.GetGame(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task AddGame_ShouldReturnCreatedAtActionResult()
        {
            // Arrange
            var newGame = new Game { Id = 1, Name = "NewGame", Description = "NewDescription", Route = "/newRoute" };
            _mockGameRepository.Setup(repo => repo.AddGame(newGame)).Returns(Task.CompletedTask);

            // Act
            var result = await _gamesController.AddGame(newGame);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnGame = Assert.IsType<Game>(createdAtActionResult.Value);
            Assert.Equal(newGame.Id, returnGame.Id);
        }

        [Fact]
        public async Task UpdateGame_ShouldReturnNoContentResult()
        {
            // Arrange
            var game = new Game { Id = 1, Name = "UpdatedGame", Description = "UpdatedDescription", Route = "/updatedRoute" };
            _mockGameRepository.Setup(repo => repo.UpdateGame(game)).Returns(Task.CompletedTask);

            // Act
            var result = await _gamesController.UpdateGame(1, game);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateGame_ShouldReturnBadRequest_WhenIdMismatch()
        {
            // Arrange
            var game = new Game { Id = 1, Name = "UpdatedGame", Description = "UpdatedDescription", Route = "/updatedRoute" };

            // Act
            var result = await _gamesController.UpdateGame(2, game);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteGame_ShouldReturnNoContentResult()
        {
            // Arrange
            var game = new Game { Id = 1, Name = "GameToDelete", Description = "DescriptionToDelete", Route = "/routeToDelete" };
            _mockGameRepository.Setup(repo => repo.GetGameById(1)).ReturnsAsync(game);
            _mockGameRepository.Setup(repo => repo.DeleteGame(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _gamesController.DeleteGame(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteGame_ShouldReturnNotFound_WhenGameDoesNotExist()
        {
            // Arrange
            _mockGameRepository.Setup(repo => repo.GetGameById(1)).ReturnsAsync((Game)null);

            // Act
            var result = await _gamesController.DeleteGame(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}