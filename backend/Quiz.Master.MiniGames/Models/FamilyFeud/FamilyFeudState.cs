namespace Quiz.Master.MiniGames.Models.FamilyFeud;

public record FamilyFeudState : BaseState
{
    public string? CurrentRoundId { get; set; } = string.Empty;
    public Guid? CurrentGuessingPlayerId { get; set; } = null;
    public List<RoundState> Rounds { get; set; } = new List<RoundState>();

    public record RoundState
    {
        public string RoundId { get; set; } = string.Empty;
        public List<RoundAnswer> Answers { get; set; } = new();
    }

    public record RoundAnswer
    {
        public required Guid PlayerId { get; set; }
        public string? Answer { get; set; }
        public string? MatchedAnswerId { get; set; }
        public string? MatchedAnswer { get; set; }
        public int Points { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}