using System.Device.Gpio;
using Microsoft.Extensions.Options;

public class GpioHostedService : IHostedService, IDisposable
{
    private readonly ILogger<GpioHostedService> _logger;
    private readonly GpioSettings _settings;
    private GpioController? _controller;
    private int _clicked = 0;
    private int _released = 0;
    private Dictionary<int, string> _pins = new Dictionary<int, string> { { 23, "Pin A" }, { 17, "Pin B" }, { 24, "Pin C" }, { 27, "Pin D" } };

    public GpioHostedService(ILogger<GpioHostedService> logger, IOptions<GpioSettings> settings)
    {
        _logger = logger;
        _settings = settings.Value;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("[v0.5] Rozpalam silniki...");

        try
        {
            _controller = new GpioController();
            _controller.OpenPin(_settings.PinA, PinMode.InputPullUp);
            _controller.RegisterCallbackForPinValueChangedEvent(_settings.PinA, PinEventTypes.Falling, OnLimitSwitchPressed);
            _controller.RegisterCallbackForPinValueChangedEvent(_settings.PinA, PinEventTypes.Rising, OnLimitSwitchReleased);
            _controller.OpenPin(_settings.PinB, PinMode.InputPullUp);
            _controller.RegisterCallbackForPinValueChangedEvent(_settings.PinB, PinEventTypes.Falling, OnLimitSwitchPressed);
            _controller.RegisterCallbackForPinValueChangedEvent(_settings.PinB, PinEventTypes.Rising, OnLimitSwitchReleased);
            _controller.OpenPin(_settings.PinC, PinMode.InputPullUp);
            _controller.RegisterCallbackForPinValueChangedEvent(_settings.PinC, PinEventTypes.Falling, OnLimitSwitchPressed);
            _controller.RegisterCallbackForPinValueChangedEvent(_settings.PinC, PinEventTypes.Rising, OnLimitSwitchReleased);
            _controller.OpenPin(_settings.PinD, PinMode.InputPullUp);
            _controller.RegisterCallbackForPinValueChangedEvent(_settings.PinD, PinEventTypes.Falling, OnLimitSwitchPressed);
            _controller.RegisterCallbackForPinValueChangedEvent(_settings.PinD, PinEventTypes.Rising, OnLimitSwitchReleased);

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

        _logger.LogInformation($"x [{DateTime.Now:HH:mm:ss.fff}] Guzior wciśnięty! {_pins[args.PinNumber]}:{_clicked++}");
    }

    private void OnLimitSwitchReleased(object sender, PinValueChangedEventArgs args)
    {
        _logger.LogInformation($"x [{DateTime.Now:HH:mm:ss.fff}] Guzior puszczony! {_pins[args.PinNumber]}:{_released++}");
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
    public int PinA { get; set; }
    public int PinB { get; set; }
    public int PinC { get; set; }
    public int PinD { get; set; }
}
