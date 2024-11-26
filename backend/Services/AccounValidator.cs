using backend.Models;
using backend.Services;
using System.Text.RegularExpressions;

namespace backend.Services;

public class AccountValidator : Validator<Account>
{
    public AccountValidator()
    {
        AddRule(a => a.Username.Length >= 3, "Username must be at least 3 characters long");
        AddRule(a => a.Password.Length >= 8, "Password must be at least 8 characters long");
        AddRule(a => Regex.IsMatch(a.Username, @"^[a-zA-Z0-9_]+$"),
            "Username can only contain alphanumeric characters and underscores");
    }
}