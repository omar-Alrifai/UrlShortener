var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

UrlStore urlStore = new();
app.MapGet("/", () => "Hello World!");

app.MapPost("/shorten", (ShortenRequest request) =>
{


    var (isValid, ErrorMessage) = UrlValidator.Validate(request.LongUrl);
    if (!isValid)
    {
        return Results.BadRequest(new ErrorResponse(ErrorMessage!));
    }
    // if (string.IsNullOrWhiteSpace(request.LongUrl))
    // {
    //     return Results.BadRequest(new ErrorResponse("URL cannot be empty"));
    // }
    // bool isValidUri = Uri.TryCreate(request.LongUrl, UriKind.Absolute, out Uri? uri);
    // bool isWebUri = isValidUri && (uri?.Scheme == Uri.UriSchemeHttp || uri?.Scheme == Uri.UriSchemeHttps);

    // if (!isWebUri)
    // {
    //     return Results.BadRequest(new ErrorResponse("The URL is invalid. Please provide a valid HTTP or HTTPS link."));
    // }
    string code = urlStore.Add(request.LongUrl!);
    return Results.Ok(new { shortCode = code });
});


app.MapGet("/{code}", (string code) =>
{
    string? url = urlStore.TryGet(code);
    return url != null ? Results.Redirect(url!) : Results.NotFound(new ErrorResponse("Short code not found"));
});
app.Run();

record ShortenRequest(string? LongUrl);
record ErrorResponse(string Error);
