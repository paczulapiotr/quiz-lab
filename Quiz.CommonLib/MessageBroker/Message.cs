namespace Quiz.CommonLib.MessageBroker;

public abstract record Message(string Exchange, string RoutingKey = "") : IMessage
{
}