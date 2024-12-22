
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Quiz.Common.CQRS;
using Quiz.Master.Persistance;
using Quiz.Master.Persistance.Models;
using Quiz.Master.Persistance.Models.MiniGames.AbcdCategories;

namespace Quiz.Master.Features.MiniGame.AbcdWithCategories.GetCategories;

public record GetCategoriesQuery(Guid GameId) : IQuery<GetCategoriesResult>;
public record GetCategoriesResult(IEnumerable<Category> Categories);
public record Category(string Id, string Text);

public class GetCategoriesHandler(IQuizRepository quizRepository) : IQueryHandler<GetCategoriesQuery, GetCategoriesResult>
{
    public async ValueTask<GetCategoriesResult?> HandleAsync(GetCategoriesQuery request, CancellationToken cancellationToken = default)
    {
        var miniGame = await quizRepository.Query<MiniGameInstance>()
            .Include(x => x.MiniGameDefinition)
            .Where(x => x.Game.Id == request.GameId && x.MiniGameDefinition.Type == MiniGameType.AbcdWithCategories)
            .FirstOrDefaultAsync();

        if (miniGame == null)
        {
            throw new InvalidOperationException("Mini game not found");
        }

        var state = JsonSerializer.Deserialize<AbcdWithCategoriesState>(miniGame.StateJsonData);
        var definition = JsonSerializer.Deserialize<AbcdWithCategoriesDefinition>(miniGame.MiniGameDefinition.DefinitionJsonData);

        if (state is null || definition is null)
        {
            throw new InvalidOperationException("Mini game state or definition not found");
        }

        var categories = definition.Rounds?.FirstOrDefault(x => x.Id == state.CurrentRoundId)
            ?.Categories;

        if (categories is null)
        {
            throw new InvalidOperationException("Question not found");
        }

        return new GetCategoriesResult(categories.Select(x => new Category(x.Id, x.Name)));
    }
}
