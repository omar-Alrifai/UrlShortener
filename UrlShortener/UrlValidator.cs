public static class UrlValidator
{
    public static (bool isValid, string? ErrorMessage) Validate(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return (false, "URL cannot be empty");
        }
        bool isValidUri = Uri.TryCreate(url, UriKind.Absolute, out Uri? uri);
        bool isWebUri = isValidUri && (uri?.Scheme == Uri.UriSchemeHttp || uri?.Scheme == Uri.UriSchemeHttps);
        if (!isWebUri)
        {
            return (false, "The URL is invalid. Please provide a valid HTTP or HTTPS link.");
        }
        return (true, null);
    }
}