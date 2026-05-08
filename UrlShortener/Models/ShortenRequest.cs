/// <summary>
/// Request model for creating a shortened URL.
/// </summary>
/// <param name="LongUrl">The original long URL to shorten.</param>
/// <param name="CustomCode">Optional custom alias (6-10 alphanumeric, hyphens, underscores).</param>
/// <param name="ExpiresAt">Optional expiration date in UTC (ISO 8601). Must be in the future.</param>
public record ShortenRequest
(
string? LongUrl,
string? CustomCode,
DateTime? ExpiresAt
);