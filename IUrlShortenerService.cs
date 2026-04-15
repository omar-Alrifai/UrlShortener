public interface IUrlShortenerService
{
    Task<string> ShortenUrlAsync(string longUrl);
    Task<string?> GetUrlAsync(string code);
}