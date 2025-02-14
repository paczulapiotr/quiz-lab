using Quiz.Master.Core.Models;
using Quiz.Master.MiniGames.Models.Sorter;

namespace Quiz.Master.Services.ContentManagement;

public record ComponentSharedMinDefSorter : MiniGameDto
{
    public List<Round> Rounds { get; set; } = new();

    public override MiniGameType GameType => MiniGameType.Sorter;

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
        public required IEnumerable<CategoryItem> Items { get; set; }
    }

    public record CategoryItem
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
    }

    override public MiniGameDefinitionData MapToDefinition()
    {
        return new SorterDefinition
        {
            Rounds = Rounds.Select(r => new SorterDefinition.Round
            {
                Id = r.Id,
                LeftCategory = new SorterDefinition.Category
                {
                    Id = r.LeftCategory.Id,
                    Name = r.LeftCategory.Name,
                    Items = r.LeftCategory.Items.Select(i => new SorterDefinition.CategoryItem
                    {
                        Id = i.Id,
                        Name = i.Name
                    }).ToList()
                },
                RightCategory = new SorterDefinition.Category
                {
                    Id = r.RightCategory.Id,
                    Name = r.RightCategory.Name,
                    Items = r.RightCategory.Items.Select(i => new SorterDefinition.CategoryItem
                    {
                        Id = i.Id,
                        Name = i.Name
                    }).ToList()
                }
            }).ToList()
        };
    }
}