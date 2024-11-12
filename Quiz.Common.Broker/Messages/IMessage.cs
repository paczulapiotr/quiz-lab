namespace Quiz.Common.Broker.Messages;

public interface IMessage
{
    Guid MessageId { get; }
    DateTime Timestamp { get; }
    string CorrelationId { get; }
}