using backend.Models;
using System;
namespace backend.Models
{
    public class Account : IComparable<Account>, IEquatable<Account>
    {



        public Account(string username, string password, Role role = Role.User)
        {
            Username = username;
            Password = password;
            this.role = role;
        }
        public string Username { get; set; }
        public string Password { get; set; }
        public Role role { get; set; }
        public int score = 0;

        public int CompareTo(Account other)
        {
            return this.score.CompareTo(other.score);
        }
        public bool Equals(Account other)
        {
            return this.Username == other.Username;
        }
    }
}