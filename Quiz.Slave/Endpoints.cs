using System.Net.Http.Headers;
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
        MapReverseProxy(endpoints);

        endpoints.MapPost("/select-answer/{answer}", async (string answer, ISyncHubClient syncClient, CancellationToken cancellationToken = default) =>
        {
            await syncClient.SelectAnswer(new SelectAnswer(answer), cancellationToken);
            return Results.Ok("Answered");
        });
    }

    private static void MapReverseProxy(IEndpointRouteBuilder endpoints)
    {
        endpoints.Map("/api/{*path}", async (HttpContext context, IConfiguration configuration, CancellationToken cancellationToken) =>
        {
            // Add the DeviceId header
            context.Request.Headers["DeviceId"] = DeviceIdHelper.DeviceUniqueId;

            // Create a new HttpClient
            using var httpClient = new HttpClient();

            // Create a new HttpRequestMessage
            var targetUrl = $"{configuration["MasterUrl"]}{context.Request.Path.Value?.TrimStart('/')}{context.Request.QueryString}";
            var requestMessage = new HttpRequestMessage(new HttpMethod(context.Request.Method), targetUrl);

            // Only set the content for methods that accept a body
            if (context.Request.Method == HttpMethod.Post.Method ||
                context.Request.Method == HttpMethod.Put.Method ||
                context.Request.Method == HttpMethod.Patch.Method)
            {
                requestMessage.Content = new StreamContent(context.Request.Body);
                if (context.Request.ContentType != null)
                {
                    requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(context.Request.ContentType);
                    requestMessage.Content.Headers.ContentLength = context.Request.ContentLength;
                }
            }

            // Copy the headers from the original request
            foreach (var header in context.Request.Headers)
            {
                requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            }

            // Send the request
            var responseMessage = await httpClient.SendAsync(requestMessage, cancellationToken);

            // Copy the response headers and status code
            context.Response.StatusCode = (int)responseMessage.StatusCode;

            // Set the Content-Type header

            // Copy the response content
            var responseContent = await responseMessage.Content.ReadAsByteArrayAsync();
            context.Response.ContentType = "application/json";
            context.Response.ContentLength = responseContent.Length;
            await context.Response.Body.WriteAsync(responseContent.AsMemory(), cancellationToken);
        });
    }
}

