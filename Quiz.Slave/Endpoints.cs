using Quiz.Slave.ApiModels.Ping;
using Quiz.Slave.Commands;
using Quiz.Slave.Hubs;
using Quiz.Slave.Hubs.Models;

namespace Quiz.Slave;

public static class Endpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/select-answer/{answer}", async (string answer, ISyncHubClient syncClient, CancellationToken cancellationToken = default) =>
        {
            await syncClient.SelectAnswer(new SelectAnswer(answer), cancellationToken);
            return Results.Ok("Answered");
        });


        endpoints.MapPost("/ping", async (PingRequest req, IPingCommandHandler handler, CancellationToken cancellationToken = default) =>
        {
            await handler.HandleAsync(new PingCommand(req.Message), cancellationToken);
            return Results.Ok("Ping message sent");
        });
    }
}

