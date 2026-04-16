public class UrlShortenerService : IUrlShortenerService
{
    private readonly IShortLinkRepository _shortLinkRepository;

    public UrlShortenerService(IShortLinkRepository repository)
    {
        this._shortLinkRepository = repository;
    }

    public async Task<string?> GetUrlAsync(string code)
    {
        var shortLink = await _shortLinkRepository.GetByCodeAsync(code);
        return shortLink?.LongUrl;
    }

    public async Task<string> ShortenUrlAsync(string longUrl)
    {
        var (isValid, errorMessage) = UrlValidator.Validate(longUrl);
        if (!isValid)
        {
            throw new ArgumentException(errorMessage, "Invalid URL.");
        }

        ShortLink shortLink = new ShortLink
        {
            Code = GenerateCode(),
            LongUrl = longUrl
        };
        await _shortLinkRepository.AddAsync(shortLink);
        return shortLink.Code;
    }
    string GenerateCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, 6).Select(s => s[Random.Shared.Next(chars.Length)]).ToArray());
    }
}