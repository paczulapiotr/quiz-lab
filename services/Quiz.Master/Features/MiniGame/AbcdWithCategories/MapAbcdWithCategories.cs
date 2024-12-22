using Quiz.Master.Features.MiniGame.AbcdWithCategories.GetAppliedPowerPlay;
using Quiz.Master.Features.MiniGame.AbcdWithCategories.GetCategories;
using Quiz.Master.Features.MiniGame.AbcdWithCategories.GetPowerPlays;
using Quiz.Master.Features.MiniGame.AbcdWithCategories.GetQuestion;
using Quiz.Master.Features.MiniGame.AbcdWithCategories.GetQuestionAnswer;
using Quiz.Master.Features.MiniGame.AbcdWithCategories.GetSelectedCategory;

namespace Quiz.Master.Features.MiniGame.SendPlayerInteraction;

public static partial class Endpoints
{
    public static void MapAbcdWithCategories(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGetAppliedPowerPlayEndpoint();
        endpoints.MapGetCategoriesEndpoint();
        endpoints.MapGetPowerPlaysEndpoint();
        endpoints.MapGetQuestionEndpoint();
        endpoints.MapGetQuestionAnswerEndpoint();
        endpoints.MapGetSelectedCategoryEndpoint();
    }
}
