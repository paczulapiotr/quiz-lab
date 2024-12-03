using Quiz.Master.Features.Game.CreateGame;
using Quiz.Master.Features.Game.GetGame;
using Quiz.Master.Features.Game.JoinGame;

namespace Quiz.Master;

public static partial class Endpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapCreateGame();
        endpoints.MapJoinGame();
        endpoints.MapGetGame();
        endpoints.MapGetMiniGame();
    }
}

