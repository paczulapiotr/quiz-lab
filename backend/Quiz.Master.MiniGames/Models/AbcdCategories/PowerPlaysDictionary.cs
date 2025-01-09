
namespace Quiz.Master.MiniGames.Models.AbcdCategories;

public enum PowerPlay
{
    Slime = 1,
    Freeze,
    Bombs,
    Letters,
}

public class PowerPlaysDictionary : Dictionary<Guid, List<PowerPlaysDictionary.Item>>
{
    public record Item(Guid FromPlayerId, PowerPlay PowerPlay);
}