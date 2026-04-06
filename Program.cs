var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

UrlStore urlStore = new();
app.MapGet("/", () => "Hello World!");

app.MapPost("/shorten", (ShortenRequest request) =>
{

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
