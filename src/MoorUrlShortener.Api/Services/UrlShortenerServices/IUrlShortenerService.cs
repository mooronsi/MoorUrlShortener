using MoorUrlShortener.Api.Domain.Entities.ShortenedUrl;

namespace MoorUrlShortener.Api.Services.UrlShortenerServices;

public interface IUrlShortenerService
{
    Task<ShortenedUrl> CreateShortenedUrlAsync(string originalUrl, CancellationToken cancellationToken = default);
    Task<string?> GetOriginalUrlAsync(string shortCode, CancellationToken cancellationToken = default);
}
