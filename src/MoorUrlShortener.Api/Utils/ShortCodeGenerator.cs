namespace MoorUrlShortener.Api.Utils;

public static class ShortCodeGenerator
{
    private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const int CodeLength = 7;

    public static string Generate()
    {
        var code = new char[CodeLength];

        for (var i = 0; i < CodeLength; i++)
        {
            code[i] = Alphabet[Random.Shared.Next(Alphabet.Length)];
        }

        return new string(code);
    }
}
