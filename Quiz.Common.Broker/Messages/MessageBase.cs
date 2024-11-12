
namespace Quiz.Common.Broker.Messages;

public record MessageBase : IMessage
{
    public Guid MessageId { get; init; }

    public DateTime Timestamp { get; init; }

    public string CorrelationId { get; init; }

    public MessageBase(string? correlationId = null)
    {
        MessageId = Guid.NewGuid();
        Timestamp = DateTime.UtcNow;
        CorrelationId = correlationId ?? Guid.NewGuid().ToString();
    }
}