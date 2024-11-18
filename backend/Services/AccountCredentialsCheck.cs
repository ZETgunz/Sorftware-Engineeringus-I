using System;
using System.Text.RegularExpressions;
using backend.Exceptions;
using backend.Interfaces;

namespace backend.Services
{
    public class AccountCredentialsCheck : IAccountCredentialsCheck
    {
        public bool IsPasswordValid(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new InvalidCredentialsException("Password cannot be null or empty.");
            }

            var passwordRequirements = new (string pattern, string errorMessage)[]
            {
                (@"[A-Z]", "Password must contain at least one uppercase letter."),
                (@"[a-z]", "Password must contain at least one lowercase letter."),
                (@"\d", "Password must contain at least one digit."),
            };

            if (password.Length < 8)
            {
                throw new InvalidCredentialsException("Password must be at least 8 characters long.");
            }

            foreach (var (pattern, errorMessage) in passwordRequirements)
            {
                if (!Regex.IsMatch(password, pattern))
                {
                    throw new InvalidCredentialsException(errorMessage);
                }
            }

            return true;
        }

        public bool IsUsernameValid(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new InvalidCredentialsException("Username cannot be null or empty.");
            }

            if (username.Length < 3 || username.Length > 20)
            {
                throw new InvalidCredentialsException("Username must be between 3 and 20 characters long.");
            }

            if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$"))
            {
                throw new InvalidCredentialsException("Username can only contain alphanumeric characters and underscores.");
            }

            return true;
        }
    }
}