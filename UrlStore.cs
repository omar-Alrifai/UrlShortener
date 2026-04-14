using Microsoft.EntityFrameworkCore;

public class UrlStore
{
    private readonly AppDbContext _context;
    public UrlStore(AppDbContext context)
    {
        this._context = context;
    }

    public async Task<string> Add(string longUrl)
    {
        var shortLink = new ShortLink
        {
            Code = GenerateCode(),
            LongUrl = longUrl
        };
        _context.ShortLinks.Add(shortLink);
        await _context.SaveChangesAsync();
        return shortLink.Code;

    }

    public async Task<string?> TryGet(string code)
    {
        var obj = await _context.ShortLinks.FirstOrDefaultAsync(x => x.Code == code);
        return obj?.LongUrl;
    }

    string GenerateCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, 6).Select(s => s[Random.Shared.Next(chars.Length)]).ToArray());
    }
}