using Carter;
using Quiz.Master.MiniGames.Models.MusicGuess;
using Quiz.Storage;

namespace Quiz.Master.Features.MiniGame.MusicGuess.GetQuestion;

public class GetQuestionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/game/{gameId}/mini-game/music/question", async (Guid gameId, IDatabaseStorage storage, CancellationToken cancellationToken = default) =>
        {
            var (state, definition) = await storage.FindCurrentMiniGameStateAndDefinitionAsync
                <MusicGuessState, MusicGuessDefinition>(gameId, cancellationToken);
                
            var question = definition?.Rounds
                .FirstOrDefault(x=>x.Id == state?.CurrentRoundId)?.Categories?
                .FirstOrDefault(x=>x.Id == state?.CurrentCategoryId)?.Questions
                .FirstOrDefault(x=>x.Id == state?.CurrentQuestionId);

            var result = new GetQuestionResult(
                question?.Id, 
                question?.Text, 
                question?.AudioUrl, 
                question?.Answers.Select(x => new Answer(x.Id, x.Text)));

            return Results.Ok(result);
        })
        .WithName("GetQuestion")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Question")
        .WithDescription("Get Question")
        .WithTags("MusicGuess");
    }
}

internal record GetQuestionResult(
    string? QuestionId = null, 
    string? Question = null, 
    string? AudioUrl = null, 
    IEnumerable<Answer>? Answers = null);
public record Answer(string Id, string Text);