namespace Quiz.Master.Persistance.Models.MiniGames.AbcdCategories;

public record AbcdWithCategories : MiniGameDataBase<AbcdWithCategories.Configuration, AbcdWithCategories.MiniGameState>
{
    public List<Round> Rounds { get; set; } = new List<Round>();

    public record Configuration
    {
        public int TimeForCategorySelectionMs { get; set; }
        public int TimeForPowerPlaySelectionMs { get; set; }
        public int TimeForAnswerSelectionMs { get; set; }
        public int MaxPointsForAnswer { get; set; }
        public int MinPointsForAnswer { get; set; }
        public int PointsDecrement { get; set; }
    }

    public record MiniGameState
    {
        public List<RoundState> Rounds { get; set; } = new List<RoundState>();

        public record RoundState
        {
            public string RoundId { get; set; } = string.Empty;
            public string CategoryId { get; set; } = string.Empty;
            // TargetPlayerId, (PowerPlay, SourcePlayerId)
            public PowerPlaysDictionary PowerPlays { get; set; } = new();
            // PlayerId, (AnswerId, Timestamp)
            public Dictionary<string, (string answerId, DateTime timestamp)> Answers { get; set; } = new();
        }
    }

    public enum PowerPlay
    {
        Slime = 1,
        Freeze,
        Bombs,
        Letters,
    }

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






