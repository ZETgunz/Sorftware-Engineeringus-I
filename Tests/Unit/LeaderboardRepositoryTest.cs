using backend.DTOs.Leaderboard;
using backend.Repositories;
using backend.Data;
using backend.DTOs.Account;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class LeaderboardRepositoryTest
    {
        private readonly AppDbContext _context;
        private readonly LeaderboardRepository _repository;

        public LeaderboardRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new AppDbContext(options);
            _repository = new LeaderboardRepository(_context);
        }

        [Fact]
        public async Task GetLeaderboard_ShouldReturnListOfLeaderboardAccounts()
        {
            // Arrange
            var accountDTOs = new List<AccountDTO>
            {
                new AccountDTO { Username = "user1", Password = "asssasadaa1223.", score = 100 },
                new AccountDTO { Username = "user2",Password = "asssasadaa1223." , score = 200 }
            };
            _context.AccountDTOs.AddRange(accountDTOs);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetLeaderboard();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("user2", result.First().Username);
            Assert.Equal(200, result.First().score);
        }

        [Fact]
        public async Task GetAccountPlace_ShouldReturnAccountPlace_WhenAccountExists()
        {
            // Act
            var result = await _repository.GetAccountPlace("user1");

            // Assert
            Assert.Equal("user1", result.Username);
            Assert.Equal(100, result.score);
        }

        [Fact]
        public async Task GetAccountPlace_ShouldThrowKeyNotFoundException_WhenAccountDoesNotExist()
        {

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _repository.GetAccountPlace("nonexistentuser"));
        }
    }
}
