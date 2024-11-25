using backend.Models;
using backend.Services;
using backend.Exceptions;
using System;
using Xunit;

namespace Tests
{
    public class ValidatorTest
    {
        private readonly Validator<Account> _accountValidator;
        private readonly Validator<Game> _gameValidator;

        public ValidatorTest()
        {
            _accountValidator = new AccountValidator();
            _gameValidator = new GameValidator();
        }

        [Fact]
        public void ValidateAccount_ShouldThrowException_WhenUsernameIsTooShort()
        {
            // Arrange
            var account = new Account("ab", "password123");

            // Act & Assert
            var exception = Assert.Throws<InvalidCredentialsException>(() => _accountValidator.Validate(account));
            Assert.Equal("Username must be at least 3 characters long", exception.Message);
        }

        [Fact]
        public void ValidateAccount_ShouldThrowException_WhenPasswordIsTooShort()
        {
            // Arrange
            var account = new Account("username", "pass");

            // Act & Assert
            var exception = Assert.Throws<InvalidCredentialsException>(() => _accountValidator.Validate(account));
            Assert.Equal("Password must be at least 8 characters long", exception.Message);
        }

        [Fact]
        public void ValidateAccount_ShouldThrowException_WhenUsernameContainsInvalidCharacters()
        {
            // Arrange
            var account = new Account("user@name", "password123");

            // Act & Assert
            var exception = Assert.Throws<InvalidCredentialsException>(() => _accountValidator.Validate(account));
            Assert.Equal("Username can only contain alphanumeric characters and underscores", exception.Message);
        }

        [Fact]
        public void ValidateAccount_ShouldPass_WhenAccountIsValid()
        {
            // Arrange
            var account = new Account("username", "password123");

            // Act
            var result = _accountValidator.Validate(account);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateGame_ShouldThrowException_WhenNameIsTooLong()
        {
            // Arrange
            var game = new Game { Name = new string('a', 51), Description = "Valid description", Route = "/valid-route", Id = 1 };

            // Act & Assert
            var exception = Assert.Throws<InvalidCredentialsException>(() => _gameValidator.Validate(game));
            Assert.Equal("Game name must not exceed 50 characters", exception.Message);
        }

        [Fact]
        public void ValidateGame_ShouldThrowException_WhenDescriptionIsTooLong()
        {
            // Arrange
            var game = new Game { Name = "Valid name", Description = new string('a', 201), Route = "/valid-route", Id = 1 };

            // Act & Assert
            var exception = Assert.Throws<InvalidCredentialsException>(() => _gameValidator.Validate(game));
            Assert.Equal("Game description must not exceed 200 characters", exception.Message);
        }

        [Fact]
        public void ValidateGame_ShouldThrowException_WhenRouteIsInvalid()
        {
            // Arrange
            var game = new Game { Name = "Valid name", Description = "Valid description", Route = "invalid-route", Id = 1 };

            // Act & Assert
            var exception = Assert.Throws<InvalidCredentialsException>(() => _gameValidator.Validate(game));
            Assert.Equal("Route must start with /", exception.Message);
        }

        [Fact]
        public void ValidateGame_ShouldPass_WhenGameIsValid()
        {
            // Arrange
            var game = new Game { Name = "Valid name", Description = "Valid description", Route = "/valid-route", Id = 1 };

            // Act
            var result = _gameValidator.Validate(game);

            // Assert
            Assert.True(result);
        }
    }
}