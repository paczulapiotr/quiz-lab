
namespace Quiz.Master.Game;



public interface IGameEngine
{
    Task StartNew();

}

public class Player
{
    public string Id { get; set; }
    public int Score { get; set; }
}

public class Question
{
    public string Text { get; set; }
    public string CorrectAnswer { get; set; }
    public List<string> Categories { get; set; }
}

public class Answer
{
    public string PlayerId { get; set; }
    public string Response { get; set; }
    public TimeSpan ResponseTime { get; set; }
}

public class GameEngine : IGameEngine
{
    private List<Player> players;
    private List<Question> questions;
    private int currentQuestionIndex;

    public GameEngine(List<Player> players, List<Question> questions)
    {
        this.players = players;
        this.questions = questions;
        this.currentQuestionIndex = 0;
    }

    public async Task StartNew()
    {
        while (currentQuestionIndex < questions.Count)
        {
            var question = questions[currentQuestionIndex];
            var selectedCategory = await SelectCategory(question.Categories);
            await Task.Delay(TimeSpan.FromSeconds(10)); // Wait 10 seconds before proceeding to the next question

            var answers = await GetAnswersFromPlayers();
            ProcessAnswers(answers, question.CorrectAnswer);

            currentQuestionIndex++;
        }
    }

    private async Task<string> SelectCategory(List<string> categories)
    {
        // Mock implementation for category selection
        await Task.Delay(TimeSpan.FromSeconds(30)); // Wait for players to vote
        return categories[new Random().Next(categories.Count)]; // Randomly select a category
    }

    private async Task<List<Answer>> GetAnswersFromPlayers()
    {
        // Mock implementation for getting answers from RabbitMQ
        await Task.Delay(TimeSpan.FromSeconds(30)); // Wait for players to answer
        return new List<Answer>(); // Return empty list as mock
    }

    private void ProcessAnswers(List<Answer> answers, string correctAnswer)
    {
        var correctAnswers = answers.Where(a => a.Response == correctAnswer).OrderBy(a => a.ResponseTime).ToList();

        for (int i = 0; i < correctAnswers.Count; i++)
        {
            var player = players.First(p => p.Id == correctAnswers[i].PlayerId);
            player.Score += i switch
            {
                0 => 400,
                1 => 300,
                2 => 200,
                _ => 100
            };
        }

        var incorrectAnswers = answers.Where(a => a.Response != correctAnswer).ToList();
        foreach (var answer in incorrectAnswers)
        {
            var player = players.First(p => p.Id == answer.PlayerId);
            player.Score += 0;
        }
    }
}