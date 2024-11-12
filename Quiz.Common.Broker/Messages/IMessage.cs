namespace Quiz.Common.Broker.Messages;

public interface IMessage
{
    string MessageId { get; }
    DateTime Timestamp { get; }
    string CorrelationId { get; }
}