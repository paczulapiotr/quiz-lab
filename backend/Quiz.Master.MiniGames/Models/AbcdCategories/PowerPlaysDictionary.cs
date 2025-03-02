
namespace Quiz.Master.MiniGames.Models.AbcdCategories;

public enum PowerPlay
{
    Slime = 1,
    Freeze,
    Bombs,
    Letters,
}

public class PowerPlaysDictionary : Dictionary<string, List<PowerPlaysDictionary.Item>>
{
    public record Item(string FromPlayerId, PowerPlay PowerPlay);
}