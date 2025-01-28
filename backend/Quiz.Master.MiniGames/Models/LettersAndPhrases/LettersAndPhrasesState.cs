namespace Quiz.Master.MiniGames.Models.LettersAndPhrases;

public record LettersAndPhrasesState : BaseState
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
        public required char Letter { get; set; }
        public bool IsCorrect { get; set; }
        public DateTime? Timestamp { get; set; }
        public int Points { get; set; }
    }
}