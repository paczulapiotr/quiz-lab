namespace Quiz.Master.Persistance.Models.MiniGames.AbcdCategories;

public record AbcdCategoryMiniGame
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public required Game Game { get; set; }
    public ICollection<AbcdCategoryRound> Rounds { get; set; } = new List<AbcdCategoryRound>();
    public Guid? CurrentRoundId { get; set; }
    public AbcdCategoryRound? CurrentRound { get; set; }
}


public record AbcdCategoryRound
{
    public Guid Id { get; set; }
    public Guid MiniGameId { get; set; }
    public AbcdCategoryMiniGame? MiniGame { get; set; }
    public ICollection<AbcdCategory> Categories { get; set; } = new List<AbcdCategory>();
    public AbcdCategory? SelectedCategory { get; set; }
}

public record AbcdCategory
{
    public Guid Id { get; set; }
    public ICollection<AbcdQuestion> Questions { get; set; } = new List<AbcdQuestion>();
}

public record AbcdAnswer : Answer { }
public record AbcdAnswerSelection : AnswerSelection<AbcdAnswer> { }
public record AbcdQuestion : Question<AbcdAnswer> { }