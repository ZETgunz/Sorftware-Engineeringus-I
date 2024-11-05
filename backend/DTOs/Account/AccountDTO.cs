using backend.Models;

namespace backend.DTOs.Account
{
    public class AccountDTO
    {
        public string Username { get; init; }
        public string Password { get; set; }
        public Role role { get; set; }
        public int score { get; set; }
    }
}