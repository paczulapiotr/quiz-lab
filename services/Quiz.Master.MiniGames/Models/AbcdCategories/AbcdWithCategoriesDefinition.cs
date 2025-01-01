using Quiz.Master.MiniGames.Models.Abstract;

namespace Quiz.Master.MiniGames.Models.AbcdCategories;

public record AbcdWithCategoriesDefinition : MiniGameDefinitionBase<AbcdWithCategoriesDefinition.Configuration>
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
        public string Text { get; set; } = string.Empty;
        public IEnumerable<Answer> Answers { get; set; } = new List<Answer>();
        public record Answer : AnswerBase { }
    }

}






