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
}