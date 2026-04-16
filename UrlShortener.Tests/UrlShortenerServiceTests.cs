using Moq;
using Xunit;
public class UrlShortenerServiceTests
{
    private readonly Mock<IShortLinkRepository> _mockRepository;
    private readonly UrlShortenerService _urlShortenerService;

    public UrlShortenerServiceTests()
    {
        _mockRepository = new Mock<IShortLinkRepository>();
        _urlShortenerService = new UrlShortenerService(_mockRepository.Object);
    }
    [Fact] // ADD THIS
    public async Task ShortenUrl_WithValidUrl_ReturnsShortCode()
    {
        string validUrl = "https://google.com";
        string code = await _urlShortenerService.ShortenUrlAsync(validUrl);
        Assert.Equal(6, code.Length);

    }

    [Fact] // ADD THIS
    public async Task ShortenUrl_WithInvalidUrl_ThrowsValidationException()
    {
        string invalidUrl = "httpps://google.com";
        await Assert.ThrowsAsync<ArgumentException>(() => _urlShortenerService.ShortenUrlAsync(invalidUrl));
    }

    [Fact] // ADD THIS
    public async Task GetLongUrl_WithUnknownCode_ReturnsNull()
    {
        _mockRepository.Setup(r => r.GetByCodeAsync("missing")).ReturnsAsync((ShortLink?)null);
        var result = await _urlShortenerService.GetUrlAsync("missing");
        Assert.Null(result);
    }
}