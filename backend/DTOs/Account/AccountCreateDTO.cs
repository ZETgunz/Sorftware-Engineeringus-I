using backend.Models;

namespace backend.DTOs.Account
{
    public record AccountCreateDTO
    {
        public string Username { get; init; }
        public string Password { get; init; }
    }
}