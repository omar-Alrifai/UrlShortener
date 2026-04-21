public class UrlShortenerService : IUrlShortenerService
{
    private readonly IShortLinkRepository _shortLinkRepository;
    private readonly ICodeGeneratorService _codeGeneratorService;
    public UrlShortenerService(IShortLinkRepository repository, ICodeGeneratorService codeGenerator)
    {
        this._shortLinkRepository = repository;
        this._codeGeneratorService = codeGenerator;
    }

    public async Task<string?> GetUrlAsync(string code)
    {
        var shortLink = await _shortLinkRepository.GetByCodeAsync(code);
        if (shortLink == null) return null;
        await ProcessRedirectAsync(code);
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
            Code = _codeGeneratorService.Generate(),
            LongUrl = longUrl
        };
        await _shortLinkRepository.AddAsync(shortLink);
        return shortLink.Code;
    }

    public async Task ProcessRedirectAsync(string code)
    {
        await _shortLinkRepository.IncrementClicksAsync(code);
    }

}