namespace Quiz.Lights;

public interface ITuyaLightClient
{
    Task SwitchLightAsync(bool isOn, CancellationToken cancellationToken = default);
    Task ChangeColorAsync(int h, int s, int v, CancellationToken cancellationToken = default);
    Task ChangeBrightnessAsync(int brightness, CancellationToken cancellationToken = default);
    Task ChangeTemperatureAsync(int temperature, CancellationToken cancellationToken = default);
}