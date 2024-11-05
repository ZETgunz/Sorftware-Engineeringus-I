using backend.Models;
using backend.Enums;
using System;
namespace backend.Models
{
    public class Account : IComparable<Account>, IEquatable<Account>
    {



        public Account(string username, string password, Role role = Role.User, int score = 0)
        {
            Username = username;
            Password = password;
            this.role = role;
            this.score = score;
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public Role role { get; set; }
        public int score { get; set; }
        public int CompareTo(Account other)
        {
            return (-this.score).CompareTo(-other.score);
        }
        public bool Equals(Account other)
        {
            return this.Username == other.Username;
        }
    }
}