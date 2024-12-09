using System.Text;

namespace Quiz.Common;

public static class IdGenerator
{
    private static int _length = 8;
    private static char[] _base62chars =
        "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"
        .ToCharArray();

    private static Random _random = new Random();

    public static string New
    {
        get
        {
            var sb = new StringBuilder(_length);

            for (int i = 0; i < _length; i++)
                sb.Append(_base62chars[_random.Next(36)]);

            return sb.ToString();
        }
    }
}