using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using MoorUrlShortener.Api.Database;
using MoorUrlShortener.Api.Extensions;
using MoorUrlShortener.Api.Repositories.ShortenedUrls;
using MoorUrlShortener.Api.Services.UrlShortenerServices;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
);

builder.Services.AddNpgsql<AppDbContext>(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddScoped<IShortenedUrlRepository, ShortenedUrlRepository>();
builder.Services.AddTransient<IUrlShortenerService, UrlShortenerService>();

var rateLimitConfig = builder.Configuration.GetSection("RateLimiting");
var permitLimit = rateLimitConfig.GetValue<int>("PermitLimit");
var window = rateLimitConfig.GetValue<int>("Window");
var queueLimit = rateLimitConfig.GetValue<int>("QueueLimit");

builder.Services.AddRateLimiter(options =>
    {
        options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
            RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                factory: _ => new FixedWindowRateLimiterOptions
                {
                    AutoReplenishment = true, PermitLimit = permitLimit,
                    QueueLimit = queueLimit, Window = TimeSpan.FromSeconds(window)
                }
            )
        );

        options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    }
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRateLimiter();

app.MapApiEndpoints();

app.Run();
