using backend.Interfaces;

namespace backend.Models
{
    public record Game : IValidatable
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required string Route { get; init; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Description) && !string.IsNullOrEmpty(Route);
        }

    }
}