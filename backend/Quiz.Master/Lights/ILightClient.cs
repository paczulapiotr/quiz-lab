namespace Quiz.Master.Lights;
public interface ILightsClient
{
    public Task SetWhite(string deviceId, int colorTemperature, int brightness);
    public Task SetColor(string deviceId, int r, int g, int b, int brightness);
    public Task TurnOff(string deviceId);
}