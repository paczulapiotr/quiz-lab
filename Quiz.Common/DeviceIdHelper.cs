using System.Net.NetworkInformation;

namespace Quiz.Common;

public static class DeviceIdHelper
{
    private static readonly object _lock = new object();
    private static string? _deviceId = null;
    public static string GetDeviceUniqueId
    {
        get
        {
            if (_deviceId == null)
            {
                lock (_lock)
                {
                    if (_deviceId == null)
                        _deviceId = GetMacAddress();
                }
            }
            return _deviceId!;
        }
    }

    private static string? GetMacAddress()
    {
        var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
        var macAddress = networkInterfaces
            .Where(nic => nic.OperationalStatus == OperationalStatus.Up)
            .Select(nic => nic.GetPhysicalAddress().ToString())
            .FirstOrDefault();

        if (string.IsNullOrEmpty(macAddress))
        {
            throw new InvalidOperationException("No network adapters with an operational status found.");
        }

        return macAddress;
    }
}