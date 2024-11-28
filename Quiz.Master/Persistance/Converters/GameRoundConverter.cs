using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Quiz.Master.Persistance.Models;

namespace Quiz.Master.Persistance.Converters;

public class GameRoundConverter : ValueConverter<IEnumerable<GameRound>, string>
{
    public GameRoundConverter() : base(
        v => string.Join(",", v.Select(gr => gr.ToString())),
        v => v.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(gr => Enum.Parse<GameRound>(gr)))
    {
    }
}