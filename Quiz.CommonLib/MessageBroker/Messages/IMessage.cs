namespace Quiz.CommonLib.MessageBroker.Messages;

public interface IMessage
{
    public MessageDest Destination();
    public MessageSrc Source();
}