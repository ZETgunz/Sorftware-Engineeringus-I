namespace backend.DTOs.Leaderboard
{
    public record LeaderboardAccountDTO
    {
        public string Username { get; init; }
        public int score { get; init; }
        public int rank { get; init; }
    }
}