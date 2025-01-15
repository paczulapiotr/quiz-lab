namespace Quiz.Master.Services.ContentManagement;

public record ContentManagementConfig
{
    public string Token { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
}