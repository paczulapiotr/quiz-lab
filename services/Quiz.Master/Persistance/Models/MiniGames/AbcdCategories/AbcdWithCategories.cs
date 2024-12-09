namespace Quiz.Master.Persistance.Models.MiniGames.AbcdCategories;

public record AbcdWithCategories
{
    public IEnumerable<Round> Rounds { get; set; } = new List<Round>();

    public record Round
    {
        public required string Id { get; set; }
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
    }

    public record Category
    {
        public required string Id { get; set; }
        public IEnumerable<Question> Questions { get; set; } = new List<Question>();
    }

    public record Question
    {
        public required string Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public IEnumerable<Answer> Answers { get; set; } = new List<Answer>();
        public record Answer : AnswerBase { }
    }

}






