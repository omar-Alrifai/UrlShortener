using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using System.Reflection;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddProblemDetails();
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IShortLinkRepository, EfShortLinkRepository>();
builder.Services.AddScoped<IUrlShortenerService, UrlShortenerService>();
builder.Services.AddScoped<ICodeGeneratorService, RandomCodeGeneratorService>();
builder.Services.AddScoped<IUniqueCodeGeneratorService, UniqueCodeGeneratorService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "URL Shortener API",
        Version = "v1",
        Description = "A simple URL shortener with custom aliases, click tracking, and link expiry.",
        Contact = new OpenApiContact
        {
            Name = "Omar ri",
            Email = "omar.bunisher2023@gmail.com"
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

});

builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("PostShortenPolicy", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            }));

    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.Headers.RetryAfter = "60";
        await context.HttpContext.Response.WriteAsJsonAsync(new
        {
            detail = "Too many requests. Please try again in a minute.",
            title = "Rate Limit Exceeded"
        }, cancellationToken);
    };
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "URL Shortener API v1"));
}

app.UseCors();
app.UseStatusCodePages();
app.UseExceptionHandler();
app.UseRateLimiter();

app.MapPost("/shorten", async (ShortenRequest request, IUrlShortenerService urlShortenerService) =>
{
    try
    {
        var link = await urlShortenerService.ShortenUrlAsync(request.LongUrl!, request.CustomCode, request.ExpiresAt);
        return Results.Ok(new { shortLink = link });
    }
    catch (ArgumentException ex)
    {
        return Results.Problem(
           detail: ex.Message,
           statusCode: StatusCodes.Status400BadRequest,
           title: "Validation Error"
       );
    }
    catch (InvalidOperationException ex)
    {
        return Results.Problem(
           detail: ex.Message,
           statusCode: StatusCodes.Status409Conflict,
           title: "Duplicate Alias Error"
        );
    }
})
.WithSummary("Create a shortened URL")
.WithDescription("Accepts a long URL with optional custom alias and expiration date, and returns a shortened link.")
.Produces<ShortLink>(StatusCodes.Status200OK)
.ProducesProblem(StatusCodes.Status400BadRequest)
.ProducesProblem(StatusCodes.Status409Conflict)
.WithTags("UrlShortener")
.WithName("ShortenUrl");

app.MapGet("/{code}", async (string code, IUrlShortenerService urlShortenerService) =>
{
    try
    {
        string? url = await urlShortenerService.GetUrlAsync(code);
        return url != null ? Results.Redirect(url) : Results.Problem(
            detail: $"The short code '{code}' was not found.",
            statusCode: StatusCodes.Status404NotFound,
            title: "Short code not found"
        );
    }
    catch (LinkExpiredException ex)
    {
        return Results.Problem(
        detail: ex.Message,
        statusCode: StatusCodes.Status410Gone,
        title: "Link Expired"
        );
    }

})
.WithSummary("Redirect to original URL")
.WithDescription("Redirects using a short code. Returns 404 if not found, 410 if expired.")
.Produces(StatusCodes.Status302Found)
.ProducesProblem(StatusCodes.Status404NotFound)
.ProducesProblem(StatusCodes.Status410Gone)
.WithTags("UrlShortener")
.WithName("RedirectToLongUrl");

app.Run();



