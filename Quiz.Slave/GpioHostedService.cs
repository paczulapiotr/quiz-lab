using System.Device.Gpio;
using Microsoft.Extensions.Options;

public class GpioHostedService : IHostedService, IDisposable
{
    private readonly ILogger<GpioHostedService> _logger;
    private readonly GpioSettings _settings;
    private GpioController? _controller;
    private int _clicked = 0;
    private int _released = 0;

    public GpioHostedService(ILogger<GpioHostedService> logger, IOptions<GpioSettings> settings)
    {
        _logger = logger;
        _settings = settings.Value;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("[v0.3] Rozpalam silniki...");

        try
        {
            _controller = new GpioController();
            _controller.OpenPin(_settings.Pin, PinMode.InputPullUp);
            _controller.RegisterCallbackForPinValueChangedEvent(_settings.Pin, PinEventTypes.Falling, OnLimitSwitchPressed);
            _controller.RegisterCallbackForPinValueChangedEvent(_settings.Pin, PinEventTypes.Rising, OnLimitSwitchReleased);

            _logger.LogInformation("Nasłuchuję guziora...");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing GPIO.");
        }

        return Task.CompletedTask;
    }

    private void OnLimitSwitchPressed(object sender, PinValueChangedEventArgs args)
    {
        _logger.LogInformation($"x [{DateTime.Now:HH:mm:ss.fff}] Guzior wciśnięty! {_clicked++}");
    }

    private void OnLimitSwitchReleased(object sender, PinValueChangedEventArgs args)
    {
        _logger.LogInformation($"x [{DateTime.Now:HH:mm:ss.fff}] Guzior puszczony! {_released++}");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _controller?.Dispose();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _controller?.Dispose();
    }
}

public class GpioSettings
{
    public int Pin { get; set; }
}
