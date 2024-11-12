
namespace Quiz.Common.Broker.Messages;

public record MessageBase : IMessage
{
    public string MessageId { get; init; }

    public DateTime Timestamp { get; init; }

    public string CorrelationId { get; init; }

    public MessageBase(string? correlationId = null)
    {
        Timestamp = DateTime.UtcNow;
        MessageId = IdGenerator.New;
        CorrelationId = correlationId ?? IdGenerator.New;
    }
}