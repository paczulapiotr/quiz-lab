using Quiz.Common.Hubs;

namespace Quiz.Slave.Hubs;

internal partial class SyncHub : SyncHubBase
{
    private readonly ILogger<SyncHub> _logger;

    public SyncHub(ILogger<SyncHub> logger, IHubConnection hubConnection) : base(hubConnection)
    {
        _logger = logger;
    }
}
