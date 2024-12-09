using Newtonsoft.Json;

namespace Quiz.Lights.Models;

public record TuyaResponse
{
    [JsonProperty("success", NullValueHandling = NullValueHandling.Ignore)]
    public bool? IsSuccess { get; set; }

    [JsonProperty("t", NullValueHandling = NullValueHandling.Ignore)]
    public long? Timestamp { get; set; }

    [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
    public string? ErrorCode { get; set; }

    [JsonProperty("msg", NullValueHandling = NullValueHandling.Ignore)]
    public string? ErrorMessage { get; set; }
}

public record TuyaResponse<T> : TuyaResponse
{
    [JsonProperty("result", NullValueHandling = NullValueHandling.Ignore)]
    public T? Result { get; set; }
}
