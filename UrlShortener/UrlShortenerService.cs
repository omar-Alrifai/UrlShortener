public class UrlShortenerService : IUrlShortenerService
{
    private readonly IShortLinkRepository _shortLinkRepository;
    private readonly IUniqueCodeGeneratorService _uniqueCodeGeneratorService;
    public UrlShortenerService(IShortLinkRepository repository, IUniqueCodeGeneratorService uniqueCodeGenerator)
    {
        this._shortLinkRepository = repository;
        this._uniqueCodeGeneratorService = uniqueCodeGenerator;
    }

    public async Task<string?> GetUrlAsync(string code)
    {
        var shortLink = await _shortLinkRepository.GetByCodeAsync(code);
        if (shortLink == null) return null;
        await _shortLinkRepository.IncrementClicksAsync(code);
        return shortLink.LongUrl;
    }

    public async Task<ShortLink> ShortenUrlAsync(string longUrl, string? customCode = null)
    {
        var (isValid, errorMessage) = ValidatorHelper.Validate(longUrl);
        if (!isValid)
            throw new ArgumentException(errorMessage);

        string finalCode;
        if (!string.IsNullOrWhiteSpace(customCode))
        {
            if (!ValidatorHelper.IsValidCustomCode(customCode))
                throw new ArgumentException("Custom code must be 6-10 alphanumeric characters.");

            if (await _shortLinkRepository.ExistsByCodeAsync(customCode))
                throw new InvalidOperationException($"Custom code '{customCode}' is already taken.");

            finalCode = customCode;
        }
        else
        {
            finalCode = await _uniqueCodeGeneratorService.GenerateUniqueCodeAsync();
        }
        var shortLink = new ShortLink
        {
            Code = finalCode,
            LongUrl = longUrl,
            Clicks = 0,
        };
        await _shortLinkRepository.AddAsync(shortLink);
        return shortLink;
    }
}
