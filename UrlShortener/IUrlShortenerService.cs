public interface IUrlShortenerService
{
    Task<ShortLink> ShortenUrlAsync(string longUrl);
    Task<string?> GetUrlAsync(string code);

}