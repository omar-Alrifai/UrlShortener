using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddProblemDetails();
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<UrlStore>();
var app = builder.Build();

app.UseStatusCodePages();
app.UseExceptionHandler();

// UrlStore urlStore = new();
app.MapGet("/", () => "Hello World!");

app.MapPost("/shorten", async (ShortenRequest request, UrlStore urlStore) =>
{


    var (isValid, ErrorMessage) = UrlValidator.Validate(request.LongUrl);
    if (!isValid)
    {
        return Results.Problem(
            detail: ErrorMessage,
            statusCode: StatusCodes.Status400BadRequest,
            title: "Validation Error"
        );
    }

    string code = await urlStore.Add(request.LongUrl!);
    return Results.Ok(new { shortCode = code });
});


app.MapGet("/{code}", async (string code, UrlStore urlStore) =>
{
    string? url = await urlStore.TryGet(code);
    return url != null ? Results.Redirect(url!) : Results.Problem(
        detail: $"The short code '{code}' was not found.",
        statusCode: StatusCodes.Status404NotFound,
        title: "Short code not found"
    );
});
app.Run();

record ShortenRequest(string? LongUrl);
