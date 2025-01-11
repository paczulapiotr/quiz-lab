using Carter;
using Quiz.Storage;

namespace Quiz.Master.Features.Game.CreateGame;


public class UploadFileEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/file", async (IFormFile file, IFileStorage storage) =>
        {
            using var stream = file.OpenReadStream();
            var id = await storage.UploadFileAsync(new(null, file.FileName, file.ContentType, stream));
            return Results.Ok(id);
        })
        .WithName("UploadFile")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Upload File")
        .WithDescription("Upload File")
        .WithTags("File")
        .DisableAntiforgery();
    }
}