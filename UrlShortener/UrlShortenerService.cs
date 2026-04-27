using System.Text.RegularExpressions;

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
        await _shortLinkRepository.IncrementClicksAsync(code);
        return shortLink.LongUrl;
    }

    public async Task<ShortLink> ShortenUrlAsync(string longUrl, string? customCode = null)
    {
        var (isValid, errorMessage) = UrlValidator.Validate(longUrl);
        if (!isValid)
            throw new ArgumentException(errorMessage);

        string finalCode;
        if (!string.IsNullOrWhiteSpace(customCode))
        {
            if (!IsValidCustomCode(customCode))
                throw new ArgumentException("Custom code must be 6-10 alphanumeric characters.");

            if (await _shortLinkRepository.ExistsByCodeAsync(customCode))
                throw new InvalidOperationException($"Custom code '{customCode}' is already taken.");

            finalCode = customCode;
        }
        else
        {
            finalCode = await GenerateUniqueCodeAsync();
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

    private bool IsValidCustomCode(string code)
    {
        return code.Length >= 6 && code.Length <= 10 &&
               Regex.IsMatch(code, "^[a-zA-Z0-9]+$");
    }

    private async Task<string> GenerateUniqueCodeAsync()
    {
        for (int i = 0; i < 10; i++)
        {
            string code = _codeGeneratorService.Generate();
            if (!await _shortLinkRepository.ExistsByCodeAsync(code))
                return code;
        }
        throw new InvalidOperationException("Could not generate unique code after 10 attempts.");
    }
}