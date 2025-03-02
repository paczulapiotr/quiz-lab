using Microsoft.AspNetCore.SignalR;
using Quiz.Common.Broker.Publisher;
using Quiz.Common.Hubs;

namespace Quiz.Master.Hubs;

internal partial class SyncHub : SyncHubBase
{
    public SyncHub(IHubConnection hubConnection): base(hubConnection)
    {
    }
}
