using Quiz.Common;
using Quiz.Common.Broker.Publisher;
using Quiz.Common.Messages.PingPong;
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

        endpoints.MapPost("/register/{player}", async (string player, IPublisher publisher, CancellationToken cancellationToken = default) =>
        {
            var id = DeviceIdHelper.DeviceUniqueId;
            await publisher.PublishAsync(new PlayerRegister(id, player), cancellationToken);
            return Results.Ok("Player registered with id: " + id);
        });
    }
}

