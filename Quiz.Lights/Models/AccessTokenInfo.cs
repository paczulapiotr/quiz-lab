using Newtonsoft.Json;

namespace Quiz.Lights.Models;

public record AccessTokenInfo
{
    /// <summary>
    /// Gets or sets the token string.
    /// </summary>
    [JsonProperty("access_token", NullValueHandling = NullValueHandling.Ignore)]
    public string? Value { get; set; }

    /// <summary>
    /// Gets or sets the expiration time.
    /// </summary>
    [JsonProperty("expire_time", NullValueHandling = NullValueHandling.Ignore)]
    public long? ExpireTime { get; set; }

    /// <summary>
    /// Gets or sets the refresh token.
    /// </summary>
    [JsonProperty("refresh_token", NullValueHandling = NullValueHandling.Ignore)]
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Gets or sets the UID.
    /// </summary>
    [JsonProperty("uid", NullValueHandling = NullValueHandling.Ignore)]
    public string? Id { get; set; }
}