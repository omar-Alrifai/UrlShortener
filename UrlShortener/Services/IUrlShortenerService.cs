public interface IUrlShortenerService
{
    Task<ShortLink> ShortenUrlAsync(string longUrl, string? customCode = null);
    Task<string?> GetUrlAsync(string code);

}