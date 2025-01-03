namespace Quiz.Master.Services.Lights;

public record LightsConfig
{
    public string Token { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
    public Dictionary<string, string> Devices { get; set; } = new();
}