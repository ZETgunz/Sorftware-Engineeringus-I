using backend.Controllers;
using backend.DTOs.Account;
using backend.Interfaces;
using backend.Enums;
using backend.Models;
using backend.Services;
using backend.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class AccountControllerTest
    {
        private readonly Mock<IAccountRepository> _mockAccountRepository;
        private readonly Mock<ILogger<AccountController>> _mockLogger;
        private readonly Mock<IValidator<Account>> _mockValidator;
        private readonly AccountController _accountController;

        public AccountControllerTest()
        {
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockLogger = new Mock<ILogger<AccountController>>();
            _mockValidator = new Mock<IValidator<Account>>();
            _accountController = new AccountController(_mockAccountRepository.Object, _mockLogger.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task GetAccounts_ShouldReturnOkResult_WithListOfAccounts()
        {
            // Arrange
            var accounts = new List<AccountDTO>
            {
                new AccountDTO { Username = "user1", Password = "password1", role = Role.User, score = 100 },
                new AccountDTO { Username = "user2", Password = "password2", role = Role.Admin, score = 200 }
            };
            _mockAccountRepository.Setup(repo => repo.GetAllAccounts()).ReturnsAsync(accounts);

            // Act
            var result = await _accountController.GetAccounts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnAccounts = Assert.IsType<List<AccountDTO>>(okResult.Value);
            Assert.Equal(2, returnAccounts.Count);
        }

        [Fact]
        public async Task GetAccount_ShouldReturnOkResult_WithAccount()
        {
            // Arrange
            var username = "user1";
            var password = "password1";
            var account = new AccountDTO { Username = username, Password = password, role = Role.User, score = 100 };
            _mockAccountRepository.Setup(repo => repo.GetAccountByUsername(username)).ReturnsAsync(account);

            // Act
            var result = await _accountController.GetAccount(username, password);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnAccount = Assert.IsType<AccountDTO>(okResult.Value);
            Assert.Equal(username, returnAccount.Username);
        }

        [Fact]
        public async Task AddAccount_ShouldReturnCreatedAtActionResult()
        {
            // Arrange
            var newAccount = new AccountCreateDTO { Username = "newuser", Password = "newpassword", score = 0 };
            _mockAccountRepository.Setup(repo => repo.GetAccountByUsername(newAccount.Username)).ThrowsAsync(new AccountNotFoundException());
            _mockAccountRepository.Setup(repo => repo.AddAccount(It.IsAny<AccountDTO>())).Returns(Task.CompletedTask);

            // Act
            var result = await _accountController.AddAccount(newAccount);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnAccount = Assert.IsType<AccountDTO>(createdAtActionResult.Value);
            Assert.Equal(newAccount.Username, returnAccount.Username);
        }

        [Fact]
        public async Task UpdateAccount_ShouldReturnOkResult_WithUpdatedAccount()
        {
            // Arrange
            var username = "user1";
            var updatedAccount = new AccountUpdateDTO { Password = "newpassword", score = 150 };
            var accountDTO = new AccountDTO { Username = username, Password = "oldpassword", role = Role.User, score = 100 };
            _mockAccountRepository.Setup(repo => repo.GetAccountByUsername(username)).ReturnsAsync(accountDTO);
            _mockAccountRepository.Setup(repo => repo.UpdateAccount(It.IsAny<AccountDTO>())).Returns(Task.CompletedTask);

            // Act
            var result = await _accountController.UpdateAccount(username, updatedAccount);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnAccount = Assert.IsType<AccountDTO>(okResult.Value);
            Assert.Equal(updatedAccount.Password, returnAccount.Password);
            Assert.Equal(updatedAccount.score, returnAccount.score);
        }

        [Fact]
        public async Task DeleteAccount_ShouldReturnNoContentResult()
        {
            // Arrange
            var username = "user1";
            _mockAccountRepository.Setup(repo => repo.DeleteAccount(username)).Returns(Task.CompletedTask);

            // Act
            var result = await _accountController.DeleteAccount(username);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        [Fact]
        public async Task GetAccounts_ShouldReturnBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            _mockAccountRepository.Setup(repo => repo.GetAllAccounts()).ThrowsAsync(new System.Exception());

            // Act
            var result = await _accountController.GetAccounts();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("An error occurred while processing your request.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetAccount_ShouldReturnBadRequest_WhenInvalidCredentialsExceptionIsThrown()
        {
            // Arrange
            var username = "user1";
            var password = "invalidpassword";
            _mockValidator.Setup(v => v.Validate(It.IsAny<Account>())).Throws(new InvalidCredentialsException("Invalid credentials"));

            // Act
            var result = await _accountController.GetAccount(username, password);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid credentials", badRequestResult.Value);
        }

        [Fact]
        public async Task GetAccount_ShouldReturnNotFound_WhenAccountNotFoundExceptionIsThrown()
        {
            // Arrange
            var username = "user1";
            var password = "password1";
            _mockAccountRepository.Setup(repo => repo.GetAccountByUsername(username)).ThrowsAsync(new AccountNotFoundException());

            // Act
            var result = await _accountController.GetAccount(username, password);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Account not found with username: " + username, notFoundResult.Value);
        }

        [Fact]
        public async Task AddAccount_ShouldReturnBadRequest_WhenInvalidCredentialsExceptionIsThrown()
        {
            // Arrange
            var newAccount = new AccountCreateDTO { Username = "newuser", Password = "short", score = 0 };
            _mockValidator.Setup(v => v.Validate(It.IsAny<Account>())).Throws(new InvalidCredentialsException("Invalid credentials"));

            // Act
            var result = await _accountController.AddAccount(newAccount);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid credentials", badRequestResult.Value);
        }

        [Fact]
        public async Task AddAccount_ShouldReturnBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var newAccount = new AccountCreateDTO { Username = "newuser", Password = "newpassword", score = 0 };
            _mockAccountRepository.Setup(repo => repo.GetAccountByUsername(newAccount.Username)).ThrowsAsync(new System.Exception());

            // Act
            var result = await _accountController.AddAccount(newAccount);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("An error occurred while processing your request.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateAccount_ShouldReturnBadRequest_WhenInvalidCredentialsExceptionIsThrown()
        {
            // Arrange
            var username = "user1";
            var updatedAccount = new AccountUpdateDTO { Password = "short", score = 150 };
            _mockValidator.Setup(v => v.Validate(It.IsAny<Account>())).Throws(new InvalidCredentialsException("Invalid credentials"));

            // Act
            var result = await _accountController.UpdateAccount(username, updatedAccount);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid credentials", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateAccount_ShouldReturnNotFound_WhenAccountNotFoundExceptionIsThrown()
        {
            // Arrange
            var username = "user1";
            var updatedAccount = new AccountUpdateDTO { Password = "newpassword", score = 150 };
            _mockAccountRepository.Setup(repo => repo.GetAccountByUsername(username)).ThrowsAsync(new AccountNotFoundException());

            // Act
            var result = await _accountController.UpdateAccount(username, updatedAccount);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Account not found with username: " + username, notFoundResult.Value);
        }

        [Fact]
        public async Task UpdateAccount_ShouldReturnBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var username = "user1";
            var updatedAccount = new AccountUpdateDTO { Password = "newpassword", score = 150 };
            _mockAccountRepository.Setup(repo => repo.GetAccountByUsername(username)).ThrowsAsync(new System.Exception());

            // Act
            var result = await _accountController.UpdateAccount(username, updatedAccount);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("An error occurred while processing your request.", badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteAccount_ShouldReturnNotFound_WhenAccountNotFoundExceptionIsThrown()
        {
            // Arrange
            var username = "user1";
            _mockAccountRepository.Setup(repo => repo.DeleteAccount(username)).ThrowsAsync(new AccountNotFoundException());

            // Act
            var result = await _accountController.DeleteAccount(username);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Account not found with username: " + username, notFoundResult.Value);
        }

        [Fact]
        public async Task DeleteAccount_ShouldReturnBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var username = "user1";
            _mockAccountRepository.Setup(repo => repo.DeleteAccount(username)).ThrowsAsync(new System.Exception());

            // Act
            var result = await _accountController.DeleteAccount(username);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("An error occurred while processing your request.", badRequestResult.Value);
        }
    }
}