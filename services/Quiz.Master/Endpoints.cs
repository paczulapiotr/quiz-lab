using Quiz.Master.Features.Game.CreateGame;
using Quiz.Master.Features.Game.GetGame;
using Quiz.Master.Features.Game.JoinGame;
using Quiz.Master.Features.MiniGame.GetMiniGame;
using Quiz.Master.Features.MiniGame.SendPlayerInteraction;

namespace Quiz.Master;

public static partial class Endpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapCreateGame();
        endpoints.MapJoinGame();
        endpoints.MapGetGame();
        endpoints.MapGetMiniGame();
        endpoints.MapGetScoresEndpoint();
        endpoints.MapSendPlayerInteraction();
        endpoints.MapAbcdWithCategories();
    }
}

