
namespace Quiz.Master.Persistance.Models.MiniGames.AbcdCategories;

public enum PowerPlay
{
    Slime = 1,
    Freeze,
    Bombs,
    Letters,
}

public class PowerPlaysDictionary : Dictionary<string, (PowerPlay powerPlay, string sourceDeviceId)[]>
{
}