public class ShortLink
{
    /// <summary>Unique identifier.</summary>
    public int Id { get; set; }
    /// <summary>Short code (alias).</summary>
    public required string? Code { get; set; } = string.Empty;
    /// <summary>Original long URL.</summary>
    public required string? LongUrl { get; set; } = string.Empty;
    /// <summary>UTC creation timestamp.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>Number of clicks.</summary>
    public int Clicks { get; set; }
    /// <summary>UTC expiration time (null = never expires).</summary>
    public DateTime? ExpiresAt { get; set; }
}