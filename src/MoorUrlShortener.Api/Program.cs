using System.Text.Json.Serialization;
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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapApiEndpoints();

app.Run();
