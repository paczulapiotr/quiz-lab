using Carter;
using Microsoft.AspNetCore.Mvc;
using Quiz.Storage;

namespace Quiz.Master.Features.Game.CreateGame;


public class SearchFilesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/file/search", async ([FromQuery]string? name, [FromQuery]string? contentType, IFileStorage storage) =>
        {
            var files = await storage.SearchFilesAsync(name, contentType);
            return Results.Ok(files);
        })
        .WithName("SearchFiles")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Search Files")
        .WithDescription("Search Files")
        .WithTags("File");
    }
}