namespace Quiz.CommonLib.MessageBroker;

public interface IMessage
{
    public string Exchange { get; }
    public string RoutingKey { get; }
}