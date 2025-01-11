using Carter;
using Quiz.Storage;

namespace Quiz.Master.Features.Game.CreateGame;

public class DownloadFileEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/file/{fileId}", async (HttpContext context, string fileId, IFileStorage storage) =>
        {
            // Retrieve the file from your storage
            var file = await storage.GetFileAsync(fileId);

            // Set Cache-Control header
            context.Response.Headers["Cache-Control"] = "public, max-age=86400, immutable";

            // Return the file
            return Results.File(file.FileStream, file.ContentType, file.FileName);
        })
        .WithName("DownloadFile")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Download File")
        .WithDescription("Download File")
        .WithTags("File");
    }
}
