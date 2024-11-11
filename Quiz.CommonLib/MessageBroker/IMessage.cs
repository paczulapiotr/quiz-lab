namespace Quiz.CommonLib.MessageBroker;

public interface IMessage
{
    public string Queue { get; }
    public string Exchange { get; }
}