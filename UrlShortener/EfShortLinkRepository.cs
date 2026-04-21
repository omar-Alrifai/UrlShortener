using Microsoft.EntityFrameworkCore;

public class EfShortLinkRepository : IShortLinkRepository
{
    private readonly AppDbContext _context;
    public EfShortLinkRepository(AppDbContext context)
    {
        this._context = context;
    }
    public async Task AddAsync(ShortLink shortLink)
    {
        _context.ShortLinks.Add(shortLink);
        await _context.SaveChangesAsync();

    }

    public async Task<ShortLink?> GetByCodeAsync(string code)
    {
        return await _context.ShortLinks.FirstOrDefaultAsync(s => s.Code == code);
    }

    public async Task IncrementClicksAsync(string code)
    {
        await _context.ShortLinks.Where(s => s.Code == code).ExecuteUpdateAsync(s => s.SetProperty(x => x.Clicks, x => x.Clicks + 1));
    }

}