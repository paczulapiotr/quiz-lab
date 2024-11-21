using Quiz.Common;
using Quiz.Common.Broker.Publisher;
using Quiz.Common.Messages;
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

        endpoints.MapPost("/register/{player}", async (string player, IPublisher publisher, CancellationToken cancellationToken = default) =>
        {
            var id = DeviceIdHelper.DeviceUniqueId;
            await publisher.PublishAsync(new PlayerRegister(id, player), cancellationToken);
            return Results.Ok("Player registered with id: " + id);
        });
    }
}

