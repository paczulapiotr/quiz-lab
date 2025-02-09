namespace Quiz.Master.MiniGames.Models.Sorter;

public record SorterState : BaseState
{
    public string? CurrentRoundId { get; set; } = string.Empty;
    public List<RoundState> Rounds { get; set; } = new List<RoundState>();

    public record RoundState
    {
        public string RoundId { get; set; } = string.Empty;
        public List<RoundAnswer> Answers { get; set; } = new();
    }

    public record RoundAnswer
    {
        public required Guid PlayerId { get; set; }
        public List<RoundAnswerItem> Items { get; set; } = new();
        public int Points { get; set; }
        public int CorrectAnswers { get; set; }
    }

    public record RoundAnswerItem
    {
        public string CategoryId { get; set; } = string.Empty;
        public string CategoryItemId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}