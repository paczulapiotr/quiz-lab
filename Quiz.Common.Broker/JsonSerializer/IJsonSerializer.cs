namespace Quiz.Common.Broker.JsonSerializer;

public interface IJsonSerializer
{
    string Serialize<T>(T obj);
    T? Deserialize<T>(string json);
}