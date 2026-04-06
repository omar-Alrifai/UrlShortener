public class UrlStore
{
    private readonly Dictionary<string, string> _urlStorage = new();

    public string Add(string longUrl)
    {
        string code = GenerateCode();
        _urlStorage[code] = longUrl;
        return code;

    }

    public string? TryGet(string code)
    {
        _urlStorage.TryGetValue(code, out string? longUrl);
        return longUrl;
    }

    string GenerateCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, 6).Select(s => s[Random.Shared.Next(chars.Length)]).ToArray());
    }
}