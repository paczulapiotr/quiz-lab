using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace Quiz.Master.Lights;

public class LightsClient : ILightsClient, IDisposable
{
    private readonly HttpClient httpClient;
    private readonly LightsConfig lightsConfig;

    public LightsClient(IOptions<LightsConfig> lightsConfig, IHttpClientFactory httpClientFactory)
    {
        this.lightsConfig = lightsConfig.Value;
        httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(new Uri(this.lightsConfig.BaseUrl), "/api/services/light/");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.lightsConfig.Token);
    }

    public async Task SetColor(string deviceId, int r, int g, int b, int brightness)
    {
        if (lightsConfig.Devices.TryGetValue(deviceId, out var lightId))
        {
            var content = new StringContent(
            JsonSerializer.Serialize(new ColorLightUpdate
            {
                EntityId = lightId,
                RgbColor = [r, g, b],
                Brightness = brightness
            }), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("turn_on", content);
            response.EnsureSuccessStatusCode();
        }
    }

    public async Task SetWhite(string deviceId, int colorTemperature, int brightness)
    {
        if (lightsConfig.Devices.TryGetValue(deviceId, out var lightId))
        {
            var content = new StringContent(
            JsonSerializer.Serialize(new WhiteLightUpdate
            {
                EntityId = lightId,
                ColorTemp = colorTemperature,
                Brightness = brightness
            }), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("turn_on", content);
            response.EnsureSuccessStatusCode();
        }
    }

    public async Task TurnOff(string deviceId)
    {
        if (lightsConfig.Devices.TryGetValue(deviceId, out var lightId))
        {
            var content = new StringContent(
                JsonSerializer.Serialize(new LightUpdate
                {
                    EntityId = lightId,
                }), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("turn_off", content);
            response.EnsureSuccessStatusCode();
        }
    }

    public void Dispose()
    {
        httpClient.Dispose();
    }

    public record ColorLightUpdate : LightUpdate
    {
        [System.Text.Json.Serialization.JsonPropertyName("rgb_color")]
        public required int[] RgbColor { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("brightness")]
        public required int Brightness { get; set; }
    }

    public record WhiteLightUpdate : LightUpdate
    {
        [System.Text.Json.Serialization.JsonPropertyName("brightness")]
        public required int Brightness { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("color_temp")]
        public required int ColorTemp { get; set; }
    }


    public record LightUpdate
    {
        [System.Text.Json.Serialization.JsonPropertyName("entity_id")]
        public required string EntityId { get; set; }
    }
}

