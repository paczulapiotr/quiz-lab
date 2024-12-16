namespace Quiz.Master.Persistance.Models.MiniGames.AbcdCategories;

public record AbcdWithCategoriesState
{
    public string CurrentRoundId { get; set; } = string.Empty;
    public string CurrentCategoryId { get; set; } = string.Empty;
    public string CurrentQuestionId { get; set; } = string.Empty;
    public List<RoundState> Rounds { get; set; } = new List<RoundState>();

    public record RoundState
    {
        public string RoundId { get; set; } = string.Empty;
        public string CategoryId { get; set; } = string.Empty;
        // TargetPlayerId, (PowerPlay, SourcePlayerId)
        public PowerPlaysDictionary PowerPlays { get; set; } = new();
        // DeviceId, (AnswerId, Timestamp)
        public List<RoundAnswer> Answers { get; set; } = new();
    }

    public record RoundAnswer
    {
        public required string DeviceId { get; set; }
        public string? AnswerId { get; set; } = null;
        public bool IsCorrect { get; set; }
        public DateTime? Timestamp { get; set; }
        public int Points { get; set; }
    }
}