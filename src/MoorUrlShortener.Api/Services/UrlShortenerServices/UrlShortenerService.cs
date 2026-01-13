using MoorUrlShortener.Api.Domain.Entities.ShortenedUrl;
using MoorUrlShortener.Api.Repositories.ShortenedUrls;
using MoorUrlShortener.Api.Utils;

namespace MoorUrlShortener.Api.Services.UrlShortenerServices;

public class UrlShortenerService(IShortenedUrlRepository shortenedUrlRepository) : IUrlShortenerService
{
    private const int MaxRetries = 5;

    public async Task<ShortenedUrl> CreateShortenedUrlAsync(string originalUrl, CancellationToken cancellationToken = default)
    {
        var shortenedUrl = await shortenedUrlRepository.GetByOriginalUrlAsync(originalUrl, cancellationToken);
        if (shortenedUrl is not null) return shortenedUrl;

        string shortCode;
        var retries = 0;

        do
        {
            shortCode = ShortCodeGenerator.Generate();
            retries++;

            if (retries > MaxRetries)
                throw new InvalidOperationException("Unable to generate unique short code after multiple attempts.");

        }
        while (await shortenedUrlRepository.IsExistsByShortCodeAsync(shortCode, cancellationToken));

        shortenedUrl = new ShortenedUrl(originalUrl, shortCode);
        await shortenedUrlRepository.AddAsync(shortenedUrl, cancellationToken);

        return shortenedUrl;
    }

    public async Task<string?> GetOriginalUrlAsync(string shortCode, CancellationToken cancellationToken = default)
    {
        var shortenedUrl = await shortenedUrlRepository.GetByShortCodeAsync(shortCode, cancellationToken);
        return shortenedUrl?.OriginalUrl;
    }
}
