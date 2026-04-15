public class UrlStore
{
    private readonly IShortLinkRepository _repository;
    public UrlStore(IShortLinkRepository repository)
    {
        this._repository = repository;
    }

    public async Task<string> Add(string longUrl)
    {
        var shortLink = new ShortLink
        {
            Code = GenerateCode(),
            LongUrl = longUrl
        };
        await _repository.AddAsync(shortLink);
        return shortLink.Code;
    }
    public async Task<string?> TryGet(string code)
    {
        var obj = await _repository.GetByCodeAsync(code);
        return obj?.LongUrl;
    }

    string GenerateCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, 6).Select(s => s[Random.Shared.Next(chars.Length)]).ToArray());
    }
}