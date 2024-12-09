namespace backend.DTOs.Account
{
    public record AccountUpdateDTO
    {
        public string? Password { get; init; }
        public int score { get; init; }
    }
}