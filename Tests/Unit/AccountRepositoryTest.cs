using backend.DTOs.Account;
using backend.Exceptions;
using backend.Interfaces;
using backend.Models;
using backend.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System;
using backend.Data;
using backend.Enums;

namespace Tests
{
    public class AccountRepositoryTest
    {
        private readonly AppDbContext _context;
        private readonly AccountRepository _accountRepository;

        public AccountRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);
            _accountRepository = new AccountRepository(_context);
        }

        [Fact]
        public async Task GetAllAccounts_ShouldReturnListOfAccounts()
        {
            // Arrange
            var accounts = new List<AccountDTO>
            {
                new AccountDTO { Username = "user1", Password = "password1", role = Role.User, score = 100 },
                new AccountDTO { Username = "user2", Password = "password2", role = Role.Admin, score = 200 }
            };
            await _context.AccountDTOs.AddRangeAsync(accounts);
            await _context.SaveChangesAsync();

            // Act
            var result = await _accountRepository.GetAllAccounts();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAccountByUsername_ShouldReturnAccount_WhenAccountExists()
        {
            // Arrange
            var username = "user1";
            var account = new AccountDTO { Username = username, Password = "password1", role = Role.User, score = 100 };
            await _context.AccountDTOs.AddAsync(account);
            await _context.SaveChangesAsync();

            // Act
            var result = await _accountRepository.GetAccountByUsername(username);

            // Assert
            Assert.Equal(username, result.Username);
        }

        [Fact]
        public async Task GetAccountByUsername_ShouldThrowException_WhenAccountDoesNotExist()
        {
            // Arrange
            var username = "nonexistentuser";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<AccountNotFoundException>(() => _accountRepository.GetAccountByUsername(username));
            Assert.Equal("Account not found with username: " + username, exception.Message);
        }

        [Fact]
        public async Task AddAccount_ShouldAddAccount()
        {
            // Arrange
            var accountDTO = new AccountDTO { Username = "newuser", Password = "newpassword", role = Role.User, score = 0 };

            // Act
            await _accountRepository.AddAccount(accountDTO);

            // Assert
            var account = await _context.AccountDTOs.FirstOrDefaultAsync(a => a.Username == accountDTO.Username);
            Assert.NotNull(account);
        }

        [Fact]
        public async Task UpdateAccount_ShouldUpdateExistingAccount()
        {
            // Arrange
            var accountDTO = new AccountDTO { Username = "user1", Password = "newpassword", role = Role.Admin, score = 150 };
            var existingAccount = new AccountDTO { Username = "user1", Password = "oldpassword", role = Role.User, score = 100 };
            await _context.AccountDTOs.AddAsync(existingAccount);
            await _context.SaveChangesAsync();

            // Act
            await _accountRepository.UpdateAccount(accountDTO);

            // Assert
            var updatedAccount = await _context.AccountDTOs.FirstOrDefaultAsync(a => a.Username == accountDTO.Username);
            Assert.Equal(accountDTO.Password, updatedAccount.Password);
            Assert.Equal(accountDTO.role, updatedAccount.role);
            Assert.Equal(accountDTO.score, updatedAccount.score);
        }

        [Fact]
        public async Task UpdateAccount_ShouldThrowException_WhenAccountDoesNotExist()
        {
            // Arrange
            var accountDTO = new AccountDTO { Username = "nonexistentuser", Password = "newpassword", role = Role.Admin, score = 150 };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<AccountNotFoundException>(() => _accountRepository.UpdateAccount(accountDTO));
            Assert.Equal("Account not found with username: " + accountDTO.Username, exception.Message);
        }

        [Fact]
        public async Task DeleteAccount_ShouldRemoveAccount()
        {
            // Arrange
            var username = "user1";
            var account = new AccountDTO { Username = username, Password = "password1", role = Role.User, score = 100 };
            await _context.AccountDTOs.AddAsync(account);
            await _context.SaveChangesAsync();

            // Act
            await _accountRepository.DeleteAccount(username);

            // Assert
            var deletedAccount = await _context.AccountDTOs.FirstOrDefaultAsync(a => a.Username == username);
            Assert.Null(deletedAccount);
        }

        [Fact]
        public async Task DeleteAccount_ShouldThrowException_WhenAccountDoesNotExist()
        {
            // Arrange
            var username = "nonexistentuser";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<AccountNotFoundException>(() => _accountRepository.DeleteAccount(username));
            Assert.Equal("Account not found with username: " + username, exception.Message);
        }
    }
}
