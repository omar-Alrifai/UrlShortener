public record ShortenRequest
(
string? LongUrl,
string? CustomCode,
DateTime? ExpiresAt
);