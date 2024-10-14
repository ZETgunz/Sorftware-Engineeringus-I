namespace backend.DTOs.Leaderboard
{
    public record LeaderBoardAccountDTO
    {
        public string Username { get; init; }
        public int score { get; init; }
        public int rank { get; init; }
    }
}