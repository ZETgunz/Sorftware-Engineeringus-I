namespace backend.DTOs.Account
{
    public record AccountUpdateDTO
    {
        public string Username { get; init; }
        public string Password { get; init; }
        public int score { get; init; }
    }
}