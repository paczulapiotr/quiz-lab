namespace Quiz.CommonLib.MessageBroker;

public abstract class Message(string queueName, string exchange) : IMessage
{
    public string Queue { get; init; } = queueName;
    public string Exchange { get; init; } = exchange;
}