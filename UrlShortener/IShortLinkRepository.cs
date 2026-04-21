public interface IShortLinkRepository
{
    Task<ShortLink?> GetByCodeAsync(string code);
    Task AddAsync(ShortLink shortLink);

    Task IncrementClicksAsync(string code);
}