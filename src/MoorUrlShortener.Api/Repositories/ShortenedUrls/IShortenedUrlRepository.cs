using MoorUrlShortener.Api.Domain.Entities.ShortenedUrl;

namespace MoorUrlShortener.Api.Repositories.ShortenedUrls;

public interface IShortenedUrlRepository
{
    Task<bool> IsExistsByShortCodeAsync(string shortCode, CancellationToken cancellationToken = default);
    Task<ShortenedUrl?> GetByShortCodeAsync(string shortCode, CancellationToken cancellationToken = default);
    Task<ShortenedUrl?> GetByOriginalUrlAsync(string originalUrl, CancellationToken cancellationToken = default);
    Task AddAsync(ShortenedUrl shortenedUrl, CancellationToken cancellationToken = default);
}
