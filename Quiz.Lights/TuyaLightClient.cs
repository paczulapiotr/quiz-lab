using Newtonsoft.Json;
using Quiz.Lights.Models;

namespace Quiz.Lights;

public class TuyaLightClient : ITuyaLightClient
{
    private readonly HttpClient httpClient;
    private readonly string clientId;
    private readonly string clientSecret;
    private readonly string deviceId;
    private DateTime? expiresAt;
    private string accessToken = string.Empty;
    private string refreshToken = string.Empty;

    public TuyaLightClient(Uri baseUrl, string clientId, string clientSecret, string deviceId)
    {
        this.clientId = clientId;
        this.clientSecret = clientSecret;
        this.deviceId = deviceId;
        httpClient = new HttpClient()
        {
            BaseAddress = baseUrl,
            DefaultRequestHeaders =
                {
                    { "client_id", clientId },
                    { "sign_method", "HMAC-SHA256" }
                },
        };
    }

    public async Task SwitchLightAsync(bool isOn, CancellationToken cancellationToken = default)
    {
        var payload = JsonConvert.SerializeObject(new
        {
            commands = new[]
            {
                new
                {
                    code = "switch_led",
                    value = isOn
                }
            }
        });

        await SendCommandAsync(payload, cancellationToken);
    }

    public async Task ChangeColorAsync(int h, int s, int v, CancellationToken cancellationToken = default)
    {
        if (h < 0 || h > 360)
        {
            throw new ArgumentOutOfRangeException(nameof(h), "Hue must be between 0 and 360.");
        }

        if (s < 0 || s > 1000)
        {
            throw new ArgumentOutOfRangeException(nameof(s), "Saturation must be between 0 and 1000.");
        }

        if (v < 0 || v > 1000)
        {
            throw new ArgumentOutOfRangeException(nameof(v), "Value must be between 0 and 1000.");
        }

        var payload = JsonConvert.SerializeObject(new
        {
            commands = new[]
            {
                new
                {
                    code = "colour_data_v2",
                    value = new
                    {
                        h,
                        s,
                        v
                    }
                }
            }
        });

        await SendCommandAsync(payload, cancellationToken);
    }

    public async Task ChangeBrightnessAsync(int brightness, CancellationToken cancellationToken = default)
    {
        if (brightness < 10 || brightness > 1000)
        {
            throw new ArgumentOutOfRangeException(nameof(brightness), "Brightness must be between 10 and 1000.");
        }

        var payload = JsonConvert.SerializeObject(new
        {
            commands = new[]
            {
                new
                {
                    code = "bright_value_v2",
                    value = brightness
                }
            }
        });

        await SendCommandAsync(payload, cancellationToken);
    }

    public async Task ChangeTemperatureAsync(int temperature, CancellationToken cancellationToken = default)
    {
        if (temperature < 0 || temperature > 1000)
        {
            throw new ArgumentOutOfRangeException(nameof(temperature), "Temperature must be between 0 and 1000.");
        }

        var payload = JsonConvert.SerializeObject(new
        {
            commands = new[]
            {
                new
                {
                    code = "temp_value_v2",
                    value = temperature
                }
            }
        });

        await SendCommandAsync(payload, cancellationToken);
    }

    private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

    protected async Task SendCommandAsync(string payload, CancellationToken cancellationToken = default)
    {
        try
        {
            await semaphoreSlim.WaitAsync(cancellationToken);
            if (expiresAt == null)
            {
                var accessTokenInfo = await GetAccessTokenAsync(cancellationToken);
                if (accessTokenInfo == null)
                {
                    throw new Exception("Failed to retrieve access token from Tuya API.");
                }

                accessToken = accessTokenInfo.Value!;
                refreshToken = accessTokenInfo.RefreshToken!;
                expiresAt = DateTime.UtcNow.AddSeconds(accessTokenInfo.ExpireTime!.Value);
            }

            if (expiresAt < DateTime.UtcNow.AddMinutes(1))
            {
                var accessTokenInfo = await RefreshAccessTokenAsync(cancellationToken);
                if (accessTokenInfo == null)
                {
                    throw new Exception("Failed to retrieve access token from Tuya API.");
                }

                accessToken = accessTokenInfo.Value!;
                refreshToken = accessTokenInfo.RefreshToken!;
                expiresAt = DateTime.UtcNow.AddSeconds(accessTokenInfo.ExpireTime!.Value);
            }
        }
        finally
        {
            semaphoreSlim.Release();
        }

        await SendRequestAsync<object>(HttpMethod.Post, $"/v1.0/devices/{deviceId}/commands", payload, cancellationToken: cancellationToken);
    }

    protected async Task<AccessTokenInfo?> GetAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<AccessTokenInfo>(HttpMethod.Get, "/v1.0/token?grant_type=1", cancellationToken: cancellationToken);
    }

    protected async Task<AccessTokenInfo?> RefreshAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<AccessTokenInfo>(HttpMethod.Get, $"/v1.0/token/{refreshToken}", includeAccessToken: false, cancellationToken: cancellationToken);
    }

    protected async Task<T?> SendRequestAsync<T>(HttpMethod method, string path, string payload = "", bool includeAccessToken = true, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var currentTimeMillis = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
        var sign = EncryptionUtils.CalculateSignature(clientId, clientSecret, method, path, payload, currentTimeMillis, includeAccessToken ? accessToken : string.Empty);

        var request = new HttpRequestMessage()
        {
            Method = method,
            RequestUri = new Uri(path, UriKind.Relative),
        };

        request.Headers.Add("t", currentTimeMillis);
        request.Headers.Add("sign", sign);

        if (includeAccessToken && !string.IsNullOrWhiteSpace(accessToken))
        {
            request.Headers.Add("access_token", accessToken);
        }

        if (payload != string.Empty)
        {
            request.Content = new StringContent(payload);
        }

        var response = await httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Received a non-success status code from Tuya API. Code: {response.StatusCode}. Reason: {response.ReasonPhrase}");
        }

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        return DeserializeResponse<T>(responseContent);
    }

    private static T? DeserializeResponse<T>(string responseContent)
    {
        try
        {
            var deserializedResponse = JsonConvert.DeserializeObject<TuyaResponse<T>>(responseContent);
            if (deserializedResponse == null)
            {
                throw new ArgumentNullException(nameof(responseContent));
            }

            if (deserializedResponse.IsSuccess == null)
            {
                throw new ArgumentNullException(nameof(responseContent));
            }

            if ((bool)!deserializedResponse.IsSuccess)
            {
                throw new Exception(deserializedResponse.ErrorMessage!);
            }

            return deserializedResponse.Result;
        }
        catch (JsonSerializationException)
        {
            throw new Exception("An error occurred while attempting to deserialize the response from Tuya API.");
        }
    }
}
