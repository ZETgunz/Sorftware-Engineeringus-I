using backend.Models;
using backend.Enums;

namespace backend.DTOs.Account
{
    public class AccountLoginDTO
    {
        public string Username { get; init; }
        public string Password { get; set; }
    }
}