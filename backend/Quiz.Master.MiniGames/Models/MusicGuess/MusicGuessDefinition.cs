using Quiz.Master.Core.Models;
using Quiz.Master.MiniGames.Models.Abstract;

namespace Quiz.Master.MiniGames.Models.MusicGuess;

public record MusicGuessDefinition : MiniGameDefinitionData
{
    public List<Round> Rounds { get; set; } = new List<Round>();

    public record Round
    {
        public required string Id { get; set; }
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
    }

    public record Category
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public IEnumerable<Question> Questions { get; set; } = new List<Question>();
    }

    public record Question
    {
        public required string Id { get; set; }
        public string? Text { get; set; }
        public IEnumerable<Answer> Answers { get; set; } = new List<Answer>();
        public string? AudioUrl { get; set; }
        public record Answer : AnswerBase { }
    }

}






