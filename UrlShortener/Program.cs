using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using System.Reflection;

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

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "URL Shortener API v1"));
}

app.UseCors();
app.UseStatusCodePages();
app.UseExceptionHandler();

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
});

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

});

app.Run();



