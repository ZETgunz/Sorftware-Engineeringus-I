namespace backend.Models
{
    public class Game
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Route { get; set; }

    }
}