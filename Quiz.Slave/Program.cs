using System.Text.Json.Serialization;
using Quiz.Slave.Hubs;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

// Register the GPIO hosted service with configuration
builder.Services.Configure<GpioSettings>(builder.Configuration.GetSection("GpioSettings"));
builder.Services.AddHostedService<GpioHostedService>();

// Add SignalR services
builder.Services.AddSignalR();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});


var app = builder.Build();

// Apply the CORS policy
app.UseCors();

var api = app.MapGroup("/health");
api.MapGet("/", () => new Health("Healthy", DateTime.Now));

// Map the PingPongHub
app.MapHub<PingPongHub>("/pingpong");

app.Run();

public record Health(string? Status, DateTime? Timestamp = null, bool IsHealthy = true);

[JsonSerializable(typeof(Health))]
internal partial class AppJsonSerializerContext : JsonSerializerContext { }
