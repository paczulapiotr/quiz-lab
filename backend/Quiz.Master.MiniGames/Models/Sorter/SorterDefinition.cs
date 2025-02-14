namespace Quiz.Master.MiniGames.Models.Sorter;

public record SorterDefinition : BaseDefinition
{
    public List<Round> Rounds { get; set; } = new List<Round>();

    public record Round
    {
        public required string Id { get; set; }
        public required Category LeftCategory { get; set; }
        public required Category RightCategory { get; set; }
    }

    public record Category
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public IEnumerable<CategoryItem> Items { get; set; } = new List<CategoryItem>();
    }

    public record CategoryItem
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
    }
}






