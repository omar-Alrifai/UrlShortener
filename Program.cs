using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

UrlStore urlStore = new();
app.MapGet("/", () => "Hello World!");

app.MapPost("/shorten", (ShortenRequest request) =>
{

    if (string.IsNullOrWhiteSpace(request.LongUrl))
    {
        return Results.BadRequest(new { error = "URL cannot be empty" });
    }
    bool isValidUri = Uri.TryCreate(request.LongUrl, UriKind.Absolute, out Uri? uri);
    bool isWebUri = isValidUri && (uri?.Scheme == Uri.UriSchemeHttp || uri?.Scheme == Uri.UriSchemeHttps);

    if (!isWebUri)
    {
        return Results.BadRequest(new { error = "The URL is invalid. Please provide a valid HTTP or HTTPS link." });
    }
    string code = urlStore.Add(request.LongUrl);
    return Results.Ok(new { shortCode = code });
});


app.MapGet("/{code}", (string code) =>
{
    string? url = urlStore.TryGet(code);
    return url != null ? Results.Redirect(url!) : Results.NotFound(new { error = "Short code not found" });
});
app.Run();

record ShortenRequest(string LongUrl);
