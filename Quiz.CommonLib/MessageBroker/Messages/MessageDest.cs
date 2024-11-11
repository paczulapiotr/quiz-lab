namespace Quiz.CommonLib.MessageBroker.Messages;
public record MessageDest(string Exchange, string RoutingKey = "")
{
}