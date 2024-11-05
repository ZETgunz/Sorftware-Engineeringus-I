using backend.Models;

namespace backend.DTOs.Account
{
    public record AccountCreateDTO
    {
        public required string Username { get; init; }
        public string Password { get; init; }
        public int score { get; init; } = 0;
    }
}