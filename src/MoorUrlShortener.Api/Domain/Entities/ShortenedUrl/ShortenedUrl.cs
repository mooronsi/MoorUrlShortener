namespace MoorUrlShortener.Api.Domain.Entities.ShortenedUrl;

public class ShortenedUrl
{
    private const int OriginalUrlMaxLength = 1024;
    private const int ShortenedCodeMaxLength = 7;

    public ShortenedUrl(string originalUrl, string shortenedCode)
    {
        if (string.IsNullOrWhiteSpace(originalUrl)) throw new ArgumentNullException(nameof(originalUrl));
        if (originalUrl.Length > OriginalUrlMaxLength) throw new ArgumentException($"Original URL cannot exceed {OriginalUrlMaxLength} characters.", nameof(originalUrl));
        if (!Uri.TryCreate(originalUrl, UriKind.Absolute, out _)) throw new ArgumentException("Original URL is not a valid absolute URL.", nameof(originalUrl));
        if (string.IsNullOrWhiteSpace(shortenedCode)) throw new ArgumentNullException(nameof(shortenedCode));
        if (shortenedCode.Length > ShortenedCodeMaxLength) throw new ArgumentException($"Shortened code cannot exceed {ShortenedCodeMaxLength} characters.", nameof(shortenedCode));

        Id = ShortenedUrlId.Create(Guid.NewGuid());
        OriginalUrl = originalUrl;
        ShortenedCode = shortenedCode;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public ShortenedUrlId Id { get; }
    public string OriginalUrl { get; }
    public string ShortenedCode { get; }
    public DateTimeOffset CreatedAt { get; }
}
