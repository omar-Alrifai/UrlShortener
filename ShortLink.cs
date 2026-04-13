public class ShortLink
{
    public int Id { get; set; }
    public required string? Code { get; set; } = string.Empty;
    public required string? LongUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}