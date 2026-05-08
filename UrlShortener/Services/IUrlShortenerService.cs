public interface IUrlShortenerService
{
    Task<ShortLink> ShortenUrlAsync(string longUrl, string? customCode = null, DateTime? expiresAt = null);
    Task<string?> GetUrlAsync(string code);

}