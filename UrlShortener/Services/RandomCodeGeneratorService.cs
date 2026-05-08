public class RandomCodeGeneratorService : ICodeGeneratorService
{
    public string Generate()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, 6).Select(s => s[Random.Shared.Next(chars.Length)]).ToArray());
    }
}