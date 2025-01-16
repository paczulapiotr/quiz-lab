using Quiz.Master.Core.Models;
using Quiz.Master.MiniGames.Models.AbcdCategories;

namespace Quiz.Master.Services.ContentManagement;

public record ComponentSharedMiniGameDefAbcd : MiniGameDto
{
    public override MiniGameType GameType => MiniGameType.AbcdWithCategories;

    public List<Round> Rounds { get; set; } = new();

    public record Round
    {
        public required string Id { get; set; }
        public List<Category> Categories { get; set; } = new();
    }

    public record Category
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public List<Question> Questions { get; set; } = new();
    }

    public record Question
    {
        public required string Id { get; set; }
        public Audio? Audio { get; set; } = null!;
        public required string Text { get; set; }
        public List<Answer> Answers { get; set; } = new();
    }

    override public MiniGameDefinitionData MapToDefinition()
    {
        return new AbcdWithCategoriesDefinition
        {
            Rounds = Rounds.Select(r => new AbcdWithCategoriesDefinition.Round
            {
                Id = r.Id,
                Categories = r.Categories.Select(c => new AbcdWithCategoriesDefinition.Category
                {
                    Id = c.Id,
                    Name = c.Name,
                    Questions = c.Questions.Select(q => new AbcdWithCategoriesDefinition.Question
                    {
                        Id = q.Id,
                        Text = q.Text,
                        AudioUrl = q.Audio?.Url,
                        Answers = q.Answers.Select(a => new AbcdWithCategoriesDefinition.Question.Answer
                        {
                            Id = a.Id,
                            IsCorrect = a.IsCorrect,
                            Text = a.Text
                        }).ToList()
                    }).ToList()
                }).ToList()
            }).ToList()
        };
    }
}
