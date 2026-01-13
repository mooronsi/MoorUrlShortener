using Microsoft.EntityFrameworkCore;
using MoorUrlShortener.Api.Database;
using MoorUrlShortener.Api.Domain.Entities.ShortenedUrl;

namespace MoorUrlShortener.Api.Repositories.ShortenedUrls;

public class ShortenedUrlRepository(AppDbContext dbContext) : IShortenedUrlRepository
{
    public Task<bool> IsExistsByShortCodeAsync(string shortCode, CancellationToken cancellationToken = default)
        => dbContext.ShortenedUrls
            .AnyAsync(x => x.ShortenedCode == shortCode, cancellationToken);

    public async Task<ShortenedUrl?> GetByShortCodeAsync(string shortCode, CancellationToken cancellationToken = default)
        => await dbContext.ShortenedUrls
            .FirstOrDefaultAsync(x => x.ShortenedCode == shortCode, cancellationToken);

    public Task<ShortenedUrl?> GetByOriginalUrlAsync(string originalUrl, CancellationToken cancellationToken = default)
        => dbContext.ShortenedUrls
            .FirstOrDefaultAsync(x => x.OriginalUrl == originalUrl, cancellationToken);

    public async Task AddAsync(ShortenedUrl shortenedUrl, CancellationToken cancellationToken = default)
    {
        await dbContext.ShortenedUrls.AddAsync(shortenedUrl, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
