using Quiz.Master.Core.Models;
using Quiz.Master.MiniGames.Models.MusicGuess;

namespace Quiz.Master.Services.ContentManagement;

public record ComponentSharedMinDefMusic : MiniGameDto
{
    public List<Round> Rounds { get; set; } = new();

    public override MiniGameType GameType => MiniGameType.MusicGuess;

    public record Round
    {
        public required string Id { get; set; }
        public List<Category> Categories { get; set; } = new();

    }

    public record Category {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public List<Question> MusicQuestions { get; set; } = new();
    }

    public record Question {
        public required string Id { get; set; }
        public string? Text { get; set; }
        public List<Answer> Answers { get; set; } = new();
        public required Audio Audio { get; set; }
    }

    override public MiniGameDefinitionData MapToDefinition()
    {
        return new MusicGuessDefinition
        {
            Rounds = Rounds.Select(r => new MusicGuessDefinition.Round
            {
                Id = r.Id,
                Categories = r.Categories.Select(c => new MusicGuessDefinition.Category
                {
                    Id = c.Id,
                    Name = c.Name,
                    Questions = c.MusicQuestions.Select(q => new MusicGuessDefinition.Question
                    {
                        Id = q.Id,
                        Answers = q.Answers.Select(a => new MusicGuessDefinition.Question.Answer
                        {
                            Id = a.Id,
                            IsCorrect = a.IsCorrect,
                            Text = a.Text
                        }).ToList(),
                        AudioUrl = q.Audio.Url,
                        Text = q.Text,
                    }).ToList()
                }).ToList()
            }).ToList()
        };
    }
}