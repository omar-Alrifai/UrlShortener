using Moq;
using Xunit;
public class UrlShortenerServiceTests
{
    private readonly Mock<IShortLinkRepository> _mockRepository;
    private readonly UrlShortenerService _urlShortenerService;

    private readonly Mock<IUniqueCodeGeneratorService> _mockUniqueGenerator;
    public UrlShortenerServiceTests()
    {
        _mockRepository = new Mock<IShortLinkRepository>();
        _mockUniqueGenerator = new Mock<IUniqueCodeGeneratorService>();
        _mockUniqueGenerator.Setup(g => g.GenerateUniqueCodeAsync()).ReturnsAsync("ABC123");
        _urlShortenerService = new UrlShortenerService(_mockRepository.Object, _mockUniqueGenerator.Object);

    }
    [Fact]
    public async Task ShortenUrl_WithValidUrl_ReturnsShortCode()
    {
        string validUrl = "https://google.com";
        ShortLink result = await _urlShortenerService.ShortenUrlAsync(validUrl);
        Assert.Equal(6, result!.Code!.Length);

    }

    [Fact]
    public async Task ShortenUrl_WithInvalidUrl_ThrowsValidationException()
    {
        string invalidUrl = "httpps://google.com";
        await Assert.ThrowsAsync<ArgumentException>(() => _urlShortenerService.ShortenUrlAsync(invalidUrl));
    }

    [Fact]
    public async Task GetLongUrl_WithUnknownCode_ReturnsNull()
    {
        _mockRepository.Setup(r => r.GetByCodeAsync("missing")).ReturnsAsync((ShortLink?)null);
        var result = await _urlShortenerService.GetUrlAsync("missing");
        Assert.Null(result);
    }
}