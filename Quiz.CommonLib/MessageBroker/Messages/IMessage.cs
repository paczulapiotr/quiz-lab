namespace Quiz.CommonLib.Messages;

public interface IMessage
{
    public MessageDest Destination();
    public MessageSrc Source();
}