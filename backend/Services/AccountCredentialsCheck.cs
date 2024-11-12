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

            // Check if password length is at least 8 characters
            if (password.Length < 8)
            {
                throw new InvalidCredentialsException("Password must be at least 8 characters long.");
            }

            // Check if password contains at least one uppercase letter
            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                throw new InvalidCredentialsException("Password must contain at least one uppercase letter.");
            }

            // Check if password contains at least one lowercase letter
            if (!Regex.IsMatch(password, @"[a-z]"))
            {
                throw new InvalidCredentialsException("Password must contain at least one lowercase letter.");
            }

            // Check if password contains at least one digit
            if (!Regex.IsMatch(password, @"\d"))
            {
                throw new InvalidCredentialsException("Password must contain at least one digit.");
            }

            // Check if password contains at least one special character
            if (!Regex.IsMatch(password, @"[\W_]"))
            {
                throw new InvalidCredentialsException("Password must contain at least one special character.");
            }

            return true;
        }

        public bool IsUsernameValid(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new InvalidCredentialsException("Username cannot be null or empty.");
            }

            // Check if username length is between 3 and 20 characters
            if (username.Length < 3 || username.Length > 20)
            {
                throw new InvalidCredentialsException("Username must be between 3 and 20 characters long.");
            }

            // Check if username contains only alphanumeric characters and underscores
            if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$"))
            {
                throw new InvalidCredentialsException("Username can only contain alphanumeric characters and underscores.");
            }

            return true;
        }
    }
}