using backend.Models;
using backend.Enums;

namespace backend.DTOs.Account
{
    public record AccountDTO
    {
        public string Username { get; init; }
        public string Password { get; init; }
        public Role role { get; init; }
        public int score { get; init; }
    }
}