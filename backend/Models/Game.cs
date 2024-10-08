namespace backend.Models
{
    public record Game
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required string Route { get; init; }

    }
}