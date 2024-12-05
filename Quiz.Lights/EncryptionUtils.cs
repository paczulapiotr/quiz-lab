using System.Security.Cryptography;
using System.Text;

namespace Quiz.Lights;

internal static class EncryptionUtils
{

    internal static string CalculateSignature(string clientId, string clientSecret, HttpMethod method, string relativeUrl, string payload, string timestamp, string accessToken = "", string nonce = "")
    {
        var stringToSign = string.Join("\n", new List<string>() { method.ToString(), payload.ToSha256(), string.Empty, relativeUrl });

        return EncryptHmac(
            $"{clientId}{accessToken}{timestamp}{nonce}{stringToSign}",
            clientSecret);
    }

    internal static string ToSha256(this string rawString)
    {
        using var sha256Hash = SHA256.Create();
        var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawString));
        return ConvertByteArrayToString(bytes);
    }

    private static string EncryptHmac(string message, string secret)
    {
        var encoding = new UTF8Encoding();
        var keyByte = encoding.GetBytes(secret);
        var messageBytes = encoding.GetBytes(message);

        using var hmacSha256 = new HMACSHA256(keyByte);
        var hashMessage = hmacSha256.ComputeHash(messageBytes);
        return ConvertByteArrayToString(hashMessage).ToUpper();
    }

    private static string ConvertByteArrayToString(byte[] bytes)
    {
        var builder = new StringBuilder();

        foreach (var b in bytes)
        {
            builder.Append(b.ToString("x2"));
        }

        return builder.ToString();
    }
}
