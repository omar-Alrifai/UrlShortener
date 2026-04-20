using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddProblemDetails();
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IShortLinkRepository, EfShortLinkRepository>();
builder.Services.AddScoped<IUrlShortenerService, UrlShortenerService>();
builder.Services.AddScoped<ICodeGeneratorService, RandomCodeGeneratorService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod();
    });
});
var app = builder.Build();
app.UseCors();
app.UseStatusCodePages();
app.UseExceptionHandler();



app.MapGet("/", () => "Hello World!");

app.MapPost("/shorten", async (ShortenRequest request, IUrlShortenerService urlShortenerService) =>
{
    try
    {
        string code = await urlShortenerService.ShortenUrlAsync(request.LongUrl!);
        return Results.Ok(new { shortCode = code });
    }
    catch (ArgumentException ex)
    {
        return Results.Problem(
           detail: ex.Message,
           statusCode: StatusCodes.Status400BadRequest,
           title: "Validation Error"
       );
    }
});


app.MapGet("/{code}", async (string code, IUrlShortenerService urlShortenerService) =>
{
    string? url = await urlShortenerService.GetUrlAsync(code);
    return url != null ? Results.Redirect(url) : Results.Problem(
        detail: $"The short code '{code}' was not found.",
        statusCode: StatusCodes.Status404NotFound,
        title: "Short code not found"
    );
});
app.Run();

record ShortenRequest(string? LongUrl);


